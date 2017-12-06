<%@ Page Language="C#" AutoEventWireup="true" Inherits="contrib_report_pages_bwgrap" Codebehind="bwgrap.aspx.cs" %>

<%@ Register TagPrefix="zgw" Namespace="ZedGraph.Web" Assembly="ZedGraph.Web" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>图表</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; background-color: White">
        <zgw:ZedGraphWeb ID="zedGraphControl" runat="server" RenderMode="ImageTag" RenderedImagePath="~/contrib/report/res/ZedGraphImages">
        </zgw:ZedGraphWeb>
    </div>
    </form>
</body>
</html>
