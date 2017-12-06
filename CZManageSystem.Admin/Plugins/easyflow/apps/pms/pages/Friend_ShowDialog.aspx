<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_pms_pages_Friend_ShowDialog" Codebehind="Friend_ShowDialog.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <script type="text/javascript" src="../../../res/js/jquery-1.7.2.min.js"></script>
    <script>
     var type=   window.dialogArguments;
     function returnval(str) {

            window.returnValue = str;
            window.close();
        }
        function form_submi() {
            if (type == 1) {
                return true;
            }
            if (type == 2) {

                var str = document.getElementById("manageRange").innerHTML.split('\n');
                var val = "";
                for (var i = 0; i < str.length; i++) { 
                val+="<li><div class='item_name' rel=" + str[i] + "><div class='items' ></div><div class='item_del' onclick='$(this).parent().remove();' title='取消选择'></div>" + str[i] + "</div></li>"
                }
            window.returnValue = val;
                window.close();
                return false;
            }
        }
        $(function () {
          
            $("#tr_" + type).show();
        })
    </script>
</head>
<body>
    <form id="form1" runat="server"  enctype="multipart/form-data" target="iframe">
      <div class="plat_box" id="div_box" style="width:70%;margin:30px auto; text-align:left;">
     
      <table class="tblClass">
      <tr id="tr_1" style="display:none"> <span >导入模板:<a href="../../../file/导入模板.txt" >模板文件下载</a></span><th>选择文件:</th><td> <input  type="file"  name="fiel_1"/></td></tr>
      <tr id="tr_2" style="display:none"><td colspan="2" ><div style="height:170px"> <textarea id='manageRange' scrolling="no" cols="62" rows="10"></textarea></div></td></tr>
      </table>
      <input  type="submit"  onclick="return form_submi()" value="确定"/>
            <input type="button" value="返回" style="margin-left:6px" onclick="window.close()" />
      </div>
      <iframe style="display:none" name="iframe" id="iframe"></iframe>
    </form>
</body>
</html>
