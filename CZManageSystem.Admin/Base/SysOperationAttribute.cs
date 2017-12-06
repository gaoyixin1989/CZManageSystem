using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin
{
    public class SysOperationAttribute: ActionFilterAttribute
    {
        protected SysLogService _sysLogService = new SysLogService();

        /// <summary>
        /// 操作描述
        /// </summary>

        private string _operationDesc = "";
        /// <summary>
        /// 操作类型
        /// </summary>
        private OperationType _operationType;



        /// <summary>
        /// 活动日志
        /// </summary>
        /// <param name="activityLogTypeName">类别名称</param>
        /// <param name="parm">参数名称列表,可以用, | 分隔</param>

        public SysOperationAttribute(OperationType operationType, string operationDesc)
        {
            _operationType = operationType;
            _operationDesc = operationDesc;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            ////方法名称
            string actionName = filterContext.ActionDescriptor.ActionName;
            ////控制器
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            // 页面传递参数
            IDictionary<string, object> dic = filterContext.ActionParameters;
            var parameters = new System.Text.StringBuilder();
            foreach (var item in dic)
            {
                parameters.Append(item.Key + "=" + item.Value + "|^|");
            }
            if (this._operationType == OperationType.Edit && parameters == null)//如果没有参数,默认为新增
                this._operationType = OperationType.Add;
            string ip=filterContext.HttpContext.Request.ServerVariables.Get("Remote_Addr").ToString();
            Users curUser = new WebWorkContext().CurrentUser;
            _sysLogService.WriteSysLog<SysOperationLog>(new SysOperationLog() {
                Operation = this._operationType,
                OperationDesc = this._operationDesc+ (parameters==null?"":" 参数：" +parameters.ToString()),
                OperationPage = controllerName + "/" + actionName,
                OperationIp= ip,
                UserName = curUser?.UserName,
                RealName=curUser?.RealName
            });


        }

    }

}