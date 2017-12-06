<%--<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true"
    Inherits="Botwave.Web.PageBase" %>--%>
    <%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true"
        Inherits="Botwave.Security.Web.PageBase"
     %>
<%@ Import Namespace="EasyFlowWeb" %>
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
                    Response.Redirect(returnUrl);
                    //this.btnBack.Attributes.Add("onclick", string.Format("return redirectUrl('{0}')", FilterKeyWord.ReplaceKey(returnUrl)));
                }
               
            }
        }
    </script>

    <script language="javascript" type="text/javascript">
<!--//
$(function() {   
	$("#<%=btnBack.ClientID%>").focus();
        $('#btnClose').onclick(function () {
            $('btnBack').click();
        });
});
function redirectUrl(returnUrl){
    if(returnUrl.indexOf("login",0) > -1)
        window.top.location = returnUrl;
    else
        window.location = returnUrl;
    return true;
}
function sentmail()
{
    parent.document.charset = "GB2312";
    parent.location.href = "mailto:<%=ReceiveMail %>?subject=<%=MailSubject %>&body=<%=MailBody %>";
     
}
//-->
    </script>

    <div class="infoBox">
        <div class="title">
		<span>系统提示</span>
	</div>
	<div class="Inner blackbox_icon_error blackbox_icon">
              <p>  <asp:Literal ID="ltlMessage" runat="server" /></p>
             <div class="BlackBoxAction">
            <a id="btnBack" runat="server"   onclick="history.go(-1);document.getElementById('txtUserName').value='';"   class="btnReturn" >返回</a>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <a id="btnSendMail"   onclick="javascript:sentmail();" title="发送邮件通知系统管理员"   class="btnClass2l">发送邮件</a> 

           </div>
	</div>
    <div id="btnClose" class="close">×</div>
    </div>
</asp:Content>
