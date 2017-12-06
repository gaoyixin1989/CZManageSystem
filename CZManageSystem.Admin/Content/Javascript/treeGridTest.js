var list = list || [];
function GetData() {
    var data = {
        "pageIndex": 1,
        "pageSize": 10
    };
    box.load("GetAllSysDeptment");
    $.ajax({
        url: CurPath + 'SysDeptment/GetAllSysDeptment',
        data: JSON.stringify(data),
        type: 'POST',
        async: false,
        contentType: 'application/json;charset=utf-8',
        success: function (data) {
            box.ready("GetAllSysDeptment");
            list = data.items;
        },
        error: function () {
            box.ready("GetAllSysDeptment");
        }
    });
}

var list = [];
function GetIdList(obj) {
    list.push(obj.DpId)
    for (var i = 0; i < obj.children.length; i++) {
        this.GetIdList(obj.children[i]);
    }
}

(function () {
    SetTree();

}).call(window);

function SetTree() {
    var app, deps;

    deps = ['treeGrid'];

    app = angular.module('treeGridTest', deps);

    app.controller('treeGridController', function ($scope, $timeout) {
        
            var tree;

            GetData();

            var myTreeData = getTree(list, 'DpId', 'ParentDpId');

            $scope.tree_data = myTreeData;
            $scope.my_tree = tree = {};
            $scope.expanding_property = {
                field: "DpName",
                displayName: "部门名称",
                sortable: true,
                filterable: true
            };
            $scope.col_defs = [
            {
                field: "DpLevel",
                displayName: "部门层级",
                sortable: true,
                sortingType: "string"
            },
            {
                field: "DeptOrderNo",
                displayName: "序号",
                sortable: true,
                sortingType: "string",
                filterable: true
            },
            {
                field: "DpId",
                displayName: "操作",
                cellTemplate: "<div class='operation-btn'><a ng-href=\"../SysDeptment/Edit?id={{row.branch.DpId}}\">修改</a>&nbsp;<a class='lia-1' style='cursor: pointer;' ng-click='cellTemplateScope.click(this)'>删除</a><div>",
                cellTemplateScope: {
                    click: function (data) {
                        box.confirm("确定要删除该数据吗？", { icon: 'question' }, function (result) {
                            if (result) {
                                list = [];
                                GetIdList(data.row.branch);
                                $.ajax({
                                    url: CurPath + 'SysDeptment/DeleteById',
                                    data: JSON.stringify({ idList: list }),
                                    type: 'POST',
                                    async: false,
                                    contentType: 'application/json;charset=utf-8',
                                    success: function (data) {
                                        //这里刷新，获取数据就行了，不要刷新整个页面
                                        //SetTree();
                                        window.location.href = window.location.href;
                                    }
                                });
                            }
                        });
                    }
                }
            }
            ];
        
        $scope.my_tree_handler = function (branch) {
        }

        function getTree(data, primaryIdName, parentIdName) {
            if (!data || data.length == 0 || !primaryIdName || !parentIdName)
                return [];

            var tree = [],
                rootIds = [],
                item = data[0],
                primaryKey = item[primaryIdName],
                treeObjs = {},
                parentId,
                parent,
                len = data.length,
                i = 0;

            angular.forEach(data, function (curObj) {
                treeObjs[curObj[primaryIdName]] = curObj;
            });

            while (i < len) {
                item = data[i++];
                primaryKey = item[primaryIdName];
                //treeObjs[primaryKey] = item;
                parentId = item[parentIdName];
                parent = treeObjs[parentId];
                if (parent) {

                    if (parent.children) {
                        parent.children.push(item);
                    } else {
                        parent.children = [item];
                    }
                } else {
                    rootIds.push(primaryKey);
                }
            }

            for (var i = 0; i < rootIds.length; i++) {
                tree.push(treeObjs[rootIds[i]]);
            };
            return tree;
        }

    });
}

