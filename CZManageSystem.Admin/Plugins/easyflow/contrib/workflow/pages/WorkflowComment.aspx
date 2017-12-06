<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_pages_WorkflowComment" Codebehind="WorkflowComment.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="cphHead" runat="server">
    <title>流程评论页</title>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
</head>
<body style="background: none !important; text-align:left">
    <div>
        <table class="tblGrayClass" style="text-align:center" cellpadding="4" cellspacing="1">
                <tr>
                    <th style="width:10%">评论人</th>
                    <th style="width:53">评论内容</th>
                    <th style="width:20%">附件</th>
                    <th style="width:17%">评论日期</th>
                </tr>
                <asp:Repeater ID="rptComments" runat="server" OnItemDataBound="rptComments_ItemDataBound">
                    <ItemTemplate>
                        <tr valign="top">
                            <td><%# Eval("Creator") %></td>
                            <td style="text-align:left;"><asp:Literal ID="ltlMessage" Text='<%# Eval("Message") %>' runat="server"></asp:Literal></td>
                            <td style="text-align:left;">
                                <asp:Literal ID="ltlAttachments" runat="server"></asp:Literal>
                            </td>
                            <td><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr valign="top" class="trClass">
                            <td><%# Eval("Creator") %></td>
                            <td style="text-align:left;"><asp:Literal ID="ltlMessage" Text='<%# Eval("Message") %>' runat="server"></asp:Literal></td>
                            <td style="text-align:left;">
                                <asp:Literal ID="ltlAttachments" runat="server"></asp:Literal>
                            </td>
                            <td><%# Eval("CreatedTime", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        
        <div id="divCommentInputs" runat="server" class="dataTable" style="margin-top:5px">
            <form id="form1" method="post" runat="server" enctype="multipart/form-data">
                <table class="tblGrayClass grayBackTable" cellpadding="4" cellspacing="1" style="margin-top:6px;">
                    <tr>
                        <th style="width:13%;">评论内容：</th>
                        <td>
                            <asp:TextBox ID="txtContent" CssClass="inputbox" TextMode="MultiLine" Width="90%" Height="50px" runat="server"></asp:TextBox>
                            <span class="require">*</span>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvTxtContent" ControlToValidate="txtContent" Display="None" runat="server" ErrorMessage="评论内容不能为空，请填写评论内容." SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th style="width:13%;">评论附件：</th>
                        <td>
                            <div id="divFiles" style="width:50%; float:left">
                                <input type="file" name="commentFile" id="commentFile1" style="width:90%"/>
                            </div>
                            <div style="width:50px; float:left">
                                <input id="btnAddInputFile" type="button" class="btnClass2m" onclick="addInputFile();" value="增加附件" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td style="margin-left:0px">
                            &nbsp;<asp:Button ID="txtSave" runat="server" CssClass="btnSaveClass"  Text="发表" onclick="txtSave_Click" />
                            <div>
                                <asp:ValidationSummary ID="summary1" runat="server" ShowMessageBox="true" ShowSummary="false" />
                            </div>
                        </td>
                    </tr>
                </table>
            </form>
        </div>
        <script language="javascript" type="text/javascript">
        <!--//
        $(function() {
            $("input:text").each(function() {
                this.className = "inputbox";
            });
            $("textarea").each(function() {
                this.className = "inputbox";
            });
            $("input:text").focus(function() {
                this.className = "inputbox_focus";
            });
            $("input:text").blur(function() {
                this.className = "inputbox";
            });
            $("textarea").focus(function() {
                this.className = "inputbox_focus";
            });
            $("textarea").blur(function() {
                this.className = "inputbox";
            });
            parent.setActivityCommentCount("<%=this.CommentCount %>");
            if (parseInt("<%=this.CommentCount %>") > 0) {
                parent.displayComment(true);
            } else {
                parent.displayComment(false);
            }
            if (window.parent) {
                var contentFrame = window.parent.document.getElementById("comment_ifAttas");
                if (contentFrame) {
                    var height = $(this).find("body").attr('scrollHeight');
                    $(contentFrame).attr("height", height);
                }
            }
        });
        var attaIndex = 1
        function addInputFile() {
            var id = "commentFile" + attaIndex;
            $("#divFiles").append("<input type=\"file\" name=\"" + id + "\" id=\"" + id + "\" style=\"width:90%\"/>");
            attaIndex++;
            parent.setCommentFrameHeight(25);
        }
        //-->
        </script>
    </div>
</body>
</html>
