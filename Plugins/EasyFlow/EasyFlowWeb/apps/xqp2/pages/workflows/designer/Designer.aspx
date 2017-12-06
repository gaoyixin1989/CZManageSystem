<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_designer_Designer" Title="流程可视化设计" Codebehind="Designer.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <link rel="stylesheet" type="text/css" href="history/history.css" />
    <script src="AC_OETags.js" language="javascript" type="text/javascript"></script>
    <script src="history/history.js" language="javascript" type="text/javascript"></script>
    <script language="JavaScript" type="text/javascript">
    <!--
    // -----------------------------------------------------------------------------
    // Flash WorkflowDesigner Globals
    // Major version of Flash required
    var requiredMajorVersion = 9;
    // Minor version of Flash required
    var requiredMinorVersion = 0;
    // Minor version of Flash required
    var requiredRevision = 28;
    // -----------------------------------------------------------------------------
    // -->
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
    <div class="titleContent">
            <h3><span>流程可视化设计</span></h3>
    </div>
    <div class="btnControl">
        <div class="btnLeft">
            <input type="button" value="返回" class="btnNewwin" onclick="window.location='../workflowDeploy.aspx';" />
        </div>
    </div>
    <div>
    <script language="javascript" type="text/javascript">
    <!--//
        // Version check for the Flash Player that has the ability to start Player Product Install (6.0r65)
        var hasProductInstall = DetectFlashVer(6, 0, 65);
        // Version check based upon the values defined in globals
        var hasRequestedVersion = DetectFlashVer(requiredMajorVersion, requiredMinorVersion, requiredRevision);

        if ( hasProductInstall && !hasRequestedVersion ) {
	        // DO NOT MODIFY THE FOLLOWING FOUR LINES
	        // Location visited after installation is complete if installation is required
	        var MMPlayerType = (isIE == true) ? "ActiveX" : "PlugIn";
	        var MMredirectURL = window.location;
            document.title = document.title.slice(0, 47) + " - Flash Player Installation";
            var MMdoctitle = document.title;

	        AC_FL_RunContent(
		        "src", "playerProductInstall",
		        "FlashVars", "MMredirectURL="+MMredirectURL+'&MMplayerType='+MMPlayerType+'&MMdoctitle='+MMdoctitle+"",
		        "width", "100%",
		        "height", "500",
		        "align", "middle",
		        "id", "WorkflowDesigner",
		        "quality", "high",
		        "bgcolor", "#ffffff",
		        "name", "WorkflowDesigner",
		        "allowScriptAccess","sameDomain",
		        "type", "application/x-shockwave-flash",
		        "pluginspage", "http://www.adobe.com/go/getflashplayer",
		        "flashvars", "key=<%=this.WorkflowKey%>"
	        );
        } else if (hasRequestedVersion) {
	        // if we've detected an acceptable version
	        // embed the Flash Content SWF when all tests are passed
	        AC_FL_RunContent(
			        "src", "WorkflowDesigner",
			        "width", "100%",
			        "height", "500",
			        "align", "middle",
			        "id", "WorkflowDesigner",
			        "quality", "high",
			        "bgcolor", "#ffffff",
			        "name", "WorkflowDesigner",
			        "allowScriptAccess","sameDomain",
			        "type", "application/x-shockwave-flash",
			        "pluginspage", "http://www.adobe.com/go/getflashplayer",
			        "flashvars", "key=<%=this.WorkflowKey%>"
	        );
          } else {  // flash is too old or we can't detect the plugin
            var alternateContent = 'Alternate HTML content should be placed here. '
  	        + 'This content requires the Adobe Flash Player. '
   	        + '<a href=http://www.adobe.com/go/getflash/>Get Flash</a>';
            document.write(alternateContent);  // insert non-flash content
          }
  
          function onSaveCompleteHandler(key){
            if(confirm("部署成功！\r\n是否立即进行流程设置?")){
                window.location = " <%=RedirectUrl%>" + key;
  	        }
          }
        // -->
        </script>
        <noscript>
  	        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"
			        id="WorkflowDesigner" width="100%" height="500"
			        codebase="http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab">
			        <param name="movie" value="WorkflowDesigner.swf" />
			        <param name="quality" value="high" />
			        <param name="bgcolor" value="#ffffff" />
			        <param name="allowScriptAccess" value="sameDomain" />
			        <param name="allowFullScreen" value="true" />
			        <param name="flashVars" value="key=<%=this.WorkflowKey%>"/>
			        <embed src="WorkflowDesigner.swf" quality="high" bgcolor="#ffffff"
				        width="100%" height="500" name="WorkflowDesigner" align="middle"
				        play="true"
				        loop="false"
				        quality="high"
				        allowScriptAccess="sameDomain"
				        type="application/x-shockwave-flash"
				        pluginspage="http://www.adobe.com/go/getflashplayer"
				        flashVars="key=<%=this.WorkflowKey%>">
			        </embed>
	        </object>
    </noscript>
    </div>
</asp:Content>
