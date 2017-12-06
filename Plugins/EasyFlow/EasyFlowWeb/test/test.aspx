<%@ Page Language="C#" MasterPageFile="~/plugins/easyflow/masters/Default.master" AutoEventWireup="true" Inherits="test_test" Codebehind="test.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" Runat="Server">
        <fieldset>

    <legend>URL Encode</legend>
    
    <asp:TextBox ID="txtUrl" runat="server" TextMode="MultiLine"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    <br />
    <asp:Label ID="txtEncode" runat="server" />
    </fieldset>
    <a href="#" onclick="onOpen();">##</a>
    
    <div>
    
        <input type="checkbox" id="chkbox" name="box1" value="test" />Test
        <input type="checkbox" id="chkbox1" name="box1" value="test1" />Test1
        <input type="checkbox" id="chkbox2" name="box1" value="test2" />Test2
    </div>
    <script src="../res/js/jquery-latest.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
    function onOpen(){
       window.showModalDialog('http://wwww.baidu.com', new Object(),'dialogWidth:700px;dialogHeight:400px;status:no;');
    }
    var key = "test";
//    alert($("input[name='box1'][value ='" + key + "']").length);
    </script>
    
    <ul style="line-height:20px">
        <li>
            <span>ACTIVITYNAME1</span>
            <div style="text-indent:30px">
               <input type="checkbox" />Admin
               <input type="checkbox" />XIEWEIHONG
               <input type="checkbox" />Admin
               <input type="checkbox" />XIEWEIHONG
               <input type="checkbox" />Admin
               <input type="checkbox" />XIEWEIHONG
            </div>
        </li>
        <li>
            <span>¡ô ACTIVITYNAME1</span>
            <div style="text-indent:30px">
               <input type="checkbox" />Admin
               <input type="checkbox" />XIEWEIHONG
               <input type="checkbox" />Admin
               <input type="checkbox" />XIEWEIHONG
               <input type="checkbox" />Admin
               <input type="checkbox" />XIEWEIHONG
            </div>
        </li>
    </ul>
</asp:Content>

