using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow;
using Botwave.Workflow.Service;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;

public partial class apps_xqp2_app_Edit : Botwave.Security.Web.PageBase
{
    #region field

    private static string ReturnUrl = AppPath + "apps/xqp2/pages/app/list.aspx";
    private static string AppAccessUrl = Botwave.GlobalSettings.Instance.Address + "ssoproxy.ashx?default.aspx?app=";
    //应用系统虚域名

    private readonly string appAccessVirtualDomain = "{%AppDomain%}";

    #endregion

    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public bool IsEdit
    {
        get { return (bool)ViewState["IsEdit"]; }
        set { ViewState["IsEdit"] = value; }
    }

    public int AppId
    {
        get { return (int)(ViewState["AppId"]); }
        set { ViewState["AppId"] = value; }
    }

    public string AppName
    {
        get { return (string)(ViewState["AppName"]); }
        set { ViewState["AppName"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadWorkflows();
            this.IsEdit = false;
            string appId = Request.QueryString["AppId"];
            if (!string.IsNullOrEmpty(appId))
            {
                // 编辑
                this.IsEdit = true;
                this.ltlTitle.Text = "编辑";

                int appIdValue = int.Parse(appId);
                if (appIdValue <= 0)
                    ShowError(Botwave.Web.MessageHelper.Message_ArgumentException, ReturnUrl);

                Apps app = Apps.LoadById(appIdValue);
                if (app == null)
                    ShowError("未找到指定接入应用系统.", ReturnUrl);

                this.AppId = appIdValue;
                this.LoadData(app);
            }
            else
            {
                // 新增
                txtAccessUrl.Text = AppAccessUrl;
            }
        }
    }

    #region 加载数据

    protected void LoadWorkflows()
    {
        IList<WorkflowDefinition> workflowList = workflowDefinitionService.GetWorkflowDefinitionList();
        cblWorkflows.DataSource = workflowList;
        cblWorkflows.DataTextField = "WorkflowName";
        cblWorkflows.DataBind();
    }

    protected void LoadData(Apps app)
    {
        string appName = app.AppName;
        this.AppName = appName;

        txtAppName.Text = appName;
        txtPassword.Text = app.Password;
        chkEnable.Checked = app.Enabled;
        txtRemark.Text = app.Remark;
        rblAccessType.SelectedValue = app.AccessType ? "1" : "0";
        txtAccessUrl.Text = app.AccessUrl.Replace(appAccessVirtualDomain, Botwave.GlobalSettings.Instance.Address);

        string[] workflowArray = app.Settings.ToString().Split('|');
        foreach (string workflowName in workflowArray)
        {
            if (string.IsNullOrEmpty(workflowName)) 
                continue;
            foreach (ListItem item in cblWorkflows.Items)
            {
                if (item.Text.Trim() == workflowName)
                {
                    item.Selected = true;
                    break;
                }
            }
        }
    }
    #endregion

    #region 保存数据

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (!IsValid) 
            return;
        string appName = txtAppName.Text.Trim();
        if (!appName.Equals(this.AppName,  StringComparison.OrdinalIgnoreCase) && Apps.IsExists(appName))
        {
            ltlNameMessage.Text = "* 应用系统名称已存在！";
            return;
        }

        Apps app = GetAppsData();
        if (IsEdit)
            this.OnEdit(app);
        else
            this.OnCreate(app);
    }

    private void OnCreate(Apps app)
    {
        app.Create();
        ShowSuccess("创建接入应用系统成功！", ReturnUrl);
    }

    private void OnEdit(Apps app)
    {
        app.AppId = this.AppId;
        if (app.Update() > 0)
            ShowSuccess("修改接入应用系统成功！", ReturnUrl);
        else
            ShowError("修改接入应用系统失败。", ReturnUrl);
    }
    protected void rblAccessType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAccessType.SelectedValue.Equals("0"))
        {
            txtAccessUrl.Text = Botwave.GlobalSettings.Instance.Address + "ssoproxy.ashx?workbench.aspx?app=" + txtAppName.Text.Trim();
        }
        else
        {
            txtAccessUrl.Text = Botwave.GlobalSettings.Instance.Address + "WS/*.asmx ";
        }
    }
    #endregion

    /// <summary>
    /// 获取输入的 Apps 数据.
    /// </summary>
    /// <returns></returns>
    private Apps GetAppsData()
    {
        Apps app = new Apps();
        app.AppName = txtAppName.Text.Trim();
        app.Password = txtPassword.Text.Trim();
        app.Enabled = chkEnable.Checked;
        app.Remark = txtRemark.Text.Trim();
        app.AccessType = (rblAccessType.SelectedValue == "1" ? true : false);
        string accessUrl = txtAccessUrl.Text;
        accessUrl = accessUrl.Replace(Botwave.GlobalSettings.Instance.Address, appAccessVirtualDomain);
        app.AccessUrl = accessUrl;

        string settings = string.Empty;
        foreach (ListItem item in cblWorkflows.Items)
        {
            if (item.Selected)
                settings += item.Text + "|";
        }
        if (!string.IsNullOrEmpty(settings))
            settings = settings.Substring(0, settings.Length - 1);
        app.Settings = settings;

        string userName = CurrentUserName;
        app.Creator = userName;
        app.LastModifier = userName;

        return app;
    }
}
