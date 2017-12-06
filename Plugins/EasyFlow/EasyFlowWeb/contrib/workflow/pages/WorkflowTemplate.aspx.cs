using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Service;

public partial class contrib_workflow_pages_WorkflowTemplate : Botwave.Security.Web.PageBase
{  
    #region service properties

    private IWorkflowFileService workflowFileService = (IWorkflowFileService)Ctx.GetObject("xqpWorkflowFileService");
    private IWorkflowAttachmentService workflowAttachmentService = (IWorkflowAttachmentService)Ctx.GetObject("xqpWorkflowAttachmentService");

    public IWorkflowFileService WorkflowFileService
    {
        get { return workflowFileService; }
        set { workflowFileService = value; }
    }

    public IWorkflowAttachmentService WorkflowAttachmentService
    {
        get { return workflowAttachmentService; }
        set { workflowAttachmentService = value; }
    }
    #endregion

    public Guid WorkflowId
    {
        get
        {
            if (null == ViewState["WorkflowId"])
                return Guid.Empty;
            return (Guid)(ViewState["WorkflowId"]);
        }
        set { ViewState["WorkflowId"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wid = Request.QueryString["wid"];
            if (!string.IsNullOrEmpty(wid))
                WorkflowId = new Guid(wid);

            LoadTemplate();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string creator = CurrentUserName;
        HttpPostedFile httpFile = this.fileUpload1.PostedFile;
        // 上传文件.
        string fileName = workflowFileService.UploadFile(httpFile);
        if (!string.IsNullOrEmpty(fileName))
        {
            Guid workflowId = this.WorkflowId;
            // 创建附件实体类.
            string attachmentId = workflowAttachmentService.CreateAttachment(httpFile, creator, fileName);
            if (!string.IsNullOrEmpty(attachmentId))
            {
                // 创建实体关系类.
                workflowAttachmentService.CreateAttachmentEntity(attachmentId, workflowId.ToString(), WorkflowUtility.EntityType_WorkflowTemplate);
            }
            LoadTemplate();
        }
        else
        {
            lblMsg.Visible = true;
            lblMsg.Text = "上传文件错误.";
        }
    }
    protected void rptTemplate_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string attachmentId = e.CommandArgument.ToString();
        if (e.CommandName == "Delete")
        {
            workflowAttachmentService.DeleteAttachment(attachmentId);
        }
        LoadTemplate();
    }
    private void LoadTemplate()
    {
        DataTable attachments = workflowAttachmentService.GetAttachmentsByEntity(this.WorkflowId.ToString(), WorkflowUtility.EntityType_WorkflowTemplate);
        if (attachments != null)
        {
            rptTemplate.DataSource = attachments;
            rptTemplate.DataBind();
        }
    }
}
