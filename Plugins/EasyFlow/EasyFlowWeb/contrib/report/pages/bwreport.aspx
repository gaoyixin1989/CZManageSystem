<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_report_pages_bwreport" Codebehind="bwreport.aspx.cs" %>

<%@ Register Src="../controls/TemplateView.ascx" TagName="TemplateView" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报表</title>
</head>
<body onload="fillData('<%=SqlWhere %>')">

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

    <form id="form1" runat="server">
    <div id="printArea" style="width: 100%; background-color: White">
        <uc1:TemplateView ID="TemplateView1" runat="server" />
        <br />
        <iframe id="frmTable" name="frmTable" marginheight="0" frameborder="0" style="width: 100%;
            background-color: White; aho: expression(autoResize('frmTable'));"></iframe>
        <br />
        <iframe id="frmGrap" name="frmGrap" marginheight="0" frameborder="0" style="width: 100%;
            aho: expression(autoResize('frmGrap'));"></iframe>
    </div>
    </form>
</body>
</html>
