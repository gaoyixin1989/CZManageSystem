using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;

public partial class apps_xqp2_workflows_controls_IntelligentRemindInput : System.Web.UI.UserControl
{
    /// <summary>
    /// 标题.
    /// </summary>
    public string ControlTitle
    {
        set { this.ltlTitle.Text = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 绑定智能提醒数据.
    /// </summary>
    /// <param name="dataItem"></param>
    public void BindData(IntelligentRemindControl dataItem)
    {
        if (dataItem != null)
        {
            this.txtStayHours.Text = dataItem.StayHours.ToString();
            this.txtRemindTimes.Text = dataItem.RemindTimes.ToString();
            string remindType = dataItem.RemindType;
            if (remindType != null)
            {
                remindType = remindType.Trim();
            }
            this.ddlRemindType.SelectedValue = remindType;
        }
    }

    /// <summary>
    /// 获取智能提醒控制对象.
    /// </summary>
    /// <param name="workflowName"></param>
    /// <param name="activityName"></param>
    /// <param name="urgency"></param>
    /// <param name="creator"></param>
    /// <returns></returns>
    public IntelligentRemindControl GetData(string workflowName, string activityName, int urgency, string creator)
    {
        IntelligentRemindControl item = new IntelligentRemindControl();
        item.WorkflowName = workflowName;
        item.ActivityName = activityName;
        item.Urgency = urgency;
        item.Creator = creator;

        item.StayHours = ToInt32(this.txtStayHours.Text.Trim());
        item.RemindTimes = ToInt32(this.txtRemindTimes.Text.Trim());
        item.RemindType = this.ddlRemindType.SelectedValue.Trim();
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
