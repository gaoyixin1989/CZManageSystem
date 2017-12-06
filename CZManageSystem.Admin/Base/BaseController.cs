using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using CZManageSystem.Admin.Base;
using System.Xml;
using System.Xml.Linq;

namespace CZManageSystem.Admin
{
    public class BaseController : Controller
    {
        protected ISysUserService _sysUserService = new SysUserService();
        protected SysLogService _sysLogService = new SysLogService();
        private WebWorkContext _webWorkContext = new WebWorkContext();
 
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            string actionName = filterContext.ActionDescriptor.ActionName.ToLower();
            string[] arrayAction = { "errorindex", "login", "chklogin", "logout" };
            if (!arrayAction.Contains(actionName))
            {
                if (filterContext.Controller is BaseController)
                {
                    var b = filterContext.Controller as BaseController;
                    if (b != null)
                    {
                        if (this.WorkContext.CurrentUser == null)
                        {

                            //Response.Redirect("~/Home/Login");
                            filterContext.Result = new RedirectResult("/Home/Login");
                            return;

                        }
                        else if (actionName == "Index" && controllerName != "Home")
                        {
                            var _sysMenuService = new SysMenuService();
                            List<SysMenu> menuList = _sysMenuService.getMenuByUser(this.WorkContext.CurrentUser.UserName, this.WorkContext.CurrentUser.UserId).ToList();
                            string PageUrl = "../" + controllerName + "/" + actionName;
                            if (!menuList.Exists(m => m.PageUrl == PageUrl))
                            {
                                filterContext.Result = new RedirectResult("../Error/ErrorIndex?ErrorMsg=" + Url.Encode("没有页面访问权限,请向系统管理员申请权限！"));

                            }
                        }

                    }
                }
            }
            base.OnActionExecuting(filterContext);

        }

       
        /// <summary>
        /// 写系统异常日志
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
            {
                ErrorDesc = filterContext.Exception.Message,
                ErrorTitle= filterContext.Exception.GetType().Name,
                ErrorPage = this.Request.RawUrl,//filterContext.Exception.TargetSite.ToJsonString(),
                RealName=  this.WorkContext.CurrentUser?.RealName,
                UserName = this.WorkContext.CurrentUser?.UserName
            });
            base.OnException(filterContext);
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectResult("../Error/ErrorIndex?ErrorMsg=" + Url.Encode("页面异常:"+ filterContext.Exception.Message+", 请向系统管理员寻求协助！"));
            //base.OnException(filterContext);
        }
        public WebWorkContext WorkContext
        {
            get { return _webWorkContext; }
        }

        /// <summary>
        /// 根据字典名称获取字典数据
        /// </summary>
        /// <param name="DDName">名称</param>
        public List<DataDictionary> GetDictListByDDName(string DDName)
        {
            IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
            var modelList = new List<DataDictionary>();
            int count = 0;
            if (!string.IsNullOrEmpty(DDName))
            {
                modelList = _dataDictionaryService.QueryDataByPage(out count, 0, int.MaxValue, DDName, null).ToList();
                modelList = modelList.Where(u => (u.EnableFlag ?? false)).ToList();
            }
            return modelList;
        }

        protected HttpResponse GetResponse(string fileName)
        {
            var context = System.Web.HttpContext.Current;
            var response = context.Response;
            //IE浏览器
            if (context.Request.Browser.Browser.Equals("InternetExplorer") || context.Request.Browser.Type.ToLower().Contains("ie"))//context.Request.UserAgent.ToLower().IndexOf("msie", System.StringComparison.Ordinal) > -1
            {
                response.AddHeader("content-disposition", "filename=" + HttpUtility.UrlEncode(fileName));
            }
            response.ContentType = "application/x-xls";
            //response.Charset = "utf-8";
            //response.ContentEncoding = System.Text.Encoding.UTF8 ; 
            return response;
        }
        /// <summary>
        /// 客户端ip(访问用户)
        /// </summary>
        public static string GetUserIp
        {
            get
            {
                string realRemoteIP = string.Empty;
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
                }
                if (string.IsNullOrEmpty(realRemoteIP))
                {
                    realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrEmpty(realRemoteIP))
                {
                    realRemoteIP = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                return realRemoteIP;
            }
        }

        //添加单条操作记录
        public void AddOperationLog(OperationType type,string desc) {
            SysOperationLog log = new SysOperationLog();
            log.RealName = this.WorkContext.CurrentUser.RealName;
            log.UserName = this.WorkContext.CurrentUser.UserName;
            log.OperationPage = Request.RawUrl;
            log.Operation = type;
            log.OperationDesc = desc;
            _sysLogService.WriteSysLog(log);

        }

    }


}
