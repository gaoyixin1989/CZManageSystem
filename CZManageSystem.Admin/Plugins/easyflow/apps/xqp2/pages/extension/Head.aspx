<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_extension_Head" Codebehind="Head.aspx.cs" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security.Extension" Namespace="Botwave.Security.Extension.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工作台菜单页</title>
    <script type="text/javascript" language="javascript">
    <!--//
	function setNavBar(code){
	    var el = document.getElementById("navbar");
	    if (el){
	        el.innerHTML = code;
	    }
	}
	function onMenuClick(id){
        for(var i=1;i<7;i++){
            var el = document.getElementById("menu" + i);
            if (el){
		        el.className="";
		    }
        }            
        document.getElementById(id).className='c';
    }
	//-->
	</script>
</head>
<body style="height: 120px; overflow: hidden;">
    <form id="form1" runat="server">
    <div class="topheader_wrapper">
        <div class="topHeader">
            <!--顶部导航 / TopNav-->
            <div class="topNav">
                <div class="navSite" id="navbar">
                </div>
                <div class="linkList">
                    <ul>
                        <li class="user">欢迎您，<asp:Literal ID="ltlGreetingText" runat="server"></asp:Literal>
                        </li>
                        <bw:AccessController ID="acTop1" ResourceValue="A004" runat="server">
                            <ContentTemplate>
                                <li><a href="<%=AppPath%>contrib/theme/pages/default.aspx" target="rightFrame">界面主题</a></li>
                            </ContentTemplate>
                        </bw:AccessController>
                        <bw:AccessController ID="acTop2" ResourceValue="A005" runat="server">
                            <ContentTemplate>
                                <li><a href="../security/authorize.aspx" target="rightFrame">委托授权</a></li>
                            </ContentTemplate>
                        </bw:AccessController>
                        <bw:AccessController ID="acTop3" ResourceValue="A014" runat="server">
                            <ContentTemplate>
                                <li><a href="<%=AppPath%>apps/xqp2/pages/workflows/config/ConfigUser.aspx" target="rightFrame">个人设置</a></li></ContentTemplate>
                        </bw:AccessController>
                        <li><a href="<%=AppPath%>apps/xqp2/pages/help/html.htm" target="_blank">帮助</a></li>
                        <li>
                            <a href="<%=AppPath%>contrib/security/pages/logout.aspx" title="退出系统" class="yellow" onclick="return confirm('确定退出系统吗？');" target="_parent">
                                <asp:Literal ID="ltlLoginText" Text="退出" runat="server"></asp:Literal>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
            <!--头部 / Header-->
            <div id="header">
                <h1><span>botwave</span></h1>
                <h2 class="xqp"><%=AppName%></h2>
                <div class="topMenu">
                    <ul>
                        <bw:AccessController ID="acTodo" ResourceValue="A011" runat="server">
                            <ContentTemplate>
                                <li id="menu1" class="c" onclick="onMenuClick(this.id);"><a href="<%=AppPath%>contrib/workflow/pages/default.aspx" class="index"
                                    target="rightFrame"><span>待办事宜</span></a></li>
                            </ContentTemplate>
                         </bw:AccessController>
                        <bw:AccessController ID="acDone" ResourceValue="A012" runat="server">
                            <ContentTemplate>
                                <li id="menu2" onclick="onMenuClick(this.id);"><a href="<%=AppPath%>contrib/workflow/pages/doneTaskByAppl.aspx" class="toMy" target="rightFrame">
                                    <span>已办事宜</span></a></li>
                            </ContentTemplate>
                         </bw:AccessController>
                        <bw:AccessController ID="acSearch" ResourceValue="A011" runat="server">
                            <ContentTemplate>
                        <li id="menu3" onclick="onMenuClick(this.id);"><a href="<%=AppPath%>contrib/workflow/pages/search.aspx" class="toOA" target="rightFrame">
                            <span>高级查询</span></a></li>
                            </ContentTemplate>
                         </bw:AccessController>
                        <bw:AccessController ID="acReport" ResourceValue="A011" runat="server">
                            <ContentTemplate>
                        <li id="menu4" onclick="onMenuClick(this.id);"><a href="<%=AppPath%>apps/xqp2/pages/workflows/report/reportIndex.aspx" class="toKM" target="rightFrame">
                            <span>报表统计</span></a></li>
                            </ContentTemplate>
                         </bw:AccessController>
                        <bw:AccessController ID="acDraft" ResourceValue="A008" runat="server">
                            <ContentTemplate>
                                <li id="menu5" onclick="onMenuClick(this.id);"><a href="<%=AppPath%>contrib/workflow/pages/draft.aspx" class="toPay" target="rightFrame">
                                    <span>草稿箱</span></a> </li>
                            </ContentTemplate>
                         </bw:AccessController>
                        <bw:AccessController ID="acNotice" ResourceValue="A016" runat="server">
                            <ContentTemplate>
                                <li id="menu6" onclick="onMenuClick(this.id);"><a href="<%=AppPath%>apps/xqp2/pages/notices/Notices.aspx" class="toLife" target="rightFrame">
                                    <span>系统公告</span></a></li>
                            </ContentTemplate>
                        </bw:AccessController>
                        <li id="liMoreMenu" runat="server"><a href="javascript:void(0);" class="toMore" id="getmore" target="rightFrame"><span>更多</span></a></li>
                    </ul>
                </div>
            </div>
            <!--Header End-->
            <bw:MoreMenuView ID="mmv" runat="server" VerifyResource="true">
                <%--<bw:MenuListItem ID="MenuListItem5" runat="server" Text="我的工单" NavigateUrl="contrib/workflow/pages/myTask.aspx" />--%>
                <bw:MenuListItem ID="MenuListItem10" runat="server" Text="借用资源管理" NavigateUrl="apps/czmcc/pages/ResourcesManage.aspx" ResourceValue="A007" />
                <bw:MenuListItem ID="MenuListItem1" runat="server" Text="用户管理" NavigateUrl="contrib/security/pages/users.aspx" ResourceValue="A002" />
                <bw:MenuListItem ID="MenuListItem2" runat="server" Text="流程设计" NavigateUrl="apps/xqp2/pages/workflows/workflowDeploy.aspx" ResourceValue="A007" />
                <bw:MenuListItem ID="MenuListItem3" runat="server" Text="权限管理" NavigateUrl="contrib/security/pages/roles.aspx" ResourceValue="A003" />
                <bw:MenuListItem ID="MenuListItem4" runat="server" Text="流程分组" NavigateUrl="apps/xqp2/pages/workflows/config/workflowGroup.aspx" ResourceValue="A007" />
                <bw:MenuListItem ID="MenuListItem9" runat="server" Text="系统公告" NavigateUrl="apps/xqp2/pages/notices/Notices.aspx" ResourceValue="A007" />
                <bw:MenuListItem ID="MenuListItem6" runat="server" Text="应用系统接入" NavigateUrl="apps/xqp2/pages/app/list.aspx" />
                <bw:MenuListItem ID="MenuListItem7" runat="server" Text="系统日志" NavigateUrl="contrib/gmcclog/pages/default.aspx" ResourceValue="A015" />
                <bw:MenuListItem ID="MenuListItem8" runat="server" Text="修改密码" NavigateUrl="contrib/security/pages/changePassword.aspx" />
                
            </bw:MoreMenuView>
        </div>
    </div>
    </form>
</body>
</html>
