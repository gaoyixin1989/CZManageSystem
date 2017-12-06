<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" runat="server">

    <script runat="server">
        string ReceiveMail
        {
            get
            {
                return "";
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
                return DateTime.Now.ToLongDateString();
            }
        }
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string errorMessage = Botwave.Web.PageBase.CurrentMessage;
                Botwave.Web.PageBase.CurrentMessage = string.Empty;
                this.ltlMessage.Text = errorMessage;
            }
        }
    </script>

    <div class="content2">
        <div class="titleContent">
            <h3>
                <span>系统提示</span></h3>
        </div>
        <div class="errInfo">
            <h4>
                <span>提示：</span></h4>
            <p>
                <asp:Literal ID="ltlMessage" runat="server" /></p>
            <input id="btnBack" runat="server" type="button" value="返回" class="btnReturn" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input id="btnSendMail" type="button" onclick="javascript:sentmail();" title="发送邮件通知系统管理员" value="发送邮件" class="btnClass2l" />
        </div>
    </div>
</asp:Content>
