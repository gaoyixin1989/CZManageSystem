using System;
using System.Collections.Generic;
using System.Data;
using Botwave.XQP.Service.ExternalProxies;
//using Botwave.IBatis.Extension;
using Botwave.Workflow.Domain;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Service;
using Botwave.Security.Service;
using System.Xml.Linq;
using Botwave.Workflow.Plugin;
using Botwave.XQP.API.Interface;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 接入对接系统，通用推送数据后续处理器
    /// </summary>
    public class XqpSystemPostHandler : IPostActivityExecutionHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(XqpSystemPostHandler));
        private delegate void InvokeService(Guid activityInstanceId, string url, string method, object[] args, string type);

        #region properties

        private IActivityService activityService;
        private IWorkflowService workflowService;
        private IUserService userService;
        private IWorkflowNotifyExtendService workflowNotifyExtendService;

        public IActivityService ActivityService
        {
            get { return activityService; }
            set { activityService = value; }
        }

        public IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }

        public IUserService UserService
        {
            get { return userService; }
            set { userService = value; }
        }

        public IWorkflowNotifyExtendService WorkflowNotifyExtendService
        {
            get { return workflowNotifyExtendService; }
            set { workflowNotifyExtendService = value; }
        }

        public  WorkflowAPIService WorkflowAPIServices { set; get; }
       
        #endregion
        #region IPostActivityExecutionHandler Members

        private IPostActivityExecutionHandler next = null;
        public IPostActivityExecutionHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        #endregion


        

        private void Execute(Botwave.Workflow.ActivityExecutionContext context, IDictionary<string, object> formVariables)
        {
            log.Info("XqpSystemPostHandler Begin...");
            try
            {
                //PostActivityExecutionHandler postActivityExecutionHandler = new PostActivityExecutionHandler();
                //postActivityExecutionHandler.Execute(context);
                Guid activityInstanceId = context.ActivityInstanceId;
                //DataRow rw = GetWorkflowinstanceid(context.ActivityInstanceId);
                ActivityInstance activityInstance = activityService.GetActivity(activityInstanceId);
                WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(activityInstanceId);
                //string workflowinstanceid = rw["WorkflowInstanceId"].ToString();
                //string workflowid = rw["WorkflowId"].ToString();
                //string operateType = rw["OperateType"].ToString();
                string workflowinstanceid = workflowInstance.WorkflowInstanceId.ToString();
                string workflowid = workflowInstance.WorkflowId.ToString() ;
                //string operateType = activityInstance.OperateType.ToString();
   
                string rtnxml = stringxml(workflowinstanceid, workflowInstance.SheetId, formVariables);
                rtnxml = GetPreActivityInfo(rtnxml, activityInstance);
                //byte[] tmpbytes = System.Text.Encoding.Default.GetBytes(rtnxml);
                //byte[] targetbytes = System.Text.Encoding.Convert(Encoding.Unicode, Encoding.UTF8, tmpbytes);
                //rtnxml = System.Text.Encoding.UTF8.GetString(targetbytes);

                //activityInstance = activityService.GetCurrentActivity(new Guid(workflowinstanceid));
                //rtnxml = GetCurActivityInfo(rtnxml, activityInstance);
                //string url = GetWebServiceUrl(workflowid);
                IDictionary<string, string> dict = GetWebServiceUrl(new Guid(workflowid));
                string[] arg = new string[4];
                //前三个为鉴权参数，最后为返回的xml
                arg[0] = "";
                arg[1] = "";
                arg[2] = "";
                arg[3] = rtnxml;
                //统一推送数据
                //WebServicesHelper.InvokeWebService(url, "SearchWorkflow", arg);
                //WebServicesHelper.InvokeWebService(url, "ManageWorkflow", arg);
                InvokeService iv = new InvokeService(InvokeWeb);
                iv.BeginInvoke(activityInstanceId, dict["url"], "SearchWorkflow", arg, dict["type"],null,null);
                //object result = WebServicesHelper.InvokeWebService(dict["url"], "SearchWorkflow", arg, dict["type"]);
                //log.Info(activityInstanceId.ToString() + "调用外部接口返回结果：" + Botwave.Commons.DbUtils.ToString(result));
            }
            catch (Exception ee)
            { log.Error(ee.ToString()); }
            log.Info("XqpSystemPostHandler End...");
        }

        public void Execute(Botwave.Workflow.ActivityExecutionContext context)
        { Execute(context, context.Variables); }

        //获取workflowinstanceid
        private DataRow GetWorkflowinstanceid(Guid Activityinstanceid)
        {
            string sql = string.Format(@"SELECT tw.WorkflowInstanceId,ActivityInstanceId
      ,WorkflowId,SheetId,State,OperateType,u.realname,u.userid,Actor,StartedTime,reason ,ta.FinishedTime,Title,Secrecy,Urgency,Importance,ExpectFinishedTime,Requirement ,CommentCount
  FROM bwwf_Tracking_Workflows tw
  left join bwwf_Tracking_Act_Completed ta
  on tw.WorkflowInstanceId = ta.WorkflowInstanceId
	left join bw_Users u
on ta.actor = u.username
 where   ta.ActivityInstanceId= '{0}'", Activityinstanceid.ToString().ToUpper());
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
                return null;
        }

        //获取返回的推送的内容
        private string stringxml(string workflowinstanceid,string sheetId, IDictionary<string, object> formVariables)
        {
            string returnxml = string.Empty;
           // Botwave.XQP.API.Interface.WorkflowAPIService ws = new Botwave.XQP.API.Interface.WorkflowAPIService();
            string objectxml = @"<Root action='Detail' username='admin' KeyPassword='123' PageIndex='0' PageCount='10'>
	<parameter>
		<item name='SheetID' value='"+sheetId+@"'/>
        <item name='WorkflowInstanceId' value='" + workflowinstanceid + @"'/>
	</parameter>
</Root>
";
//            string objecexml = @"<Root action='Start' username='zhouyaning' keypassword='password'>//	     <parameter>//		<item name='workflowId' value='监控变更管理流程'/>//		<item name='workflowTitle' value='test2011122101'/>//		<item name='workflowProperties'>//			<workflow secrecy='0' urgency='1' importance='0' expectFinishedTime='2010-10-10'>//				<fields>//					<item name='F1'>新增</item>//					<item name='F2'>省管理</item>//					<item name='F3' value='天河分公司'/>//				</fields>//				//				<nextActivities>//					<item name='室经理审批' actors='zhouyaning'/>//				</nextActivities>//			</workflow>//		</item>//	</parameter>//</Root>";

            returnxml = WorkflowAPIServices.SearchWorkflow(objectxml, formVariables);
            return returnxml;
            //return objecexml;
        }

        /// <summary>
        /// 获取对接系统webservice地址
        /// </summary>
        /// <param name="workflowid">对接流程id</param>
        /// <returns></returns>
        private string GetWebServiceUrl(string workflowid)
        {
            string sql = string.Format("select url from xqp_SystemAccess where workflowid='{0}'", workflowid.ToUpper());
            return IBatisDbHelper.ExecuteScalar(CommandType.Text, sql).ToString();
        }

        /// <summary>
        /// 获取对接系统webservice地址
        /// </summary>
        /// <param name="workflowid">对接流程id</param>
        /// <returns></returns>
        private IDictionary<string,string> GetWebServiceUrl(Guid workflowid)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            string sql = string.Format("select url from xqp_SystemAccess where workflowid='{0}'", workflowid.ToString().ToUpper());
            string url = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql).ToString();
            sql = string.Format("select type from xqp_SystemAccess where workflowid='{0}'", workflowid.ToString().ToUpper());
            string type = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql).ToString();
            dict.Add("url", url);
            dict.Add("type",type);
            return dict;
        }


        private string GetPreActivityInfo(string xml, ActivityInstance activityInstance)
        {
            string ret_val = string.Empty;

            XElement xe = XElement.Parse(xml);
            XElement item = xe.Element("item");
            XElement detail = item.Element("detail");
            string node = string.Empty;
            node = "<PreActivity>	<item name=\"ACTIVITYINSTANCEID\" value=\"" + activityInstance.ActivityInstanceId + "\"/>	<item name=\"ACTIVITYID\" value=\"" + activityInstance.ActivityId + "\"/>   <item name=\"NAME\" value=\"" + activityInstance.ActivityName + "\"/>	<item name=\"ACTORS\" value=\"" + activityInstance.Actor + "\"/>    <item name=\"COMMAND\" value=\"" + activityInstance.Command + "\"/>    </PreActivity>";
            XElement nodes = XElement.Parse(node);
            detail.Add(nodes);
            ret_val = xe.ToString();
            return ret_val;
        }

        private string GetCurActivityInfo(string xml, ActivityInstance activityInstance)
        {
            string ret_val = string.Empty;
            //object actor = IBatisDbHelper.ExecuteScalar(CommandType.Text,string.Format("select username from bwwf_tracking_todo t where activityinstanceid='{0}'",activityInstance.ActivityInstanceId));
            XElement xe = XElement.Parse(xml);
            XElement item = xe.Element("item");
            XElement detail = item.Element("detail");
            //node = "<CurActivity>	<item name=\"ACTIVITYINSTANCEID\" value=\"" + activityInstance.ActivityInstanceId + "\"/>   <item name=\"ACTIVITYID\" value=\"" + activityInstance.ActivityId + "\"/>	<item name=\"NAME\" value=\"" + activityInstance.ActivityName + "\"/>	<item name=\"ACTORS\" value=\"" + actor.ToString() + "\"/>  <item name=\"OPERATETYPE\" value=\"" + activityInstance.OperateType + "\"/> </CurActivity>";
            //XElement nodes = XElement.Parse(node);
            //detail.Add(nodes);
            IEnumerable<XElement> activities = detail.Elements("Activitys");
            //node = "<item name=\"CurActivityName\" value=\"" + activityInstance.ActivityName + "\"/>" ;
            foreach (XElement activitie in activities)
            {
                XElement node = new XElement("item", new XAttribute("name", "CurActivityName"), new XAttribute("value", activityInstance.ActivityName));
                activitie.Add(node);
            }
            ret_val = xe.ToString();
            return ret_val;
        }

        private void InvokeWeb(Guid activityInstanceId, string url, string method, object[] args, string type)
        {
            try
            {
                object result = WebServicesHelper.InvokeWebService(url, method, args, type);
                log.Info(activityInstanceId.ToString() + "调用外部接口返回结果：" + Botwave.Commons.DbUtils.ToString(result));
            }
            catch (Exception ex)
            {
                log.Info(activityInstanceId.ToString() + "调用外部接口返回错误：" + ex.ToString());
            }
        }
        public void AddActivityAssist(string AssistUsers, Guid ActivityInstanceId, string ActivityName, WorkflowInstance workflowInstance) { }

      
    }
}
