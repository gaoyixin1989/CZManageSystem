using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Web;

public partial class apps_xqp2_pages_security_Authorize : Botwave.Web.PageBase
{
    static readonly string ImageYes = string.Format("<img alt=\"是\" src=\"{0}res/img/ico_yes.gif\" />", WebUtils.GetAppPath());
    static readonly string ImageNo = string.Format("<img alt=\"否\" src=\"{0}res/img/ico_no.gif\" />", WebUtils.GetAppPath());

    #region service

    private IUserService userService = (IUserService)Ctx.GetObject("userService");
    private IAuthorizeService authorizeService = (IAuthorizeService)Ctx.GetObject("authorizeService");

    public IUserService UserService
    {
        get { return userService; }
        set { userService = value; }
    }

    public IAuthorizeService AuthorizeService
    {
        get { return authorizeService; }
        set { authorizeService = value; }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime now = DateTime.Now;
            this.dtpBeginTime.Text = now.ToString("yyyy-MM-dd HH:mm:ss");
            this.dtpEndTime.Text = now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");
            this.BindAuthorizationRepeater(0, 0);
        }
    }

    protected void listPager_PageIndexChanged(object sender, Botwave.Web.Controls.PageChangedEventArgs e)
    {
        this.BindAuthorizationRepeater(listPager.TotalRecordCount, e.NewPageIndex);
    }

    private void BindAuthorizationRepeater(int recordCount, int pageIndex)
    {
        Guid userId = LoginHelper.User.UserId;
        this.authorizationRepeater.DataSource = authorizeService.GetAuthorizationsByPager(userId, pageIndex, listPager.ItemsPerPage, ref recordCount);
        this.authorizationRepeater.DataBind();
    }

    protected void authorizationRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Literal tempLiteral = e.Item.FindControl("ltlEnabled") as Literal;
        Literal ltlIsFullAuthorized = e.Item.FindControl("ltlIsFullAuthorized") as Literal;
        LinkButton tempButton = e.Item.FindControl("cmd1") as LinkButton;

        DataRowView row = e.Item.DataItem as DataRowView;
        bool isFullAuthorized = DbUtils.ToBoolean(row["IsFullAuthorized"]);
        ltlIsFullAuthorized.Text = isFullAuthorized ? ImageYes : ImageNo;
        bool enabled = IsEnabled(row);
        tempLiteral.Text = enabled ? ImageYes : ImageNo;

        if (enabled)
        {
            tempButton.Text = "收回";
            //tempButton.Attributes.Add("onclick", "return confirm('请确定要收回当前授权?');");
        }
        else
        {
            tempButton.Visible = false;
        }
    }

    protected void authorizationRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);

        if (authorizeService.UpdateAuthorization(id, false) > 0)
        {
            //ShowSuccess("收回授权成功！", AppPath + "Admin/Membership/Authorization.aspx");
            ShowSuccess("收回授权成功！");
        }
        else
        {
            //throw new Exception("收回授权失败。", AppPath + "Admin/Membership/Authorization.aspx");
            ShowError("收回授权失败。");
        }
    }

    protected void btnInsert_Click(object sender, EventArgs e)
    {
        LoginUser currentUser = LoginHelper.User;

        string toUser = textName.Text;
        string[] tempArray = toUser.Split(new char[] { '\t' });
        if (tempArray == null || tempArray.Length == 0
            || tempArray[0].IndexOf("<\"") < 0 || tempArray[0].IndexOf("\">") < 0)
            ShowError("未指定授权用户。");
        int index = tempArray[0].IndexOf("<\"");
        string inputUserName = tempArray[0];
        inputUserName = inputUserName.Substring(index + 2).Replace("\">", "");
        if (string.IsNullOrEmpty(inputUserName))
            ShowError("未指定授权用户。");

        string currUserName = currentUser.UserName;
        if (currUserName.Equals(inputUserName, StringComparison.CurrentCultureIgnoreCase))
            ShowError("不能授权给自己。");

        DateTime beginTime = Convert.ToDateTime(dtpBeginTime.Text.Trim());
        DateTime endTime = Convert.ToDateTime(dtpEndTime.Text.Trim());

        if (beginTime >= endTime)
        {
            ShowError("授权的截止时间必须大于授权的开始时间。");
        }

        UserInfo user = userService.GetUserByUserName(inputUserName);
        if (user != null)
        {
            AuthorizationInfo item = new AuthorizationInfo();
            item.BeginTime = beginTime;
            item.EndTime = endTime;
            item.IsFullAuthorized = radionAuthrizationFull.Checked;
            item.FromUserId = currentUser.UserId;
            item.ToUserId = user.UserId;
            item.Enabled = true;

            Botwave.XQP.Commons.XQPHelper.UpdateAuthorizationDisable(currUserName);
            authorizeService.InsertAuthorization(item);
            //WriteNomalLog(currUserName, "委托授权", string.Format("授权给 {0} 成功.", user.UserName));
            ShowSuccess("授权成功！");
        }
        else
        {
            //WriteExLog(currUserName, "用户授权", "用户授权异常", "未找到当前被授权用户.");
            //throw new Exception("未找到当前被授权用户，请重试！");
            ShowError("未找到当前被授权用户，请重试！");
        }

    }

    public static bool IsEnabled(DataRowView item)
    {
        bool enabled = DbUtils.ToBoolean(item["Enabled"]);
        DateTime beginTime = Convert.ToDateTime(item["BeginTime"].ToString());
        DateTime endTime = Convert.ToDateTime(item["EndTime"].ToString());
        if (enabled == false || beginTime >= DateTime.Now || endTime <= DateTime.Now)
            return false;
        return true;
    }
}
