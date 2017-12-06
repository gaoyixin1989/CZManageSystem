using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;

namespace Botwave.Workflow.Extension.WebServices
{
    /// <summary>
    /// 流程 AJAX Web Service 类.
    /// </summary>
    [WebService(Namespace = "Botwave.Workflow.Extension.WebServices.WorkflowAjaxService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class WorkflowAjaxService : System.Web.Services.WebService
    {
        #region properties

        private IWorkflowService workflowService;
        private IWorkflowUserService workflowUserService;

        /// <summary>
        /// 流程服务.
        /// </summary>
        public IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }

        /// <summary>
        /// 流程用户服务.
        /// </summary>
        public IWorkflowUserService WorkflowUserService
        {
            get { return workflowUserService; }
            set { workflowUserService = value; }
        }

        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowAjaxService()
        {
            this.workflowService = Spring.Context.Support.WebApplicationContext.Current[WorkflowUtility.Object_WorkflowService] as IWorkflowService;
            this.workflowUserService = Spring.Context.Support.WebApplicationContext.Current[WorkflowUtility.Object_WorkflowUserService] as IWorkflowUserService;
        }

        /// <summary>
        /// 获取指定用户名的用户工作负荷实例.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [WebMethod]
        public TooltipInfo GetActorTooltipInfo(string userName)
        {
            TooltipInfo item = workflowUserService.GetActorTooltip(userName);
            item.DpFullName = string.IsNullOrEmpty(item.DpFullName) ? "无" : item.DpFullName;
            item.Email = string.IsNullOrEmpty(item.Email) ? "无" : item.Email;
            item.Tel = string.IsNullOrEmpty(item.Tel) ? "无" : item.Tel;
            item.Mobile = string.IsNullOrEmpty(item.Mobile) ? "无" : item.Mobile;

            return item;
        }

        /// <summary>
        /// 获取指定匹配流程标题的流程自动完成列表.
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] GetCompletionWorkflowTitles(string prefixText, int count)
        {
            IList<WorkflowInstance> workflows = workflowService.GetWorkflowInstances(prefixText);
            int flowCount = workflows.Count;
            string[] titles = new string[flowCount];
            for (int i = 0; i < flowCount; i++)
            {
                titles[i] = workflows[i].Title;
            }
            return titles;
        }

        /// <summary>
        /// 获取指定匹配用户名或者真实姓名的用户自动完成列表.
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] GetCompletionUserNames(string prefixText, int count)
        {
            IList<string> results = workflowUserService.GetActorsLikeName(prefixText, count);
            int resultCount = results.Count;
            string[] names = new string[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                names[i] = results[i];
            }
            return names;
        }
    }
}
