<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_extension_Left" Codebehind="Left.aspx.cs" %>
<%@ Register TagPrefix="bw" TagName="LeftMenu" Src="../../controls/LeftMenu.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工作台菜单页</title>
    <script type="text/javascript" src="../../../../res/js/common.js"></script>
    <script type="text/javascript" src="../../../../res/js/jquery-latest.pack.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container2">
        <div class="mainbox">
            <!--主体区域 / MainContent-->
            <div class="mainContent">
                <!--左侧侧边栏 / Sidebar -->
                <div class="sidebar" style="width:190px; height:900px">
                    <h3 class="titleSidebar">流程菜单</h3>
                    <div class="menuSidebar">
                        <div class="menuList" id="menuList" style="overflow:auto">
                            <ul>
                                <bw:LeftMenu id="leftMenu1" runat="server" />
                                <asp:PlaceHolder ID="phMenu" runat="server"></asp:PlaceHolder>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- Sidebar End-->
                <div class="clearDiv">
                </div>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $(function(){
            var id = 0;
            $(".menuList").children("ul").children("li").children("h4").each(function(){
                $(this).css("cursor","pointer");
                this.id = id;
                id++;
            });
            $(".menuList").children("ul").children("li").children("h4").click(function(){
                var gid = this.id;
                $(".menuList").children("ul").children("li").children("h4").each(function(){
                    if(gid == this.id){
                        $(this).parent().toggleClass("c");
                    }
                    else{
                        $(this).parent().removeClass("c");
                    }
                });
            });
        });
    </script>
</body>
</html>