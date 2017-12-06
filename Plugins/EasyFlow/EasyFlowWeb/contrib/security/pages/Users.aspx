<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_security_pages_Users" Title="用户管理" Codebehind="Users.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    function onAddClick(){
        location = "editUser.aspx";
    }
    // ]]>
    </script>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
    <script language="javascript" type="text/javascript">
        function check() {
            if ($("#<%=file.ClientID %>").val().length == 0) {
                alert("请选择文件"); return false;
            }
            var v = $("#<%=file.ClientID %>").val().split('.');
            if (v.length > 1) {
                if (v[1] != "xls" && v[1] != "xlsx") { alert("请选择xls格式的文件"); return false; }
            } else {
                alert("请选择xls格式的文件"); return false;
            }
            return true;
        }
        function clickimport() {
            if ($("#spImport").css("display") == "none") {
                $("#spImport").css("display", "inline");
                $("#btnImport").attr("value", "取消");

            }
            else {
                $("#spImport").css("display", "none");
                $("#btnImport").attr("value", "导入");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <asp:ScriptManager ID="scriptManager1"  runat="server">
        <Services>
            <asp:ServiceReference Path="securityAjaxService.asmx" />
        </Services>
    </asp:ScriptManager>
    <div class="titleContent">
        <h3><span>用户管理</span></h3>
        <div class="rightSite">
            <input type="button" value="新增用户" onclick="onAddClick()" class="btnPassClass" />
        </div>
    </div>
    <div class="btnControl">
        <div class="btnRight">
            <input type="button" value="新增用户" onclick="onAddClick()" class="btnFW" />
        </div>
    </div>
    <div class="toolBlock" style="border-bottom:solid 1px #C0CEDF; margin-bottom:10px; padding-bottom:5px;">
        <span style="margin-right:5px">
            <asp:DropDownList ID="droplistRoles" runat="server"></asp:DropDownList>
            <input type="text" id="txtKeyword" name="txtKeyword" value="<%=this.keywords%>" />
        </span>
        <button id="btnSearch" class="btn_query" runat="server" onserverclick="btnSearch_Click">
            搜索</button>
        <span id="spImport" style="display:none">
                        下载模板： <a href="<%=AppPath %>contrib/security/res/Templates/userlist.xls" target="_blank">用户导入模板</a>
                        选择文件：
                                <asp:FileUpload ID="file" runat="server" />
                                <asp:Button ID="buttonOK" runat="server" Text="导入" class="btn_sav" OnClientClick="return check()" OnClick="btnImport_Click"/>
                                </span>
        <input type="button" id="btnImport" value="导入" title="导入Excel文件" class="btn_sav" onclick="clickimport();" />
        <input type="button" id="btnBack" class="btnReturnClass" runat="server" onserverclick="btnBack_Click" value="返回"/>
            
    </div>
     <div id="dataDiv1">
        <div class="dataTable" id="dataTable1">
            <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
                <tr>
                    <th></th>
                    <th width="15%">用户名</th>
                    <th width="13%">姓名</th>
                    <th width="43%">所属部门</th>
                    <th width="22%">操作</th>
                </tr>
                <asp:Repeater ID="usersRepeater" runat="server" 
                    onitemcommand="usersRepeater_ItemCommand" 
                    onitemdatabound="usersRepeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="checkbox" id="chkbox_user" name="chkbox_user" value="<%# Eval("UserId") %>" />
                            </td>
                            <td style="text-align:left;"><span tooltip="<%# Eval("UserName") %>"><%# Eval("UserName") %></span></td>
                            <td><span tooltip="<%# Eval("UserName") %>"><%# Eval("RealName") %></span></td>
                            <td style="text-align:left;"><%# Eval("DpFullName")%></td>
                            <td style="text-align:center;">
                              <asp:LinkButton ID="btnEnabled" CommandName="Disabled" CommandArgument='<%# Eval("UserId") %>' runat="server" ForeColor="Green" OnClientClick="return confirm('确定要启用用户?');">启用</asp:LinkButton>
                                <asp:LinkButton ID="btnDisabied" CommandName="Disabled" CommandArgument='<%# Eval("UserId") %>' runat="server" ForeColor="Red" OnClientClick="return confirm('确定要禁用用户?');">禁用</asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("UserId") %>' runat="server" ForeColor="Red" OnClientClick="return confirm('确定要删除用户?');">删除</asp:LinkButton>
                                <a href="edituser.aspx?UserId=<%# Eval("UserId") %>">编辑</a>
                                <a href="userRoles.aspx?UserId=<%# Eval("UserId") %>">分配角色</a>
                                <asp:LinkButton ID="btnLogin" CommandName="Login" CommandArgument='<%# Eval("UserName") %>' runat="server">登录</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="trClass" tooltip="<%# Eval("UserName") %>">
                            <td>
                                <input type="checkbox" id="chkbox_user" name="chkbox_user" value="<%# Eval("UserId") %>" />
                            </td>
                            <td style="text-align:left;"><span tooltip="<%# Eval("UserName") %>"><%# Eval("UserName") %></span></td>
                            <td><span tooltip="<%# Eval("UserName") %>"><%# Eval("RealName") %></span></td>
                            <td style="text-align:left;"><%# Eval("DpFullName")%></td>
                            <td style="text-align:center;">
                            <asp:LinkButton ID="btnEnabled" CommandName="Enabled" CommandArgument='<%# Eval("UserId") %>' runat="server" ForeColor="Green" OnClientClick="return confirm('确定要启用用户?');">启用</asp:LinkButton>
                            <asp:LinkButton ID="btnDisabied" CommandName="Disabled" CommandArgument='<%# Eval("UserId") %>' runat="server" ForeColor="Red" OnClientClick="return confirm('确定要禁用用户?');">禁用</asp:LinkButton>
                                <asp:LinkButton ID="btnDelete" CommandName="Delete" CommandArgument='<%# Eval("UserId") %>' runat="server" ForeColor="Red" OnClientClick="return confirm('确定要删除用户?');">删除</asp:LinkButton>
                                <a href="edituser.aspx?UserId=<%# Eval("UserId") %>">编辑</a>
                                <a href="userRoles.aspx?UserId=<%# Eval("UserId") %>">分配角色</a>
                                <asp:LinkButton ID="btnLogin" CommandName="Login" CommandArgument='<%# Eval("UserName") %>' runat="server">登录</asp:LinkButton>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
                
            </table>
            <div class="toolBlock" style="border-top:solid 1px #C0CEDF; clear:both">
                <div style="float:left; width:29%; text-align:left">
                -  <input type="checkbox" id="selectAllUser" onclick="onToggleSelect('chkbox_user', this.checked);" title="全选" />全选 
                -  <asp:LinkButton ID="btnRecycle" CssClass="ico_idt" runat="server" Text="收回角色"  OnClientClick="return confirm('确定要收回选中用户的角色？');"
                        onclick="btnRecycle_Click"></asp:LinkButton>
                </div>
                <div style="float:right; width:70%">                
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="20" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
            </div>
        </div>
    </div>
    <div style="margin-top:20px"></div>
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    function onToggleSelect(chkName, isChecked){
        var inputArray = document.getElementsByTagName("input");
		for(var i=0; i<inputArray.length; i++) {
			if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
				inputArray[i].checked = isChecked;
			}
		}
    };
    // ]]>
    </script>
    <script type="text/javascript">
    $(document).keypress(function(event){
        if(event.keyCode == 13){
            document.getElementById("<%=btnSearch.ClientID%>").click();
        }
    });
    $("#txtKeyword").keypress(function(event){
        if(event.keyCode == 13){
            this.blur();
            document.getElementById("<%=btnSearch.ClientID%>").click();
        }
    });
    $(function(){
        $("[tooltip]").hover(function(){            
            var current = this;
            var tooltipLayout = $("#tooltip"); // 显示提示信息层.
            if (tooltipLayout.length == 0) {
                tooltipLayout = $('<div id="tooltip" style="position:absolute;z-index:1000;"></div>').prependTo("body"); 
            }
            var onTooltip = function(){
                // 调用 Ajax Web Service
                var name = $(current).attr("tooltip");
                if(name != ""){
                    tooltipLayout.prepend("正在加载 ...");
                    Botwave.Security.Extension.WebServices.SecurityAjaxService.GetUserInfo(name, dispalyTooltip, errorHandler, timeoutHandler);
                }
            };
            var dispalyTooltip = function(result){
                //var titleHtml = "姓　名：" + result.RealName;
                //var titleHtml = "<br />部　门：" + result.DpFullName;
                titleHtml = "电子邮箱：" + result.Email;
                titleHtml += "<br />手　　机：" + result.Mobile;
                titleHtml += "<br />固定电话：" + result.Tel;
                // 显示提示.
                tooltipLayout.empty();
                tooltipLayout.prepend(titleHtml);
            };
            // Ajax 超时.
            var timeoutHandler = function (result){
               alert("Timeout :" + result);
            };               
            // Ajax 错误.
            var errorHandler = function (result){
//               var msg=result.get_exceptionType() + "\r\n";
//               msg += result.get_message() + "\r\n";
//               msg += result.get_stackTrace();
//               alert(msg);
            };
            
            onTooltip(); // 执行 Ajax 提示    
            $(this).mousemove(function(e){
                e = e || window.event;
                var pageY = e.pageY;
                var x = e.pageX - 16;
                if(x - 2 < 0)
                    x = 2;
                if(x + 280 > document.body.clientWidth)
                    x = document.body.clientWidth - 280;
                var y = pageY + 18;
                if(pageY > (document.body.clientHeight - 80))
                    y = pageY - 80;
                $("#tooltip").css({"left": x, "top": y, "display": "block"}); 
            });
        },
        function(){
            $("#tooltip").remove();                
        })
    });
    </script>
</asp:Content>
