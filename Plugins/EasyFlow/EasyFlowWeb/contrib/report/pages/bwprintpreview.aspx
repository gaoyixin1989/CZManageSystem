<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_report_pages_bwprintpreview" Codebehind="bwprintpreview.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>打印预览</title>
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312">
    <object id="WebBrowser" classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2" height="0"
        width="0">
    </object>
</head>
<style>
    @media print
    {
        .notprint
        {
            display: none;
        }
    }
    @media screen
    {
        .notprint
        {
            display: inline;
            cursor: hand;
        }
    }
</style>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>

<script type="text/javascript">
function window.onload(){
    var printArea= opener.document.all.printArea;
    window.document.body.innerHTML=printArea.innerHTML;
    window.focus();
    window.document.all.WebBrowser.ExecWB(7,1);
    window.close(); 
}
</script>

</html>
