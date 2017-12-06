using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Core.Caching;
using CZManageSystem.Data;
using CZManageSystem.Service.SysManger;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Core.Helpers;
using System.Linq.Expressions;
using CZManageSystem.Admin.Models;
using System.Configuration;
using CZManageSystem.Admin.Base;
using System.Text.RegularExpressions;

namespace CZManageSystem.Admin.Controllers
{
    public class Passwd
    {
        public string NewPassword { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
    public class treeMenu
    {
        public int MenuId { get; set; }
        public string MenuType { get; set; }
        public string MenuName { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string PageUrl { get; set; }
        public List<treeMenu> Child { get; set; }
    }

    public class HomeController : BaseController
    { 
        ITracking_ToReview_DetailService tracking_ToReview_DetailService = new Tracking_ToReview_DetailService();
        private static string _codeKey = "2016CZManageSystem";
       
        public ActionResult Login(string Account, string Pwd)
        {
            ViewBag.CodeKey = _codeKey;

            #region 验证是否已Portal单点登录
            try
            {
                string token = this.Request.Cookies["iPlanetDirectoryPro"].ToString();
                if (!string.IsNullOrEmpty(token))
                {
                    AuthResult authrReuslt = SSOHelper.CheckLogin(token);
                    if (authrReuslt.authResult)
                    {
                        var sysUser = _sysUserService.FindById(authrReuslt.account);
                        if (sysUser != null)
                        {
                            ///登录成功
                            this.WorkContext.CurrentUser = sysUser;
                            _sysLogService.WriteSysLog<SysOperationLog>(new SysOperationLog()
                            {
                                Operation = OperationType.Loin,
                                OperationDesc = "Portal单点登录成功",
                                OperationPage = Request.RawUrl,
                                RealName = sysUser.RealName,
                                UserName = sysUser.UserName
                            });
                           
                            Response.Redirect("/Home/Index");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            #endregion

            #region 深信通单点登录

            //ViewBag.SSOUrl = ConfigurationManager.AppSettings["SSO_Login"] + "?RequestUrl=" + HttpUtility.UrlEncode(Request.Url.AbsoluteUri);
            //if (Request.UserHostAddress != "127.0.0.1" && Request.UserHostAddress != "58.63.251.76")
            //{
            //    string currentUserID = string.Empty;

            //    try
            //    {
            //        currentUserID = SXT.SSO.Client.SSOEntry.CurrentUserID ?? "";
            //        if (!string.IsNullOrEmpty(currentUserID))
            //        {
            //            var user = _sysUserService.FindById(currentUserID);
            //            if (user != null)
            //            {
            //                HttpCookie cookies = new HttpCookie("UserName");
            //                cookies.Value = SXT.SSO.Client.SSOEntry.CurrentUserName;
            //                Response.Redirect("/Home/Index");
            //            }
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        SystemResult result = new SystemResult() { IsSuccess = false, Message = ex.ToString() };
            //        _operateLogService.LoggerHelper(currentUserID, "用户登录", "登录异常", "单点登录验正失败" + ex.Message);
            //    }
            //}
            #endregion
            return View();
        }
        [HttpPost]
        [SysOperation(OperationType.Loin,"用户登击验证")]
        public ActionResult chkLogin(Users user, string ident)
        {

            SystemResult result = new SystemResult() { IsSuccess = false, Message = "账户或密码不正确！" };
            try
            {

                if (user == null)
                    return Json(result);
                if (user.UserName == "" || user.Password == "")
                    return Json(result);
                user.UserName = EncryptHelper.DecodeBase64(user.UserName);
                string pwd = EncryptHelper.DecodeBase64(user.Password);
                pwd = pwd.Replace(EncryptHelper.GetMd5Code(_codeKey), "").Replace(_codeKey, "");

                if (SafeCheck.CheckData(user.UserName) || SafeCheck.CheckData(pwd))
                {
                    result.Message = "账号或密号包含非法字符！";
                    return Json(result);

                }

                Expression<Func<Users, bool>> _Expression = u => u.UserName == user.UserName;
                Users sysUser = _sysUserService.FindByFeldName(_Expression);
                if (ident == "统一认证")
                {
                    AuthResult authResult = SSOHelper.SSOLogin(user.UserName, pwd);
                    if (!authResult.authResult)
                    {
                        result.Message = authResult.authMsg;
                        return Json(result);
                    }
                    if (sysUser == null)
                    {
                        result.Message = "本地系统不存在此portal账号，请与系统管理员联系。";
                        return Json(result);
                    }

                }
                else if (sysUser.Password != EncryptHelper.Encrypt(pwd) && ident == "本地认证")
                {
                    return Json(result);
                }
                if (sysUser == null)
                    return Json(result);
                ///登录成功
                this.WorkContext.CurrentUser = sysUser;
                result.data = "Home/Index";
                result.IsSuccess = true;
               
                //_sysLogService.WriteSysLog<SysOperationLog>(new SysOperationLog()
                //{
                //    Operation = OperationType.Loin,
                //    OperationDesc = "用户登录成功",
                //    OperationPage = Request.RawUrl,
                //    RealName = sysUser.RealName,
                //    UserName = sysUser.UserName
                //});
                

                return Json(result);
            }
            catch (Exception ex)
            {
                
                result.Message = "账号或密码错误！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString()+ex.InnerException ?.Message ,
                    ErrorPage= Request.RawUrl,
                    ErrorTitle="登录错误",
                    RealName = user.RealName,
                    UserName = user.UserName
                });
                return Json(result);
            }


        }
        
        // GET: Index
        public ActionResult Index()
        {
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            ViewBag.MenuId = "0";
            @ViewBag.UserName = this.WorkContext.CurrentUser.UserName;
            @ViewBag.RealName = this.WorkContext.CurrentUser.RealName;
            //ViewBag.DeptName = this.WorkContext.CurrentUser.Dept?.DpName;
            ViewBag.DeptName = "";
            if (!string.IsNullOrEmpty(this.WorkContext.CurrentUser.Dept?.DpFullName))
            {
                string[] temp = this.WorkContext.CurrentUser.Dept?.DpFullName.Split('>');
                if (temp.Length > 1)
                    ViewBag.DeptName = temp[temp.Length - 2] + "-" + temp[temp.Length - 1];
                else
                    ViewBag.DeptName = temp[temp.Length - 1];
            }


            SysMenuService _sysMenuService = new SysMenuService();
            List<SysMenu> menuList = _sysMenuService.getMenuByUser(this.WorkContext.CurrentUser.UserName, this.WorkContext.CurrentUser.UserId).ToList();
            ViewBag.MenuList = menuList.Where(m => (m.ParentId == null || m.ParentId == 0) && m.MenuType == "菜单" && m.EnableFlag == true).OrderBy(m => m.OrderNo).ToList();
            return View();

        }
        public ActionResult Index2()
        {

            ViewBag.MenuId = "0";
            @ViewBag.UserName = this.WorkContext.CurrentUser.UserName;
            SysMenuService _sysMenuService = new SysMenuService();
            List<SysMenu> menuList = _sysMenuService.getMenuByUser(this.WorkContext.CurrentUser.UserName, this.WorkContext.CurrentUser.UserId).ToList();
            ViewBag.MenuList = menuList.Where(m => (m.ParentId == null || m.ParentId == 0) && m.MenuType == "菜单" && m.EnableFlag == true).OrderBy(m => m.OrderNo).ToList();
            return View();

        }
        public ActionResult Content()
        {
            return View();
        }
        public ActionResult Content1()
        {
            return View();
        }
        public ActionResult Admin(string MenuId)
        {
            @ViewBag.UserName = this.WorkContext.CurrentUser.UserName;
            if (!string.IsNullOrEmpty(MenuId))
            {
                ViewBag.MenuId = MenuId;
                SysMenuService _sysMenuService = new SysMenuService();
                List<SysMenu> menuList = _sysMenuService.List().ToList();
                //List<SysMenu> templist = _sysMenuService.List().ToList();
                //List<SysMenu> list = new List<SysMenu>();
                //foreach (var menu in menuList)
                //{
                //    list.AddRange(templist.Where(m => m.ParentId == menu.Id));
                //}
                ViewBag.MenuList = menuList;
            }
            else
                ViewBag.MenuList = null;
            return View();
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [SysOperation(OperationType.Logout, "用户正常退出")]
        public ActionResult LogOut()
        {
            try
            {
                this.WorkContext.ClearCurrentUser();



            }
            catch (Exception ex)
            {

            }
            return View();


        }
        public ActionResult Index1()
        {
            ViewBag.MenuId = "0";
            @ViewBag.UserName = this.WorkContext.CurrentUser.UserName;

            SysMenuService _sysMenuService = new SysMenuService();
            List<SysMenu> menuList = _sysMenuService.getMenuByUser(this.WorkContext.CurrentUser.UserName, this.WorkContext.CurrentUser.UserId).ToList();


            ViewBag.MenuList = menuList.Where(m => (m.ParentId == null || m.ParentId == 0) && m.MenuType == "菜单");

            return View();
        }
        public ActionResult GetMenuList(int Id)
        {
            SystemResult sysReuslt = new SystemResult() { IsSuccess = false };
            try
            {
                SysMenuService _sysMenuService = new SysMenuService();
                //---------------------------------------------------------
                //返回所拥有菜单的的list
                //List<SysMenu> menuList = _sysMenuService.List().ToList();
                List<SysMenu> menuList = _sysMenuService.getMenuByUser(this.WorkContext.CurrentUser.UserName, this.WorkContext.CurrentUser.UserId).ToList();

                List<treeMenu> treeMenuList = getChild(menuList, Id);
                sysReuslt.data = treeMenuList;
                sysReuslt.IsSuccess = true;

            }
            catch (Exception ex)
            {
                sysReuslt.Message = ex.Message;
            }
            return Json(sysReuslt, JsonRequestBehavior.AllowGet);
        }

        private List<treeMenu> getChild(List<SysMenu> menuList, int Id)
        {
            List<treeMenu> list = new List<treeMenu>();
            treeMenu tMenu = null;
            foreach (var menu in menuList.Where(m => m.ParentId == Id && m.EnableFlag == true).OrderBy(m => m.OrderNo))
            {
                tMenu = new treeMenu()
                {
                    MenuId = menu.Id,
                    MenuType = menu.MenuType,
                    MenuName = menu.MenuName,
                    PageUrl = menu.PageUrl,
                    OrderNo = menu.OrderNo,
                    ParentId = menu.ParentId,
                    Child = new List<treeMenu>()

                };

                if (menuList.Exists(m => m.ParentId == menu.Id))
                {
                    tMenu.Child = getChild(menuList, menu.Id);
                }
                list.Add(tMenu);

            }
            return list;
        }

        //-----------------------------------
        /// <summary>
        /// 获取待办工作数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetList_dbgz(int pageIndex = 1, int pageSize = 5)
        {
            ITracking_TodoService tracking_TodoService = new Tracking_TodoService();
            int count = 0;
            string UserName = this.WorkContext.CurrentUser.UserName;
            var query = new
            {
                UserName = UserName,
                //State = 0
                IsCompleted = false
            };
            var modelList = tracking_TodoService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 获取已办工作数据列表
        /// </summary>
        public ActionResult GetList_ybgz(int pageIndex = 1, int pageSize = 5)
        {
            ITracking_Todo_CompletedService _service = new Tracking_Todo_CompletedService();
            int count = 0;
            string UserName = this.WorkContext.CurrentUser.UserName;
            var query = new
            {
                Actor = UserName
            };
            var modelList = _service.GetForPaging(out count, query, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 获取我的工单数据列表
        /// </summary>
        public ActionResult GetList_wdgd(int pageIndex = 1, int pageSize = 5)
        {
            pageIndex = pageIndex <= 0 ? 0 : (pageIndex - 1);
            pageSize = pageSize <= 0 ? 5 : pageSize;
            ITracking_Todo_CompletedService _service = new Tracking_Todo_CompletedService();
            int count = 0;
            string UserName = this.WorkContext.CurrentUser.UserName;
            var query = new
            {
                Actor = UserName
            };
            var modelList = _service.Entitys.Where(u => u.Actor == UserName && (UserName.Contains(u.CreatorName) || UserName.Contains(u.Creator)))
                .OrderByDescending(u => u.finishedtime);
            count = modelList.Count();
            var curPageData = modelList.Skip(pageIndex * pageSize).Take(pageSize);
            return Json(new { items = curPageData.ToList(), count = count });
        }

        /// <summary>
        /// 获取待阅信息列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetList_dyxx(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            string UserName = this.WorkContext.CurrentUser.UserName;
            var query = new
            {
                UserName = UserName,
                State = 0
            };
            var modelList = tracking_ToReview_DetailService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 获取已阅信息列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetList_yyxx(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            string UserName = this.WorkContext.CurrentUser.UserName;
            var query = new
            {
                UserName = UserName,
                State = 1
            };
            var modelList = tracking_ToReview_DetailService.GetForPaging(out count, query, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 获取“常用流程”和“收藏流程”信息
        /// </summary>
        /// <param name="type">0为常用流程，1为收藏流程</param>
        /// <returns></returns>
        public ActionResult GetFavoriteList(int? type)
        {
            type = type ?? 0;
            ISysFavoriteService _sysFavoriteService = new SysFavoriteService();
            IWorkflowsService _workflowsService = new WorkflowsService();

            var sysFavoriteList = (type == 0 ?
                _sysFavoriteService.List().Where(
                f => f.Type == type && f.EnableFlag == true)
                :
                _sysFavoriteService.List().Where(
                f => f.Type == type && f.EnableFlag == true && f.UserId == WorkContext.CurrentUser.UserId
                )).Select(u => u.WorkflowId).ToList();

            int count = 0;
            var modelList = _workflowsService.GetForPaging(out count, new { Enabled = true, IsCurrent = true, IsDeleted = false })
                .Where(u => sysFavoriteList.Contains(u.WorkflowId)).ToList();

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 获取“常用链接(友情链接)”和“收藏链接”信息
        /// </summary>
        /// <param name="type">0为常用链接，其他为收藏链接</param>
        /// <returns></returns>
        public ActionResult GetFriendLinkList(int? type)
        {
            type = type ?? 0;
            int count = 0;
            if (type == 0)
            {
                ISysLinkService _sysLinkService = new SysLinkService();
                var modelList = _sysLinkService.GetForPaging(out count, new { EnableFlag = true }).OrderBy(u => u.OrderNo);
                return Json(new { items = modelList, count = count });
            }
            else
            {
                ISysFavoriteLinkService _sysFavoriteLinkService = new SysFavoriteLinkService();
                var modelList = _sysFavoriteLinkService.GetForPaging(out count, new { UserId = this.WorkContext.CurrentUser.UserId }).OrderBy(u => u.OrderNo);
                return Json(new { items = modelList, count = count });
            }
        }

        /// <summary>
        /// 获取一个月的日程数据列表
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        public ActionResult GetScheduleMonthData(int? year, int? month)
        {
            IScheduleService _scheduleService = new ScheduleService();
            year = year ?? DateTime.Now.Year;
            month = month ?? DateTime.Now.Month;
            DateTime Time_start = new DateTime();
            DateTime Time_end = new DateTime();
            Time_start = new DateTime(year.Value, month.Value, 1);
            Time_end = Time_start.AddMonths(1).AddSeconds(-1);

            var curUserId = this.WorkContext.CurrentUser.UserId;
            if (curUserId == null || curUserId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return Json(new { items = new List<object>(), count = 0 });
            }

            var condition = new
            {
                UserId = curUserId,
                Time_start = Time_start,
                Time_end = Time_end
            };
            int count = 0;
            var modelList = _scheduleService.GetForPaging(out count, condition);
            modelList = modelList.OrderBy(u => u.Time);
            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 获取有效的公告信息
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns>默认前5条数据</returns>
        public ActionResult GetSysNotice_ValidData(int pageIndex = 1, int pageSize = 5)
        {
            ISysNoticeService _noticeService = new SysNoticeService();
            var queryBuilder = new
            {
                ValidTime_start = DateTime.Now.Date,
                EnableFlag = true
            };
            int count = 0;
            //var condition = queryBuilder.GetType().GetProperties();

            var modelList = _noticeService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            foreach (SysNotice mode in modelList)
            {
                mode.Content = CZHtmlHelper.RemoveHTML(mode.Content);
            }

            return Json(new { items = modelList, count = count }, JsonRequestBehavior.AllowGet);
        }

    }

}