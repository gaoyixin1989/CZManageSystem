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
using Botwave.XQP.Domain;

namespace Botwave.XQP.Web.Controls
{
    /// <summary>
    /// 任务列表表格顶部模板类.
    /// </summary>
    public class TaskHeaderTemplte : ITemplate
    {
        private IList<WorkflowField> _displayFields;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public TaskHeaderTemplte()
            : this(new List<WorkflowField>())
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="displayFields"></param>
        public TaskHeaderTemplte(IList<WorkflowField> displayFields)
        {
            this._displayFields = displayFields;
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
            container.Controls.Add(row);
        }

        private void row_DataBinding(object sender, EventArgs e)
        {
            HtmlTableRow row = sender as HtmlTableRow;
            row.Style.Add("word-break", "keep-all");
            row.Style.Add("word-wrap", "normal");

            // 流程别名.
            HtmlTableCell cell = new HtmlTableCell("th");
            cell.Style.Add("width", "8%");
            cell.InnerHtml = "类别";
            row.Controls.Add(cell);

            // 工单标题(名称).
            cell = new HtmlTableCell("th");
            cell.Style.Add("width", "220px");
            cell.InnerHtml = "标题";
            row.Controls.Add(cell);

            // 工单号.
            cell = new HtmlTableCell("th");
            cell.Style.Add("width", "10%");
            cell.InnerHtml = "受理号";
            row.Controls.Add(cell);

            // 当前步骤.
            cell = new HtmlTableCell("th");
            cell.InnerHtml = "当前步骤";
            row.Controls.Add(cell);

            // 发起人.
            cell = new HtmlTableCell("th");
            cell.InnerHtml = "发起人";
            row.Controls.Add(cell);

            // 期望完成时间
            //cell = new HtmlTableCell("th");
            //cell.Attributes.Add("width", "14%");
            //cell.InnerHtml = "期望完成时间";
            //row.Controls.Add(cell);

            // 自定义字段的列显示标题.
            if (this._displayFields == null || this._displayFields.Count == 0)
                return;
            foreach (WorkflowField item in this._displayFields)
            {
                cell = new HtmlTableCell("th");
                cell.InnerHtml = item.HeaderText; // 字段名称.
                row.Controls.Add(cell);
            }
        }

        #endregion
    }
}
