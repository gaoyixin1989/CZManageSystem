<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="contrib_workflow_pages_WorkflowAttachment" Codebehind="WorkflowAttachment.aspx.cs" %>

<%@ Register TagPrefix="bw" TagName="Attachments" Src="../controls/Attachments.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>流程附件信息</title>
</head>
<body style="background: none !important;">
    <form id="form1" runat="server">
    <div>
        <bw:Attachments ID="atta1" runat="server" />
    </div>
    <script src="<%=AppPath %>res/js/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link href="<%=AppPath %>res/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script src="<%=AppPath %>res/uploadify/jquery.uploadify-3.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
             $("#atta1_trFile").show();
                var main = $(window.parent.document).find("#ifAttachment");
                 var thisheight = $("#atta1_trFile").height()+30;
                 $(".tblGrayClass").each(function(){
                    thisheight+=$(this).height();
                 })
                
                main.height(thisheight);
            
           
            try{
            $("#atta1_fileUpload2").uploadify({
                height: 15,
                swf: '<%=AppPath %>res/uploadify/uploadify.swf',
                uploader: '<%=AppPath %>contrib/workflow/uploadify/UploadAttachment.ashx',
                //checkExisting: '/uploader/uploadify-check-existing.php', //检查上传文件是否存，触发的url，返回1/0
                width: 120,
                cancelImg: '<%=AppPath %>res/uploadify/uploadify-cancel.png',
                buttonText: '添加附件',
                //fileTypeExts: '*.7z;*.bmp;*.doc;*.docx;*.fla;*.flv;*.gif;*.gzip;*.jpeg;*.jpg;*.mid;*.mpeg;*.mpg;*.pdf;*.png;*.ppt;*.pptx;*.pxd;*.ram;*.rar;*.rtf;*.swf;*.tgz;*.tif;*.tiff;*.txt;*.vsd;*.xls;*.xlsx;*.xml;*.zip"',
                //fileSizeLimit: '20MB',
                uploadLimit: 5,
                removeCompleted: true,
                removeTimeout: 1,
                auto: true,
                multi: true,
                method: 'post',
                formData: { 'wiid': "<%=this.Request.QueryString["wiid"] %>" },
                onDialogClose: function (swfuploadifyQueue) {//当文件选择对话框关闭时触发
                },
                onDialogOpen: function () {//当选择文件对话框打开时触发
                    //alert('Open!');
                },
                onSelect: function (file) {//当每个文件添加至队列后触发
                   var main = $(window.parent.document).find("#ifAttachment");
                    var thisheight = $("#atta1_trFile").height()+30;
                    $(".tblGrayClass").each(function(){
                        thisheight+=$(this).height();
                    });
                
                    main.height(thisheight);
                },
                onSelectError: function (file, errorCode, errorMsg) {//当文件选定发生错误时触发
                },
                onQueueComplete: function (stats) {//当队列中的所有文件全部完成上传时触发
                    window.location = "<%=Request.RawUrl %>";
                },
                onUploadSuccess: function (file, data, response) {//上传完成时触发（每个文件触发一次）
                    if (data.indexOf('错误提示') > -1) {
                        alert(data);
                    }
                    else {

                    }
                },
                onUploadError: function (file, errorCode, errorMsg, errorString) {//当单个文件上传出错时触发
                    alert('文件：' + file.name + ' 上传失败: ' + errorString);
                }
            });
            }
            catch(e){}
        });
    </script>
    </form>
</body>
</html>
