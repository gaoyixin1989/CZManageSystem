using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Spring.Context.Support;
using Botwave.Web.Controls.ExtendedSiteMap;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 动态网站路径处理类.
    /// </summary>
    public class DynamicSitePathHandler : IDynamicSitePathHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DynamicSitePathHandler));
        private static Hashtable _workflowCache = new Hashtable();
        private static IWorkflowDefinitionService workflowDefinitionService;
        private IActivityService activityService;

        public static string GetWorkflowName(Guid workflowId)
        {
            if (_workflowCache.ContainsKey(workflowId))
                return _workflowCache[workflowId].ToString();

            if (workflowDefinitionService == null)
                workflowDefinitionService = WebApplicationContext.Current["workflowDefinitionService"] as IWorkflowDefinitionService;
            if (workflowDefinitionService == null)
                return null;
            WorkflowDefinition _workflow = workflowDefinitionService.GetWorkflowDefinition(workflowId);
            if (_workflow != null)
            {
                string workflowName = _workflow.WorkflowName;
                // 移除重复流程名称.
                foreach (object key in _workflowCache.Keys)
                {
                    if (workflowName.Equals(_workflowCache[key].ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        _workflowCache.Remove(key);
                        break;
                    }
                }
                _workflowCache.Add(workflowId, workflowName);
                return workflowName;
            }
            return null;
        }

        #region IDynamicSitePathHandler 成员

        public void Handle(string url, NameValueCollection parameters, IList<PathNode> pathNodes)
        {
            try
            {
                if (parameters["wid"] != null || parameters["wfid"] != null)
                {
                    Guid workflowId = Guid.Empty;
                    if (parameters["wid"] != null)
                        workflowId = new Guid(parameters["wid"]);
                    else if (parameters["wfid"] != null)
                        workflowId = new Guid(parameters["wfid"]);

                    string workflowName = GetWorkflowName(workflowId);
                    if (!string.IsNullOrEmpty(workflowName))
                    {
                        string _workflowId = workflowId.ToString();
                        string redirectUrl = string.Format("~/workflow/workflowindex.aspx?wid={0}", _workflowId);
                        pathNodes.Add(new PathNode(redirectUrl, workflowName));

                        url = url.ToLower();
                        if (url.Contains("start.aspx"))
                            pathNodes.Add(new PathNode("~/workflow/start.aspx?wid=" + _workflowId, "发起工单"));
                    }
                }
                else if (parameters["aiid"] != null)
                {
                    if (activityService == null)
                        activityService = WebApplicationContext.Current["activityService"] as IActivityService;
                    if (activityService == null)
                        return;
                    Guid activityInstanceId = new Guid(parameters["aiid"]);
                    ActivityInstance _activity = activityService.GetActivity(activityInstanceId);
                    if (_activity != null)
                    {
                        string redirectUrl = string.Format("{0}?aiid={1}", url, activityInstanceId.ToString());
                        if (!string.IsNullOrEmpty(_activity.WorkItemTitle))
                        {
                            pathNodes.Add(new PathNode(redirectUrl, _activity.WorkItemTitle));
                        }
                        if (!string.IsNullOrEmpty(_activity.ActivityName))
                        {
                            pathNodes.Add(new PathNode(redirectUrl, _activity.ActivityName));
                        }
                    }
                }
                else if (parameters["fdid"] != null) // 表单定义.
                {

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion
    }
}
