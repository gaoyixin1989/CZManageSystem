angular.module('filesuploadify', []).directive('filesuploadify', [function () {
    return {
        require: '?ngModel',
        restrict: 'A',
        link: function ($scope, element, attrs, ngModel) {
            //var opts = angular.extend({}, $scope.$eval(attrs.nlUploadify));
            var auth = "@(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value)";
            var ASPSESSID = "@Session.SessionID";
            element.uploadify({
                //'fileObjName': opts.fileObjName || 'upfile',
                'auto': true,
                'swf': '/Content/Javascript/Uploadify/uploadify.swf',
                'uploader': '/Upload/FileUpload',//图片上传方法  
                'buttonText': '添加附件',
                'width': 80,
                'height': 25,
                'cancelImg': '/Content/Javascript/Uploadify/uploadify-cancel.png',
                'method': 'post',
                //'formData': { Upguid: attrs.tempdata },
                'onSelect': function () {//当每个文件添加至队列后触发
                    //alert("Here");
                    //alert(attrs.tempdata);
                },
                'onUploadStart': function (file) {
                    element.uploadify('settings', 'formData', { 'Upguid': attrs.tempdata, 'FilePath': attrs.tempfilepath, ASPSESSID: ASPSESSID, AUTHID: auth });
                },
                'onUploadSuccess': function (file, d, response) {
                    //alert(d);
                    if (ngModel) {
                        var result = eval("[" + d + "]")[0];
                        if (result.IsSuccess == true) {
                            $scope.$apply(function () {
                                //$("#files ul").append("<li><a style='color: Blue;' href='/download.ashx?id=" + attrs.tempdata + "' title='点击下载'>" + file.name + "</a><a href='/OGSM/OGSMUploadDelete?id=" + result.data + "' style=' width:70px' class='lia-3'><i></i>删除</a></li>");
                                //$("#AttachmentTable").append("<tr><td><a style='color: Blue;' href='' title='点击下载'>" + file.name + "</a></td><td><a href='/OGSM/OGSMUploadDelete?id=" + result.data + "' style=' width:70px'>删除</a></td></tr>");
                                $scope.GetAttachmentFileData(result.data);
                            });
                        } else {
                            box.alert("上传失败，请联系管理员！", { icon: 'error' });
                        }
                    }
                }
            });
        }
    };

    //return {
    //    restrict: 'EA',
    //    template: '<div  class="the-text-area">' +
    //        '<input type="file" id="fileupload" name="fileupload" />' +
    //        ' </div> ',

    //};
}]);
angular.module('fileImport', []).directive('importdata', function ($timeout) {
    return {
        restrict: 'A',//属性
        link: function (scope, element, attr) {
            var auth = "@(Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value)";
            var ASPSESSID = "@Session.SessionID";
            //var opts = angular.extend({}, $scope.$eval(attr.nlUploadify));
            element.uploadify({
                formData: { folder: '/Import/ImportFiles', ASPSESSID: ASPSESSID, AUTHID: auth, Data: data },
                auto: false,
                swf: '/Content/Javascript/Uploadify/uploadify.swf',
                uploader: '/Import/ImportFiles?type=' + type,//图片上传方法
                //fileObjName: 'Filedata',//文件上传对象的名称，如果命名为’the_files’,程序可以用Request.Files["the_files"]来处理上传的文件对象。
                //requeueErrors: true,//如果设置为true，则单个任务上传失败后将返回错误，并重新加入任务队列上传。
                buttonText: '选择文件',
                width: 60,
                height: 15,
                successTimeout:600,
                cancelImg: '/Content/Javascript/Uploadify/uploadify-cancel.png',
                method: 'post',
                fileTypeExts: fileTypeExts,//设置可以选择的文件的类型，格式如：’*.doc;*.pdf;*.rar’ 。
                fileTypeDesc: fileTypeDesc,//用来设置选择文件对话框中的提示文本
                multi: true,
                fileSizeLimit: 0,
                removeCompleted: false,
                onSelect: function () {//当每个文件添加至队列后触发
                },
                onUploadStart: function (file) {
                },
                onUploadSuccess: function (file, data, response) {
                    var result = eval("[" + data + "]")[0];
                    //console.info(file);
                    //console.info(data);
                    //console.info(response);
                    var color = 'red';
                    var msg = "";
                    if (result.IsSuccess == true) {
                        color = 'green'; //span.addClass("isSuccess");
                        msg = result.Message;//append(result.Message); 
                    } else if (result.IsSuccess == false) {
                        color = 'red';
                        msg = "导入失败:" + result.Message;
                    }
                    else
                        msg = "导入失败:未知异常！";

                    var span = angular.element("#" + file.id).find(".data")[0];
                    span.style.color = color;
                    span.innerHTML = '- ' + msg; //.append("上传失败:" + result.Message);

                },
                onUploadError: function (file, errorCode, errorMsg, errorString) {
                    console.info(file);
                    console.info(errorCode);
                    console.info(errorMsg);
                    console.info(errorString);
                    var msgText = "上传失败\n";
                    switch (errorCode) {
                        case SWFUpload.UPLOAD_ERROR.HTTP_ERROR:
                            msgText += "HTTP 错误\n" + errorMsg;
                            break;
                        case SWFUpload.UPLOAD_ERROR.MISSING_UPLOAD_URL:
                            msgText += "上传文件丢失，请重新上传";
                            break;
                        case SWFUpload.UPLOAD_ERROR.IO_ERROR:
                            msgText += "IO错误";
                            break;
                        case SWFUpload.UPLOAD_ERROR.SECURITY_ERROR:
                            msgText += "安全性错误\n" + errorMsg;
                            break;
                        case SWFUpload.UPLOAD_ERROR.UPLOAD_LIMIT_EXCEEDED:
                            msgText += "每次最多上传 " + this.settings.uploadLimit + "个";
                            break;
                        case SWFUpload.UPLOAD_ERROR.UPLOAD_FAILED:
                            msgText += errorMsg;
                            break;
                        case SWFUpload.UPLOAD_ERROR.SPECIFIED_FILE_ID_NOT_FOUND:
                            msgText += "找不到指定文件，请重新操作";
                            break;
                        case SWFUpload.UPLOAD_ERROR.FILE_VALIDATION_FAILED:
                            msgText += "参数错误";
                            break;
                        default:
                            msgText += "文件:" + file.name + "\n错误码:" + errorCode + "\n"
                                + errorMsg + "\n" + errorString + "\若导入的文件内容有格式要求，请确保使用正确的格式！";
                    }
                    //console.info("文件:" + file.name + msgText);
                    var span = angular.element("#" + file.id).find(".data")[0];
                    span.style.color = 'red';
                    span.innerHTML = '- ' + "导入失败:" + msgText;//.append("上传失败:" + result.Message);
                }

            });
        }
    };
});
