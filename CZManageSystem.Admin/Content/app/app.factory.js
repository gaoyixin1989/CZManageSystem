app.factory('DirectiveUtil', [function () {
	var DirectiveUtil = {};

	DirectiveUtil.DirectiveBuilder = function (directiveName) {

		directive = directiveName;
		directiveBuffer = '<' + directiveName + ' ';

		this.setDirectiveName = function (directiveName) {
			directive = directiveName;
			directiveBuffer = '<' + directiveName + ' ';
		}

		this.getDirectiveName = function () {
			return directive;
		}
		// name: directive中scope的name, value: clone directive操作时，数据绑定的名称
		this.appendAttr = function (name, value) {
			directiveBuffer += name + '=\'' + value + '\' ';
			return this;
		}

		this.build = function (compile, scope) {
			return compile(directiveBuffer + ' />')(scope);
		}
	};

	return DirectiveUtil;
}]);
