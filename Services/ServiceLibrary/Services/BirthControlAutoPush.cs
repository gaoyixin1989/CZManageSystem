using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative;
using CZManageSystem.Service.Administrative.BirthControl;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ServiceLibrary
{
    public class BirthControlAutoPush:ServiceJob
    {
        
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
        IBirthControlApplyService _birthcontrolapplyservice = new BirthControlApplyService();
        IBirthControlConfigService _birthcontrolconfigService = new BirthControlConfigService();

        /// <summary>
        /// 在管理设置的时间内自动推送计划生育登记代办，不在设置时间内的自动结束计划生育登记代办
        /// </summary>
        public override bool Execute()
        {
            #region 查询当前服务策略信息
            string sTemp = "";
            if (!SetStrategyInfo(out sTemp))
            {
                sMessage = sTemp;
                return false;
            }
            #endregion
            bool boolResult = true;
            DateTime NowTime = DateTime.Now;
            int intSuccrss = 0, intError = 0;

            //获取在需要重新推送情况下，推送设置开始时间之前的需要取消计划生育代办的用户数据
            List<BirthControlApply> listCancelData = GetCancelPushUser(NowTime);
            //对用户数据取消计划生育登记代办
            AutoCancelPushData(listCancelData, out intSuccrss, out intError);
            //sMessage = string.Format("取消计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError);
            LogRecord.WriteLog(string.Format("取消计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError), LogResult.success);
            AddStrategyLog(string.Format("取消计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError), true);

            //获取需要推送计划生育代办的用户数据
            List<BirthControlApplyUser> listData = GetPushUser(NowTime);            
            //对用户数据推送计划生育登记代办
            AutoPushData(listData, out intSuccrss, out intError);
            //sMessage = string.Format("推送计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError);
            LogRecord.WriteLog(string.Format("推送计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError), LogResult.success);
            AddStrategyLog(string.Format("推送计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError), true);

            //获取需要结束计划生育代办的用户数据
            List<BirthControlApply> listEndData = GetEndPushUser(NowTime);
            //对用户数据结束计划生育登记代办
            AutoEndPushData(listEndData, out intSuccrss, out intError);
            //sMessage = string.Format("结束计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError);
            LogRecord.WriteLog(string.Format("结束计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError), LogResult.success);
            AddStrategyLog(string.Format("结束计划生育登记代办:成功{0}条,失败{1}条", intSuccrss, intError), true);

            SaveStrategyLog();
            if (boolResult)
                sMessage = "服务执行成功";
            return true;
        }
        /// <summary>
        /// 取消所有在设置推送开始时间之前的未结束计划生育流程数据
        /// </summary>
        /// <param name="listData"></param>
        /// <param name="intSuccrss"></param>
        /// <param name="intError"></param>
        public void AutoCancelPushData(List<BirthControlApply> listData, out int intSuccrss, out int intError)
        {
            intSuccrss = 0;
            intError = 0;
            foreach (var curData in listData)
            {
                bool isSuccess = true;
                BirthControlApply CurApplyData = new BirthControlApply();
                Tracking_Todo tempActivity = new Tracking_Todo();
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == curData.WorkflowInstanceId).FirstOrDefault();
                if (tempActivity != null && tempActivity.ActivityInstanceId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    string objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                            + "<parameter>"
                                + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                + "<item name=\"command\" value=\"cancel\"/>"
                                + "<item name=\"workflowProperties\">"
                                    + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{3}\">"
                                    + "</workflow>"
                                + "</item>"
                                + "<item name = \"Content\" value = \"{2}\" />"
                               + "</parameter>"
                        + "</Root>";
                    objectXML = string.Format(objectXML, curData.CurrentActors.ToString().Trim(), tempActivity.ActivityInstanceId.ToString(), "系统自动取消流程", DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

                    string[] args = new string[4];
                    args[0] = ConfigData.Workflow_SystemID;
                    args[1] = ConfigData.Workflow_SystemAcount;
                    args[2] = ConfigData.Workflow_SystemPwd;
                    args[3] = objectXML;
                    string resultXml = WebServicesHelper.InvokeWebService(ConfigData.Workflow_SystemUrl, ConfigData.WorkflowType_ManageWorkflow, args).ToString();

                    System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                    xdoc.LoadXml(resultXml);
                    System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
                    string success = resutNode.Attributes["Success"].Value;
                    string errmsg = resutNode.Attributes["ErrorMsg"].Value;
                    int intSuccess = 0;
                    int.TryParse(success, out intSuccess);
                    if (intSuccess > 0)
                    {
                        CurApplyData = curData;
                        CurApplyData.State = 99;
                        CurApplyData.CurrentActors = "";
                        CurApplyData.CurrentActivity = _tracking_WorkflowService.GetCurrentActivityNames(curData.WorkflowInstanceId.Value).FirstOrDefault();
                        _birthcontrolapplyservice.Update(CurApplyData);
                        LogRecord.WriteLog(string.Format("取消计划生育ID为{0}的代办信息成功", curData.UserId), LogResult.success);
                        AddStrategyLog(string.Format("取消计划生育ID为{0}的代办信息成功", curData.UserId), true);
                    }
                    else
                    {
                        isSuccess = false;
                        LogRecord.WriteLog(string.Format("取消计划生育ID为{0}的代办信息失败,其他提示：{1}", curData.UserId, errmsg), LogResult.fail);
                        AddStrategyLog(string.Format("取消计划生育ID为{0}的代办信息失败,其他提示：{1}", curData.UserId, errmsg), false);
                    }

                    if (isSuccess)
                        intSuccrss++;
                    else
                        intError++;
                }
            }
        }


        /// <summary>
        /// 对取得的流程申请进行结束计划生育登记代办
        /// </summary>
        /// <param name="listData"></param>
        /// <param name="intSuccrss"></param>
        /// <param name="intError"></param>
        public void AutoEndPushData(List<BirthControlApply> listData, out int intSuccrss, out int intError)
        {
            intSuccrss = 0;
            intError = 0;
            foreach (var curData in listData)
            {
                bool isSuccess = true;
                BirthControlApply CurApplyData = new BirthControlApply();
                Tracking_Todo tempActivity = new Tracking_Todo();
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == curData.WorkflowInstanceId).FirstOrDefault();
                if (tempActivity != null && tempActivity.ActivityInstanceId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    string objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                            + "<parameter>"
                                + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                + "<item name=\"command\" value=\"approve\"/>"
                                + "<item name=\"workflowProperties\">"
                                    + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{3}\">"
                                    + "</workflow>"
                                + "</item>"
                                + "<item name = \"Content\" value = \"{2}\" />"
                               + "</parameter>"
                        + "</Root>";
                    objectXML = string.Format(objectXML, curData.CurrentActors.ToString().Trim(), tempActivity.ActivityInstanceId.ToString(), "系统自动结束流程", DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

                    string[] args = new string[4];
                    args[0] = ConfigData.Workflow_SystemID;
                    args[1] = ConfigData.Workflow_SystemAcount;
                    args[2] = ConfigData.Workflow_SystemPwd;
                    args[3] = objectXML;
                    string resultXml = WebServicesHelper.InvokeWebService(ConfigData.Workflow_SystemUrl, ConfigData.WorkflowType_ManageWorkflow, args).ToString();

                    System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                    xdoc.LoadXml(resultXml);
                    System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
                    string success = resutNode.Attributes["Success"].Value;
                    string errmsg = resutNode.Attributes["ErrorMsg"].Value;
                    int intSuccess = 0;
                    int.TryParse(success, out intSuccess);
                    if (intSuccess > 0)
                    {
                        CurApplyData = curData;
                        CurApplyData.State = 2;
                        CurApplyData.CurrentActors = "";
                        CurApplyData.CurrentActivity = _tracking_WorkflowService.GetCurrentActivityNames(curData.WorkflowInstanceId.Value).FirstOrDefault();
                        _birthcontrolapplyservice.Update(CurApplyData);
                        LogRecord.WriteLog(string.Format("结束计划生育ID为{0}的代办信息成功", curData.UserId), LogResult.success);
                        AddStrategyLog(string.Format("结束计划生育ID为{0}的代办信息成功", curData.UserId), true);
                    }
                    else
                    {
                        isSuccess = false;
                        LogRecord.WriteLog(string.Format("结束计划生育ID为{0}的代办信息失败,其他提示：{1}", curData.UserId, errmsg), LogResult.fail);
                        AddStrategyLog(string.Format("结束计划生育ID为{0}的代办信息失败,其他提示：{1}", curData.UserId, errmsg), false);
                    }

                    if (isSuccess)
                        intSuccrss++;
                    else
                        intError++;
                }
            }
            
        }
        /// <summary>
        /// 对取得的用户进行推送计划生育登记代办
        /// </summary>
        /// <param name="listData"></param>
        /// <param name="intSuccrss"></param>
        /// <param name="intError"></param>
        public void AutoPushData(List<BirthControlApplyUser> listData, out int intSuccrss, out int intError)
        {
            intSuccrss = 0;
            intError = 0;
            foreach (var curData in listData)
            {
                bool isSuccess = true;
                BirthControlApply CurApplyData = new BirthControlApply();
                string objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, "admin", "计划生育", curData.RealName+"的计划生育登记", curData.UserId, "登记", curData.UserName, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));
                string[] args = new string[4];
                args[0] = ConfigData.Workflow_SystemID;
                args[1] = ConfigData.Workflow_SystemAcount;
                args[2] = ConfigData.Workflow_SystemPwd;
                args[3] = objectXML;

                string resultXml = WebServicesHelper.InvokeWebService(ConfigData.Workflow_SystemUrl, ConfigData.WorkflowType_ManageWorkflow, args).ToString();
                System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
                xdoc.LoadXml(resultXml);
                System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
                string success = resutNode.Attributes["Success"].Value;
                string errmsg = resutNode.Attributes["ErrorMsg"].Value;
                string strWorkflowInstanceId = "";
                string strActivityinstanceId = "";
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
                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    CurApplyData.State = 1;
                    CurApplyData.WorkflowInstanceId = new Guid(strWorkflowInstanceId);
                    CurApplyData.SheetId = workFlow.SheetId;
                    CurApplyData.ApplyTime = DateTime.Now;
                    CurApplyData.UserId = curData.UserId;
                    CurApplyData.Title = curData.RealName + "的计划生育登记";
                    CurApplyData.CurrentActors = curData.UserName;
                    CurApplyData.CurrentActivity = _tracking_WorkflowService.GetCurrentActivityNames(Guid.Parse(strWorkflowInstanceId)).FirstOrDefault();
                    _birthcontrolapplyservice.Insert(CurApplyData);

                    LogRecord.WriteLog(string.Format("推送计划生育ID为{0}的代办信息成功", curData.UserId), LogResult.success);
                    AddStrategyLog(string.Format("推送计划生育ID为{0}的代办信息成功", curData.UserId), true);
                }
                else
                {
                    isSuccess = false;
                    LogRecord.WriteLog(string.Format("结束计划生育ID为{0}的代办信息失败,其他提示：{1}", curData.UserId, errmsg), LogResult.fail);
                    AddStrategyLog(string.Format("结束计划生育ID为{0}的代办信息失败,其他提示：{1}", curData.UserId, errmsg), false);
                }

                if (isSuccess)
                    intSuccrss++;
                else
                    intError++;
            }
            if (listData.Count > 0)
            {
                BirthControlConfig obj = _birthcontrolconfigService.FindById(1);
                obj.IsPush = "1";
                _birthcontrolconfigService.Update(obj);
            }
        }

        /// <summary>
        /// 获取在设置推送开始时间之前的所有未结束计划生育流程数据
        /// </summary>
        /// <param name="nowtime"></param>
        /// <returns></returns>

        public List<BirthControlApply> GetCancelPushUser(DateTime nowtime)
        {
            List<BirthControlApply> listResult = new List<BirthControlApply>();
            BirthControlConfig obj = _birthcontrolconfigService.FindById(1);
            string ConfirmStartdate, ConfirmEnddate,  IsPush;
            if (obj != null)
            {
                ConfirmStartdate = obj.ConfirmStartdate == null ? "" : obj.ConfirmStartdate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                ConfirmEnddate = obj.ConfirmEnddate == null ? "" : obj.ConfirmEnddate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                IsPush = obj.IsPush.ToString().Trim();
                if (ConfirmStartdate.CompareTo(nowtime.ToString("yyyy-MM-dd")) > 0 && IsPush == "0")
                {
                     listResult = new EfRepository<BirthControlApply>().ExecuteResT<BirthControlApply>(string.Format("SELECT * FROM dbo.BirthControlApply  where DATEDIFF(day,ApplyTime,'{0}')>0 and State=1", ConfirmStartdate)).ToList();
                }
            }
            return listResult;
        }


        /// <summary>
        /// 获取结束计划生育代办的流程数据
        /// </summary>
        /// <param name="nowtime"></param>
        /// <returns></returns>
        public List<BirthControlApply> GetEndPushUser(DateTime nowtime)
        {
            List<BirthControlApply> listResult = new List<BirthControlApply>();
            BirthControlConfig obj = _birthcontrolconfigService.FindById(1);

            string ConfirmStartdate, ConfirmEnddate, ManAge, WomenAge, IsPush;

            if (obj != null)
            {
                ConfirmStartdate = obj.ConfirmStartdate == null ? "" : obj.ConfirmStartdate.Value.ToString("yyyy-MM-dd");
                ConfirmEnddate = obj.ConfirmEnddate == null ? "" : obj.ConfirmEnddate.Value.ToString("yyyy-MM-dd");
                ManAge = obj.ManAge.ToString().Trim();
                WomenAge = obj.WomenAge.ToString().Trim();
                IsPush = obj.IsPush.ToString().Trim();
                if (nowtime.ToString("yyyy-MM-dd").CompareTo(ConfirmEnddate) >= 1)
                {
                    listResult = _birthcontrolapplyservice.GetProcessingList().ToList();
                }
                //if (nowtime.ToString("yyyy-MM-dd").CompareTo(ConfirmEnddate) >= 1 || ConfirmStartdate.CompareTo(nowtime.ToString("yyyy-MM-dd")) > 0)
                //{
                //    listResult = _birthcontrolapplyservice.GetProcessingList().ToList();
                //}
            }
            return listResult;
        }
        /// <summary>
        /// 获取推送计划生育代办的用户数据
        /// </summary>
        /// <param name="nowtime"></param>
        /// <returns></returns>
        public List<BirthControlApplyUser> GetPushUser(DateTime nowtime)
        {
            List<BirthControlApplyUser> listResult = new List<BirthControlApplyUser>();
            IBirthControlConfigService _birthcontrolconfigService = new BirthControlConfigService();
            BirthControlConfig obj = _birthcontrolconfigService.FindById(1);

            string ConfirmStartdate, ConfirmEnddate, ManAge, WomenAge, IsPush;

            if (obj!=null)
            {
                ConfirmStartdate = obj.ConfirmStartdate == null ? "" : obj.ConfirmStartdate.Value.ToString("yyyy-MM-dd");
                ConfirmEnddate = obj.ConfirmEnddate == null ? "" : obj.ConfirmEnddate.Value.ToString("yyyy-MM-dd");
                ManAge = obj.ManAge.ToString().Trim();
                WomenAge = obj.WomenAge.ToString().Trim();
                IsPush = obj.IsPush.ToString().Trim();
                if (nowtime.ToString("yyyy-MM-dd").CompareTo(ConfirmStartdate) >= 0 && nowtime.ToString("yyyy-MM-dd").CompareTo(ConfirmEnddate) <= 0)
                {
                    if (IsPush == "0")
                    {
                        listResult = new EfRepository<BirthControlApplyUser>().ExecuteResT<BirthControlApplyUser>(string.Format("SELECT UserId,RealName,UserName FROM bw_Users where UserType=1 AND EmployeeId IN (SELECT employee FROM UUM_USERINFO WHERE  (DATEDIFF(year,UserBirthday,GETDATE()) <'{0}' and Sex='1') or (DATEDIFF(year,UserBirthday,GETDATE() )<'{1}' and Sex='2')) and userid not in (SELECT UserId FROM dbo.BirthControlApply  where ApplyTime between '{2}' and '{3}' and State=1)", ManAge, WomenAge, obj.ConfirmStartdate.Value.ToString("yyyy-MM-dd HH:mm:ss"), obj.ConfirmEnddate.Value.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"))).ToList();
                        //listResult = new EfRepository<BirthControlApplyUser>().ExecuteResT<BirthControlApplyUser>(string.Format("SELECT UserId,RealName,UserName FROM bw_Users where UserType=1 AND EmployeeId IN (SELECT employee FROM UUM_USERINFO WHERE  (DATEDIFF(year,UserBirthday,GETDATE()) <'{0}' and Sex='1') or (DATEDIFF(year,UserBirthday,GETDATE() )<'{1}' and Sex='2'))", ManAge, WomenAge)).ToList();
                    }
                }
            }
            return listResult;
        }
    }
}
