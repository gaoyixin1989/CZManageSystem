using System;
using System.Collections.Generic;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;

namespace CZManageSystem.Admin.Base
{
    public class WorkFlowHelper
    {
        private static ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
       
        /// <summary>
        /// 提交审请表单
        /// </summary>
        /// <param name="username">用户</param>
        /// <param name="workflowTitle">表单标题</param>
        /// <param name="applyId">表单id</param>
        /// <param name="name">下一步骤</param>
        /// <param name="actors">下一审批</param>
        /// <param name="workflowType">流程名</param>
        /// <param name="action">当前节点</param>
        /// <returns></returns>
        public static WorkFlowResult ActionFlow(string username, string workflowTitle, Dictionary<string, string> dictField, string name, string actors, string workflowType, string action = "Start")
        {
            string strFields = "";
            if (dictField != null)
            {
                foreach (var curField in dictField)
                {
                    strFields += string.Format("<item name=\"{0}\" value=\"{1}\"></item>", curField.Key, curField.Value);
                }
            }

            string objectXML = "<Root action=\"{6}\" username=\"{0}\" keypassword=\"123\">"
                               + "<parameter>"
                                   + "<item name=\"workflowId\" value=\"{1}\"/>"
                                   + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                   + "<item name=\"workflowProperties\">"
                                       + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"2900-01-01\">"
                                           + "<fields>{3}</fields>"
                                           + "<nextactivities>"
                                               + "<item name=\"{4}\" actors=\"{5}\"/>"
                                           + "</nextactivities>"
                                       + "</workflow>"
                                   + "</item>"
                               + "</parameter>"
                           + "</Root>";
            objectXML = string.Format(objectXML, username, workflowType, workflowTitle, strFields, name, actors, action);//caiwencheng
            return GetResult(objectXML);
        }
        private static WorkFlowResult GetResult(string objectXML)
        {
            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;

            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            Tracking_Workflow workFlow = new Tracking_Workflow();
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {

                System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                for (int i = 0; i < xmlList.Count; i++)
                {
                    switch (xmlList[i].Attributes["name"].Value)
                    {
                        case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                        case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                        default: break;
                    }
                }
                workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
            }
            return new WorkFlowResult { Success = intSuccess, Errmsg = errmsg, WorkFlow = workFlow };
        }
    }
}