<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_workflow_pages_WorkflowTemplate"
    Title="Untitled Page" CodeBehind="WorkflowTemplate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3>
            <span>流程模板设置</span></h3>
        <div class="rightSite">
            <input type="button" value="返 回" id="btnStart" onclick="javascript: history.go(-1);" class="btnReturnClass" />
        </div>
    </div>
    <div class="btnControl">
        <div class="btnRight">
            <input type="button" value="返 回" id="btnStart" onclick="javascript: history.go(-1);" class="btnFW" />
        </div>
    </div>
    <div class="dataList">
        <div class="showControl">
            <h4>上传模板</h4>
            <button onclick="return showContent(this,'tableUpload');" title="收缩">
                <span>折叠</span></button>
        </div>
        <table id="tableUpload" width="100%" cellspacing="4">
            <tr>
                <th width="14%" align="right">上传模板文件：

                </th>
                <td width="86%">
                    <asp:FileUpload ID="fileUpload1" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" OnClientClick="return checkIsUpload();"
                        Text="上 传" CausesValidation="false" CssClass="button2" OnClick="btnUpload_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <div class="showControl">
            <h4>模板列表</h4>
            <button onclick="return showContent(this,'tbList');" title="收缩">
                <span>折叠</span></button>
        </div>
        <table class="tblGrayClass" id="tbList" style="text-align: center" cellpadding="4"
            cellspacing="0">
            <tr id="trTemplateHeader" runat="server">
                <th align="left">模板名称
                </th>
                <th align="left">备注
                </th>
                <th align="left" width="70px">上传人

                </th>
                <th align="left" width="80px">上传时间
                </th>
                <th align="center" width="50px">操作
                </th>
            </tr>
            <asp:Repeater ID="rptTemplate" runat="server" OnItemCommand="rptTemplate_ItemCommand">
                <ItemTemplate>
                    <tr>
                        <td align="left">
                            <a class="ico_download" href="download.ashx?id=<%# Eval("ID") %>" title="点击下载模板文件">
                                <%# Eval("Title") %></a>
                        </td>
                        <td align="left">
                            <%# Eval("Remark") %>
                        </td>
                        <td align="left">
                            <%# Eval("Creator")%>
                        </td>
                        <td align="left">
                            <%# Eval("CreatedTime")%>
                        </td>
                        <td align="center">
                            <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("ID") %>' ID="btnDelete"
                                CausesValidation="false" CssClass="ico_del" OnClientClick="return (confirm('确定删除该模板吗？'));"
                                runat="server">删除</asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="trClass">
                        <td align="left">
                            <a class="ico_download" href="download.ashx?id=<%# Eval("ID") %>" title="点击下载模板文件">
                                <%# Eval("Title") %></a>
                        </td>
                        <td align="left">
                            <%# Eval("Remark") %>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblCreator" runat="server"></asp:Label>
                        </td>
                        <td align="left">
                            <%# Eval("CreatedTime")%>
                        </td>
                        <td align="center">
                            <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("ID") %>' ID="btnDelete"
                                CausesValidation="false" CssClass="ico_del" OnClientClick="return (confirm('确定删除该模板吗？'));"
                                runat="server">删除</asp:LinkButton>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        function checkIsUpload() {
            var attachment = document.getElementById("<%= fileUpload1.ClientID %>");
        if (attachment.value == "") {
            alert('请选择要上传的文件');
            return false;
        }

        var tem = new String(attachment.value);
        var ext = tem.toLocaleLowerCase().substring(tem.indexOf(".", 0), tem.length);

        if (ext == ".exe" || ext == ".com" || ext == ".bat") {
            alert('上传文件的类型不能为可执行文件');
            return false;
        }
        return true;
    }
    </script>

</asp:Content>
