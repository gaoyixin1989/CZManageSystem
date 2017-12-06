using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension;
using Botwave.Workflow.Extension.Service;

public partial class contrib_workflow_pages_WorkflowDeploy : Botwave.Security.Web.PageBase
{
    private static string ReturnUrl = AppPath + "contrib/workflow/pages/workflowDeploy.aspx";
    private static string AliasImageFormat = AppPath + "contrib/workflow/res/groups/flow_{0}.gif";

    #region service interfaces

    private IDeployService deployService = (IDeployService)Ctx.GetObject("deployService");
    private IWorkflowDefinitionService workflowDefinitionService = (IWorkflowDefinitionService)Ctx.GetObject("workflowDefinitionService");
    private IWorkflowResourceService workflowResourceService = (IWorkflowResourceService)Ctx.GetObject("workflowResourceService");

    public IDeployService DeployService
    {
        get { return deployService; }
        set { deployService = value; }
    }
    public IWorkflowDefinitionService WorkflowDefinitionService
    {
        get { return workflowDefinitionService; }
        set { workflowDefinitionService = value; }
    }

    public IWorkflowResourceService WorkflowResourceService
    {
        get { return workflowResourceService; }
        set { workflowResourceService = value; }
    }
    #endregion

    private Botwave.Workflow.ActionResult acitonResult;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        IList<WorkflowDefinition> list = workflowDefinitionService.GetAllWorkflowDefinition();
        rpFlowList.DataSource = list;
        rpFlowList.DataBind();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        HttpPostedFile httpFile = this.fuAttachment.PostedFile;
        // application/x-xml, text/xml
        if (!(httpFile.ContentType == "text/xml" || httpFile.ContentType == "application/x-xml"))
        {
            lblInfo.Text = "只允许上传 <font color=red>.xml</font> 格式的文件，请重新选择！";
            return;
        }
        if (httpFile.ContentLength <= 0)
        {
            lblInfo.Text = "文件太小，请重新选择！";
            return;
        }
        string saveFileName = GetSaveFileName(httpFile);
        this.fuAttachment.SaveAs(saveFileName);

        acitonResult = new Botwave.Workflow.ActionResult();
        XmlReader xmlReader = XmlReader.Create(saveFileName);
        if (xmlReader == null)
            throw new Exception("未正确创建 XML 输入流，请检查文件.");
        acitonResult = deployService.CheckWorkflow(xmlReader);
        if (acitonResult.Success)
        {
            using (xmlReader = XmlReader.Create(saveFileName))
            {
                acitonResult = deployService.DeployWorkflow(xmlReader, CurrentUser.UserName);
                if (xmlReader.ReadState != ReadState.Closed)
                    xmlReader.Close();
            }
            if (acitonResult.Success)
            {
                if (File.Exists(saveFileName))
                    File.Delete(saveFileName);
                //Logger.Info(acitonResult.Message);
                //WriteNomalLog("部署流程", "部署流程成功");
                ShowSuccess(acitonResult.Message, ReturnUrl);
            }
        }

        lblInfo.Text = acitonResult.Message;
    }

    /// <summary>
    /// 获取上传文件保存的路径.
    /// </summary>
    /// <param name="httpFile"></param>
    /// <returns></returns>
    private string GetSaveFileName(HttpPostedFile httpFile)
    {
        string fileName = httpFile.FileName;//获取上传文件的全路径
        string extensionName = System.IO.Path.GetExtension(fileName);//获取扩展名


        string tempFileName = DateTime.Now.ToString("yyyyMMddHHmmfff") + extensionName;

        string saveFileName = Path.Combine(Request.PhysicalApplicationPath + Botwave.GlobalSettings.Instance.TemporaryDir, tempFileName);//合并两个路径为上传到服务器上的全路径]
        return saveFileName.Replace("/", "\\");
    }

    protected void rpFlowList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        WorkflowDefinition item = e.Item.DataItem as WorkflowDefinition;
        LinkButton btnSetEnable = e.Item.FindControl("btnSetEnable") as LinkButton;
        if (!item.Enabled)
        {
            btnSetEnable.Text = "启用";
            btnSetEnable.CssClass = "ico_enable";
        }
        else
        {
            btnSetEnable.OnClientClick = "return confirm('确定要停用流程?');";
        }

        if (!String.IsNullOrEmpty(item.WorkflowAlias))
        {
            string workflowAlias = item.WorkflowAlias;
            string aliasImageUrl = String.Format(AliasImageFormat, workflowAlias.ToLower());
            if (!string.IsNullOrEmpty(aliasImageUrl))
            {
                Literal ltlWorkflowAlias = e.Item.FindControl("ltlWorkflowAlias") as Literal;
                ltlWorkflowAlias.Text = string.Format("<img alt=\"{0}\" class=\"groupImage\" src=\"{1}\" />", workflowAlias, aliasImageUrl);
            }
        }
    }

    protected void rpFlowList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Guid workflowId = new Guid(e.CommandArgument.ToString());
        if (e.CommandName == "Export")
        {
            ExportFlow(workflowId);
        }
        else if (e.CommandName == "Delete")
        {
            acitonResult = deployService.DeleteWorkflow(workflowId);
            if (!acitonResult.Success)
            {
                //WriteExLog("删除流程", "删除流程失败");
                ShowError(acitonResult.Message, ReturnUrl);
            }
            else
            {
                this.UpdateResourceVisible(workflowId, false);
                //WriteNomalLog("删除流程", "删除流程成功");
                BindData();
            }
        }
        else if (e.CommandName == "SetEnabled")
        {
            LinkButton btnSetEnable = e.Item.FindControl("btnSetEnable") as LinkButton;
            bool enabled = (btnSetEnable.Text == "启用");

            workflowDefinitionService.UpdateWorkflowEnabled(workflowId, enabled);
            this.UpdateResourceVisible(workflowId, enabled);

            BindData();
        }
    }
    private void ExportFlow(Guid flowId)
    {
        acitonResult = new Botwave.Workflow.ActionResult();
        string fileName = DateTime.Now.ToString("yyyyMMddHHmm") + DateTime.Now.Ticks + ".xml";
        string xmlFile = Path.Combine(Request.PhysicalApplicationPath + Botwave.GlobalSettings.Instance.TemporaryDir, fileName);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml("<root></root>");
        doc.Save(xmlFile);

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.Encoding = System.Text.Encoding.UTF8;
        XmlWriter writer = XmlWriter.Create(xmlFile, settings);
        acitonResult = deployService.ExportWorkflow(writer, flowId);
        if (acitonResult.Success)
        {
            WorkflowDefinition wd = workflowDefinitionService.GetWorkflowDefinition(flowId);
            Response.Redirect(string.Format("download.ashx?path=Temp/{0}&displayName={1}", fileName, HttpUtility.UrlDecode(wd.WorkflowName)));
        }
        else
            lblInfo.Text = acitonResult.Message;
    }

    private void UpdateResourceVisible(Guid workflowId, bool isVisible)
    {
        workflowResourceService.UpdateWorkflowResourceVisible(workflowId, isVisible);
    }
}
