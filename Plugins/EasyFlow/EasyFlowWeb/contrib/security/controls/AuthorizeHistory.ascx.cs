using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Security.Domain;
using Botwave.Security.Service;

public partial class contrib_security_controls_AuthorizeHistory : Botwave.Security.Web.UserControlBase
{
    private static readonly string ImageYes = AppPath + "res/img/ico_yes.gif";
    private static readonly string ImageNo = AppPath + "res/img/ico_no.gif";

    private IAuthorizeService authorizeService;

    public IAuthorizeService AuthorizeService
    {
        get { return authorizeService; }
        set { authorizeService = value; }
    }

    /// <summary>
    /// 控件标题.
    /// </summary>
    public string Title
    {
        get { return (string)ViewState["ControlTitle"]; }
        set { ViewState["ControlTitle"] = value; }
    }

    /// <summary>
    /// 指定授权列表的用户编号.
    /// </summary>
    public Guid UserId
    {
        get { return (Guid)ViewState["UserId"]; }
        set { ViewState["UserId"] = value; }
    }

    /// <summary>
    /// 是否属于被授权(true 则表示被授权历史记录; false 则表示委托授权历史记录).
    /// </summary>
    public bool IsAuthorized
    {
        get { return (bool)ViewState["IsAuthorized"]; }
        set { ViewState["IsAuthorized"] = value; }
    }

    public contrib_security_controls_AuthorizeHistory()
    {
        this.Title = "授权记录";
        this.UserId = Guid.Empty;
        this.IsAuthorized = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void authorizationRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Literal tempLiteral = e.Item.FindControl("ltlEnabled") as Literal;
        Literal ltlIsFullAuthorized = e.Item.FindControl("ltlIsFullAuthorized") as Literal;
        LinkButton tempButton = e.Item.FindControl("btnRecycle") as LinkButton;

        DataRowView row = e.Item.DataItem as DataRowView;
        bool isFullAuthorized = DbUtils.ToBoolean(row["IsFullAuthorized"]);
        bool enabled = DbUtils.ToBoolean("Enabled");

        ltlIsFullAuthorized.Text = isFullAuthorized ? ImageYes : ImageNo;
        tempLiteral.Text = IsEnabled(row, enabled) ? ImageYes : ImageNo;

        if (this.IsAuthorized || !enabled)
            tempButton.Visible = false;
        // tempButton.Attributes.Add("onclick", "return confirm('请确定要收回当前授权?');");
    }

    protected void authorizationRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (authorizeService.UpdateAuthorization(id, false) > 0)
            ShowSuccess("收回授权成功！", AppPath + "Admin/Membership/Authorization.aspx");
        else
            ShowError("收回授权失败。", AppPath + "Admin/Membership/Authorization.aspx");
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.BindHistory(e.NewPageIndex, listPager.TotalRecordCount);
    }

    private void BindHistory(int pageIndex, int recordCount)
    {
        DataTable source = authorizeService.GetAuthorizationsByPager(this.UserId, pageIndex, listPager.ItemsPerPage, ref recordCount);

        this.authorizationRepeater.DataSource = source;
        this.authorizationRepeater.DataBind();

        this.listPager.TotalRecordCount = recordCount;
        this.listPager.DataBind();
    }

    /// <summary>
    /// 绑定数据.
    /// </summary>
    /// <param name="isAuthorized">是否属于被授权(true 则表示被授权历史记录; false 则表示委托授权历史记录).</param>
    public void BindData(bool isAuthorized)
    {
        this.BindData(CurrentUser.UserId, isAuthorized);
    }

    /// <summary>
    /// 绑定数据.
    /// </summary>
    /// <param name="userId">指定授权列表的用户编号.</param>
    /// <param name="isAuthorized">是否属于被授权(true 则表示被授权历史记录; false 则表示委托授权历史记录).</param>
    public void BindData(Guid userId, bool isAuthorized)
    {
        this.UserId = userId;
        this.IsAuthorized = isAuthorized;

        this.BindHistory(0, 0);
    }

    public string FormatHeaderText()
    {
        return (this.IsAuthorized ? "授权用户" : "被授权用户");
    }

    public string FormatName(string fromName, string toName)
    {
        return (this.IsAuthorized ? fromName : toName);
    }

    public static bool IsEnabled(DataRowView row, bool enabled)
    {
        DateTime? beginTime = ToDateTime(row["BeginTime"]);
        DateTime? endTime = ToDateTime(row["EndTime"]);
        if (enabled == false || (beginTime.HasValue && beginTime >= DateTime.Now) || (endTime.HasValue && endTime <= DateTime.Now))
            return false;
        return true;
    }

    private static DateTime? ToDateTime(object input)
    {
        DateTime? result = null;
        if (input == null || input == DBNull.Value)
            return result;
        DateTime temp;
        if (DateTime.TryParse(input.ToString(), out temp))
            result = temp;
        return result;
    }
}
