<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="contrib_mobile_controls_WorkflowView" Codebehind="WorkflowView.ascx.cs" %>
<div class="row">
    <div class="col-xs-12 colspan">
        <table style="width: 100%; min-height: 38px">
            <tr>
                <th style="width: 6.5em; height: 100%">
                    标题
                </th>
                <td>
                    <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-md-4 col" id="thSheetId" runat="server">
        <table style="width: 100%; min-height: 38px">
            <tr>
                <th style="width: 6.5em; height: 100%">
                    受理号：
                </th>
                <td><asp:Literal ID="ltlSheetId" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
            
    <%=BasicInfoHtml %>
</div>
<div class="row" style="border-top: 0;">
    <div class="col-xs-12 col-md-12 col" style="border-bottom: solid 1px #DFDFE8;">
        <table style="width: 100%; min-height: 38px">
            <tr>
                <th style="width: 6.5em; height: 100%">
                    当前流程：
                </th>
                <td style="font-weight: bold; color: blue;">
                    <asp:Literal ID="ltlWorkflowName" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
</div>
<div class="row" style="border-top: 0;">
    <div class="col-xs-12" style="border-bottom: solid 1px #DFDFE8;">
        <div style="font-weight: bold; color: blue; text-align: center; height: 1.5em; vertical-align: middle;
            padding-top: 4px" onclick="return showContent(this,'divMoreBase');">
            展开更多...</div>
    </div>
</div>
<div class="row" style="border-top: 0; display: none" id="divMoreBase">
    <asp:Literal ID="ltlMoreBasicHtml" runat="server" />
</div>
