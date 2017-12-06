using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Util;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;
using System.IO;

public partial class contrib_workflow_controls_Attachments : Botwave.Security.Web.UserControlBase
{
    /// <summary>
    /// 附件类中的附件编号的列名.
    /// </summary>
    static readonly string ColumnName_AttachmentId = "Id";

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

    #region properties

    public bool Started
    {
        get { return (null == ViewState["Started"]) ? false : (bool)(ViewState["Started"]); }
        set { ViewState["Started"] = value; }
    }

    public Guid WorkflowInstanceId
    {
        get
        {
            if (null == ViewState["WorkflowInstanceId"])
                return Guid.Empty;
            return (Guid)(ViewState["WorkflowInstanceId"]);
        }
        set { ViewState["WorkflowInstanceId"] = value; }
    }

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

    /// <summary>
    /// 是否支持上传.
    /// </summary>
    public bool EnableUpload
    {
        get { return (ViewState["EnableUpload"] == null ? true : (bool)ViewState["EnableUpload"]); }
        set { ViewState["EnableUpload"] = value; }
    }
    #endregion

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.tableFile.Visible = this.EnableUpload;
        string wid = Request.QueryString["wid"];
        string wiid = Request.QueryString["wiid"];

        if ((string.IsNullOrEmpty(wid) || (wid == "null")) && (string.IsNullOrEmpty(wiid) || wiid == "null"))
        {
            // 无数据显示.
        }
        else
        {
            //Guid workflowId = (string.IsNullOrEmpty(wid) || (wid == "null")) ? Guid.Empty : new Guid(wid);
            //Guid workflowInstanceId = (string.IsNullOrEmpty(wiid) || (wiid == "null")) ? Guid.Empty : new Guid(wiid);
            Guid workflowId = (string.IsNullOrEmpty(wid) || (wid == "null")) ? Guid.Empty : new Guid(Server.HtmlEncode(wid));
            Guid workflowInstanceId = (string.IsNullOrEmpty(wiid) || (wiid == "null")) ? Guid.Empty : new Guid(Server.HtmlEncode(wiid));
            this.WorkflowId = workflowId;
            this.WorkflowInstanceId = workflowInstanceId;

            if (workflowId != Guid.Empty && workflowInstanceId != Guid.Empty)
            {
                //处理工单时流程ID及流程实例ID都不为空
                this.LoadData(workflowId, workflowInstanceId);
            }
            else if (workflowId != Guid.Empty)
            {
                //发起工单时，唯有流程ID
                this.LoadData(workflowId);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ClearInvalidAttachments();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string creator = CurrentUserName;

        HttpPostedFile httpFile = this.fileUpload1.PostedFile;
        // 上传文件.
        string fileName = null;
        try
        {
            FileInfo file = new FileInfo(httpFile.FileName);
            if (file.Extension == ".ext" || file.Extension == ".com" || file.Extension == ".bat" || file.Extension == ".dll")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "上传文件的类型不能为可执行文件!";
                return;
            }

            fileName = workflowFileService.UploadFile(httpFile);
        }
        catch (Exception ex)
        {
            Botwave.XQP.Commons.LogWriter.Write(creator, ex);
            lblMsg.Visible = true;
            lblMsg.Text = "上传文件错误." + ex.Message;
            return;
        }
        if (!string.IsNullOrEmpty(fileName))
        {
            Guid workflowInstanceId = this.WorkflowInstanceId;
            // 创建附件实体类.
            string attachmentId = workflowAttachmentService.CreateAttachment(httpFile, creator, fileName);
            if (!string.IsNullOrEmpty(attachmentId))
            {
                // 创建实体关系类.
                workflowAttachmentService.CreateAttachmentEntity(attachmentId, workflowInstanceId.ToString(), WorkflowUtility.EntityType_WorkflowAttachment);
            }
            LoadAttachment(workflowInstanceId);
        }
        else
        {
            lblMsg.Visible = true;
            lblMsg.Text = "上传文件错误.";
        }
    }
    protected void rptAttachment_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string attachmentId = e.CommandArgument.ToString();
        if (e.CommandName == "Delete")
        {
            workflowAttachmentService.DeleteAttachment(attachmentId);
        }
        LoadAttachment();
    }

    protected void rptAttachment_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HiddenField hdCreator = e.Item.FindControl("hdCreator") as HiddenField;
        LinkButton btnDelete = e.Item.FindControl("btnDelete") as LinkButton;
        Literal ltlAttachSource = e.Item.FindControl("ltlAttachSource") as Literal;
        string currentUser = CurrentUserName;

        DataRowView row = e.Item.DataItem as DataRowView;
        bool isRefAttachment = Botwave.Commons.DbUtils.ToBoolean(row["IsRef"]);
        //if (isRefAttachment || !String.Equals(hdCreator.Value, currentUser, StringComparison.OrdinalIgnoreCase))
        //{
        //    btnDelete.Visible = false;
        //}
        if (Request.RawUrl.ToLower().IndexOf("workflowview.aspx") > -1)
        {
            btnDelete.Visible = false;
        }
        else if (isRefAttachment || !String.Equals(hdCreator.Value, currentUser, StringComparison.OrdinalIgnoreCase))
        {
            btnDelete.Visible = false;
        }
        ltlAttachSource.Text = isRefAttachment ? "引用附件" : "用户上传";
        if (e.Item.FindControl("lblFileSite") != null)
        {
            Label lbl = (Label)e.Item.FindControl("lblFileSite");
            string fileSite = lbl.Text.Trim();
            fileSite = string.IsNullOrEmpty(fileSite) ? "0" : fileSite;
            long site = long.Parse(fileSite);
            string FactSize = "";
            if (site < 1024)
                FactSize = site.ToString("F2") + " Byte";
            else if (site >= 1024.00 && site < 1048576)
                FactSize = (site / 1024.00).ToString("F2") + " K";
            else if (site >= 1048576 && site < 1073741824)
                FactSize = (site / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (site >= 1073741824)
                FactSize = (site / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            lbl.Text = FactSize;
        }
    }

    /// <summary>
    /// 载入模板/附件信息(处理时用).
    /// </summary>
    /// <param name="instance"></param>
    public void LoadData(Guid workflowId, Guid workflowInstanceId)
    {
        this.WorkflowInstanceId = workflowInstanceId;
        LoadTemplate(workflowId);
        LoadAttachment(workflowInstanceId);
    }
    /// <summary>
    /// 载入模板/附件信息(查看时用).
    /// </summary>
    /// <param name="instance"></param>
    public void LoadData(WorkflowInstance instance)
    {
        Guid workflowInstanceId = instance.WorkflowInstanceId;
        this.WorkflowInstanceId = workflowInstanceId;
        if (instance.Creator.Equals(CurrentUserName))
            EnableUpload = true;
        LoadTemplate(instance.WorkflowId);
        LoadAttachment(workflowInstanceId);
    }

    /// <summary>
    /// 载入附件信息(发单时用).
    /// </summary>
    /// <param name="workflowId"></param>
    public void LoadData(Guid workflowId)
    {
        //先根据流程Id上传附件,提交完工单后更新Id
        this.WorkflowInstanceId = (CurrentUser != null ? CurrentUser.UserId : workflowId); ;
        this.Started = true;
        LoadTemplate(workflowId);
        LoadAttachment(workflowId);
    }

    /// <summary>
    /// 载入附件列表.
    /// </summary>
    protected void LoadAttachment()
    {
        this.LoadAttachment(this.WorkflowInstanceId);
    }

    /// <summary>
    /// 载入附件列表.
    /// </summary>
    /// <param name="workflowInstanceId"></param>
    protected void LoadAttachment(Guid workflowInstanceId)
    {
        DataTable attachments = CZWorkflowRelation.GetRelationAttachments(workflowInstanceId, CurrentUserName, this.Started);
        //workflowAttachmentService.GetAttachmentsByEntity(workflowInstanceId.ToString(), WorkflowUtility.EntityType_WorkflowAttachment);
        if (attachments != null)
        {
            rptAttachment.DataSource = attachments;
            rptAttachment.DataBind();
        }

        if (attachments != null && attachments.Rows.Count > 0)
            trAttHeader.Style.Remove("display");
        else
            trAttHeader.Style.Add("display", "none");
        
    }

    /// <summary>
    /// 载入模板列表.
    /// </summary>
    /// <param name="workflowId"></param>
    protected void LoadTemplate(Guid workflowId)
    {
        DataTable templates = workflowAttachmentService.GetAttachmentsByEntity(workflowId.ToString(), WorkflowUtility.EntityType_WorkflowTemplate);
        if (templates != null)
        {
            rptTemplate.DataSource = templates;
            rptTemplate.DataBind();
        }

        if (templates != null && templates.Rows.Count > 0)
            trTemplateHeader.Style.Add("display", "block");
    }

    /// <summary>
    /// 清除残留无效的附件.
    /// </summary>
    /// <param name="workflowId"></param>
    protected void ClearInvalidAttachments()
    {
        if (WorkflowId == Guid.Empty)
            return;

        string creator = CurrentUserName;
        Guid entityId = (null != CurrentUser ? CurrentUser.UserId : WorkflowId);
        DataTable attaTable = workflowAttachmentService.GetAttachmentsByEntity(entityId.ToString(), WorkflowUtility.EntityType_WorkflowAttachment, creator);
        if (attaTable != null)
        {
            foreach (DataRow row in attaTable.Rows)
            {
                if (row[ColumnName_AttachmentId] != null)
                {
                    string attachmentId = row[ColumnName_AttachmentId].ToString();
                    workflowAttachmentService.DeleteAttachment(attachmentId);
                    workflowAttachmentService.DeleteAttachmentEntity(attachmentId, entityId.ToString(), WorkflowUtility.EntityType_WorkflowAttachment);
                }
            }
        }
    }
}
