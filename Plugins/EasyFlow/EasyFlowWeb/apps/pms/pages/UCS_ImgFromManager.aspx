<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="apps_pms_pages_UCS_ImgFromManager" Title="用户管理" Codebehind="UCS_ImgFromManager.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    function onAddClick(){
        location = "UCS_ImgFromManagerEdit.aspx";
    }
    // ]]>
    </script>
    <script type="text/javascript" src="../../../res/js/jquery-latest.pack.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">

 
    <div class="toolBlock">
        <span style="margin-right:5px">
            <input type="text" id="txtKeyword" name="txtKeyword" value="<%=Request.Form["txtKeyword"]%>" />
        </span>
        <button id="btnSearch" class="btn_query" runat="server" onserverclick="btnSearch_Click">
            搜索</button> <input type="button" value="新增图表" onclick="onAddClick()" class="btnPassClass" />
    </div>
     <div id="dataDiv1">
        <div class="dataTable" id="dataTable1">
            <table cellpadding="0" cellspacing="0" class="tblClass" id="tblId1" style="text-align:center;">
                <tr>
                    <th >图表名称</th>
                    <th >对应数据库表(视图)</th>
                    <th >创建时间</th>
                    <th>修改时间</th>
                    <th>最后操作人</th>
                    <th>操作</th>
                </tr>
                <asp:Repeater ID="usersRepeater" runat="server" >
                    <ItemTemplate>
                        <tr>
                            <td ><%# Eval("formname")%></td>
                            <td><%# Eval("datasource")%></td>
                            <td ><%# Eval("createtime")%></td>
                              <td ><%# Eval("updatetime")%></td>
                                 <td ><%# Eval("lasthandlers")%></td>
                            <td style="text-align:center;">

                                <a href="UCS_ImgFromManagerEdit.aspx?Id=<%# Eval("id") %>">编辑</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                                 <tr>
                            <td ><%# Eval("formname")%></td>
                            <td><%# Eval("datasource")%></td>
                            <td ><%# Eval("createtime")%></td>
                              <td ><%# Eval("updatetime")%></td>
                                 <td ><%# Eval("lasthandlers")%></td>
                            <td style="text-align:center;">

                                <a href="UCS_ImgFromManagerEdit.aspx?Id=<%# Eval("id") %>">编辑</a>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
                
            </table>
            <div class="toolBlock">
     
                <div class="toolBlock_right" style="float:right; width:70%">                
                    <bw:VirtualPager ID="listPager" runat="server" DisplayCurrentPage="true" Font-Names="verdana"
                        Font-Size="9pt" ItemsPerPage="20" PagerStyle="NumericPages" BorderWidth="0px"
                        OnPageIndexChanged="listPager_PageIndexChanged" />
                </div>
            </div>
        </div>
    </div>
    <div style="margin-top:20px"></div>
    <script language="javascript" type="text/javascript">
    // <!CDATA[
    function onToggleSelect(chkName, isChecked){
        var inputArray = document.getElementsByTagName("input");
		for(var i=0; i<inputArray.length; i++) {
			if (inputArray[i].type == "checkbox" && inputArray[i].name.indexOf(chkName) != -1) {
				inputArray[i].checked = isChecked;
			}
		}
    };
    // ]]>
    </script>
    <script type="text/javascript">
    $(document).keypress(function(event){
        if(event.keyCode == 13){
            document.getElementById("<%=btnSearch.ClientID%>").click();
        }
    });
    $("#txtKeyword").keypress(function(event){
        if(event.keyCode == 13){
            this.blur();
            document.getElementById("<%=btnSearch.ClientID%>").click();
        }
    });

    </script>
</asp:Content>
