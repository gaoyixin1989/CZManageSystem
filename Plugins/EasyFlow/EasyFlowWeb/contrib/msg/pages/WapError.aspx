<%--<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true"
    Inherits="Botwave.Web.PageBase" %>--%>
    <%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Mobile.master" AutoEventWireup="true" Inherits="contrib_msg_WapError" Codebehind="WapError.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">

    <script runat="server">
        string ReceiveMail
        {
            get
            {
                return "";//Botwave.Configuration.ExceptionConfig.Default.ReceiveMail;
            }
        }
        string MailSubject
        {
            get
            {
                return "系统出错报告_" + DateTime.Today.ToShortDateString();
            }
        }
        string MailBody
        {
            get
            {
                //return DateTime.Now.ToLongDateString();
                return Botwave.Commons.ExceptionLogger.ExceptionLogBody.Replace("\\", "/");
            }
        }
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string errorMessage = Botwave.Web.PageBase.CurrentMessage;
                Botwave.Web.PageBase.CurrentMessage = string.Empty;
                this.ltlMessage.Text = errorMessage;

                string returnUrl = FilterKeyWord.ReplaceKey(Request.QueryString["returnUrl"]);
                string javascript = Server.HtmlEncode(Request.QueryString["js"]);
                if (string.IsNullOrEmpty(returnUrl) && null != Request.UrlReferrer)
                    returnUrl = Request.UrlReferrer.PathAndQuery;
                if (!string.IsNullOrEmpty(javascript))
                {
                    javascript = HttpUtility.UrlDecode(javascript);
                    this.btnBack.Attributes.Add("onclick", javascript);
                }
                else if (!string.IsNullOrEmpty(returnUrl))
                {
                    //this.btnBack.Attributes.Add("onclick", string.Format("return redirectUrl('{0}')", FilterKeyWord.ReplaceKey(returnUrl)));
                }
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
<!--//
$(function() {
    $('.loading-backdrop').hideLoading();
    $('.loading-backdrop').hide();
});
/*function redirectUrl(returnUrl){
    if(returnUrl.indexOf("login",0) > -1)
        window.top.location = returnUrl;
    else
        window.location = returnUrl;
    return true;
}*/
function sentmail()
{
    parent.document.charset = "GB2312";
    parent.location.href = "mailto:<%=ReceiveMail %>?subject=<%=MailSubject %>&body=<%=MailBody %>";
     
}
//-->
    </script>
    <div class="page-header">
        <!-- Fixed navbar -->
        <div class="navbar navbar-inverse navbar-fixed-top ui-header" role="navigation">
            <h1 class="text-center ui-title"  style="color: rgb(69, 125, 179);" id="header">
                系统提示
            </h1>
        </div>
    </div>
    <div class=" container-fluid theme-showcase" role="main" style="padding-top:6.5em">
    <div class="panel panel-danger"><div class="panel-heading">提示：</div>
    <div class="panel-body">
    <p><asp:Literal ID="ltlMessage" runat="server" /></p>
    </div>
    <div style="text-align:center;" class="panel-footer">
            <input id="btnBack" runat="server" class="btn btn-default" type="button" onclick="history.go(-1);document.getElementById('txtUserName').value='';" value="返回" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input id="btnSendMail" type="button" onclick="javascript:sentmail();" title="发送邮件通知系统管理员" value="发送邮件" class="btn btn-default" />
  </div>
</div></div>
   
</asp:Content>
