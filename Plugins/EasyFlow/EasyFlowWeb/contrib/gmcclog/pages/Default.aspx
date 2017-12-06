<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" Inherits="Botwave.Web.PageBase" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Botwave.Log.GMCC" %>
<script runat="server" type="text/C#">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindExceptions();
            BindOperations();
            this.Search(0, 0);
        }
    }

    private void BindExceptions()
    {
        DataView view = LogSearcherFactory.GetLogSearcher().GetExceptionList().DefaultView;
        this.SetExceptionList(view, "-1", 0);
    }

    private void SetExceptionList(DataView source, string parentId, int depth)
    {
        source.RowFilter = string.Format("parentID='{0}'", parentId);
        for (int i = 0; i < source.Count; i++)
        {
            ListItem list = new ListItem(RepeatString("—", depth) + source[i]["description"].ToString(), source[i]["exceptionId"].ToString().Trim());
            ddlExceptionCatalog.Items.Add(list);
            SetExceptionList(source, source[i]["exceptionId"].ToString(), depth + 1);
            source.RowFilter = String.Format("parentId='{0}'", parentId);
        }
    }

    private void BindOperations()
    {
        DataView view = LogSearcherFactory.GetLogSearcher().GetOperationList().DefaultView;
        this.SetOperationList(view, "0", 0);
    }

    private void SetOperationList(DataView source, string parentId, int depth)
    {
        source.RowFilter = string.Format("parentId='{0}'", parentId);
        for (int i = 0; i < source.Count; i++)
        {
            ListItem list = new ListItem(RepeatString("—", depth) + source[i]["description"].ToString(), source[i]["operationId"].ToString().Trim());
            ddlOperationCatalog.Items.Add(list);
            SetOperationList(source, source[i]["operationId"].ToString(), depth + 1);
            source.RowFilter = String.Format("parentId='{0}'", parentId);
        }
    }

    private static string RepeatString(string s, int count)
    {
        StringBuilder builder = new StringBuilder();
        if (count > 0)
            builder.Append("|");
        for (int i = 0; i < count; i++)
        {
            builder.Append(s);
        }
        return builder.ToString();
    }

    protected void listPager_PageIndexChanged(object sender, PageChangedEventArgs e)
    {
        int pageIndex = e.NewPageIndex;
        pageIndex = pageIndex + 1;
        Search(listPager.TotalRecordCount, pageIndex);
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Search(0, 0);
        this.listPager.CurrentPageIndex = 0;
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        Export(0, listPager.CurrentPageIndex);
    }

    private void Search(int recordCount, int pageIndex)
    {
        string where = GetSearchCondition();
        DataTable dt = Botwave.Log.GMCC.LogSearcherFactory.GetLogSearcher().GetLogListByPage(pageIndex, listPager.ItemsPerPage, ref recordCount, where);
        dgLogs.DataSource = dt;
        dgLogs.DataBind();
        listPager.TotalRecordCount = recordCount;
        listPager.DataBind();
        ltlTotalRecordCount.Text = recordCount.ToString();
    }

    private void Export(int recordCount, int pageIndex)
    {
        string where = GetSearchCondition();
        DataTable dt = Botwave.Log.GMCC.LogSearcherFactory.GetLogSearcher().GetLogListByPage(pageIndex, listPager.ItemsPerPage, ref recordCount, where);
        Response.ContentType = "application/ms-excel";
        Response.ContentEncoding = System.Text.Encoding.Default;
        Response.AppendHeader("Content-Disposition", "attachment;filename=Log.xls");
        EnableViewState = false;
        System.IO.StringWriter sw = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter htw = new HtmlTextWriter(sw);
        dgLogs.EnableViewState = false;
        dgLogs.GridLines = GridLines.Both;
        dgLogs.DataSource = dt;
        dgLogs.DataBind();
        dgLogs.RenderControl(htw);
        Response.Write(sw.ToString());
        Response.End();
    }

    private string GetSearchCondition()
    {
        //string handler = txtHandler.Text.Trim().Replace('\'', ' ');
        //string startDT = dtpBeginDT.Text.Trim().Replace('\'', ' ');
        //string endDT = dtpEndDT.Text.Trim().Replace('\'', ' ');
        //string operationID = ddlOperationCatalog.SelectedValue.Trim();
        //string exceptionID = ddlExceptionCatalog.SelectedValue.Trim();
        string handler = EasyFlowWeb.FilterKeyWord.ReplaceKey(txtHandler.Text.Trim().Replace('\'', ' '));
        string startDT = EasyFlowWeb.FilterKeyWord.ReplaceKey(dtpBeginDT.Text.Trim().Replace('\'', ' '));
        string endDT = EasyFlowWeb.FilterKeyWord.ReplaceKey(dtpEndDT.Text.Trim().Replace('\'', ' '));
        string operationID = EasyFlowWeb.FilterKeyWord.ReplaceKey(ddlOperationCatalog.SelectedValue.Trim());
        string exceptionID = EasyFlowWeb.FilterKeyWord.ReplaceKey(ddlExceptionCatalog.SelectedValue.Trim());

        StringBuilder whereBuilder = new StringBuilder();
        whereBuilder.Append("(available=1)");
        if (!string.IsNullOrEmpty(handler))
        {
            whereBuilder.AppendFormat(" AND (portalID like '%{0}%')", handler);
        }
        if (!string.IsNullOrEmpty(startDT))
        {
            whereBuilder.AppendFormat(" AND (opStartTime>='{0}')", startDT);
        }
        if (!string.IsNullOrEmpty(endDT))
        {
            DateTime dtEnd = Convert.ToDateTime(endDT);
            dtEnd = dtEnd.AddDays(1);
            whereBuilder.AppendFormat(" AND (opStartTime<='{0}')", dtEnd.ToString("yyyy-MM-dd"));
        }
        if (operationID != "1")
        {
            whereBuilder.AppendFormat(" AND (operationID like '{0}%')", operationID);
        }
        if (exceptionID != "")
        {
            whereBuilder.AppendFormat(" AND (exceptionID like '{0}%')", exceptionID);
        }
        return whereBuilder.ToString();
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3><span>系统日志</span></h3>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>查询日志</h4>
            <button onclick="return showContent(this,'queryInput');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="queryInput">
            <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-bottom: 16px;">
                <tr>
                    <th width="12%" align="right">起始时间:</th>
                    <td width="38%">
                        <bw:DateTimePicker ID="dtpBeginDT" runat="server" IsRequired="false" Width="80px" ExpressionValidatorText="*" ExpressionErrorMessage="起始时间范围最小时间日期格式错误." />
                        到

                         <bw:DateTimePicker ID="dtpEndDT" runat="server" IsRequired="false" Width="80px" ExpressionValidatorText="*" ExpressionErrorMessage="起始时间范围最大时间日期格式错误." />
                    </td>
                    <th width="12%" align="right">操作人:</th>
                    <td width="38%">
                        <asp:TextBox ID="txtHandler" runat="server" CssClass="inputbox" size="15"></asp:TextBox></td>
                </tr>
                <tr>
                    <th align="right">操作类型:</th>
                    <td>
                        <asp:DropDownList ID="ddlOperationCatalog" runat="server"></asp:DropDownList></td>
                    <th align="right">异常类型:</th>
                    <td>
                        <asp:DropDownList ID="ddlExceptionCatalog" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnQuery" runat="server" CssClass="btn_query" Text="搜索" OnClick="btnQuery_Click" />
                        <asp:Button ID="btnExport" runat="server" CausesValidation="false" CssClass="btn" Text="导出" OnClick="btnExport_Click" />
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="validationSummary1" ShowSummary="false" ShowMessageBox="true" runat="server" />
        </div>
        <div class="showControl">
            <h4>日志列表</h4>
            <button onclick="return showContent(this,'logsTable');" title="收缩"><span>折叠</span></button>
        </div>
        <div class="dataTable" id="logsTable">
            <asp:DataGrid ID="dgLogs" runat="server" GridLines="None" CssClass="tblClass" UseAccessibleHeader="true" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundColumn ItemStyle-Width="8%" HeaderText="序号" DataField="uid" />
                    <asp:BoundColumn ItemStyle-Width="22%" HeaderText="操作人portal账号" DataField="portalID" />
                    <asp:BoundColumn ItemStyle-Width="25%" HeaderText="操作时间" DataField="opStartTime" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                    <asp:BoundColumn ItemStyle-Width="15%" HeaderText="操作描述" DataField="description" />
                    <asp:BoundColumn ItemStyle-Width="15%" HeaderText="操作类型" DataField="operationDescription" />
                    <asp:BoundColumn ItemStyle-Width="15%" HeaderText="异常类型" DataField="exceptionDescription" />
                </Columns>
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                <AlternatingItemStyle CssClass="trClass" />
            </asp:DataGrid>
            <div class="toolBlock" style="border-top: solid 1px #C0CEDF">
                共有<strong><asp:Literal ID="ltlTotalRecordCount" runat="server"></asp:Literal></strong>条记录

                <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                    Font-Size="9pt" ItemsPerPage="15" PagerStyle="NumericPages" BorderWidth="0px"
                    OnPageIndexChanged="listPager_PageIndexChanged" />
            </div>
        </div>
    </div>
</asp:Content>
