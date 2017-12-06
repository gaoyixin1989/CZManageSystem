<%@ Page Language="C#" AutoEventWireup="true" Inherits="main_Head" Codebehind="Head.aspx.cs" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.Security.Extension" Namespace="Botwave.Security.Extension.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                        <li><a href="../contrib/theme/pages/default.aspx" target="rightFrame">界面主题</a></li>
                        <li><a href="#" target="rightFrame">个人设置</a></li>
                        <li><a href="#">帮助</a></li>
                        <li>
                            <a href="../contrib/security/pages/logout.aspx" title="退出系统" class="yellow" onclick="return confirm('确定退出系统吗？');" target="_parent">
                                <asp:Literal ID="ltlLoginText" Text="退出" runat="server"></asp:Literal>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
            <!--头部 / Header-->
            <div id="header">
                <h1>
                    <span>botwave</span></h1>
                <h2 class="xqp">
                    BizSdk</h2>
                <div class="topMenu">
                    <ul>
                        <li id="menu1" class="c" onclick="onMenuClick(this.id);"><a href="../contrib/workflow/pages/default.aspx" class="index"
                            target="rightFrame"><span>待办事宜</span></a></li>
                        <li id="menu2" onclick="onMenuClick(this.id);"><a href="../contrib/workflow/pages/doneTaskByAppl.aspx" class="toMy" target="rightFrame">
                            <span>已办事宜</span></a></li>
                        <li id="menu5" onclick="onMenuClick(this.id);"><a href="../contrib/workflow/pages/draft.aspx" class="toPay" target="rightFrame">
                            <span>草稿箱</span></a> </li>
                        <li id="menu6" onclick="onMenuClick(this.id);"><a href="#" class="toLife" target="rightFrame">
                            <span>系统公告</span></a></li>
                        <li id="menu3" onclick="onMenuClick(this.id);"><a href="../contrib/attachment/pages/default.aspx" class="toDL" target="rightFrame">
                            <span>下载</span></a></li>
                        <li id="menu7" onclick="onMenuClick(this.id);"><a href="../contrib/report/pages/ReportList.aspx"
                            class="toKM" target="rightFrame"><span>报表</span></a></li>
                        <li id="liMoreMenu" runat="server"><a href="javascript:void(0);" class="toMore" id="getmore" target="rightFrame"><span>更多</span></a></li>
                    </ul>
                </div>
            </div>
            <!--Header End-->
            <bw:MoreMenuView ID="mmv" runat="server">
                <bw:MenuListItem ID="MenuListItem1" runat="server" Text="流程部署" NavigateUrl="contrib/workflow/pages/WorkflowDeploy.aspx" />
            </bw:MoreMenuView>
        </div>
    </div>
    </form>
</body>
</html>
