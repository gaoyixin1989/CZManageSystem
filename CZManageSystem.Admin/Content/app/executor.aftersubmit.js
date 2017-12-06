/**
 * Angularjs环境下分页
 * name: executor.aftersubmit
 * Version: 1.0.0
 */
//StepData：下一步骤的详细信息数据
//activityNameSelected:选中的下一步骤名称
//selectedItem：选中的下一步的条目信息
//executor.aftersubmit
angular.module('executor.aftersubmit', []).directive('executorAftersubmit', ['$filter', '$http', function ($filter, $http) {
    return {
        restrict: 'EA',

        template: '<table><tr>' +
                    '<th>下一步骤：</th>' +
                    '<td>' +
                        '<div class="ui-input">' +
                            '<div class="selectdrapdown">' +
                                '<select class="cell-input" ng-model="activityNameSelected" style="width:222px;">' +
                                    '<option ng-repeat="x in StepData.items" value="{{x.activityName}}">{{x.activityName}}</option>' +
                                '</select>' +
                            '</div>' +
                        '</div>' +
                    '</td>' +
                '</tr>' +
                '<tr ng-hide="selectedItem.users==null||selectedItem.users.length<1">' +
                    '<th>下一步骤执行人：</th>' +
                    '<td>' +
                        '<label ng-repeat="x in selectedItem.users" style="margin-right:10px;"><input type="checkbox" ng-value="x.UserName" ng-model="x.isSelected" />{{x.RealName}}</label>' +
                    '</td>' +
                '</tr>' +
                '<tr ng-hide="selectedItem.CC_ReviewType==null||selectedItem.CC_ReviewType==\'None\'">' +
                    '<th>抄送列表：</th>' +
                    '<td>' +
                        '<div ng-show="selectedItem.CC_ReviewType==\'CheckBox\'">' +
                            '<label ng-repeat="x in selectedItem.listCC" style="margin-right:10px;"><input type="checkbox" ng-value="x.UserName" ng-model="x.isSelected" />{{x.RealName}}</label>' +
                        '</div>' +
                        '<div ng-show="selectedItem.CC_ReviewType==\'Classic\'">' +
                            '<div class="ui-input">' +
                                '<input type="text"ng-model="CC_person.texts" class="cell-input" readonly ng-click="editUserForCC(CC_person.ids)" />' +
                            '</div>' +
                        '</div>' +
                    '</td>' +
                '</tr></table>',

        replace: true,
        scope: {
            conf: '='
        },
        link: function (scope, element, attrs) {

            scope.CC_person = {
                datas: [],
                ids: '',
                values: '',
                texts: ''
            }

            //返回数据
            scope.conf.getValue = function () {
                var isSuccess = false;
                var errorMessage = '请选择下一个步骤';
                var obj1 = '';
                var obj2 = [];
                var obj3 = [];
                if (scope.selectedItem != null) {
                    obj1 = scope.selectedItem.activityName;
                    obj2 = $filter('filter')(scope.selectedItem.users, { 'isSelected': true });
                    if (scope.selectedItem.CC_ReviewType == 'CheckBox') {
                        obj3 = $filter('filter')(scope.selectedItem.listCC, { 'isSelected': true });
                    }
                    else if (scope.selectedItem.CC_ReviewType == 'Classic') {
                        obj3 = scope.CC_person.datas;
                    }
                }

                if (obj1.length < 1) {
                    isSuccess = false;
                    errorMessage = '请选择下一个步骤';
                }
                else if (obj2.length < 1) {
                    isSuccess = false;
                    errorMessage = '请选择下一步骤执行人';
                }
                else if (scope.selectedItem.CC_ReviewActorCount > -1 && obj3.length > scope.selectedItem.CC_ReviewActorCount) {
                    isSuccess = false;
                    errorMessage = '抄送人数过多，不能超过' + scope.selectedItem.CC_ReviewActorCount + '人';
                }
                else {
                    isSuccess = true;
                    errorMessage = '';
                }

                return {
                    isSuccess: isSuccess,
                    errorMessage: errorMessage,
                    nextActivity: obj1,
                    nextActors: obj2,
                    nextCC: obj3
                };
            }

            scope.$watch(function () {
                var newValue = scope.activityNameSelected;
                return newValue;
            }, selectActivity);
            // 步骤选中事件
            function selectActivity() {
                if (scope.StepData != null)
                    var temp = $filter('filter')(scope.StepData.items, { 'activityName': scope.activityNameSelected })[0];
                scope.selectedItem = deepClone(temp);
                if (scope.selectedItem != null && scope.selectedItem.CC_ReviewType == "CheckBox" && scope.selectedItem.listCC != null) {
                    $.each(scope.selectedItem.listCC, function (i, item) {
                        item.isSelected = scope.selectedItem.CC_ReviewValidateType;
                    });
                }
            };

            //获取流程步骤信息
            scope.getWFStepAfterSumbit = function () {//获取数据
                return $http({
                    method: 'POST',
                    url: CurPath + 'Workflow/getWFStepAfterSumbit',
                    data: { workflowName: scope.conf.workflowName }
                });
            };

            //选择用户（抄送用户）
            scope.editUserForCC = function (users) {
                var iframeId = 'user_edit';
                var iframeSrc = '../UsersGrounp/SelectUsers?selectedId=' + users;
                var iframeStr = "<iframe id='" + iframeId + "' name='" + iframeId + "' frameborder='0' marginwidth='0' style='width:100%;height:100%;overflow:hidden;' src='" + iframeSrc + "'></iframe>";
                box.popup(iframeStr,
                    {
                        title: '选择用户',
                        width: 760,//窗口宽度，默认400
                        height: 380,//窗口高度，默认400
                        hasOk: true,
                        hasCancel: true,
                        onBeforeClose: function (data) {
                            if (data == true) {
                                var theWindow = window;
                                while ((!theWindow.frames[iframeId]) && (!!theWindow.parent)) {
                                    theWindow = theWindow.parent;
                                }
                                if (!theWindow.frames[iframeId]) {
                                    box.alert('出错！', { icon: 'error' });
                                }
                                else {
                                    var result = theWindow.frames[iframeId].getResult();
                                    scope.CC_person.datas = [];
                                    $.each(result, function (i, item) {
                                        scope.CC_person.datas.push({ UserName: item.UserName, RealName: item.text });
                                    });
                                    scope.CC_person.ids = getAttrByArr(result, 'id').join(",");
                                    scope.CC_person.values = getAttrByArr(result, 'UserName').join(",");
                                    scope.CC_person.texts = getAttrByArr(result, 'text').join(",");
                                    scope.$apply();
                                }
                            }
                        }
                    });
            }

            //查询流程信息，初始化数据
            function initData() {
                if (scope.conf.workflowName == undefined)
                    return;
                scope.getWFStepAfterSumbit().success(function (response) {
                    scope.StepData = response;
                    scope.activityNameSelected = null;
                    scope.selectedItem = null;
                    if (scope.StepData.items.length > 0)
                        scope.activityNameSelected = scope.StepData.items[0].activityName;
                }).error(function (err) {
                    box.alert("网络出错！", { icon: 'error' });
                });
            }

            //绑定数据改变
            function changeStepData() {
                if (scope.conf.StepData == undefined)
                    return;
                scope.StepData = deepClone(scope.conf.StepData);
                scope.activityNameSelected = null;
                scope.selectedItem = null;
                if (scope.StepData.items.length > 0)
                    scope.activityNameSelected = scope.StepData.items[0].activityName;
            }

            //两种初始化方法
            //一、获取到流程名称，初始化
            scope.$watch(function () {
                var newValue = scope.conf.workflowName
                return newValue;
            }, initData);

            //二、绑定数据改变
            scope.$watch(function () {
                var newValue = -1;
                newValue = JSON.stringify(scope.conf.StepData)
                return newValue;
            }, changeStepData);

        }
    };
}]);


//深度克隆
function deepClone(obj) {
    var result, oClass = isClass(obj);
    //确定result的类型
    if (oClass === "Object") {
        result = {};
    } else if (oClass === "Array") {
        result = [];
    } else {
        return obj;
    }
    for (key in obj) {
        var copy = obj[key];
        if (isClass(copy) == "Object") {
            result[key] = arguments.callee(copy);//递归调用
        } else if (isClass(copy) == "Array") {
            result[key] = arguments.callee(copy);
        } else {
            result[key] = obj[key];
        }
    }
    return result;
}
//返回传递给他的任意对象的类
function isClass(o) {
    if (o === null) return "Null";
    if (o === undefined) return "Undefined";
    return Object.prototype.toString.call(o).slice(8, -1);
}

