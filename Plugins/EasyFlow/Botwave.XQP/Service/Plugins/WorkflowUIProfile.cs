using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Extension.UI;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 流程界面个性化实现类.
    /// </summary>
    public class WorkflowUIProfile : IWorkflowUIProfile
    {
        #region IWorkflowUIProfile 成员

        public string BuildHandlerOpinionHtml(System.Web.HttpContext context, object user)
        {
            IList<WorkflowRemark> remarks = new List<WorkflowRemark>();
            Botwave.Security.LoginUser currentUser = user as Botwave.Security.LoginUser;
            if (currentUser != null && currentUser.Properties.ContainsKey(WorkflowRemark.WorkflowRemarkPropertyKey))
            {
                remarks = currentUser.Properties[WorkflowRemark.WorkflowRemarkPropertyKey] as IList<WorkflowRemark>;
            }
            else
            {
                remarks = WorkflowRemark.SelectByUserId(currentUser.UserId);
                currentUser.Properties[WorkflowRemark.WorkflowRemarkPropertyKey] = remarks;
            }

            string remarksHtml = string.Empty;
            int count = remarks.Count;
            for (int i = 0; i < count; i++)
            {
                string optionText = remarks[i].RemarkText;
                string optionValue = remarks[i].RemarkValue;
                if (string.IsNullOrEmpty(optionValue))
                    optionValue = optionText;

                if (optionText != "同意" || optionText != "不同意")
                {
                    remarksHtml += string.Format("<option value=\"{0}\">{1}</option>", optionValue, optionText);
                }
            }
            return remarksHtml;
        }

        #endregion
    }
}
