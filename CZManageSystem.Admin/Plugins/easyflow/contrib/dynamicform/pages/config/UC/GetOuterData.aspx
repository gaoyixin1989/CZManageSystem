<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_dynamicform_pages_config_UC_GetOuterData" Codebehind="GetOuterData.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            padding-left: 16px;
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
            vertical-align: center;
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
                <dt>
                    <asp:RadioButton ID="radWay_0" Text="方式0：不做任何设置。" runat="server" GroupName="pub" /></dt>
                <dd>
                    不做任何设置</dd>
            </dl>
            <dl>
                <dt>
                    <asp:RadioButton ID="radWay1" Text="方式1：两个表单字段的加减乘除。" runat="server" GroupName="pub" /></dt>
                <dd>
                    
                    说明：可输入以下计算符号：* 乘号;/ 除号;% 相除取余;+ 加号; - 减号;<br />
                    例子：@F1@+@F2@<br>
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <th width="18%">
                                计算公式
                            </th>
                            <td width="82%">
                                <textarea name="TB_JS" rows="5" class="expand_input" id="TB_JS" runat="server"></textarea>
                            </td>
                        </tr>
                    </table>
                    
                </dd>
            </dl>
            <dl>
                <dt>
                    <asp:RadioButton ID="radWay2" Text="方式2：SQL数据库获取。" runat="server" GroupName="pub" /></dt>
                <dd> 
                    参数："#WorkflowName#"(流程名称),<br />
                    "#Wiid#"(工单ID), "#Title#"(工单标题), "#SheetId#"(工单受理号),
                    <br />
                    "#CurrentUser#"(当前登录用户), "#DpId#"(当前用户部门ID),<br />
                    "#Aiid#"(当前步骤实例ID), "#ActivityName#"(流程活动步骤)。
                    <br />
                    例子1(有参数)：Select Addr From 商品表 WHERE No=#FName# <br />
                    说明：FName是本表单中的任意字段名，如F1，F2 ...<br />
                    例子2(无参数)：Select Addr From 商品表 WHERE No=1 <br />
                    数据源输入为SQLServer数据库连接字符串，不填表示本地数据库。<br />
                    例子3(有参数)：填充数据到DataList中，数据会按列的先后顺序填充。SQL示例：Select No,Name,Creator,Addr From 商品表 WHERE No=#FName# <br />
                    说明：FName是本表单中的任意字段名，如F1，F2 ...<br />
                    例子4(无参数)：填充数据到DataList中，数据会按列的先后顺序填充。Select No,Name,Creator,Addr From 商品表 WHERE No=1 <br />
                    数据源输入为SQLServer数据库连接字符串，不填表示本地数据库。<br />
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <th width="18%">
                                SQL语句
                            </th>
                            <td width="82%">
                                <textarea name="txtSQL" rows="5" class="expand_input" id="txtSQL" runat="server"></textarea>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                数据源
                            </th>
                            <td>
                                <asp:TextBox ID="txtSqlConnection" runat="server" CssClass="expand_input"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
            <dl>
                <dt>
                    <asp:RadioButton ID="radWay3" runat="server" GroupName="pub" Text="方式3：WebService接口获取。" /></dt>
                <dd>
                    方法参数比如:SearchWorkflow:#FName1#,#FName2#，<br />
                    说明： <span style="color: Red">当前版本只提供.net版和java版的接口连接。</span>
                    <br />
                    基本条件字段格式参数有："#WorkflowName#"(流程名称),
                    <br />
                    "#Wiid#"(工单ID), "#Title#"(工单标题), "#SheetId#"(工单受理号),
                    <br />
                    "#CurrentUser#"(当前登录用户), "#DpId#"(当前用户部门ID),<br />
                    "#Aiid#"(当前步骤实例ID), "#ActivityName#"(流程活动步骤)。
                    <br />
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <th width="18%">
                                WebService方法+参数
                            </th>
                            <td width="82%">
                                <asp:TextBox ID="txtWSFunction" CssClass="expand_input" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <th>
                                URL
                            </th>
                            <td>
                                <asp:TextBox ID="txtWSConnection" CssClass="expand_input" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </dd>
                <div class="expand_btn">
                    <asp:Button ID="btnSave" Text=" 保存 " class="expand_btn_b" runat="server" OnClientClick="return checkValue();"
                        OnClick="btnSave_Click" />&nbsp;
                    <asp:Button ID="btnSaveClose" Text=" 保存并关闭" class="expand_btn_a" runat="server" OnClientClick="return checkValue();"
                        OnClick="btnSaveClose_Click"  /></div>
            </dl>
        </div>
        <div class="clear">
        </div>
    </div>

    <script type="text/javascript" language="javascript">
        function checkValue() {
            if (document.getElementById("<%=radWay1.ClientID %>").checked) {
                if (document.getElementById("<%=TB_JS.ClientID %>").value.length == 0) {
                    alert("请填写数据计算表达式！");
                    return false;
                }
            }
            else if (document.getElementById("<%=radWay2.ClientID %>").checked) {
                if (document.getElementById("<%=txtSQL.ClientID %>").value.length == 0) {
                    alert("请填写SQL语句表达式！");
                    return false;
                }
            }
            else if (document.getElementById("<%=radWay3.ClientID %>").checked) {
                if (document.getElementById("<%=txtWSConnection.ClientID %>").value.length == 0) {
                    alert("请填写WebService地址！");
                    return false;
                }
                else if (document.getElementById("<%=txtWSFunction.ClientID %>").value.length == 0) {
                    alert("请填写SWebService方法！");
                    return false;
                }
            }
            EncodeForms();
            return true;
        }
        function EncodeForms() {
            var b = new Base64();
            $("input [type='text']").each(function () {
                var content = b.encode($(this).val());
                $(this).val(content);
            })
            $("textarea").each(function () {
                var content = b.encode($(this).val());
                $(this).val(content);
            })
        }
    </script>

    </form>
</body>
</html>
