using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;

public partial class contrib_workflow_pages_WorkflowComment : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(contrib_workflow_pages_WorkflowComment));

    private DataTable attachementTable = null;

    #region service
    private ICommentService commentService = (ICommentService)Ctx.GetObject("commentService");
    private IWorkflowNotifyService workflowNotifyService = (IWorkflowNotifyService)Ctx.GetObject("workflowNotifyService");
    private IWorkflowUserService workflowUserService = (IWorkflowUserService)Ctx.GetObject("workflowUserService");
    private IWorkflowFileService workflowFileService = (IWorkflowFileService)Ctx.GetObject("xqpWorkflowFileService");
    private IWorkflowAttachmentService workflowAttachmentService = (IWorkflowAttachmentService)Ctx.GetObject("xqpWorkflowAttachmentService");

    public ICommentService CommentService
    {
        set { commentService = value; }
    }

    public IWorkflowNotifyService WorkflowNotifyService
    {
        set { workflowNotifyService = value; }
    }

    public IWorkflowUserService WorkflowUserService
    {
        set { workflowUserService = value; }
    }

    public IWorkflowAttachmentService WorkflowAttachmentService
    {
        set { workflowAttachmentService = value; }
    }

    public IWorkflowFileService WorkflowFileService
    {
        set { workflowFileService = value; }
    }

    #endregion

    #region properties

    /// <summary>
    /// 流程实例编号.
    /// </summary>
    public Guid WorkflowInstanceId
    {
        get { return (Guid)ViewState["WorkflowInstanceId"]; }
        set { ViewState["WorkflowInstanceId"] = value; }
    }

    /// <summary>
    /// 流程步骤实例编号.
    /// </summary>
    public Guid ActivityInstanceId
    {
        get { return (Guid)ViewState["ActivityInstanceId"]; }
        set { ViewState["ActivityInstanceId"] = value; }
    }

    /// <summary>
    /// 流程步骤实例执行人.
    /// </summary>
    public string ActivityActor
    {
        get { return (string)ViewState["ActivityActor"]; }
        set { ViewState["ActivityActor"] = value; }
    }

    /// <summary>
    /// 流程标题.
    /// </summary>
    public string WorkflowTitle
    {
        get { return (string)ViewState["WorkflowTitle"]; }
        set { ViewState["WorkflowTitle"] = value; }
    }

    /// <summary>
    /// 评论数.
    /// </summary>
    public int CommentCount
    {
        get { return (int)ViewState["CommentCount"]; }
        set { ViewState["CommentCount"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string wiid = this.Request.QueryString["wiid"];
            string aiid = this.Request.QueryString["aiid"];
            string actor = this.Request.QueryString["actor"];          // 处理人.
            string workflowTitle = this.Request.QueryString["t"];     // 流程实例标题.
            string hiddenInput = this.Request.QueryString["hid"];   // 隐藏发表评论.

            if (string.IsNullOrEmpty(wiid) || string.IsNullOrEmpty(aiid))
            {
                ShowError();
            }
            Guid workflowInstanceId = new Guid(wiid);
            Guid activityInstanceId = new Guid(aiid);

            this.CommentCount = 0;

            this.BindWorkflowRepeater(workflowInstanceId);

            this.WorkflowInstanceId = workflowInstanceId;
            this.ActivityInstanceId = activityInstanceId;
            if (!string.IsNullOrEmpty(actor))
            {
                this.ActivityActor = actor;
            }
            this.WorkflowTitle = string.IsNullOrEmpty(workflowTitle) ? "" : workflowTitle;

            //if (!string.IsNullOrEmpty(hiddenInput))
            //{
            //    this.divCommentInputs.Visible = false;
            //}
        }
    }

    #region 绑定数据

    /// <summary>
    /// 绑定流程实例评论.
    /// </summary>
    /// <param name="workflowInstanceId"></param>
    private void BindWorkflowRepeater(Guid workflowInstanceId)
    {
        IList<Comment> comments = commentService.GetWorkflowComments(workflowInstanceId);
        this.attachementTable = workflowAttachmentService.GetCommentAttaByWorkflowInstanceId(workflowInstanceId);

        if (comments.Count > 0)
        {
            this.CommentCount = comments.Count;
            this.rptComments.DataSource = comments;
            this.rptComments.DataBind();
        }
    }

    /// <summary>
    /// 绑定流程步骤实例评论.
    /// </summary>
    /// <param name="activityInstanceId"></param>
    private void BindActivityRepeater(Guid activityInstanceId)
    {
        IList<Comment> comments = commentService.GetActivityComments(activityInstanceId);
        this.attachementTable = workflowAttachmentService.GetCommentAttaByActivityInstanceId(activityInstanceId);

        if (comments.Count > 0)
        {
            this.CommentCount = comments.Count;
            this.rptComments.DataSource = comments;
            this.rptComments.DataBind();
        }
    }

    protected void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Comment item = e.Item.DataItem as Comment;

        Literal ltlMessage = e.Item.FindControl("ltlMessage") as Literal;
        ltlMessage.Text = EncodeHtml(item.Message);

        Literal ltlAttachemtns = e.Item.FindControl("ltlAttachments") as Literal;
        ltlAttachemtns.Text = GetAttachemntsHtml(item.Id.Value);
    }

    private string EncodeHtml(string content)
    {
        content = content.Replace(" ", "&nbsp;");
        content = content.Replace("\r\n", "<br />");
        return content;
    }

    private string GetAttachemntsHtml(Guid commentId)
    {
        if (attachementTable == null || attachementTable.Rows.Count == 0)
            return string.Empty;
        DataRow[] rows = attachementTable.Select(string.Format("EntityId = '{0}'", commentId));
        if (rows == null || rows.Length == 0)
            return string.Empty;

        StringBuilder builder = new StringBuilder();
        foreach (DataRow row in rows)
        {
            string id = row["Id"].ToString();
            string title = row["Title"].ToString() + row["MimeType"].ToString();

            builder.AppendFormat("<a class=\"ico_download\" href=\"download.ashx?id={0}\">{1}</a><br />", id, title);
        }
        return builder.ToString();
    }

    #endregion

    #region 发表评论

    protected void txtSave_Click(object sender, EventArgs e)
    {
        string creator = CurrentUser.UserName;
        string creatorRealName = CurrentUser.RealName;
        Guid workflowInstanceId = this.WorkflowInstanceId;
        Guid activityInstanceId = this.ActivityInstanceId;
        Guid commentId = Guid.NewGuid();
        string content = txtContent.Text;

        Comment item = new Comment();
        item.WorkflowInstanceId = workflowInstanceId;
        item.ActivityInstanceId = activityInstanceId;
        item.Id = commentId;
        item.Creator = creator;
        item.Message = content;

        // 添加评论
        commentService.AddComment(item);

        // 上传附件.
        this.HtmlInputUpload(commentId, creator);

        // 发送短信通知
        this.SendNotifyMessage(activityInstanceId, creator, creatorRealName, content);

        this.txtContent.Text = "";
        this.BindWorkflowRepeater(workflowInstanceId);
    }

    /// <summary>
    /// HTML 文件上传.
    /// </summary>
    protected void HtmlInputUpload(Guid commentId, string creator)
    {
        log.Warn("upload ...");
        try
        {
            HttpFileCollection files = this.Request.Files;
            if (files == null || files.Count == 0)
                return;
            foreach (string key in files.AllKeys)
            {
                HttpPostedFile file = files[key];

                // 上传附件到文件服务器
                string fileName = workflowFileService.UploadFile(file);
                // 保存附件信息到数据库
                string attachmentId = workflowAttachmentService.CreateAttachment(file, creator, fileName);
                // 保存评论附件关系
                workflowAttachmentService.CreateAttachmentEntity(attachmentId, commentId.ToString(), Comment.EntityType);

            }
        }
        catch (Exception ex)
        {
            log.Warn(ex);
        }
        log.Warn("upload completed.");
    }

    /// <summary>
    /// 评论的短信通知消息格式.
    /// </summary>
    static readonly string CommentMessageFormat = "潮州综合管理平台: 您的工单 {0} 现有 {1} 发表一条新评论:{2}";

    /// <summary>
    /// 发送短信提醒.
    /// </summary>
    /// <param name="activityInstanceId"></param>
    /// <param name="sender"></param>
    /// <param name="creatorRealName"></param>
    /// <param name="comment"></param>
    protected void SendNotifyMessage(Guid activityInstanceId, string sender, string creatorRealName, string comment)
    {
        string title = this.WorkflowTitle;
        string actorName = this.ActivityActor;
        comment = comment.Replace("\r\n", " ");

        if (!string.IsNullOrEmpty(actorName))
        {
            // 获取指定待办人，并发送短信通知
            ActorDetail actor = workflowUserService.GetActorDetail(actorName);
            if (actor != null)
            {
                this.SendSMS(actor.UserName, sender, activityInstanceId, title, creatorRealName, comment);
            }
        }
        else
        {
            // 获取指定流程步骤的待办人，并发送短信通知
            IList<NotifyActor> notifyActors = workflowNotifyService.GetNotifyActors(activityInstanceId);
            if (notifyActors.Count > 0)
            {
                foreach (NotifyActor item in notifyActors)
                {
                    this.SendSMS(item.UserName, sender, activityInstanceId, title, creatorRealName, comment);
                }
            }
        }
    }

    private void SendSMS(string receiver,  string sender, Guid activityInstanceId, string title, string senderRealName, string comment)
    {
        if (!string.IsNullOrEmpty(receiver))
        {
            string message = string.Format(CommentMessageFormat, title, senderRealName, comment);
            message = (message.Length > 255 ? (message.Substring(0, 222) + "..") : message);
            Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(receiver, sender, message, ActivityInstance.EntityType, activityInstanceId.ToString());
            //workflowNotifyService.SendSMS("", receiver, message);
        }
    }
    #endregion
}
