using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;

public partial class apps_xqp2_pages_notices_controls_NoticeView : Botwave.Security.Web.UserControlBase// System.Web.UI.UserControl
{
    public string TitleRowCssClass = "";

    private INoticeService noticeService = (INoticeService)Ctx.GetObject("noticeService");

    public INoticeService NoticeService
    {
        get { return noticeService; }
        set { noticeService = value; }
    }

    #region properties
    /// <summary>
    /// 呈现公告数据之前的处理委托.
    /// </summary>
    /// <param name="entityId"></param>
    public delegate void PreRenderNoticeHandler(Notice sender);

    /// <summary>
    /// 在显示公告数据之前的处理事件.
    /// </summary>
    public event PreRenderNoticeHandler PreRenderNotice;

    private bool entityContentVisible = false;
    private string entityContentHeaderText = "所属实体：";

    /// <summary>
    /// 公告的实体内容是否可见(默认值为 false).
    /// </summary>
    public bool EntityContentVisible
    {
        get { return entityContentVisible; }
        set { entityContentVisible = value; }
    }

    /// <summary>
    /// 公告的实体的内容的标题文本.
    /// </summary>
    public string EntityContentHeaderText
    {
        get { return entityContentHeaderText; }
        set { entityContentHeaderText = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string noticeIdValue = Request.QueryString["noticeId"];
            this.trTitle.Attributes["class"] = "trClass";
            if (!string.IsNullOrEmpty(noticeIdValue))
            {
                int noticeId = Convert.ToInt32(noticeIdValue);
                this.BindData(noticeId);
            }
        }
    }

    /// <summary>
    /// 绑定数据.
    /// </summary>
    /// <param name="noticeId"></param>
    public void BindData(int noticeId)
    {
        if (noticeId <= 0)
            return;

        Notice item = noticeService.GetNotice(noticeId);
        this.LoadNotice(item);
    }

    /// <summary>
    /// 加载公告数据.
    /// </summary>
    /// <param name="item"></param>
    private void LoadNotice(Notice item)
    {
        if (item == null)
            return;

        this.OnPreRenderNotice(item);
        this.ltlTitle.Text = item.Title;
        this.LoadEntityContent(EntityContentHeaderText, item.EntityId);
        this.ltlStartTime.Text = (item.StartTime.HasValue ? string.Format("{0:yyyy-MM-dd}", item.StartTime) : "");
        this.ltlEndTime.Text = (item.EndTime.HasValue ? string.Format("{0:yyyy-MM-dd}", item.EndTime) : "");
        this.ltlCreator.Text = item.Creator;
        string content = item.Content;
        if (!string.IsNullOrEmpty(content))
            content = content.Replace(" ", "&nbsp;&nbsp;").Replace("\r\n", "<br />");
        this.ltlContent.Text = content;
    }

    private void LoadEntityContent(string headerText, string entityContent)
    {
        if (this.EntityContentVisible == false || string.IsNullOrEmpty(entityContent))
            return;
        this.trTitle.Attributes.Remove("class");
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<tr class=\"trClass\">");
        builder.AppendLine(string.Format("\t<th>{0}</th>", headerText));
        builder.AppendLine(string.Format("\t<td align=\"left\">{0}</td>", entityContent));
        builder.AppendLine("</tr>");

        this.ltlEntity.Text = builder.ToString();
    }

    /// <summary>
    /// 处理呈现公告之前的事件.
    /// </summary>
    /// <param name="sender"></param>
    protected virtual void OnPreRenderNotice(Notice sender)
    {
        if (this.PreRenderNotice != null)
        {
            this.PreRenderNotice.Invoke(sender);
        }
    }
}
