//angular.module('KindApp').directive('uiKindeditor', ['uiLoad', function (uiLoad) {
//    return {
//        restrict: 'EAC',
//        require: '?ngModel',
//        link: function (scope, element, attrs, ctrl) {
//            uiLoad.load('kindeditor-min.js').then(function () {
//                var _initContent, editor;
//                var fexUE = {
//                    initEditor: function () {
//                        editor = KindEditor.create(element[0], {
//                            width: '100%',
//                            height: '400px',
//                            resizeType: 1,
//                           // uploadJson: '/Upload/Upload_Ajax.ashx',
//                            formatUploadUrl: false,
//                            allowFileManager: true,
//                            afterChange: function () {
//                                ctrl.$setViewValue(this.html());
//                            }
//                        });
//                    },
//                    setContent: function (content) {
//                        if (editor) {
//                            editor.html(content);
//                        }
//                    }
//                }
//                if (!ctrl) {
//                    return;
//                }
//                _initContent = ctrl.$viewValue;
//                ctrl.$render = function () {
//                    _initContent = ctrl.$isEmpty(ctrl.$viewValue) ? '' : ctrl.$viewValue;
//                    fexUE.setContent(_initContent);
//                };
//                fexUE.initEditor();
//            });
//        }
//    }
//}]);
(function (window, angular, undefined) {

    angular.module('ngKeditor', ['ng']).directive('keditor',function(){
    return { require : 'ngModel', 
        scope : {  config : '=config' },
        link : function (scope,elm,attr,ctrl) { 
            if(typeof KindEditor === "undefined") {
                console.error('Please import the local resources of kindeditor!');
                return; 
            } 
            var _config = { uploadJson: '', autoHeightMode: false, afterCreate: function () { 
                    this.loadPlugin('autoheight'); 
                },
                width: '100%' 
            };
           
            var editorId = elm[0]; 
            var editorConfig = scope.config || _config; 
            editorConfig.afterChange = function () { 
                if (!scope.$$phase) {
                    ctrl.$setViewValue(this.html());
                    scope.$apply(); 
                } 
            };
            KindEditor.ready(function (editor) {
                editor.create(editorId, editorConfig);
            });
            ctrl.$render(function (modelValue) {
                //if (editor) {
                    editorId.html(modelValue);
                //}
            });
            
   
            ctrl.$parsers.push(function (viewValue) { 
                ctrl.$setValidity('keditor', viewValue); 
                return viewValue; 
            }); 
        } 
    }
});
})
(window, window.angular);