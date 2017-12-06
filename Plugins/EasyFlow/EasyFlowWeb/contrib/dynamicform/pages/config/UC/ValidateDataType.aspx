<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_config_UC_ValidateDataType" Codebehind="ValidateDataType.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>//为[所属系统1][F10]设置扩展设置 </title>
</head>
<body style="repeat-y left;">
    
    <style>
        body
        {
            font-family: Arial, simsun;
            font-size: 12px;
            margin: 0;
            padding: 0;
            color: #333;
            overflow-y: scroll; overflow-y:inherit;}
        input, textarea, select, button
        {
            color: #000;
            margin: 0;
            vertical-align: middle;
        }
        form, ul, ol, li
        {
            margin: 0;
            padding: 0;
            list-style: none;
        }
        img
        {
            border: 0;
        }
        h1, h2, h3, h4, h5
        {
            margin: 0;
            padding: 0;
            font-size: 12px;
        }
        p
        {
            margin: 0 0 8px;
        }
        a:visited, a:link
        {
            text-decoration: underline;
            color: #333;
        }
        a:hover
        {
            color: #F50 !important;
            text-decoration: underline;
        }
        .clear
        {
            clear: both;
            height: 1px;
            font-size: 0;
            overflow: hidden;
        }
        .expand
        {
            width: 790px;
        }
        .right_e
        {
            float: left;
            margin-top: 20px;
        }
        .right_e dl
        {
            margin: 0;
            padding-bottom: 20px;
        }
        .right_e dt
        {
            margin: 0;
            font-weight: bold;
            color: #247ecf;
            padding-bottom: 10px;
            _padding-bottom: 6px;
        }
        .right_e dd
        {
            margin: 0;
            
            line-height: 18px;
        }
        .expand_input
        {
            width: 98%;
            border: 1px solid #cdcdcd;
        }
        table
        {
            width: 100%;
            border-collapse: collapse;
        }
        th
        {
            background: #f4f4f4;
            padding: 5px 8px;
            border: 1px solid #eaeaea;
            vertical-align:middle;
        }
        td
        {
            border: 1px solid #eaeaea;
            padding: 5px 8px;
        }
        .expand_btn
        {
            margin-top: 15px;
        }
        .expand_btn_a
        {
            background: url(<%=AppPath%>App_Themes/new/images/expand_btn.gif) repeat-x left top;
            height: 30px;
            line-height: 30px;
            border: none;
            text-align: center;
            width: 97px;
        }
        .expand_btn_b
        {
            background: url(<%=AppPath%>App_Themes/new/images/expand_btn.gif) repeat-x left -30px;
            height: 30px;
            line-height: 30px;
            border: none;
            text-align: center;
            width: 69px;
        }
    </style>
    <form id="form1" runat="server">
    <div>
        <div class="right_e">
        
            <dl>
            <dt> 脚本验证<span style="color:Red"><asp:Literal ID="ltlInfo" runat="server"></asp:Literal></span></dt>
                <dd>
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <th width="18%">
                                类型
                            </th>
                            <th width="30%">
                                验证函数中文名
                            </th>
                            <th width="40%">
                                显示
                            </th>
                            <th width="15%">
                                操作
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <input type="hidden" id="hidId" runat="server" />
                                <asp:Literal ID="ltlType" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <input type="hidden" id="hidDescription" runat="server" />
                                <asp:Literal ID="ltlDescript" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <input type="hidden" id="hidFunction" runat="server" />
                                <asp:Literal ID="ltlFunction" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnDelete" runat="server" Text="删除" 
                                    onclick="lbtnDelete_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                        <td colspan="4">
                       
	

        <asp:Repeater ID="rptUsers" runat="server">
                                <HeaderTemplate>
                                 <select size="10" class="expand_input" multiple="multiple" id="lblLibrary" width="100%">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <option value='<%#Eval("ID") %>' function='<%#Eval("Function") %>' description='<%#Eval("title") %>'><%#Eval("Events") %>&nbsp;<%#Eval("Title") %></option>
                                </ItemTemplate>
                                <FooterTemplate>
                                </select>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                        </tr>
                    </table>
                </dd>
                </dl>
                <dl>
                <dt> 内容加密<span style="color:Red"><asp:Literal ID="ltlInfo1" runat="server"></asp:Literal></span></dt>
                <dd>
                说明： 加密起始位（第几位开始加密:如1111*****）<br />
                结束保留位（最后保留多少位:如*****8888）<br />
                    例子：如 123445567890，加密起始位4，结束保留位4：123*****7890<br/>
                <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <th width="35%">
                                字段名
                            </th>
                            <th width="25%">
                                加密起始位
                            </th>
                            <th width="25%">
                                结束保留位
                            </th>
                            <th width="15%">
                                操作
                            </th>
                        </tr>
                        <tr>
                            <td>
                                <input type="hidden" id="Hidden1" runat="server" />
                                <asp:Literal ID="ltlFullName" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="ltlStart" runat="server" CssClass="expand_input" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}" onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="ltlEnd" runat="server" CssClass="expand_input" onkeyup="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}" onafterpaste="if(isNaN(value)){alert('请输入0-9之间的数字！');execCommand('undo');}"></asp:TextBox>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnDelete1" runat="server" Text="删除" 
                                    onclick="lbtnDelete1_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </dd>
                <div class="expand_btn">
                    <asp:Button ID="btnSave" Text=" 保存 " class="expand_btn_b" runat="server" OnClientClick="return checkValue();"
                        OnClick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnSaveClose" Text=" 保存并关闭" class="expand_btn_a" runat="server" OnClientClick="return checkValue();"
                        OnClick="btnSaveClose_Click" /></div>
            </dl>
        </div>
        <div class="clear">
        </div>
    </div>

    <script type="text/javascript" language="javascript">
        $(function() {
            $("#lblLibrary option").each(function() {
                if ($(this).attr("function") == $("#<%=hidFunction.ClientID %>").val())
                    $(this).attr("selected","selected");
            });
        });
    
        function checkValue() {
            var count = 0;
            $("#lblLibrary option").each(function() {
                if ($(this).attr("selected"))
                    count++;
            });
            if (count > 1) {
                alert("只能选择一个类型。");
                return false;
            }
            else if (count == 1) {
                document.getElementById("<%=hidId.ClientID %>").value = $("#lblLibrary").val();
                $("#<%=hidDescription.ClientID %> ").attr("value", $("#lblLibrary option:selected").attr("description"));
                $("#<%=hidFunction.ClientID %>").attr("value", $("#lblLibrary option:selected").attr("function"));
                //alert($("#<%=hidDescription.ClientID %>").val());
                return true;
            }
            if ($("#<%=ltlStart.ClientID %>").val().length > 0 && $("#<%=ltlEnd.ClientID %>").val().length == 0)
            {
                alert("请输入大于0的结束保留位。");
                return false;
            }
            if ($("#<%=ltlStart.ClientID %>").val().length == 0 && $("#<%=ltlEnd.ClientID %>").val().length > 0) {
                alert("请输入大于0的加密起始位。");
                return false;
            }
            return true;
        }
    </script>

    </form>
</body>
</html>
