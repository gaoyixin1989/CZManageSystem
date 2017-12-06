<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_controls_Attachments" Codebehind="Attachments.ascx.cs" %>
<table class="tblGrayClass" cellpadding="4" cellspacing="0" style="text-align: center; width:100%;">
    <tr id="trTemplateHeader" runat="server" style="display: none">
        <th align="left">模板名称</th>
        <th align="left">备注</th>
    </tr>
    <asp:Repeater ID="rptTemplate" runat="server">
        <ItemTemplate>
            <tr>
                <td align="left">
                    <a class="ico_download" href="<%=AppPath %>contrib/workflow/pages/download.ashx?id=<%# Eval("ID") %>" title="点击下载模板文件">
                        <%# Eval("Title") %></a>
                </td>
                <td align="left"><%# Eval("Remark") %></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trClass">
                <td align="left">
                    <a class="ico_download" href="<%=AppPath %>contrib/workflow/pages/download.ashx?id=<%# Eval("ID") %>" title="点击下载模板文件">
                        <%# Eval("Title") %></a>
                </td>
                <td align="left"><%# Eval("Remark") %></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>
<table class="tblGrayClass" cellpadding="4" cellspacing="0" style="text-align:center; width:100%;">
    <tr id="trAttHeader" runat="server" style="display: none; text-align:center;">
        <th style="text-align:left;">附件名称</th>
        <th style="text-align:center;width:90px">上传人</th>
        <th style="text-align:center;width:130px">上传时间</th>
        <th style="text-align:center;width:110px">附件大小</th>
        <th style="text-align:center;width:60px">来源</th>
        <th style="text-align:center;width:60px">操作</th>
    </tr>
    <asp:Repeater ID="rptAttachment" runat="server" OnItemCommand="rptAttachment_ItemCommand" OnItemDataBound="rptAttachment_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td align="left">
                    <a class="ico_download" href="<%=AppPath %>contrib/workflow/pages/download.ashx?id=<%# Eval("ID") %>" title="点击下载"><%# Eval("Title") %><%# Eval("MimeType") %></a>
                </td>
                <td><%# Eval("RealName") %></td>
                <td style="text-align:center;width:130px"><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                 <td><asp:Label ID="lblFileSite" runat="server" Text='<%# Eval("FileSize") %>'></asp:Label></td>
                 <td><asp:Literal ID="ltlAttachSource" runat="server"></asp:Literal></td>
                <td align="center">
                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("ID") %>' ID="btnDelete"
                        CausesValidation="false" CssClass="ico_del" OnClientClick="return (confirm('确定删除该附件吗？'));" runat="server">删除</asp:LinkButton>
                        <asp:HiddenField ID="hdCreator" Value='<%# Eval("Creator") %>' runat="server" />
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="trClass">
                <td align="left">
                    <a class="ico_download" href="<%=AppPath %>contrib/workflow/pages/download.ashx?id=<%# Eval("ID") %>" title="点击下载"><%# Eval("Title") %><%# Eval("MimeType") %></a>
                </td>
                <td><%# Eval("RealName") %></td>
                <td><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                <td><asp:Label ID="lblFileSite" runat="server" Text='<%# Eval("FileSize") %>'></asp:Label></td>
                 <td><asp:Literal ID="ltlAttachSource" runat="server"></asp:Literal></td>
                <td align="center">
                    <asp:LinkButton CommandName="Delete" CommandArgument='<%# Eval("ID") %>' ID="btnDelete"
                        CausesValidation="false" CssClass="ico_del" OnClientClick="return (confirm('确定删除该附件吗？'));"
                        runat="server">删除</asp:LinkButton>
                       <%--<a href="#" class="ico_del" onclick='DeleteAttachment(" + data + ",this)'></a>--%>
                        <asp:HiddenField ID="hdCreator" Value='<%# Eval("Creator") %>' runat="server" />
                </td>
            </tr>
        </AlternatingItemTemplate>
    </asp:Repeater>
</table>
<table id="tableFile" runat="server" align="left" width="100%">
    <tr id="trFile" style="display:none">
        <td colspan="2" style="text-align:left;">
            <asp:FileUpload ID="fileUpload2" runat="server" />
            &nbsp;
            <asp:FileUpload ID="fileUpload1" runat="server" />
            <asp:Button ID="btnUpload" runat="server" OnClientClick="return CheckIsUpload();"
                Text="单文件上传" CausesValidation="false" CssClass="btnPassClass" OnClick="btnUpload_Click">
            </asp:Button>&nbsp;
            <input type="button" id="btnCancel" value="取消" class="button2" onclick="DisplayFile(this);" style="display: none" />     
            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red" Visible="false"></asp:Label>       
        </td>
    </tr>
    <tr style="display: none">
        <td style="width:200px; text-align:left;">
            <input type="button" class="button2l" style="display: block" id="btnShow" value="增加附件"
                onclick="DisplayFile(this);">
        </td>
        <td>
            
        </td>
    </tr>
</table>
<script type="text/javascript" language="javascript">
    function DisplayFile(v){
        var obj = document.getElementById("<%=trFile.ClientID%>");
        if(v.id == "btnShow"){
            obj.style.display = "inline";
            v.style.display = "none";
        }
        else{
            obj.style.display = "none";
            document.all.btnShow.style.display = "inline";
	    }
    }

    function CheckIsUpload(){
        var attachment = document.getElementById("<%= fileUpload1.ClientID %>");
        if (attachment.value==""){
            alert('请选择要上传的文件');
            return false;
        }
        
        var tem=new String(attachment.value);
        var ext=tem.toLocaleLowerCase().substring(tem.indexOf(".",0),tem.length);

        /*if (ext==".exe"||ext==".com"||ext==".bat"){
            alert('上传文件的类型不能为可执行文件');
            return false;
        }*/
	    return true;
    }	
</script>
