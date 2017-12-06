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

public partial class apps_xqp2_pages_workflows_maintenance_WorkflowComment : Botwave.Security.Web.PageBase
{
    private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(apps_xqp2_pages_workflows_maintenance_WorkflowComment));

    private DataTable attachementTable = null;
    private static IList<Comment> comments;

    #region service
    private ICommentService commentService;
    private IWorkflowNotifyService workflowNotifyService;
    private IWorkflowUserService workflowUserService;
    private IWorkflowAttachmentService workflowAttachmentService;
    private IWorkflowFileService workflowFileService;

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
        comments = commentService.GetWorkflowComments(workflowInstanceId);
        this.attachementTable = workflowAttachmentService.GetCommentAttaByWorkflowInstanceId(workflowInstanceId);

        if (comments.Count > 0)
        {
            txtSave.Visible = true;
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
        comments = commentService.GetActivityComments(activityInstanceId);
        this.attachementTable = workflowAttachmentService.GetCommentAttaByActivityInstanceId(activityInstanceId);

        if (comments.Count > 0)
        {
            txtSave.Visible = true;
            this.CommentCount = comments.Count;
            this.rptComments.DataSource = comments;
            this.rptComments.DataBind();
        }
    }

    protected void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Comment item = e.Item.DataItem as Comment;

        TextBox ltlMessage = e.Item.FindControl("ltlMessage") as TextBox;
        ltlMessage.Text = EncodeHtml(item.Message);

        Literal ltlAttachemtns = e.Item.FindControl("ltlAttachments") as Literal;
        Guid _id = item.Id.Value;
        ltlAttachemtns.Text = GetAttachemntsHtml(_id);
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
        Guid workflowInstanceId = this.WorkflowInstanceId;
        IList<Comment> newcomments = new List<Comment>();
        for (int i = 0; i < comments.Count;i++ )
        {
            string index = i < 10 ? "0" + i.ToString() : i.ToString();
            Comment newitem = new Comment();
            newitem.Id = comments[i].Id;
            newitem.Message = Request.Form["rptComments$ctl" + index + "$ltlMessage"];
            newcomments.Add(newitem);
        }
        //item.Message = content;

        // 添加评论
        //commentService.UpdateComment(newcomments);

        this.BindWorkflowRepeater(workflowInstanceId);
    }

    /// <summary>
    /// HTML 文件上传.
    /// </summary>
    /*protected void HtmlInputUpload(Guid commentId, string creator)
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
    }*/

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
