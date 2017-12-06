using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave;
using Botwave.Commons;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Domain;
using Botwave.DynamicForm.Services;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.ImportExport;

public partial class contrib_dynamicform_pages_Import : Botwave.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_dynamicform_pages_Import));

    private IWorkflowDefinitionService workflowDefinitionService = Spring.Context.Support.WebApplicationContext.Current["WorkflowDefinitionService"] as IWorkflowDefinitionService;
    private IFormDefinitionImporter formDefinitionImporter = Spring.Context.Support.WebApplicationContext.Current["FormDefinitionImporter"] as IFormDefinitionImporter;
    private IFormDefinitionService formDefinitionService = Spring.Context.Support.WebApplicationContext.Current["FormDefinitionService"] as IFormDefinitionService;

    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        set { workflowDefinitionService = value; }
    }

    public IFormDefinitionImporter FormDefinitionImporter
    {
        set { formDefinitionImporter = value; }
    }

    public IFormDefinitionService FormDefinitionService
    {
        set { formDefinitionService = value; }
    }

    public Guid WorkflowId
    {
        get { return ViewState["WorkflowId"] == null ? Guid.Empty : (Guid)ViewState["WorkflowId"]; }
        set { ViewState["WorkflowId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Guid workflowId = this.WorkflowId;
            WorkflowDefinition workflowDefinition = workflowDefinitionService.GetWorkflowDefinition(workflowId);
            if (null == workflowDefinition)
                ShowError(string.Format("未找到指定流程定义：{0}，请重试", workflowId));
            this.hiddenWorkflowID.Value = workflowId.ToString();
            string workflowName = workflowDefinition.WorkflowName;
            this.ltlTitle.Text = workflowName + " - ";
            this.txtName.Text = string.Format("{0}表单", workflowName);

            this.linkTemplate.NavigateUrl = string.Format("{0}apps/xqp2/pages/workflows/download.ashx?path=/App_Data/Templates/WorkflowFormTemplate.xls&displayName={1}",
                AppPath, HttpUtility.UrlEncode(workflowName + "导入表单模板"));
        }
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        if (!IsPostBack)
        {
            Guid? workflowId = DbUtils.ToGuid(Request["wid"]);
            if (!workflowId.HasValue)
            {
                ShowError(GlobalSettings.Instance.ArgumentExceptionMessage);
            }
            this.WorkflowId = workflowId.Value;
        }
    }

    private string GetSaveFileName(HttpPostedFile httpFile)
    {
        string fileName = httpFile.FileName;//获取上传文件的全路径.

        string tempFileName = string.Format("WorkflowForm_{0:yyyyMMddHHmmfff}{1}", DateTime.Now, System.IO.Path.GetExtension(fileName));

        string saveFileName = Path.Combine(Request.PhysicalApplicationPath + Botwave.GlobalSettings.Instance.TemporaryDir, tempFileName);//合并两个路径为上传到服务器上的全路径]
        return saveFileName.Replace("/", "\\");
    }

    protected bool Import(ref string path)
    {
        string fileName = this.fupload.FileName;
        if (string.IsNullOrEmpty(fileName))
        {
            ShowError("未选择要上传的表单模板导入文件，请重新选择。");
        }
        if (!(fileName.EndsWith(".xls") || fileName.EndsWith(".xlsx")) || this.fupload.PostedFile.ContentLength == 0)
        {
            ShowError("表单模板导入文件格式错误，请选择 Excel 文件（*.xls）导入。");
        }
        try
        {
            path = GetSaveFileName(this.fupload.PostedFile);
            this.fupload.SaveAs(path);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            ShowError("上传文件出错：" + ex.Message);
        }
        return true;
    }

    protected void buttonOK_Click(object sender, EventArgs e)
    {
        string path = null;
        Guid workflowId = DbUtils.ToGuid(this.hiddenWorkflowID.Value).Value;
        Import(ref path);
        if (string.IsNullOrEmpty(path))
        {
            ShowError("表单模板路径错误，请重试。");
        }
        string name = this.txtName.Text.Trim();

        FormDefinition item = new FormDefinition();
        item.Name = name;
        item.Version = Botwave.XQP.ImportExport.FormDefinitionImporter.GetFormVersion(name);
        item.IsCurrentVersion = this.chkboxIsCurrent.Checked;
        item.Comment = this.txtDescription.Text.Trim();
        item.Creator = item.LastModifier = Botwave.Security.LoginHelper.UserName;

        //创建表单.
        Guid newFormDefinitionId = formDefinitionService.SaveFormDefinition(item);

        Botwave.XQP.ImportExport.ActionResult result = formDefinitionImporter.Import(newFormDefinitionId, name, path);

        if (!result.Success)
        {
            ShowError("表单模板导入出错：" + result.Message);
        }
        // 更新流程表单版本.
        if (item.IsCurrentVersion)
        {
            WorkflowFormHelper.SetCurrentVersionRelation(workflowId, newFormDefinitionId);
        }

        ShowSuccess("导入表单模板成功。", string.Format("{0}contrib/dynamicform/pages/list.aspx?wid={1}", AppPath, workflowId));
    }
}
