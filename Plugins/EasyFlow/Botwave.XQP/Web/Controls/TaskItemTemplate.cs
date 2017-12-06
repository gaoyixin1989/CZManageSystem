using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Commons;
using Botwave.Web;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Web.Controls
{
    /// <summary>
    /// 任务列表项显示模板类.
    /// </summary>
    public class TaskItemTemplate : ITemplate
    {
        private string _cssClass;
        private IList<WorkflowField> _displayFields;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public TaskItemTemplate()
            : this(new List<WorkflowField>())
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="displayFields">显示的字段集合.</param>
        public TaskItemTemplate(IList<WorkflowField> displayFields)
        {
            this._displayFields = displayFields;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="cssClass">数据显示样式类.</param>
        public TaskItemTemplate(string cssClass)
            : this(new List<WorkflowField>(), cssClass)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="displayFields">显示的字段集合.</param>
        /// <param name="cssClass">数据显示样式类.</param>
        public TaskItemTemplate(IList<WorkflowField> displayFields, string cssClass)
        {
            this._displayFields = displayFields;
            this._cssClass = cssClass;
        }

        #region ITemplate Members

        /// <summary>
        /// 实现模板容器.
        /// </summary>
        /// <param name="container"></param>
        public void InstantiateIn(Control container)
        {
            HtmlTableRow row = new HtmlTableRow();
            row.DataBinding += new EventHandler(row_DataBinding);
            if (!string.IsNullOrEmpty(this._cssClass))
                row.Attributes["class"] = this._cssClass;
            container.Controls.Add(row);
        }

        private void row_DataBinding(object sender, EventArgs e)
        {
            HtmlTableRow row = sender as HtmlTableRow;
            string workflowAlias = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.WorkflowAlias"));
            string aliasImageUrl = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.AliasImageUrl"));
            string title = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.Title"));
            string sheetId = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.SheetId"));
            string activityInstanceId = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.ActivityInstanceId"));
            string activityName = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.ActivityName"));
            string creator = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.Creator"));
            string creatorName = DbUtils.ToString(DataBinder.Eval(row.NamingContainer, "DataItem.CreatorName"));
            //DateTime expectFinishTime = Convert.ToDateTime(DataBinder.Eval(row.NamingContainer, "DataItem.ExpectFinishedTime"));
            bool isReaded = !TodoInfo.IsUnReaded(Convert.ToInt32(DataBinder.Eval(row.NamingContainer, "DataItem.State")));

            int urgency = DbUtils.ToInt32(DataBinder.Eval(row.NamingContainer, "DataItem.Urgency"));
            int importance = DbUtils.ToInt32(DataBinder.Eval(row.NamingContainer, "DataItem.Importance"));
            int operateType = DbUtils.ToInt32(DataBinder.Eval(row.NamingContainer, "DataItem.OperateType")); // 等于 1 即为退还            
            // 流程别名
            HtmlTableCell cell = new HtmlTableCell();
            if (!string.IsNullOrEmpty(aliasImageUrl))
            {
                workflowAlias = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}\" />", workflowAlias, WebUtils.GetAppPath() + aliasImageUrl);
            }
            cell.InnerHtml = workflowAlias;
            row.Controls.Add(cell);

            // 工单标题(名称)
            cell = new HtmlTableCell();
            cell.Style["text-align"] = "left";
            cell.Style["font-weight"] = "bold";

            string titlePrefix = string.Empty;
            if (operateType == 1) // 退回
            {
                titlePrefix = titlePrefix + "<font color=\"red\">[退回]</font>";
            }
            if (importance > 0)
            {
                titlePrefix = titlePrefix + "<font color=\"red\">[重要]</font>";
            }
            if (urgency > 0)
            {
                titlePrefix = titlePrefix + "[紧急]";
            }
            title = string.Format("<span class=\"{0}\">{1}</span>", (isReaded ? "readed" : "unread"), title);
            cell.InnerHtml = string.Format("{0}<a href=\"process.aspx?aiid={1}\">{2}</a>", titlePrefix, activityInstanceId, title);
            row.Controls.Add(cell);

            // 工单号
            cell = new HtmlTableCell();
            cell.InnerHtml = string.Format("<a href=\"process.aspx?aiid={0}\">{1}</a>", activityInstanceId, sheetId);
            row.Controls.Add(cell);

            // 当前步骤
            cell = new HtmlTableCell();
            cell.InnerHtml = string.Format("<a href=\"process.aspx?aiid={0}\">{1}</a>", activityInstanceId, activityName);
            row.Controls.Add(cell);
            
            // 发起人
            cell = new HtmlTableCell();
            cell.InnerHtml = string.Format("<span tooltip=\"{0}\">{1}</span>", creator, creatorName);
            row.Controls.Add(cell);

            //// 期望完成时间
            //cell = new HtmlTableCell();
            //cell.InnerHtml = string.Format("{0:yyyy-MM-dd}", expectFinishTime); ;
            //row.Controls.Add(cell);

            // 自定义显示字段的数据子控件
            if (this._displayFields == null || this._displayFields.Count == 0)
                return;
            foreach (WorkflowField item in this._displayFields)
            {
                cell = new HtmlTableCell();
                object itemValue = DataBinder.Eval(row.NamingContainer, "DataItem." + item.FieldName);
                cell.InnerHtml = DbUtils.ToString(itemValue);
                row.Controls.Add(cell);
            }            
        }

        #endregion
    }
}
