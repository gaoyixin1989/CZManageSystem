using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_controls_WorkflowCreationControl : System.Web.UI.UserControl
{
    /// <summary>
    /// 标题，只读.
    /// </summary>
    public string ControlTitle
    {
        set { this.ltlTitle.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 绑定流程发单控制数据.
    /// </summary>
    /// <param name="dataItem"></param>
    public void BindData(WorkflowCreationControl dataItem)
    {
        if (dataItem != null)
        {
            this.txtMonthCount.Text = dataItem.MaxCreationInMonth.ToString();
            this.txtWeekCount.Text = dataItem.MaxCreationInWeek.ToString();
            string creationControlType = dataItem.CreationControlType;
            if (creationControlType != null)
                creationControlType = creationControlType.Trim();
            this.ddlCreationTypes.SelectedValue = creationControlType;
        }
    }

    /// <summary>
    /// 获取流程发单控制对象.
    /// </summary>
    /// <param name="workflowName"></param>
    /// <param name="urgency"></param>
    /// <returns></returns>
    public WorkflowCreationControl GetData(string workflowName, int urgency)
    {
        WorkflowCreationControl item = new WorkflowCreationControl();
        item.WorkflowName = workflowName;
        item.Urgency = urgency;

        item.CreationControlType = this.ddlCreationTypes.SelectedValue.Trim();
        item.MaxCreationInMonth = ToInt32(this.txtMonthCount.Text.Trim());
        item.MaxCreationInWeek = ToInt32(this.txtWeekCount.Text.Trim());

        return item;
    }

    /// <summary>
    /// 转换为整型.
    /// </summary>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    private static int ToInt32(string inputValue)
    {
        if (string.IsNullOrEmpty(inputValue))
            return -1;
        int result = -1;
        if (int.TryParse(inputValue, out result))
            return result;
        return -1;
    }
}
