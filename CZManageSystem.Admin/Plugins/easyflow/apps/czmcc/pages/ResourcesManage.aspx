<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_czmcc_pages_ResourcesManage"
     Codebehind="ResourcesManage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <div class="titleContent">
        <h3>
            <span>笔记本与EDGE卡</span></h3>
    </div>
    <div class="dataList">
        <div id="dataDiv1">
            <div class="dataTable" id="dataTable1">
                <fieldset>
                    <legend>新增笔记本或EDGE卡</legend>
                    <div style="margin-bottom: 10px;">
                        资源类型：<asp:DropDownList ID="ddlReType" runat="server" onchange="changLbName()">
                            <asp:ListItem Value="1">笔记本</asp:ListItem>
                            <asp:ListItem Value="2">EDGE卡</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;<asp:Label ID="lbName" runat="server" Text="电脑型号："></asp:Label>
                        <asp:TextBox ID="txtReModel" runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<asp:Label
                            ID="lbSerialNumber" runat="server" Text="序列号："></asp:Label><asp:TextBox ID="txtSerialNumber"
                                runat="server"></asp:TextBox>&nbsp;&nbsp;&nbsp;<span id="spGroupMsg" style="color: Red"></span>
                        <asp:Button ID="btnAddGroup" runat="server" Text="添加" class="btn_add" onclick="btnAddGroup_Click" OnClientClick="return checkValue();"  />
                    </div>
                </fieldset>
                <br />
                <div>
                    <a href="#" onclick="showDiv(1)" id="comp" style="color:Highlight">笔记本</a> | <a href="#" id="edge" onclick="showDiv(2)" style="color:">EDGE卡</a>
                </div>
                <div id="Note">
                    <asp:GridView ID="gvResourcesList" Width="100%" CssClass="tblClass" DataKeyNames="ID"
                        runat="server" AutoGenerateColumns="False" OnRowDataBound="gvResourcesList_RowDataBound"
                        OnRowCancelingEdit="gvResourcesList_RowCancelingEdit" OnRowEditing="gvResourcesList_RowEditing"
                        OnRowUpdating="gvResourcesList_RowUpdating" OnRowDeleting="gvResourcesList_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ResourcesModel" HeaderText="笔记本型号">
                                <HeaderStyle Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SerialNumber" HeaderText="序列号">
                                <HeaderStyle Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="当前状态">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lbState" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="9%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="对应工单">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <a href="BorrowWorkFlow.aspx?reId=<%# Eval("ID") %>">查看</a>
                                </ItemTemplate>
                                <HeaderStyle Width="9%" />
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="操作" EditText="编辑" UpdateText="更新" ShowEditButton="true"
                                ShowDeleteButton="true" DeleteText="&lt;span id=&quot;del&quot; onclick=&quot;javascript:return confirm('确定删除吗？')&quot;&gt;删除&lt;/span&gt; "
                                CancelText="取消" HeaderStyle-Width="10%" />
                        </Columns>
                        <AlternatingRowStyle CssClass="trClass" />
                    </asp:GridView>
                </div>
                <div id="Card" style="display:none">
                    <asp:GridView ID="gvCard" Width="100%" CssClass="tblClass" DataKeyNames="ID" runat="server"
                        AutoGenerateColumns="False" OnRowDataBound="gvCard_RowDataBound" OnRowCancelingEdit="gvCard_RowCancelingEdit"
                        OnRowEditing="gvCard_RowEditing" OnRowUpdating="gvCard_RowUpdating" OnRowDeleting="gvCard_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="ResourcesModel" HeaderText="EDGE卡号">
                                <HeaderStyle Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="SerialNumber" HeaderText="SIM卡号码">
                                <HeaderStyle Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="当前状态">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:Label ID="lbState" runat="server"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="9%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="对应工单">
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <a href="BorrowCardWorkFlow.aspx?reId=<%# Eval("ID") %>">查看</a>
                                </ItemTemplate>
                                <HeaderStyle Width="9%" />
                            </asp:TemplateField>
                            <asp:CommandField HeaderText="操作" EditText="编辑" UpdateText="更新" ShowEditButton="true"
                                ShowDeleteButton="true" DeleteText="&lt;span id=&quot;del&quot; onclick=&quot;javascript:return confirm('确定删除吗？')&quot;&gt;删除&lt;/span&gt; "
                                CancelText="取消" HeaderStyle-Width="10%" />
                        </Columns>
                        <AlternatingRowStyle CssClass="trClass" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>

    <script language="javascript" type="text/javascript">
        var addCookie=function(objName,objValue,objHours){//添加cookie
            var str = objName + "=" + escape(objValue);
            if(objHours > 0){//为0时不设定过期时间，浏览器关闭时cookie自动消失
                var date = new Date();
                var ms = objHours*3600*1000;
                date.setTime(date.getTime() + ms);
                str += "; expires=" + date.toGMTString();
            }
            document.cookie = str;
        };
        
        var getCookie=function(objName){//获取指定名称的cookie的值

            var arrStr = document.cookie.split("; ");
            for(var i = 0;i < arrStr.length;i ++){
                var temp = arrStr[i].split("=");
                if(temp[0] == objName){                
                return unescape(temp[1]);
                }
            } 
        };  
        var cookie_value = getCookie("divState");
        
        if (cookie_value != "")
        {
            showDiv(cookie_value);
        }
        
        function changLbName()
        {
            var reType = document.getElementById("<%=ddlReType.ClientID %>");
            
            var model = document.getElementById("<%=lbName.ClientID %>");
            var sNum = document.getElementById("<%=lbSerialNumber.ClientID %>");
            var type = reType.options[reType.selectedIndex].value;
            
            
            if (type == "1")
            {
                model.innerHTML = "电脑型号：";
                sNum.innerHTML = "序列号：";
            }
            else if (type == "2")
            {
                model.innerHTML = "EDGE卡号：";
                sNum.innerHTML = "SIM卡号码：";
            }
        }
        function showDiv(type)
        {
            var note = document.getElementById("Note");
            var card = document.getElementById("Card");
            
            if (type == "1")
            {
                addCookie("divState","1",0);
                note.style.display = "";
                card.style.display = "none";
                $("#comp").css("color","Highlight");
                $("#edge").css("color","");
            }
            else if(type == "2")
            {
                addCookie("divState","2",0);
                note.style.display = "none";
                card.style.display = "";
                $("#comp").css("color","");
                $("#edge").css("color","Highlight");
            }
        }
        function checkValue()
        {
            var reType = document.getElementById("<%=ddlReType.ClientID %>");
            var mode = document.getElementById("<%=txtReModel.ClientID %>"); 
            var sNum = document.getElementById("<%=txtSerialNumber.ClientID %>");
            
            var type = reType.options[reType.selectedIndex].value;
            
            if (mode.value == "")
            {
                if (type == "1")
                {
                    alert('请输入电脑型号!!');
                }
                else if (type == "2")
                {
                    alert('请输入EDGE卡号!!');
                }
                mode.focus();
                return false;
            }  
            if(sNum.value == "")
            {
                if (type == "1")
                {
                    alert('请输入电脑序列号!!');
                }
                else if (type == "2")
                {
                    alert('请输入SIM卡号码!!');
                } 
                sNum.focus();
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
