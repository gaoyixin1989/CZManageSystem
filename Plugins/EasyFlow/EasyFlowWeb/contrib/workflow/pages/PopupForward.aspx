<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_workflow_pages_PopupForward" Codebehind="PopupForward.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ת������</title>
    <script type="text/javascript" src="../../../res/js/common.js"></script>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
</head>
<body>
    <div class="newBody">
		<div class="content2">
			<div class="titleContent">
				<h3><span>ת������</span></h3>
			</div>
            <script type="text/javascript" src="script/tooltipAjax.js"></script>
            <form id="form1" runat="server">
                <asp:ScriptManager ID="scriptManager1" runat="server">
                    <Services>
                        <asp:ServiceReference Path="WorkflowAjaxService.asmx" />
                    </Services>
                </asp:ScriptManager>
	            <div class="btnControl">
		            <div class="btnLeft">
		                <asp:Button ID="btnFw1" runat="server" CssClass="btnFW" Text="ȷ��ת��"  onclick="btnFw_Click" />
		            </div>
	            </div>
                <div class="dataList newContent1" style="height:245px;width:96%;text-align:left;overflow:auto">
                    <div id="divAssignmentActorContainer">
                        <fieldset style="height:105px;width:96%">
                            <legend>��ͬ����/������Ա��ѡ��ת����</legend>   
		                    <asp:Literal ID="ltlAssignmentActors" runat="server"></asp:Literal>
                        </fieldset>
                    </div>
                    <%--<div style="padding-top:3px">ת����ע��</div>--%>
                    <div>
                        <asp:TextBox ID="txtRemark" TextMode="MultiLine" Height="40px" Width="98%" runat="server" style="display:none"></asp:TextBox>
                    </div>
                    <div style="padding-top:3px" id="divMesages" runat="server"></div>
		        </div>
	            <div class="btnControl">
		            <div class="btnLeft">
		                <asp:Button ID="btnFw2" runat="server" CssClass="btnFW" onclick="btnFw_Click" OnClientClick="return validateCheck();" Text="ȷ��ת��" />
		            </div>
	            </div>
            </form>
			<div class="closeBtnDiv">
				<input type="button" value="�رմ���" class="btnClose" onclick="windowClose();" />
			</div>
		</div>
	</div>
	<script type="text/javascript">
    <!--//
    function openPopupUserPick(){
        var h = 400;
	    var w = 700;
	    var iTop = (window.screen.availHeight-30-h)/2;    
	    var iLeft = (window.screen.availWidth-10-w)/2; 
	    window.open('<%=AppPath%>contrib/security/pages/popupUserPicker2.aspx?func=onCompleteUserPickResult', '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no');	
	    return false;
    }
    
    function onCompleteUserPickResult(result){
        var htmlText = "";
        for(var i=0; i<result.length;i++){
            htmlText += "<input type=\"radio\" name=\"chkboxUser\" value=\""+result[i].key+"\" /><span tooltip=\""+result[i].key+"\">"+result[i].value+"</span>";
        }
        $("#divAssignActorFromCompany").html(htmlText);
        $("input[name='chkboxUser']").attr("checked","checked");
    }
    
    function clearUserCheckState(){
        $("input[name='chkboxUser']").each(function(){
            if(this.checked)
                 this.checked = false;
        });   
    }
    
    function validateCheck(){
        var hasSelected = false;
        $("input[name='chkboxUser']").each(function(){
            if(this.checked)
                hasSelected = true;
        });
        if(!hasSelected)
            alert("��ѡ��ת����"); 
        return hasSelected;
    }
    
    var isAssignFromCompany = "<%=IsAssignFromCompany %>";
    if(isAssignFromCompany.toLowerCase() == "true"){
        var html = "<fieldset style=\"height:60px;width:96%\">"+
                            "<legend>��ȫ��˾��Ա��ѡ��ת����</legend>"+   
                            "<div id=\"divAssignActorFromCompany\" style=\"margin-bottom:5px;margin-top:5px\"></div>"+
                            "<a href=\"#\" onclick=\"javascrpt:return openPopupUserPick();\" title=\"�����ȫ��˾��Ա��ѡ��ת����\" style=\"margin-left:6px;\">���ѡ��ת����</a>"+
                         "</fieldset>";
        $("#divAssignmentActorContainer").append(html);
    }
     //-->
    </script>
</body>
</html>
