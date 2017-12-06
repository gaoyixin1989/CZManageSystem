using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.XQP.Domain;

public partial class apps_czmcc_pages_WorkflowInterfaceEdit : Botwave.Web.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindSortOrders(CZWorkflowInterface.Count());
            string id = Request.QueryString["id"];
            string actionType = Request.QueryString["action"];
            if (!string.IsNullOrEmpty(id))
            {
                int i = DbUtils.ToInt32(id);
                if (actionType == "delete")
                {
                    CZWorkflowInterface.Delete(i);
                    ShowSuccess("删除外部接入流程成功。");
                }
                else
                {
                    Load(id, CZWorkflowInterface.Get(i));
                }
            }
        }
    }

    protected void BindSortOrders(int count)
    {
        count = count < 0 ? 1 : count + 1;
        for (int i = 0; i < count; i++)
        {
            this.ddlSortOrders.Items.Add(i.ToString());
        }
    }

    protected void Load(string id, CZWorkflowInterface item)
    {
        if (item == null)
            ShowError(string.Format("未找到外部接入流程：{0}。", id));
        this.hiddenItemID.Value = item.Id.ToString();
        this.txtName.Text = item.Name;
        this.txtUrl.Text = item.Url;
        this.txtDescription.Text = item.Description;
        this.chkboxStatus.Checked = item.Status == 1;
        this.ddlSortOrders.SelectedValue = item.SortOrder.ToString();
    }

    protected void buttonOK_Click(object sender, EventArgs e)
    {
        int id = DbUtils.ToInt32(this.hiddenItemID.Value.Trim());
        string name = this.txtName.Text.Trim();
        string url = this.txtUrl.Text.Trim();
        string description = this.txtDescription.Text.Trim();
        int sortOrder = DbUtils.ToInt32(this.ddlSortOrders.SelectedValue);

        string creator = Botwave.Security.LoginHelper.UserName;

        CZWorkflowInterface item = new CZWorkflowInterface(id, name, url, description, sortOrder, this.chkboxStatus.Checked ? 1 : 0, creator);
        item.Update();

        ShowSuccess((id > 0 ? "更新" : "新增") + "外部流程接入成功。");
    }
}
