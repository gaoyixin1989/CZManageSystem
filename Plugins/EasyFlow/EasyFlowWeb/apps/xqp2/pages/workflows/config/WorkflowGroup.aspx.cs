using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;

public partial class apps_xqp2_pages_workflows_config_WorkflowGroup : Botwave.Web.PageBase
{
    private static readonly string ReturnUrl = AppPath + "apps/xqp2/pages/workflows/config/workflowGroup.aspx";
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_config_WorkflowGroup));

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindMenuGroup();
            BindFlowInMenuGroup();
        }
    }
    protected void BindMenuGroup()
    {
        gvMenuGroup.DataSource = WorkflowMenuGroup.Select();
        gvMenuGroup.DataBind();
    }

    private void BindFlowInMenuGroup()
    {
        gvFlowInMenuGroup.DataSource = WorkflowInMenuGroup.Select();
        gvFlowInMenuGroup.DataBind();
    }

    protected void gvMenuGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvMenuGroup.EditIndex = e.NewEditIndex;
        BindMenuGroup();
    }

    protected void gvMenuGroup_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtGroupName = (TextBox)gvMenuGroup.Rows[e.RowIndex].Cells[0].Controls[0];
        TextBox txtShowOrder = (TextBox)gvMenuGroup.Rows[e.RowIndex].Cells[1].Controls[0];
        int groupID = Convert.ToInt32(this.gvMenuGroup.DataKeys[e.RowIndex].Value.ToString());

        WorkflowMenuGroup wm = new WorkflowMenuGroup();
        wm.GroupID = groupID;
        wm.GroupName = txtGroupName.Text.Trim();
        wm.ShowOrder = Convert.ToInt32(txtShowOrder.Text.Trim());
        wm.Update();

        gvMenuGroup.EditIndex = -1;
        BindMenuGroup();
    }
    protected void gvMenuGroup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvMenuGroup.EditIndex = -1;
        BindMenuGroup();
    }
    protected void btnAddGroup_Click(object sender, EventArgs e)
    {
        WorkflowMenuGroup wm = new WorkflowMenuGroup();
        wm.GroupName = txtGroupName.Text.Trim();
        string order = txtShowOrder.Text.Trim();
        wm.ShowOrder = order.Length > 0 ? Convert.ToInt32(order) : 1;

        if (!WorkflowMenuGroup.IsExists(wm.GroupName))
            wm.Create();
        else
            ShowError("对不起，该分组名称已存在！", ReturnUrl);

        Response.Redirect(ReturnUrl);
    }
    protected void gvMenuGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int groupID = Convert.ToInt32(this.gvMenuGroup.DataKeys[e.RowIndex].Value.ToString());
        WorkflowMenuGroup wm = new WorkflowMenuGroup();
        wm.GroupID = groupID;

        wm.Delete();
        BindMenuGroup();
    }
    protected void gvFlowInMenuGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            DropDownList ddlGroupName = (DropDownList)e.Row.FindControl("ddlGroupName");
            ddlGroupName.DataSource = WorkflowMenuGroup.Select();
            ddlGroupName.DataBind();

            ddlGroupName.Items.Insert(0, new ListItem("未设置", "0"));

            Label lblGroupID = (Label)e.Row.FindControl("lblGroupID");
            ddlGroupName.SelectedValue = lblGroupID.Text;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gvFlowInMenuGroup.Rows)
            {
                DropDownList ddlGroupName = (DropDownList)row.FindControl("ddlGroupName");
                int groupID = Convert.ToInt32(ddlGroupName.SelectedValue);
                string workflowName = gvFlowInMenuGroup.DataKeys[row.RowIndex].Value.ToString();
                int showOrder = Convert.ToInt32(((TextBox)row.FindControl("txtShowOrder")).Text.Trim());

                WorkflowInMenuGroup wim = new WorkflowInMenuGroup();
                wim.MenuGroupId = groupID;
                wim.ShowOrder = showOrder;
                wim.WorkflowName = workflowName;

                if (WorkflowInMenuGroup.IsExists(workflowName))
                    wim.Update();
                else
                    wim.Create();
            }

            ShowSuccess("保存成功!", ReturnUrl);
        }
        catch (Exception ex)
        {
            log.Info(ex);
        }
    }
}
