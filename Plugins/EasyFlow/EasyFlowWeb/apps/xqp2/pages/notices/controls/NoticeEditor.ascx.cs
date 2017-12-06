using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;

public partial class apps_xqp2_pages_notices_controls_NoticeEditor : Botwave.Security.Web.UserControlBase
{
    /// <summary>
    /// 实体下拉列表数据绑定委托.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="arg"></param>
    public delegate void EntityDataBindHandler(DropDownList container, EventArgs arg);

    private INoticeService noticeService = (INoticeService)Ctx.GetObject("noticeService");

    public INoticeService NoticeService
    {
        get { return noticeService; }
        set { noticeService = value; }
    }
    #region properties

    /// <summary>
    /// 实体下拉列表数据绑定事件类(用于 Init、PreLoad 事件，即用于 Load 事件之前).
    /// </summary>
    public event EntityDataBindHandler EntityDataBind;

    /// <summary>
    /// 公告编号.
    /// </summary>
    public int NoticeId
    {
        get { return (int)ViewState["NoticeId"]; }
        set { ViewState["NoticeId"] = value; }
    }

    /// <summary>
    /// 是否属于编辑状态.
    /// </summary>
    public bool IsEdit
    {
        get { return (bool)ViewState["IsEdit"]; }
        set { ViewState["IsEdit"] = value; }
    }

    /// <summary>
    /// 关联的实体类型.
    /// </summary>
    public string EntityType
    {
        get { return (string)ViewState["EntityType"]; }
        set { ViewState["EntityType"] = value; }
    }

    /// <summary>
    /// 关联的实体编号.
    /// </summary>
    public string EntityId
    {
        get { return (string)ViewState["EntityId"]; }
        set { ViewState["EntityId"] = value; }
    }

    /// <summary>
    /// 当前操作用户.
    /// </summary>
    public string UserName
    {
        get { return (string)ViewState["UserName"]; }
        set { ViewState["UserName"] = value; }
    }

    /// <summary>
    /// 返回 URL.
    /// </summary>
    public string ReturnUrl
    {
        get { return (string)ViewState["ReturnUrl"]; }
        set { ViewState["ReturnUrl"] = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (string.IsNullOrEmpty(EntityType))
                this.EntityType = "notice";
            if (string.IsNullOrEmpty(this.ReturnUrl))
                this.ReturnUrl = AppPath + "contrib/cms/pages/notice/notices.aspx";

            this.UserName = CurrentUserName;
            this.IsEdit = false;
            this.OnEntityDataBind(this.ddlEntities, e);

            string noticeIdValue = Request.QueryString["noticeId"];
            string action = Request.QueryString["action"];

            if (string.IsNullOrEmpty(noticeIdValue))
            {
                // 新增
                this.NoticeId = -1;
            }
            else
            {
                int noticeId = Convert.ToInt32(noticeIdValue);
                this.NoticeId = noticeId;

                // 删除
                if (!string.IsNullOrEmpty(action) && action.Equals("delete", StringComparison.OrdinalIgnoreCase))
                {
                    if (noticeService.DeleteNotice(noticeId) > 0)
                        ShowSuccess("删除成功.", this.ReturnUrl);
                    else
                        ShowError("删除失败.", this.ReturnUrl);
                }

                // 编辑
                this.IsEdit = true;
                Notice item = noticeService.GetNotice(noticeId);
                this.LoadNotice(item);
            }
        }
    }

    /// <summary>
    /// 加载 Notice 数据.
    /// </summary>
    /// <param name="item"></param>
    private void LoadNotice(Notice item)
    {
        if (item == null)
            return;
        this.txtTitle.Text = item.Title;
        this.ddlEntities.SelectedValue = item.EntityId;
        if (item.StartTime.HasValue)
            this.dtpBegin.Text = item.StartTime.Value.ToString("yyyy-MM-dd");
        if (item.EndTime.HasValue)
            this.dtpEnd.Text = item.EndTime.Value.ToString("yyyy-MM-dd");
        string content = item.Content;
        if (!string.IsNullOrEmpty(content))
            content = HttpUtility.HtmlDecode(content);
        this.txtContent.Text = content;

        // 已经发布.
        if (item.Enabled)
        {
            this.btnSave.Text = "停用";
            this.btnSave2.Text = "停用";
        }
    }

    /// <summary>
    /// 实体下拉列表绑定数据实现方法.
    /// </summary>
    /// <param name="container"></param>
    /// <param name="arg"></param>
    protected void OnEntityDataBind(DropDownList container, EventArgs arg)
    {
        if (this.EntityDataBind != null)
        {
            this.EntityDataBind.Invoke(container, arg);
        }
    }

    /// <summary>
    /// 保存.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Notice item = GetNoticeData();
        item.Enabled = false;
        if (SaveData(item))
            ShowSuccess("保存成功.", this.ReturnUrl);
        else
            ShowError("保存失败.");
    }

    /// <summary>
    /// 发布.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        Notice item = GetNoticeData();
        item.Enabled = true;
        if (SaveData(item))
            ShowSuccess("发布成功.", this.ReturnUrl);
        else
            ShowError("发布失败.");
    }

    /// <summary>
    /// 保存数据.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool SaveData(Notice item)
    {
        if (this.IsEdit)
            return (noticeService.UpdateNotice(item) > 0);
        else
            return (noticeService.InsertNotice(item) > 0);
    }

    /// <summary>
    /// 获取输入的公告数据.
    /// </summary>
    /// <returns></returns>
    public Notice GetNoticeData()
    {
        string userName = this.UserName;
        Notice item = new Notice();
        if (this.NoticeId > 0)
            item.NoticeId = this.NoticeId;

        item.Title = this.txtTitle.Text.Trim();

        string content = this.txtContent.Text;
        content = HttpUtility.HtmlEncode(content);
        item.Content = content;

        string startTime = this.dtpBegin.Text.Trim();
        string endTime = this.dtpEnd.Text.Trim();
        item.StartTime = ToDateTime(startTime);
        item.EndTime = ToDateTime(endTime);

        item.Creator = userName;
        item.LastModifier = userName;
        item.CreatedTime = DateTime.Now;
        item.LastModTime = DateTime.Now;

        // 关联实体
        item.EntityType = this.EntityType;
        item.EntityId = string.IsNullOrEmpty(this.EntityId) ? this.ddlEntities.SelectedValue : this.EntityId;

        return item;
    }

    /// <summary>
    /// 将指定值转换为时间对象.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected static DateTime? ToDateTime(string input)
    {
        if (string.IsNullOrEmpty(input))
            return null;
        DateTime output;
        if (DateTime.TryParse(input, out output))
            return output;
        return null;
    }
}
