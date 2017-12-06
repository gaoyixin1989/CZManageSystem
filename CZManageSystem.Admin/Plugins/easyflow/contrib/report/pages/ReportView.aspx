<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="contrib_report_ReportView" Codebehind="ReportView.aspx.cs" %>

<%@ Register Src="../controls/TemplateView.ascx" TagName="TemplateView" TagPrefix="uc1" %>
<%@ Register Src="../controls/NavigationTools.ascx" TagName="NavigationTools" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="Server">
    <link href="../res/css/Report.css" type="text/css" rel="Stylesheet" rev="Stylesheet" />

    <script type="text/javascript">
    function GetSqlWhere()
    {
        var sql = "";
	    var cInput=document.all.tags('input');
	    for(var i in cInput) 
	    {
		    if(cInput[i].sql && cInput[i].type=='text' && cInput[i].value != '')
		    {
			   sql= sql + " and " + cInput[i].sql.replace('{0}',cInput[i].value);
		    }
		    else if(cInput[i].sql && cInput[i].type=='checkbox')
		    {
		        //暂不支持
		    }
		    else if(cInput[i].sql && cInput[i].type=='radio')
		    {
		        //暂不支持
		    }
	    }
	    var cSelect = document.all.tags('select')
	    for(var j in cSelect) 
	    {
	        if(cSelect[j].sql && cSelect[j].value != '' && cSelect[j].value != '0' )
            {
                sql= sql + " and " + cSelect[j].sql.replace('{0}',cSelect[j].value);
            }
	    }
	    if(sql.length > 3)
	        sql = sql.substring(4);
	    var s = encodeURIComponent(sql);
	    fillData(s);
	}
	function fillData(w)
	{
	    document.getElementById("frmGrap").src = "bwgrap.aspx?id=<%=ReportID %>&where=" + w;
        document.getElementById("frmTable").src = "bwtable.aspx?id=<%=ReportID %>&where=" + w;
	}
	function autoResize(v)
    {
        try
        {
            var t = v == "frmTable" ? frmTable : frmGrap;
            var h = t.document.body.scrollHeight;
            document.all[v].style.height = h;
        }
        catch(e){}        
    }
    function printPage() 
    {
        var body = document.body.innerHTML;
        var printArea = document.getElementById("printArea").innerHTML;
        document.body.innerHTML = printArea;
        window.print();
        document.body.innerHTML = body;
    }
    function exporExcel()
    {
         frmTable.document.getElementById('btnExpor').click();
    }
    </script>

    <script language="javascript" event="onload" for="window">  
        fillData('<%=SqlWhere %>');
    </script>

    <div id="divList" runat="server">
        <div class="titleContent">
            <uc1:NavigationTools ID="NavigationTools1" runat="server" />
        </div>
        <div class="dataList" id="printArea">
            <uc1:TemplateView ID="TemplateView1" runat="server" />
            <br />
            <iframe id="frmTable" name="frmTable" marginheight="0" frameborder="0" style="width: 100%;
                aho: expression(autoResize('frmTable'));"></iframe>
            <br />
            <iframe id="frmGrap" name="frmGrap" marginheight="0" frameborder="0" style="width: 100%;
                aho: expression(autoResize('frmGrap'));"></iframe>
        </div>
    </div>
</asp:Content>
