<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_gmcc_Head" Codebehind="Head.aspx.cs" %>

<%@ Register TagPrefix="bw" Assembly="Botwave.Security" Namespace="Botwave.Security.Web.Controls" %>
<%@ Register TagPrefix="bw" Assembly="Botwave.XQP" Namespace="Botwave.XQP.Web.Controls" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工作台</title>
    <script type="text/javascript" src="../scripts/script.js"></script>
</head>
<body id="master">
    <script type="text/javascript" language="javascript">
    <!--        //


	//-->
    </script>
    <!----------- 页面头部 ------------------>
    <div id="header">
        <h1>
            流程易（EF）－中国移动广东公司潮州分公司
        </h1>
        <div class="userinfo">
            <asp:Literal ID="ltlGreetingText" runat="server" Text=""></asp:Literal>，您好！
        </div>
        <!----------- 一级导航 ------------------>
        <ul class="navigation">
            <bw:AccessController ID="acTop1" ResourceValue="A011" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>contrib/workflow/pages/default.aspx" target="rightFrame">待办事宜</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController1" ResourceValue="A012" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>contrib/workflow/pages/doneTaskByAppl.aspx" target="rightFrame">
                        已办事宜</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController7" ResourceValue="A011" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>contrib/workflow/pages/review.aspx" target="rightFrame">待阅事宜</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController2" ResourceValue="A011" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>contrib/workflow/pages/search.aspx" target="rightFrame">高级查询</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController15" ResourceValue="E005" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>contrib/workflow/pages/searchbyorg.aspx" target="rightFrame">部门工单</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController16" ResourceValue="E006" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>contrib/workflow/pages/searchbyorg.aspx" target="rightFrame">科室工单</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController3" ResourceValue="A017,A019" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>apps/xqp2/pages/workflows/report/reportIndex.aspx" target="rightFrame">
                        报表统计</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController4" ResourceValue="A008" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>contrib/workflow/pages/draft.aspx" target="rightFrame">草稿箱</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController6" ResourceValue="A011" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>apps/czmcc/pages/WorkflowAttentions.aspx" target="rightFrame">
                        我的关注</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <bw:AccessController ID="AccessController5" ResourceValue="A016" runat="server">
                <ContentTemplate>
                    <li><a href="<%=AppPath%>apps/xqp2/pages/notices/Notices.aspx" target="rightFrame">系统公告</a></li>
                </ContentTemplate>
            </bw:AccessController>
            <%--<bw:AccessController ID="AccessController8" ResourceValue="A007" runat="server">
            <ContentTemplate>--%>
                <li><a href="javascript:void(0);" target="rightFrame" onclick="showMenu('divMaintenance', this);" id="maintenance">系统运维▼</a></li>
            <%--</ContentTemplate>
        </bw:AccessController>--%>
            <li><a href="javascript:void(0);" target="rightFrame" id="getmore">更多▼</a></li>
        </ul>
        <ul class="button">
            <li>
                <img src="<%=AppPath%>app_themes/gmcc/img/btn_hd_support.gif" title="个人设置" onclick="window.parent.frames['rightFrame'].location='<%=AppPath%>apps/xqp2/pages/workflows/config/ConfigUser.aspx';" /></li>
            <li>
                <img src="<%=AppPath%>app_themes/gmcc/img/btn_hd_help.gif" title="帮助" onclick="window.top.location='<%=AppPath%>apps/xqp2/pages/help/help.htm';" /></li>
            <li>
                <img src="<%=AppPath%>app_themes/gmcc/img/btn_hd_exit.gif" title="退出" onclick="window.top.location='<%=AppPath%>contrib/security/pages/logout.aspx';" /></li>
        </ul>
        <bw:MoreMenuView ID="mmv" runat="server" VerifyResource="true">
            <bw:MenuListItem ID="MenuListItem10" runat="server" Text="笔记本与EDGE卡" NavigateUrl="apps/czmcc/pages/ResourcesManage.aspx"
                ResourceValue="A018" />
            <bw:MenuListItem ID="MenuListItem6" runat="server" Text="应用系统接入" NavigateUrl="apps/xqp2/pages/app/list.aspx"
                ResourceValue="A007" />
            <bw:MenuListItem ID="MenuListItem11" runat="server" Text="委托授权" NavigateUrl="apps/xqp2/pages/security/authorize.aspx" />
            <bw:MenuListItem ID="MenuListItem1" runat="server" Text="用户管理" NavigateUrl="contrib/security/pages/users.aspx"
                ResourceValue="A002" />
            <bw:MenuListItem ID="MenuListItem2" runat="server" Text="流程设计" NavigateUrl="apps/xqp2/pages/workflows/workflowDeploy.aspx"
                ResourceValue="A007" />
             <bw:MenuListItem ID="MenuListItem5" runat="server" Text="表单库设计" NavigateUrl="contrib/dynamicform/pages/FormIndex.aspx"
                ResourceValue="A007" />
            <bw:MenuListItem ID="MenuListItem3" runat="server" Text="权限管理" NavigateUrl="contrib/security/pages/roles.aspx"
                ResourceValue="A003" />
            <bw:MenuListItem ID="MenuListItem4" runat="server" Text="流程分组" NavigateUrl="apps/xqp2/pages/workflows/config/workflowGroup.aspx"
                ResourceValue="A007" />
            <bw:MenuListItem ID="MenuListItem7" runat="server" Text="系统日志" NavigateUrl="contrib/gmcclog/pages/default.aspx"
                ResourceValue="A015" />
            <bw:MenuListItem ID="MenuListItem8" runat="server" Text="修改密码" NavigateUrl="contrib/security/pages/changePassword.aspx"
                ResourceValue="A006" />
            <%--<bw:MenuListItem ID="MenuListItem5" runat="server" Text="流程辅助功能" NavigateUrl="contrib/workflow/pages/Extension/WorkflowHelper.aspx" ResourceValue="E001" />
            <bw:MenuListItem ID="MenuListItem12" runat="server" Text="任务改派功能" NavigateUrl="apps/xqp2/pages/workflows/Extension/WorkflowHelper.aspx" ResourceValue="E004" />
            <bw:MenuListItem ID="MenuListItem15" runat="server" Text="工单管理功能" NavigateUrl="apps/xqp2/pages/workflows/maintenance/search.aspx" ResourceValue="M001" />--%>
            <bw:MenuListItem ID="MenuListItem9" runat="server" Text="短信审批查看" NavigateUrl="contrib/workflow/pages/Customize/SMSView/SMSmessageView.aspx"
                ResourceValue="E002" />
            <%--<bw:MenuListItem ID="MenuListItem12" runat="server" Text="外部流程接入" NavigateUrl="apps/czmcc/pages/workflowInterface.aspx" ResourceValue="E003" />--%>
            <bw:MenuListItem ID="MenuListItem13" runat="server" Text="应用系统注册" NavigateUrl="contrib/security/pages/registsystem.aspx"
                ResourceValue="A007" />
            <bw:MenuListItem ID="MenuListItem14" runat="server" Text="推送接口设置" NavigateUrl="apps/xqp2/pages/System/SystemAccess.aspx"
                ResourceValue="A007" />
        </bw:MoreMenuView>
        <div id="divMaintenance" style="display: none;">
            <div class="main_menu_popup" style="padding: 6px; border: 1px solid #9B9B9B; background: #F9F9F9 url(<%=AppPath%>res/img/menu_popup_bg.gif) repeat-x bottom;">
                <table width="124" style="margin: 0; padding: 0; line-height: 22px; cursor: pointer;
                    font-size: 12px; border-collapse: collapse;">
                    <bw:AccessController ID="AccessController9" ResourceValue="E001" runat="server">
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>contrib/workflow/pages/Extension/WorkflowHelper.aspx';">
                                        <span style="padding-left: 26px;">流程名称修改</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ID="AccessController18" ResourceValue="M006" runat="server">
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/xqp2/pages/workflows/maintenance/workflowhelper.aspx';">
                                        <span style="padding-left: 26px;">步骤名称修改</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ID="AccessController17" ResourceValue="M005" runat="server">
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/xqp2/pages/workflows/workflowHistoryDeploy.aspx';">
                                        <span style="padding-left: 26px;">历史流程管理</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ID="AccessController10" ResourceValue="E004" runat="server">
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/xqp2/pages/workflows/Extension/WorkflowHelper.aspx';">
                                        <span style="padding-left: 26px;">任务改派</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ID="AccessController11" ResourceValue="M001" runat="server">
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/xqp2/pages/workflows/maintenance/search.aspx';">
                                        <span style="padding-left: 26px;">工单管理</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ID="AccessController19" ResourceValue="E004" runat="server">
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/xqp2/pages/workflows/maintenance/transfertask.aspx';">
                                        <span style="padding-left: 26px;">任务转移</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ResourceValue="M002" runat="server" ID="AccessController12" >
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/pms/pages/StatInstanceReport.aspx?formid=0A652E41-CA03-4649-942F-D6B198509C64';">
                                        <span style="padding-left: 26px;">流程监控</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ResourceValue="M004" runat="server" ID="AccessController8" >
                        <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/pms/pages/ActivityStatInstanceReport.aspx?formid=18300f17-d97d-4fe1-80d4-253124c697b2';">
                                        <span style="padding-left: 26px;">步骤监控</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                    </bw:AccessController>
                    <bw:AccessController ResourceValue="M003" runat="server" ID="AccessController13" >
                         <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/pms/pages/UCS_FromManager.aspx';">
                                        <span style="padding-left: 26px;">报表配置</span>
                                </td>
                            </tr>
                         </ContentTemplate>
                     </bw:AccessController>
                     <bw:AccessController ResourceValue="M003" runat="server" ID="AccessController14" >
                         <ContentTemplate>
                            <tr>
                                <td background="<%=AppPath%>res/img/menu_popup_normal.gif" onmouseover="background='<%=AppPath%>res/img/menu_popup_hover.gif';"
                                    onmouseout="background='<%=AppPath%>res/img/menu_popup_normal.gif';" onclick="top.rightFrame.location = '<%=AppPath %>apps/pms/pages/UCS_ImgFromManager.aspx';">
                                        <span style="padding-left: 26px;">图表配置</span>
                                </td>
                            </tr>
                          </ContentTemplate>
                     </bw:AccessController>
                </table>
            </div>
        </div>
        
    </div>
</body>
</html>
