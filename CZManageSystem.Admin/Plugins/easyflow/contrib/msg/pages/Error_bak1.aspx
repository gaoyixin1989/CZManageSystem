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
                return "ϵͳ������_" + DateTime.Today.ToShortDateString();
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
                <span>ϵͳ��ʾ</span></h3>
        </div>
        <div class="errInfo">
            <h4>
                <span>��ʾ��</span></h4>
            <p>
                <asp:Literal ID="ltlMessage" runat="server" /></p>
            <input id="btnBack" runat="server" type="button" value="����" class="btnReturn" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input id="btnSendMail" type="button" onclick="javascript:sentmail();" title="�����ʼ�֪ͨϵͳ����Ա" value="�����ʼ�" class="btnClass2l" />
        </div>
    </div>
</asp:Content>
