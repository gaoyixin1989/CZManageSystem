<%@ Control Language="C#" AutoEventWireup="true" Inherits="contrib_security_controls_AuthorizeHistory" Codebehind="AuthorizeHistory.ascx.cs" %>
<div class="showControl">
    <h4><%=this.Title%></h4>
    <button onclick="return showContent(this,'<%=this.ClientID%>authorizationList');" title="收缩"><span>折叠</span></button>
</div>    
<div id="<%=this.ClientID%>authorizationList">      
    <table class="tblGrayClass" style="text-align:center;" cellpadding="4" cellspacing="1">
        <tr>
          <th width="10%"><%=this.FormatHeaderText()%></th>
          <th width="33%">所属部门</th>
          <th width="18%">起始时间</th>
          <th width="18%">结束时间</th>
          <th width="8%">是否有效</th>
          <th width="8%">完全授权</th>
          <th width="5%">操作</th>
        </tr>
        <asp:Repeater ID="authorizationRepeater" runat="server" onitemcommand="authorizationRepeater_ItemCommand" onitemdatabound="authorizationRepeater_ItemDataBound">
            <ItemTemplate>
                <tr class="trClass">
                    <td><%#  FormatName(Eval("FromRealName").ToString(), Eval("ToRealName").ToString()) %></td>
                    <td style="text-align:left;"><%# Eval("ToDpFullName") %></td>
                    <td><%# Eval("BeginTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td><%# Eval("EndTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                    <td><asp:Literal ID="ltlEnabled" runat="server" /></td>
                    <td><asp:Literal ID="ltlIsFullAuthorized" runat="server" /></td>
                    <td>
                        <asp:LinkButton ID="btnRecycle" CausesValidation="false" CommandName="Recycle" CommandArgument='<%# Eval("Id") %>' runat="server">收回</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>               
        </asp:Repeater>
    </table>
    <div class="toolBlock">
        <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
            Font-Size="9pt" ItemsPerPage="15" PagerStyle="NumericPages" BorderWidth="0px"
            OnPageIndexChanged="listPager_PageIndexChanged" />
    </div>
</div>
