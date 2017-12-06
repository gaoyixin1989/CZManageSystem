/*
项目：雷劈网流程设计器
官网：http://flowdesign.leipi.org
Q 群：143263697
基本协议：apache2.0

88888888888  88                             ad88  88                ad88888ba   8888888888   
88           ""                            d8"    88               d8"     "88  88           
88                                         88     88               8P       88  88  ____     
88aaaaa      88  8b,dPPYba,   ,adPPYba,  MM88MMM  88  8b       d8  Y8,    ,d88  88a8PPPP8b,  
88"""""      88  88P'   "Y8  a8P_____88    88     88  `8b     d8'   "PPPPPP"88  PP"     `8b  
88           88  88          8PP"""""""    88     88   `8b   d8'            8P           d8  
88           88  88          "8b,   ,aa    88     88    `8b,d8'    8b,    a8P   Y8a     a8P  
88           88  88           `"Ybbd8"'    88     88      Y88'     `"Y8888P'     "Y88888P"   
                                                          d8'                                
2014-3-15 Firefly95、xinG  
*/
(function ($) {
    var defaults = {
        processData: {}, //步骤节点数据
        //processUrl:'',//步骤节点数据
        fnRepeat: function () {
            alert("步骤连接重复");
        },
        fnClick: function () {
            alert("单击");
        },
        fnDbClick: function () {
            alert("双击");
        },
        canvasMenus: {
            "one": function (t) { alert('画面右键') }
        },
        processMenus: {
            "one": function (t) { alert('步骤右键') }
        },
        /*右键菜单样式*/
        menuStyle: {
            border: '1px solid #5a6377',
            minWidth: '150px',
            padding: '5px 0'
        },
        itemStyle: {
            fontFamily: 'verdana',
            color: '#333',
            border: '0',
            /*borderLeft:'5px solid #fff',*/
            padding: '5px 40px 5px 20px'
        },
        itemHoverStyle: {
            border: '0',
            /*borderLeft:'5px solid #49afcd',*/
            color: '#fff',
            backgroundColor: '#5a6377'
        },
        mtAfterDrop: function (params) {
            //alert('连接成功后调用');
            //alert("连接："+params.sourceId +" -> "+ params.targetId);
        },
        //这是连接线路的绘画样式
        connectorPaintStyle: {
            lineWidth: 3,
            strokeStyle: "#49afcd",
            joinstyle: "round"
        },
        //鼠标经过样式
        connectorHoverStyle: {
            lineWidth: 3,
            strokeStyle: "#da4f49"
        },
        reset: function () {
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
        },
        //获取流程步骤下行分派值.
        getActivityAllocatorValues: function (activityId) {
            var $superiorArgs = $("[name = chkOrgArgs]:checkbox:checked");
            var $roleArgs = $("[name = chkRoleArgs]:checkbox:checked");
            var $nodeDiv = $("div[activityid = '" + activityId + "']");
            var $activity = $("#activity" + activityId);
            var $assignment = $("#assignment" + activityId);
            //activity.CommandRules = this.txtCommandRules.Text;
            var activityName = $activity.attr("activityname");
            $(".process-step div[activityname]").each(function () {
                if ($("#txtActivityName").val() == $(this).attr("activityname") && $(this).attr("activityname") != activityName) {
                    mAlert("步骤名称重复了");
                    return;
                }
            })
            $activity.attr("activityname", $("#txtActivityName").val());
            $activity.prev("span").text($("#txtActivityName").val());
            if ($("#radOpenPrint").attr("checked"))
                $activity.attr("CanPrint", 1);
            if ($("#radClosePrint").attr("checked"))
                $activity.attr("CanPrint", -1);
            $activity.attr("PrintAmount", $("#txtPrintAmount").val());
            if ($("#chkOption").attr("checked"))
                $activity.attr("CanEdit", 1);
            else
                $activity.attr("CanEdit", -1);
            $activity.attr("ReturnToPrev", $("#chkReturn").attr("checked") ? true : false);
            $activity.attr("IsMobile", $("#chkIsMobile").attr("checked") ? true : false);
            if ($("#ddlrejectOption").val() == "customize")
                $activity.attr("rejectOption", $("#ddlcustomize").val());
            else
                $activity.attr("rejectOption", $("#ddlrejectOption").val());
            $activity.attr("joincondition", $("#txtjoincondition").val());
            $activity.attr("splitcondition", $("#txtsplitcondition").val());
            $activity.attr("countersignedcondition", $("#txtcountersignedcondition").val());
            $activity.attr("DefaultAllocator", $("#selDefaultTypes").val());
            $activity.attr("AllocatorUsers", $("#chkUsers").attr("checked") ? $("#txtUsers").val() : null);
            var ResourceId = $activity.attr("AllocatorResource");
            $activity.attr("AllocatorResource", formatResourceId(ResourceId, $("#chkRes").attr("checked")));

            var extendAllocators = "";
            var extendAllocatorArgs = "";
            if ($("#chkStarter").attr("checked")) {
                extendAllocators += ",starter";
            }
            if ($("#chkPssor").attr("checked")) {
                extendAllocators += ",processor";
            }
            if ($("#chkPssctl").attr("checked")) {
                extendAllocators += ",processctl";
                if ($("#drdlPssor").val() != "") {
                    extendAllocatorArgs += ";processctl:" + $("#drdlPssor").val();
                }
            }
            if ($("#chkOrg").attr("checked")) {
                extendAllocators += ",superior";
                if ($superiorArgs.length > 0) {
                    var superiorArgs = "";
                    $superiorArgs.each(function () {
                        superiorArgs += $(this).val() + ","
                    });
                    extendAllocatorArgs += ";superior:" + superiorArgs;
                }
            }
            if ($("#chkRole").attr("checked")) {
                var roleArgs = "";
                $roleArgs.each(function () {
                    roleArgs += $(this).val() + ","
                });
                extendAllocators += ",role";
                extendAllocatorArgs += ";role:" + roleArgs;
            }
            if ($("#chkControl").attr("checked")) {
                extendAllocators += ",activity";
                if ($("#drdlActivities").val() != "") {
                    extendAllocatorArgs += ";activity:" + $("#drdlActivities").val();
                }
            }
            if (extendAllocators.length > 1)
                extendAllocators = extendAllocators.substring(1);
            if (extendAllocatorArgs.length > 1)
                extendAllocatorArgs = extendAllocatorArgs.substring(1);

            $activity.attr("ExtendAllocators", extendAllocators);
            $activity.attr("ExtendAllocatorArgs", extendAllocatorArgs);
        },
        getAssignAllocatorValues: function (activityId) {
            // starter,processor,superior
            var $assignSuperiorArgs = $("[name = chkOrgArgsAssign]:checkbox:checked");
            var $assighRoleArgs = $("[name = chkRoleAssignArgs]:checkbox:checked");
            var $nodeDiv = $("div[activityid = '" + activityId + "']");
            var $activity = $("#activity" + activityId);
            var $assignment = $("#assignment" + activityId);
            $assignment.attr("DefaultAllocator", $("#selAssignDefaultTypes").val());
            $assignment.attr("AllocatorUsers", $("#chkUsersAssign").attr("checked") ? $("#txtUsersAssign").val() : null);
            var ResourceId = $assignment.attr("AllocatorResource");
            $assignment.attr("AllocatorResource", formatResourceId(ResourceId, $("#chkResAssign").attr("checked")));

            var extendAllocators = "";
            var extendAllocatorArgs = "";
            if ($("#chkStarterAssign").attr("checked")) {
                extendAllocators += ",starter";
            }
            if ($("#chkPssorAssign").attr("checked")) {
                extendAllocators += ",processor";
            }
            if ($("#chkPssctlAssign").attr("checked")) {
                extendAllocators += ",processctl";
                if ($("#drdlPssorAssign").val() != "") {
                    extendAllocatorArgs += ";processctl:" + $("#drdlPssorAssign").val();
                }
            }
            if ($("#chkOrgAssign").attr("checked")) {
                extendAllocators += ",superior";
                if ($assignSuperiorArgs.length > 0) {
                    var superiorArgs = "";
                    $assignSuperiorArgs.each(function () {
                        superiorArgs += $(this).val() + ","
                    });
                    extendAllocatorArgs += ";superior:" + superiorArgs;
                }
            }
            if ($("#chkRoleAssign").attr("checked")) {
                //extendAllocators += ",role";
                var roleArgs = "";
                $assighRoleArgs.each(function () {
                    roleArgs += $(this).val() + ","
                });
                extendAllocators += ",role";
                extendAllocatorArgs += ";role:" + roleArgs;
            }
            if ($("#chkControlAssign").attr("checked")) {
                extendAllocators += ",activity";
                if ($("#drdlActivitiesAssign").val() != "") {
                    extendAllocatorArgs += ";activity:" + $("#drdlActivitiesAssign").val();
                }
            }
            if (extendAllocators.length > 1)
                extendAllocators = extendAllocators.substring(1);
            if (extendAllocatorArgs.length > 1)
                extendAllocatorArgs = extendAllocatorArgs.substring(1);

            $assignment.attr("ExtendAllocators", extendAllocators);
            $assignment.attr("ExtendAllocatorArgs", extendAllocatorArgs);
        }
    }; /*defaults end*/

    var formatResourceId = function (resourceId, isChecked) {
        if (resourceId == "" || typeof (resourceId) == "undefined")
            return "";

        var results = resourceId.toUpperCase();
        var isContain = results.indexOf("#NONE#_");
        var disablePattern = "#NONE#_";

        if (isChecked) {
            // 允许权限控制
            return (isContain ? results.replace(disablePattern, "") : results);
        }
        else {
            return (isContain ? results : disablePattern + results);
        }
    }
    var initEndPoints = function () {
        $(".process-flag").each(function (i, e) {
            var p = $(e).parent();
            jsPlumb.makeSource($(e), {
                parent: p,
                anchor: "Continuous",
                endpoint: ["Dot", { radius: 1}],
                connector: ["Flowchart", { stub: [5, 5]}],
                connectorStyle: defaults.connectorPaintStyle,
                hoverPaintStyle: defaults.connectorHoverStyle,
                dragOptions: {},
                maxConnections: -1
            });
        });
    }

    /*设置隐藏域保存关系信息*/
    var aConnections = [];
    var setConnections = function (conn, remove) {
        if (!remove) aConnections.push(conn);
        else {
            var idx = -1;
            for (var i = 0; i < aConnections.length; i++) {
                if (aConnections[i] == conn) {
                    idx = i; break;
                }
            }
            if (idx != -1) aConnections.splice(idx, 1);
        }
        if (aConnections.length > 0) {
            var s = "";
            for (var j = 0; j < aConnections.length; j++) {
                var from = $('#' + aConnections[j].sourceId).attr('process_id');
                var target = $('#' + aConnections[j].targetId).attr('process_id');
                var hid = "<input type='hidden' value=\"" + from + "," + target + "\">";
                //if (!s.indexOf(hid) > -1)
                s = s + hid

                // if ($("#leipi_process_info input[value='" + from + "," + target + "']").length == 0)
                //$('#leipi_process_info').append(hid);
            }
            $('#leipi_process_info').html(s);
        } else {
            $('#leipi_process_info').html('');
        }
        jsPlumb.repaintEverything(); //重画
    };

    /*Flowdesign 命名纯粹为了美观，而不是 formDesign */
    $.fn.Flowdesign = function (options) {
        var _canvas = $(this);
        //右键步骤的步骤号
        _canvas.append('<input type="hidden" id="leipi_active_id" value="0"/><input type="hidden" id="leipi_copy_id" value="0"/>');
        _canvas.append('<div id="leipi_process_info"></div>');


        /*配置*/
        $.each(options, function (i, val) {
            if (typeof val == 'object' && defaults[i])
                $.extend(defaults[i], val);
            else
                defaults[i] = val;
        });
        /*画布右键绑定*/
        var contextmenu = {
            bindings: defaults.canvasMenus,
            menuStyle: defaults.menuStyle,
            itemStyle: defaults.itemStyle,
            itemHoverStyle: defaults.itemHoverStyle
        }
        $(this).contextMenu('canvasMenu', contextmenu);

        jsPlumb.importDefaults({
            DragOptions: { cursor: 'pointer' },
            EndpointStyle: { fillStyle: '#225588' },
            Endpoint: ["Dot", { radius: 1}],
            ConnectionOverlays: [
                ["Arrow", { location: 1}],
                ["Label", {
                    location: 0.1,
                    id: "label",
                    cssClass: "aLabel"
                }]
            ],
            Anchor: 'Continuous',
            ConnectorZIndex: 5,
            HoverPaintStyle: defaults.connectorHoverStyle
        });
        if ($.browser.msie && $.browser.version < '9.0') { //ie9以下，用VML画图
            jsPlumb.setRenderMode(jsPlumb.VML);
        } else { //其他浏览器用SVG
            jsPlumb.setRenderMode(jsPlumb.SVG);
        }


        //初始化原步骤
        var lastProcessId = 0;
        var processData = defaults.processData;
        var b = new Base64();
        if (processData.list) {
            aConnections = [];
            $.each(processData.list, function (i, row) {
                maxid++;
                var nodeDiv = document.createElement('div');
                var activityDiv = document.createElement('div');
                var assignmentDiv = document.createElement('div');
                var nodeId = "window" + row.id, badge = 'badge-inverse', icon = 'icon-star';
                if (lastProcessId == 0)//第一步
                {
                    badge = 'badge-info';
                    icon = 'icon-play';
                }
                if (row.process_to == "") //最后一步
                {
                    badge = 'badge-info';
                    icon = 'icon-play';
                }
                if (row.icon) {
                    icon = row.icon;
                }
                //if (row.process_type == "end-activity")//最后一步
                //maxid = maxid == 0 ? parseInt(row.id) : maxid;
                $(nodeDiv).attr("id", nodeId)
            .attr("style", row.style)
            .attr("process_type", row.process_type)
            .attr("process_to", row.process_to)
            .attr("process_id", row.id)
            .attr("activityid", row.process_id)
            .addClass("process-step btn btn-small")
            .html('<span class="process-flag badge ' + badge + '"><i class="' + icon + ' icon-white"></i></span>&nbsp;<span>' + row.process_name + "</span>")
            .mousedown(function (e) {
                if (e.which == 3) { //右键绑定
                    _canvas.find('#leipi_active_id').val(row.process_id);
                    contextmenu.bindings = defaults.processMenus;
                    if (row.process_type == "start-activity" || row.process_type == "end-activity") {//开始和完成步骤不能删除
                        $("#pmDelete").remove();
                    }
                    else if ($("#pmDelete") && $("#pmDelete").length == 0) {
                        $("#processMenu ul").append("<li id=\"pmDelete\"><i class=\"icon-trash\"></i>&nbsp;<span class=\"_label\">删除</span></li>");
                    }
                    $(this).contextMenu('processMenu', contextmenu);
                }
            });
                $(activityDiv).attr("id", "activity" + row.process_id)
            .attr("style", "display:none");
                var activity = row.activity;
                for (var item in activity) {
                    $(activityDiv).attr(item, activity[item]);
                }
                if ($.toJSON(ActivityTemplate) == "{}") {
                    for (var item in activity) {
                        ActivityTemplate[item] = null;
                    }
                    //console.info(ActivityTemplate)
                }
                $(activityDiv).attr("rules", row.rules);
                $(assignmentDiv).attr("id", "assignment" + row.process_id)
            .attr("style", "display:none");
                var assignment = row.assignment;
                for (var item in assignment) {
                    $(assignmentDiv).attr(item, assignment[item]);
                }
                if ($.toJSON(AssignmentTemplate) == "{}") {
                    for (var item in assignment) {
                        AssignmentTemplate[item] = null;
                    }
                    //console.info(AssignmentTemplate)
                }
                //activityDiv.innerHTML = jQuery.parseJSON(activity);
                nodeDiv.appendChild(activityDiv);
                nodeDiv.appendChild(assignmentDiv);
                _canvas.append(nodeDiv);
                //索引变量
                lastProcessId = row.id;
            }); //each
        }

        var timeout = null;
        //点击或双击事件,这里进行了一个单击事件延迟，因为同时绑定了双击事件
        $(".process-step").live('click', function () {
            //激活
            _canvas.find('#leipi_active_id').val($(this).attr("activityid")),
        clearTimeout(timeout);
            var obj = this;
            timeout = setTimeout(defaults.fnClick, 300);
        }).live('dblclick', function () {
            clearTimeout(timeout);
            defaults.fnDbClick();
        });

        //使之可拖动
        jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
        initEndPoints();

        //绑定添加连接操作。画线-input text值  拒绝重复连接
        jsPlumb.bind("jsPlumbConnection", function (info) {
            setConnections(info.connection);
        });
        //绑定删除connection事件
        jsPlumb.bind("jsPlumbConnectionDetached", function (info) {
            setConnections(info.connection, true); ;
        });
        //绑定删除确认操作
        jsPlumb.bind("click", function (c) {
            if (confirm("你确定取消连接吗?")) {
                jsPlumb.detach(c);
                defaults.reset();
            }
        });

        //连接成功回调函数
        function mtAfterDrop(params) {
            //console.log(params)
            defaults.mtAfterDrop({ sourceId: $("#" + params.sourceId).attr('process_id'), targetId: $("#" + params.targetId).attr('process_id') });

        }

        jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
            dropOptions: { hoverClass: "hover", activeClass: "active" },
            anchor: "Continuous",
            maxConnections: -1,
            endpoint: ["Dot", { radius: 1}],
            paintStyle: { fillStyle: "#ec912a", radius: 1 },
            hoverPaintStyle: this.connectorHoverStyle,
            beforeDrop: function (params) {
                if (params.sourceId == params.targetId) return false; /*不能链接自己*/
                var j = 0;
                $('#leipi_process_info').find('input').each(function (i) {
                    var str = $('#' + params.sourceId).attr('process_id') + ',' + $('#' + params.targetId).attr('process_id');
                    if (str == $(this).val()) {
                        j++;
                        return;
                    }
                })
                if (j > 0) {
                    defaults.fnRepeat();
                    return false;
                } else {
                    mtAfterDrop(params);

                    return true;
                }
            }
        });
        //reset  start
        var _canvas_design = function () {

            //连接关联的步骤
            $('.process-step').each(function (i) {
                var sourceId = $(this).attr('process_id');
                //var nodeId = "window"+id;
                var prcsto = $(this).attr('process_to');
                var toArr = prcsto.split(",");
                var processData = defaults.processData;
                $.each(toArr, function (j, targetId) {

                    if (targetId != '' && targetId != 0) {
                        //检查 source 和 target是否存在
                        var is_source = false, is_target = false;
                        $.each(processData.list, function (i, row) {
                            if (row.id == sourceId) {
                                is_source = true;
                            } else if (row.id == targetId) {
                                is_target = true;
                            }
                            if (is_source && is_target)
                                return true;
                        });

                        if (is_source && is_target) {
                            jsPlumb.connect({
                                source: "window" + sourceId,
                                target: "window" + targetId
                                /* ,labelStyle : { cssClass:"component label" }
                                ,label : id +" - "+ n*/
                            });
                            return;
                        }
                    }
                })
            });
        } //_canvas_design end reset 
        _canvas_design();

        //--------GUID
        var uuid = function () {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) { var r = Math.random() * 16 | 0, v = c == 'x' ? r : r & 0x3 | 0x8; return v.toString(16); });
        };

        //-----外部调用----------------------

        var Flowdesign = {

            addProcess: function (row) {

                if (row.id <= 0) {
                    return false;
                }
                var nodeDiv = document.createElement('div');
                var activityDiv = document.createElement('div'); //步骤属性
                var assignmentDiv = document.createElement('div'); //转交属性
                var nodeId = "window" + row.id, badge = 'badge-inverse', icon = 'icon-star';
                var guid = uuid();
                if (row.icon) {
                    icon = row.icon;
                }
                $(nodeDiv).attr("id", nodeId)
                .attr("style", row.style)
                .attr("process_type", row.process_type)
                .attr("process_to", row.process_to)
                .attr("process_id", row.id)
                .attr("activityid", guid)
                .addClass("process-step btn btn-small")
                .css("width", "auto")
                .html('<span class="process-flag badge ' + badge + '"><i class="' + icon + ' icon-white"></i></span>&nbsp;<span>' + row.process_name + "</span>")
                .mousedown(function (e) {
                    if (e.which == 3) { //右键绑定
                        _canvas.find('#leipi_active_id').val(guid);
                        contextmenu.bindings = defaults.processMenus
                        if ($("#pmDelete") && $("#pmDelete").length == 0) {
                            $("#processMenu ul").append("<li id=\"pmDelete\"><i class=\"icon-trash\"></i>&nbsp;<span class=\"_label\">删除</span></li>");
                        }
                        $(this).contextMenu('processMenu', contextmenu);
                    }
                });
                $(activityDiv).attr("id", "activity" + guid)
            .attr("style", "display:none")
            .attr("activityname", row.process_name)
            .attr("sortorder", row.id)
            .attr("state", 1)
            .attr("rules", "");
                $(assignmentDiv).attr("id", "assignment" + guid)
            .attr("style", "display:none");
                //activityDiv.innerHTML = jQuery.parseJSON(activity);
                nodeDiv.appendChild(activityDiv);
                nodeDiv.appendChild(assignmentDiv);
                _canvas.append(nodeDiv);
                //使之可拖动 和 连线
                jsPlumb.draggable(jsPlumb.getSelector(".process-step"));
                initEndPoints();
                //使可以连接线
                jsPlumb.makeTarget(jsPlumb.getSelector(".process-step"), {
                    dropOptions: { hoverClass: "hover", activeClass: "active" },
                    anchor: "Continuous",
                    maxConnections: -1,
                    endpoint: ["Dot", { radius: 1}],
                    paintStyle: { fillStyle: "#ec912a", radius: 1 },
                    hoverPaintStyle: this.connectorHoverStyle,
                    beforeDrop: function (params) {
                        var j = 0;
                        $('#leipi_process_info').find('input').each(function (i) {
                            var str = $('#' + params.sourceId).attr('process_id') + ',' + $('#' + params.targetId).attr('process_id');
                            if (str == $(this).val()) {
                                j++;
                                return;
                            }
                        })
                        if (j > 0) {
                            defaults.fnRepeat();
                            return false;
                        } else {
                            return true;
                        }
                    }
                });
                return true;

            },
            getActiveId: function () {
                return _canvas.find("#leipi_active_id").val();
            },
            copy: function (active_id) {
                if (!active_id)
                    active_id = _canvas.find("#leipi_active_id").val();

                _canvas.find("#leipi_copy_id").val(active_id);
                return true;
            },
            paste: function () {
                return _canvas.find("#leipi_copy_id").val();
            },
            getProcessInfo: function () {
                try {
                    /*连接关系*/
                    var aProcessData = {};
                    var processData = {};
                    processData["total"] = _canvas.find("div.process-step").length;
                    processData["flow_id"] = $("#flow_id").val();
                    processData["remark"] = $("#workflowProfileModal_txtRemark").val();
                    processData["managerIds"] = $("#workflowProfileModal_hidManager").val();
                    processData["setting"] = ProcessData["setting"]; //获取初始设置;
                    var ExpectFinishTime = "0";
                    if ($("#workflowProfileModal_chkboxExpectFinishTime").attr("checked")) {
                        ExpectFinishTime = "1";
                    }
                    var Secrecy = "0";
                    if ($("#workflowProfileModal_chkboxSecrecy").attr("checked")) {
                        Secrecy = "1";
                    }
                    var Urgency = "0";
                    if ($("#workflowProfileModal_chkboxUrgency").attr("checked")) {
                        Urgency = "1";
                    }
                    var Importance = "0";
                    if ($("#workflowProfileModal_chkboxImportance").attr("checked")) {
                        Importance = "1";
                    }
                    processData["setting"]["WorkflowName"] = $("#workflowProfileModal_ltlWorkflowName").val();
                    processData["setting"]["BasicFields"] = ExpectFinishTime + Secrecy + Urgency + Importance;
                    processData["setting"]["WorkflowAlias"] = $("#workflowProfileModal_txtAlias").val();
                    processData["setting"]["AliasImage"] = $("#workflowProfileModal_hiddenAliasImage").val();
                    processData["setting"]["TaskNotifyMinCount"] = $("#workflowProfileModal_txtMinNotifyTaskCount").val();
                    processData["setting"]["UndoneMaxCount"] = $("#workflowProfileModal_txtMaxUndone").val();
                    processData["profile"] = ProcessData["profile"]; //获取初始设置
                    processData["profile"]["WorkflowName"] = $("#workflowProfileModal_ltlWorkflowName").val();
                    processData["profile"]["BasicFields"] = ExpectFinishTime + Secrecy + Urgency + Importance;
                    processData["profile"]["WorkflowAlias"] = $("#workflowProfileModal_txtAlias").val();
                    processData["profile"]["AliasImage"] = $("#workflowProfileModal_hiddenAliasImage").val();
                    processData["profile"]["MinNotifyTaskCount"] = $("#workflowProfileModal_txtMinNotifyTaskCount").val();
                    processData["profile"]["SmsNotifyFormat"] = $("#workflowProfileModal_txtSmsNotifyFormat").val();
                    processData["profile"]["EmailNotifyFormat"] = $("#workflowProfileModal_txtEmailNotifyFormat").val();
                    processData["profile"]["StatSmsNodifyFormat"] = $("#workflowProfileModal_txtStatSmsNotifyFormat").val();
                    processData["profile"]["StatEmailNodifyFormat"] = $("#workflowProfileModal_txtStatEmailNotifyFormat").val();
                    processData["profile"]["CreationControlType"] = $("#ddlCreationTypes").val();
                    processData["profile"]["MaxCreationInMonth"] = $("#workflowProfileModal_txtMaxInMonth").val();
                    processData["profile"]["MaxCreationInWeek"] = $("#workflowProfileModal_txtMaxInWeek").val();
                    processData["profile"]["MaxCreationUndone"] = $("#workflowProfileModal_txtMaxUndone").val();
                    //processData["profile"]["SMSAuditNotifyFormat"] = $("#workflowProfileModal_txtMaxUndone").val();
                    //processData["profile"]["IsSMSAudit"] = $("#workflowProfileModal_txtMaxUndone").val();
                    //processData["profile"]["SMSAuditActivities"] = $("#workflowProfileModal_txtMaxUndone").val();
                    //processData["profile"]["IsReview"] = $("#workflowProfileModal_txtMaxUndone").val();
                    //processData["profile"]["IsClassicReviewType"] = $("#workflowProfileModal_txtMaxUndone").val();
                    //processData["profile"]["ReviewNotifyMessage"] = $("#workflowProfileModal_txtMaxUndone").val();
                    //processData["profile"]["ReviewActorCount"] = $("#workflowProfileModal_txtMaxUndone").val();
                    processData["profile"]["WorkflowInstanceTitle"] = $("#workflowProfileModal_txtWorkflowInstanceTitle").val();
                    //processData["profile"]["IsAutoContinue"] = $("#workflowProfileModal_txtWorkflowInstanceTitle").val();
                    //processData["profile"]["AutoContinueActivities"] = $("#workflowProfileModal_txtWorkflowInstanceTitle").val();
                    //processData["profile"]["IsDefault"] = $("#workflowProfileModal_txtWorkflowInstanceTitle").val();
                    var PrintAndExp = 0;
                    if ($("#workflowProfileModal_chkPrint").attr("checked") && !$("#workflowProfileModal_chkExp").attr("checked"))
                        PrintAndExp = 2;
                    else if ($("#workflowProfileModal_chkExp").attr("checked") && !$("#workflowProfileModal_chkPrint").attr("checked"))
                        PrintAndExp = 1;
                    else if ($("#workflowProfileModal_chkExp").attr("checked") && $("#workflowProfileModal_chkPrint").attr("checked"))
                        PrintAndExp = 0;
                    else
                        PrintAndExp = 3;
                    processData["profile"]["PrintAndExp"] = PrintAndExp;
                    processData["profile"]["PrintAmount"] = $("#workflowProfileModal_txtPrintCount").val();
                    //processData["profile"]["StepWarningNotifyformat"] = $("#workflowProfileModal_txtPrintCount").val();
                    //processData["profile"]["StepTimeoutNotifyformat"] = $("#workflowProfileModal_txtPrintCount").val();
                    //processData["profile"]["WorkOrderWarningNotifyformat"] = $("#workflowProfileModal_txtPrintCount").val();
                    //processData["profile"]["WorkOrderTimeoutNotifyformat"] = $("#workflowProfileModal_txtPrintCount").val();
                    processData["profile"]["Depts"] = $("#workflowProfileModal_txtDepts").val();
                    processData["profile"]["Manager"] = $("#workflowProfileModal_txtManager").val();
                    processData["profile"]["IsMobile"] = $("#workflowProfileModal_chkIsMobile").attr("checked") ? 1 : 0;
                    processData["list"] = [];
                    $("#leipi_process_info input[type=hidden]").each(function (i) {
                        var processVal = $(this).val().split(",");
                        if (processVal.length == 2) {
                            if (!aProcessData[processVal[0]]) {
                                aProcessData[processVal[0]] = { "top": 0, "left": 0, "process_to": [] };
                            }
                            aProcessData[processVal[0]]["process_to"].push(processVal[1]);
                        }
                    })
                    /*位置*/
                    _canvas.find("div.process-step").each(function (i) { //生成Json字符串，发送到服务器解析
                        if ($(this).attr('id')) {
                            var pId = $(this).attr('process_id');
                            var pLeft = parseInt($(this).css('left'));
                            var pTop = parseInt($(this).css('top'));
                            var properties = {}; //步骤属性
                            var activity = {};
                            var assignment = {};
                            var activityid = $(this).attr('activityid');
                            if (!aProcessData[pId]) {
                                aProcessData[pId] = { "top": 0, "left": 0, "process_to": [] };
                            }
                            aProcessData[pId]["top"] = pTop;
                            aProcessData[pId]["left"] = pLeft;
                            properties["id"] = parseInt(pId);
                            properties["process_id"] = activityid;
                            properties["process_name"] = "";
                            properties["process_to"] = aProcessData[pId]["process_to"];
                            properties["process_type"] = $(this).attr('process_type');
                            if (properties["process_type"] == "start-activity")
                                properties["icon"] = "icon-ok"
                            else if (properties["process_type"] == "end-activity")
                                properties["icon"] = "icon-stop"
                            else if (properties["process_type"] == "activity")
                                properties["icon"] = "icon-star"
                            properties["style"] = $(this).attr('style');
                            var $divActivity = $("#activity" + activityid);
                            var $divAssignment = $("#assignment" + activityid);
                            properties["process_name"] = $divActivity.attr("activityname");
                            ActivityTemplate["NextActivityNames"] = [];
                            ActivityTemplate["PrevActivityNames"] = [];
                            for (var item in ActivityTemplate) {
                                var val = $divActivity.attr(item);
                                if (typeof (val) == "undefined")
                                    continue;
                                activity[item] = val;
                            }
                            activity["NextActivityNames"] = [];
                            activity["PrevActivityNames"] = [];
                            activity["x"] = pLeft;
                            activity["y"] = pTop;
                            properties["activity"] = activity;
                            for (var item in AssignmentTemplate) {
                                var val = $divAssignment.attr(item);
                                if (typeof (val) == "undefined")
                                    continue;
                                assignment[item] = val;
                            }
                            properties["assignment"] = assignment;
                            var rules = $divActivity.attr("rules");
                            properties["rules"] = typeof (rules) == "undefined" ? null : rules;
                            properties["top"] = pTop;
                            properties["left"] = pLeft;
                            processData["list"].push(properties);
                        }
                    })
                    //关联上下级步骤
                    var activities = processData["list"];
                    var NextActivity = "";
                    var PrevActivity = "";
                    for (var i = 0; i < activities.length; i++) {
                        var item = activities[i];
                        var process_to = item.process_to;
                        for (var k = 0; k < process_to.length; k++) {
                            var nextid = process_to[k];
                            for (var j = 0; j < activities.length; j++) {
                                var nextitem = activities[j];
                                if (parseInt(nextid) == nextitem.id) {
                                    item.activity.NextActivityNames.push(nextitem.process_name);
                                    item.activity["NextActivity"] += ","
                                    nextitem.activity.PrevActivityNames.push(item.process_name);
                                    item.activity["PrevActivity"]
                                }
                            }
                        }
                    }
                    //return JSON.stringify(aProcessData);
                    return processData;
                } catch (e) {
                    //return '';
                    return {};
                }

            },
            clear: function () {
                try {

                    jsPlumb.detachEveryConnection();
                    jsPlumb.deleteEveryEndpoint();
                    $('#leipi_process_info').html('');
                    jsPlumb.repaintEverything();
                    return true;
                } catch (e) {
                    return false;
                }
            }, refresh: function () {
                try {
                    //jsPlumb.reset();
                    this.clear();
                    _canvas_design();
                    return true;
                } catch (e) {
                    return false;
                }
            },
            delProcess: function (processInfo) {
                //if (activeId <= 0) return false;

                //$("#window" + activeId).remove();

                //var processInfo = getProcessInfo();
                defaults.processData = processInfo;
                //连接关联的步骤
                $('.process-step').each(function (i) {
                    var sourceId = $(this).attr('process_id');
                    //var nodeId = "window"+id;
                    var prcsto = $(this).attr('process_to');
                    var toArr = prcsto.split(",");
                    var processData = defaults.processData;
                    $.each(toArr, function (j, targetId) {

                        if (targetId != '' && targetId != 0) {
                            //检查 source 和 target是否存在
                            var is_source = false, is_target = false;
                            $.each(processData.list, function (i, row) {
                                if (row.id == sourceId) {
                                    is_source = true;
                                } else if (row.id == targetId) {
                                    is_target = true;
                                }
                                if (is_source && is_target)
                                    return true;
                            });

                            if (is_source && is_target) {
                                jsPlumb.connect({
                                    source: "window" + sourceId,
                                    target: "window" + targetId
                                    /* ,labelStyle : { cssClass:"component label" }
                                    ,label : id +" - "+ n*/
                                });
                                return;
                            }
                        }
                    })
                });
                return true;
            },
            Reset: function () {
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
            },
            saveAttribute: function (activityid) {
                defaults.getActivityAllocatorValues(activityid);
                defaults.getAssignAllocatorValues(activityid);
            }
        };
        return Flowdesign;


    } //$.fn
})(jQuery);