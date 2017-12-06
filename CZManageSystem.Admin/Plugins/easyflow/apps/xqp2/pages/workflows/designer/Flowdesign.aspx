<%@ Page Language="C#" AutoEventWireup="true" Inherits="apps_xqp2_pages_workflows_designer_Flowdesign" Codebehind="Flowdesign.aspx.cs" %>

<%@ Register TagPrefix="bw" TagName="Modal" Src="../../../../../apps/xqp2/pages/workflows/designer/Flowdesign/ModalDialog/WorkflowProfile.ascx" %>
<!DOCTYPE HTML>
<html>
<head runat="server">
    <title>WEB流程设计器 = jQuery + jsPlumb + Bootstrap</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="author" content="leipi.org">
    <link href="Flowdesign/Public/css/bootstrap/css/bootstrap.css?2025" rel="stylesheet"
        type="text/css" />
    <!--[if lte IE 6]>
    <link rel="stylesheet" type="text/css" href="Flowdesign/Public/css/bootstrap/css/bootstrap-ie6.css?2025">
    <![endif]-->
    <!--[if lte IE 7]>
    <link rel="stylesheet" type="text/css" href="Flowdesign/Public/css/bootstrap/css/ie.css?2025">
    <![endif]-->
    <link rel="stylesheet" type="text/css" href="Flowdesign/Public/js/flowdesign/flowdesign.css" />
    <!--select 2-->
    <link rel="stylesheet" type="text/css" href="Flowdesign/Public/js/multiselect2side/css/jquery.multiselect2side.css" />
   
    
</head>
<body>
    <form runat="server">
    <style>
    .modal.fade.in{top:5%;}
    </style>
    <!-- fixed navbar -->
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="navbar-inner">
            <div class="container">
                <div class="pull-right">
                    <button class="btn btn-success" type="button" id="Button1" onclick="$('#profileModal').show();$('#profileModal').addClass('active');$('#profileModal').modal('show');">
                        流程设置</button>
                    <button class="btn btn-info" type="button" id="leipi_save">
                        保存设计</button>
                    <button class="btn btn-danger" type="button" id="leipi_clear">
                        清空连接</button>
                    <button class="btn btn-danger" type="button" onclick="window.location='<%=AppPath %>apps/xqp2/pages/workflows/workflowDeploy.aspx'">
                        返回</button>
                </div>
                <div class="nav-collapse collapse">
                    <ul class="nav">
                        <li><a href="javascript:void(0);" id="title">正在设计【<asp:Literal ID="ltlFlowName" runat="server"></asp:Literal>】</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal -->
    <div id="alertModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                ×</button>
            <h3>
                消息提示</h3>
        </div>
        <div class="modal-body">
            <p>
                提示内容</p>
        </div>
        <div class="modal-footer">
            <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">
                我知道了</button>
        </div>
    </div>
    <!-- attributeModal -->
    <div id="attributeModal" class="modal hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true" style="width: 800px; margin-left: -350px">
        <div class="modal-body" style="max-height: 400px;">
            <ul class="nav nav-tabs" id="attributeTab">
                <li id="liBasic"><a href="#attrBasic" url="<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/modaldialog/attrbasic.aspx">常规</a></li>
                <li id="liPower"><a href="#attrPower" url="<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/modaldialog/attrpower.aspx">处理人</a></li>
                <li id="liOperat"><a href="#attrOperate" url="<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/modaldialog/attroperate.aspx">高级设置</a></li>
                <!--id="tab_attrJudge"-->
                <li id="liJudge"><a href="#attrJudge" url="<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/modaldialog/attrjudge.aspx">流转规则</a></li>
            </ul>
            <input type="hidden" name="flow_id" id="flow_id" value="<%=this.WorkflowKey %>"/>
            <input type="hidden" name="process_id" id="process_id" value=""/>
            <div class="tab-content">
            </div>
            <div>
                <hr>
                <span class="pull-right"><a href="#" class="btn" data-dismiss="modal" aria-hidden="true">
                    取消</a>
                    <input class="btn btn-primary" type="button" id="attributeOK" value="确定保存"/>
                </span>
            </div>
            <iframe id="hiddeniframe" style="display: none;" name="hiddeniframe"></iframe>
        </div>
        <div class="modal-footer" style="padding: 5px;">
        </div>
    </div>
    <bw:Modal ID="workflowProfileModal" runat="server" />
    <!--contextmenu div-->
    <div id="processMenu" style="display: none;">
        <ul>
            <!--li id="pm_begin"><i class="icon-play"></i>&nbsp;<span class="_label">设为第一步</span></li-->
            <!--li id="pm_addson"><i class="icon-plus"></i>&nbsp;<span class="_label">添加子步骤</span></li-->
            <!--li id="pm_copy"><i class="icon-check"></i>&nbsp;<span class="_label">复制</span></li-->
            <li id="pmAttribute"><i class="icon-cog"></i>&nbsp;<span class="_label">属性</span></li>
            <%--<li id="pmForm"><i class="icon-th"></i>&nbsp;<span class="_label">表单字段</span></li>--%>
            <li id="pmJudge"><i class="icon-share-alt"></i>&nbsp;<span class="_label">流转规则</span></li>
            <li id="pmDelete"><i class="icon-trash"></i>&nbsp;<span class="_label">删除</span></li>
        </ul>
    </div>
    <div id="canvasMenu" style="display: none;">
        <ul>
            <li id="cmSave"><i class="icon-ok"></i>&nbsp;<span class="_label">保存设计</span></li>
            <li id="cmAdd"><i class="icon-plus"></i>&nbsp;<span class="_label">添加步骤</span></li>
            <li id="cmRefresh"><i class="icon-refresh"></i>&nbsp;<span class="_label">刷新 F5</span></li>
            <!--li id="cmPaste"><i class="icon-share"></i>&nbsp;<span class="_label">粘贴</span></li-->
            <%--<li id="cmHelp"><i class="icon-search"></i>&nbsp;<span class="_label">帮助</span></li>--%>
        </ul>
    </div>
    <!--end div-->
    <div class="container mini-layout" id="flowdesign_canvas">
        <!--div class="process-step btn" style="left: 189px; top: 340px;"><span class="process-num badge badge-inverse"><i class="icon-star icon-white"></i>3</span> 步骤3</div-->
    </div>
    <!-- /container -->
     <link rel="stylesheet" type="text/css" href="<%=AppPath %>App_Themes/gmcc/showLoading.css" />
    <script type="text/javascript" src="Flowdesign/Public/js/jquery-1.7.2.min.js?2025"></script>
    <script type="text/javascript" src="<%=AppPath %>res/js/jquery.showLoading.js"></script>
    <script type="text/javascript" src="Flowdesign/Public/css/bootstrap/js/bootstrap.min.js?2025"></script>
    <script type="text/javascript" src="Flowdesign/Public/js/jquery-ui/jquery-ui-1.9.2-min.js?2025"></script>
    <script type="text/javascript" src="Flowdesign/Public/js/jsPlumb/jquery.jsPlumb-1.3.16-all-min.js?2025"></script>
    <script type="text/javascript" src="Flowdesign/Public/js/jquery.contextmenu.r2.js?2025"></script>
    <!--select 2-->
    <script type="text/javascript" src="Flowdesign/Public/js/multiselect2side/js/jquery.multiselect2side.js?2025"></script>
    <!--flowdesign-->
    <script type="text/javascript" src="Flowdesign/Public/js/flowdesign/leipi.flowdesign.v3.js?2025"></script>
    <script type="text/javascript" src="Flowdesign/Public/js/flowdesign/attribute.js?2025"></script>
    <script type="text/javascript" src="<%=AppPath %>res/js/Base64.js"></script>
    <script type="text/javascript" src="<%=AppPath %>res/js/jquery.json.min.js"></script>
    <script type="text/javascript">
        var the_flow_id = '4';
        var ProcessData = {};
        var ActivityTemplate = {}; //步骤信息模板
        var AssignmentTemplate = {}; //转交设置信息模板
        var maxid = 0;
        /*页面回调执行    callbackSuperDialog
        if(window.ActiveXObject){ //IE  
        window.returnValue = globalValue
        }else{ //非IE  
        if(window.opener) {  
        window.opener.callbackSuperDialog(globalValue) ;  
        }
        }  
        window.close();
        */
        function callbackSuperDialog(selectValue) {
            var aResult = selectValue.split('@leipi@');
            $('#' + window._viewField).val(aResult[0]);
            $('#' + window._hidField).val(aResult[1]);
            //document.getElementById(window._hidField).value = aResult[1];

        }
        /**
        * 弹出窗选择用户部门角色
        * showModalDialog 方式选择用户
        * URL 选择器地址
        * viewField 用来显示数据的ID
        * hidField 隐藏域数据ID
        * isOnly 是否只能选一条数据
        * dialogWidth * dialogHeight 弹出的窗口大小
        */
        function superDialog(URL, viewField, hidField, isOnly, dialogWidth, dialogHeight) {
            dialogWidth || (dialogWidth = 620)
    , dialogHeight || (dialogHeight = 520)
    , loc_x = 500
    , loc_y = 40
    , window._viewField = viewField
    , window._hidField = hidField;
            // loc_x = document.body.scrollLeft+event.clientX-event.offsetX;
            //loc_y = document.body.scrollTop+event.clientY-event.offsetY;
            if (window.ActiveXObject) { //IE  
                var selectValue = window.showModalDialog(URL, self, "edge:raised;scroll:1;status:0;help:0;resizable:1;dialogWidth:" + dialogWidth + "px;dialogHeight:" + dialogHeight + "px;dialogTop:" + loc_y + "px;dialogLeft:" + loc_x + "px");
                if (selectValue) {
                    callbackSuperDialog(selectValue);
                }
            } else {  //非IE 
                var selectValue = window.open(URL, 'newwindow', 'height=' + dialogHeight + ',width=' + dialogWidth + ',top=' + loc_y + ',left=' + loc_x + ',toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no');

            }
        }




        $(document).ready(function () {
            $("#flowdesign_canvas").showLoading();
            var alertModal = $('#alertModal'), attributeModal = $("#attributeModal");
            //消息提示
            mAlert = function (messages, s) {
                if (!messages) messages = "";
                if (!s) s = 2000;
                alertModal.find(".modal-body").html(messages);
                alertModal.modal('toggle');
                setTimeout(function () { alertModal.modal("hide") }, s);
            }

            //属性设置
            attributeModal.on("hidden", function () {
                $(this).removeData("modal"); //移除数据，防止缓存
            });
            ajaxModal = function (url, processInfo, fn) {
                url += url.indexOf('?') ? '&' : '?';
                url += '_t=' + new Date().getTime();
                //attributeModal.find(".modal-body").html('<img src="Flowdesign/Public/images/loading.gif"/>');
                attributeModal.modal("show");
                attributeModal.find(".tab-content").html('<img src="<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/Public/images/loading.gif"/>');
                var aid = $("#process_id").val();
                var flowid = $("#flow_id").val();
                var processInfoJSON = JSON.stringify(processInfo);

                $.ajax({
                    type: "post",
                    dataType: "html",
                    url: url,
                    data: { wid: flowid, aid: aid, ProcessData: processInfoJSON },
                    async: true,
                    timeout: 300000,
                    success: function (data) {
                        attributeModal.find(".tab-content").html(data);
                    },
                    error: function () {
                    },
                    complete: function () {
                        fnSetPower(aid);
                        fnSetCondition();
                    }

                });
                /*attributeModal.modal({
                remote:url
                });*/
                //加载完成执行
                if (fn) {
                    attributeModal.on('shown', fn);
                }

            }
            //刷新页面
            function page_reload() {
                location.reload();
            }



            /*
            php 命名习惯 单词用下划线_隔开
            js 命名习惯：首字母小写 + 其它首字线大写
            */
            /*步骤数据*/
            /*var processData = { "total": 6,  "flow_id": "4","list": [{ "id": "61", "process_type": "start-activity", "process_name": "\u53d1\u8d77\u7533\u8bf7", "process_to": "63,64", "icon": "icon-ok", "style": "width:121px;height:41px;line-height:41px;color:#0e76a8;left:193px;top:132px;" }
            , { "id": "62", "process_type": "activity", "process_name": "\u5ba1\u62792", "process_to": "65", "icon": "icon-star", "style": "width:120px;height:30px;line-height:30px;color:#0e76a8;left:486px;top:337px;" }
            , { "id": "63", "process_type": "activity", "process_name": "\u5feb\u6377\u5ba1\u6279", "process_to": "65", "icon": "icon-star", "style": "width:120px;height:30px;line-height:30px;color:#0e76a8;left:193px;top:472px;" }
            , { "id": "64", "process_type": "activity", "process_name": "\u5ba1\u62791", "process_to": "62,65", "icon": "icon-star", "style": "width:120px;height:30px;line-height:30px;color:#ff66b5;left:486px;top:137px;" }
            , { "id": "65", "process_type": "activity", "process_name": "\u5f52\u6863\u6574\u7406\u4eba", "process_to": "66", "icon": "icon-star", "style": "width:120px;height:30px;line-height:30px;color:#0e76a8;left:738px;top:472px;" }
            , { "id": "66", "process_type": "end-activity", "process_name": "完成", "process_to": "", "icon": "icon-stop", "style": "width:121px;height:41px;line-height:41px;color:#0e76a8;left:737px;top:603px;"}]
            };*/

            /*
            * 初始化流程设置
            */
            var FlowInit = function (processInfo) {
                ProcessData = processInfo;
                /*创建流程设计器*/
                var _canvas = $("#flowdesign_canvas").Flowdesign({
                    "processData": ProcessData
                    /*,mtAfterDrop:function(params)
                    {
                    //alert("连接："+params.sourceId +" -> "+ params.targetId);
                    }*/
                    /*画面右键*/
                      , canvasMenus: {
                          "cmAdd": function (t) {
                              var mLeft = $("#jqContextMenu").css("left"), mTop = $("#jqContextMenu").css("top");

                              /*重要提示 start*/
                              //alert("这里使用ajax提交，请参考官网示例，可使用Fiddler软件抓包获取返回格式");
                              /*重要提示 end */

                              var url = "/index.php?s=/Flowdesign/add_process.html";
                              var data = {};
                              data.info = { flow_id: "<%=this.WorkflowKey %>", icon: "", id: (maxid + 1), process_name: "新建步骤" + (maxid + 1), process_type: "activity", process_to: "", style: "left:" + mLeft + ";top:" + mTop + ";color:#0e76a8;" };

                              _canvas.addProcess(data.info);
                              maxid = maxid + 1;
                              /*$.post(url, { "flow_id": the_flow_id, "left": mLeft, "top": mTop }, function (data) {

                              if (!data.status) {
                              mAlert(data.msg);
                              } else if (!_canvas.addProcess(data.info))//添加
                              {
                              mAlert("添加失败");
                              }

                              }, 'json');*/

                          },
                          "cmSave": function (t) {
                              var processInfo = _canvas.getProcessInfo(); //连接信息

                              /*重要提示 start*/
                              alert("这里使用ajax提交，请参考官网示例，可使用Fiddler软件抓包获取返回格式");
                              /*重要提示 end */

                              var url = "/index.php?s=/Flowdesign/save_canvas.html";
                              $.post(url, { "flow_id": the_flow_id, "process_info": processInfo }, function (data) {
                                  mAlert(data.msg);
                              }, 'json');
                          },
                          //刷新
                          "cmRefresh": function (t) {
                              location.reload(); //_canvas.refresh();
                          },
                          /*"cmPaste": function(t) {
                          var pasteId = _canvas.paste();//右键当前的ID
                          if(pasteId<=0)
                          {
                          alert("你未复制任何步骤");
                          return ;
                          }
                          alert("粘贴:" + pasteId);
                          },*/
                          "cmHelp": function (t) {
                              mAlert('<ul><li><a href="http://flowdesign.leipi.org/doc.html" target="_blank">流程设计器 开发文档</a></li><li><a href="http://formdesign.leipi.org/doc.html" target="_blank">表单设计器 开发文档</a></li><li><a href="http://formdesign.leipi.org/demo.html" target="_blank">表单设计器 示例DEMO</a></li></ul>', 20000);
                          }

                      }
                    /*步骤右键*/
                      , processMenus: {
                          "pmDelete": function (t) {
                              if (confirm("你确定删除步骤吗？")) {
                                  var activeId = _canvas.getActiveId(); //右键当前的ID

                                  //var $activity = $("div[activityid='" + activeId + "']");
                                  //var process_to = $activity.attr("process_to");
                                  //$("#leipi_process_info input[type=hidden]").each(function (i) {
                                  //$("div[activityid='" + activeId + "']").remove();

                                  //ProcessData = processInfo;
                                  //_canvas.clear();
                                  //$("#flowdesign_canvas").empty(); //移除数据，防止缓存
                                  //_canvas.refresh();
                                  // FlowInit(processInfo);
                                  //重新设置节点的上下级关系
                                  $('div.process-step').attr("process_to", "");
                                  $("#leipi_process_info input[type=hidden]").each(function (i) {
                                      var processVal = $(this).val().split(",");
                                      if (processVal.length == 2) {
                                          //aProcessData[processVal[0]]["process_to"].push(processVal[1]);
                                          var process_to = $('#window' + processVal[0]).attr('process_to');
                                          $('#window' + processVal[0]).attr('process_to', (process_to == "" ? "" : process_to + ",") + processVal[1]);
                                      }
                                  });
                                  _canvas.clear();
                                  $("div[activityid='" + activeId + "']").remove();
                                  var processInfo = _canvas.getProcessInfo(); //连接信息

                                  _canvas.delProcess(processInfo);
                              }
                          },
                          "pmAttribute": function (t) {
                              $("#attributeTab li").removeClass("active");
                              $("#liBasic").addClass("active");
                              var activeId = _canvas.getActiveId(); //右键当前的ID
                              var processInfo = _canvas.getProcessInfo(); //连接信息
                              var flowid = $("#flow_id").val();
                              $("#process_id").val(activeId);
                              var url = '<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/modaldialog/attribute.aspx?wid=' + flowid + '&aid=' + activeId;
                              ajaxModal(url, processInfo, function () {
                                  //alert('加载完成执行')
                              });
                          },
                          "pmJudge": function (t) {
                              var activeId = _canvas.getActiveId(); //右键当前的ID
                              var processInfo = _canvas.getProcessInfo(); //连接信息
                              $("#attributeTab li").removeClass("active");
                              $("#liJudge").addClass("active");
                              $("#liJudge a").tab('show');
                              var flowid = $("#flow_id").val();
                              $("#process_id").val(activeId);
                              var url = '<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/modaldialog/attribute.aspx?wid=' + flowid + '&aid=' + activeId + "&type=judge";
                              ajaxModal(url, processInfo, function () {
                                  //alert('加载完成执行')
                              });
                          }
                      }
                      , fnRepeat: function () {
                          //alert("步骤连接重复1");//可使用 jquery ui 或其它方式提示
                          mAlert("步骤连接重复了，请重新连接");

                      }
                    /*, fnClick: function () {
                    var activeId = _canvas.getActiveId();
                    mAlert("查看步骤信息 " + activeId);
                    }*/
                      , fnDbClick: function () {
                          //和 pmAttribute 一样
                          var activeId = _canvas.getActiveId(); //右键当前的ID
                          $("#attributeTab li").removeClass("active");
                          $("#liBasic").addClass("active");
                          var activeId = _canvas.getActiveId(); //右键当前的ID
                          var processInfo = _canvas.getProcessInfo(); //连接信息
                          var flowid = $("#flow_id").val();
                          $("#process_id").val(activeId);
                          var url = '<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/modaldialog/attribute.aspx?wid=' + flowid + '&aid=' + activeId;
                          ajaxModal(url, processInfo, function () {
                              //alert('加载完成执行')

                          });
                      }, fnClick: function () {
                          var activeId = _canvas.getActiveId(); //右键当前的ID#B3B2B2
                          $("div.process-step").removeClass("checked");
                          $("div[activityid = '" + activeId + "']").addClass("checked"); //选中样式
                      }
                });

                /*保存*/
                $("#leipi_save").bind('click', function () {
                    if (confirm("确认保存？")) {
                        $("#flowdesign_canvas").showLoading();
                        var processInfo = _canvas.getProcessInfo(); //连接信息
                        var activeId = _canvas.getActiveId();
                        _canvas.saveAttribute(activeId);
                        var processInfoJSON = JSON.stringify(processInfo);
                        $.ajax({
                            type: "post",
                            dataType: "json",
                            contentType: "application/json",
                            //dataType: "json", 
                            url: "DesignerService.asmx/SaveWorkflowJson",
                            data: JSON.stringify({ jsonData: processInfoJSON }),
                            async: true,
                            success: function (data) {
                                var json = $.parseJSON(data.d);
                                if (json.result == "success") {
                                    if (confirm("流程保存成功，是否立即设置？"))
                                        window.location = "<%=AppPath %>apps/xqp2/pages/workflows/config/ConfigWorkflow.aspx?wid=" + json.info;
                                }
                                else
                                    mAlert(json.info);
                                $("#flowdesign_canvas").hideLoading();
                            },
                            error: function (e) {
                                console.info(e.info)
                            }

                        });
                    }
                });
                /*清除*/
                $("#leipi_clear").bind('click', function () {
                    if (_canvas.clear()) {
                        //alert("清空连接成功");
                        mAlert("清空连接成功，你可以重新连接");
                    } else {
                        //alert("清空连接失败");
                        mAlert("清空连接失败");
                    }
                });
                /*保存*/
                $("#attributeOK").bind('click', function () {
                    var processInfo = _canvas.getProcessInfo(); //连接信息
                    var activeId = _canvas.getActiveId();
                    _canvas.saveAttribute(activeId);
                    attributeModal.modal("hide")
                });
                $("body").click(function () {
                    $("div.process-step").removeClass("checked");
                });
            }
            $.post("<%=AppPath %>apps/xqp2/pages/workflows/designer/flowdesign/ajax/processdataajax.aspx", { "wid": "<%=this.WorkflowKey %>" }, function (data) {
                FlowInit(data);
                $("#flowdesign_canvas").hideLoading();
            }, 'json');
        });

 
    </script>
    </form>
</body>
</html>
