using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Easyflow.API.Entity;

namespace Botwave.XQP.API.Interface
{
    public interface IWorkflowAPIService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        string LoginWorkflow(string userName,string passWord);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        string SearchWorkflow(SearchEntity search);

        string SearchWorkflow(string workflowProperties, IDictionary<string, object> formVariables);

        /// <summary>
        /// 处理
        /// </summary>
        /// <param name="manage"></param>
        /// <returns></returns>
        string ManageWorkflow(ManageEntity manage);

        /// <summary>
        ///嵌入页面保存判断
        /// </summary>
        /// <param name="workflowinstanceid">工单ID</param>
        /// <param name="page">页面</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        string SaveAs(string workflowinstanceid, string page, int state);
    }
}
