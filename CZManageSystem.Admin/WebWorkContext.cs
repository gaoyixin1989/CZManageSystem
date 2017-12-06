using System;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;


namespace CZManageSystem.Admin
{
    /// <summary>
    /// Work context for web application
    /// </summary>
    public partial class WebWorkContext
    {
        #region Const

        private const string UserCookieName = "CZMS.SysUser";

        #endregion

        #region Fields

        private  HttpContextBase _httpContext=null;
        private ISysUserService _userService =null;

        private ISysDeptmentService _sysDeptmentService = null;
        //private   WebWorkContext _webWorkContext=null;


        private Users _cachedUser;


        #endregion
        //public static WebWorkContext getWebWorkContent()
        //{
        //    //if (_webWorkContext == null)
        //    //{
        //    //    _webWorkContext = new WebWorkContext();
        //    //}
        //    _webWorkContext = new WebWorkContext();
        //    return _webWorkContext;
        //}

        //public static void ClearCurrentUser()
        //{
        //    _
        //}
        public WebWorkContext()
        {
            _httpContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            _userService = new SysUserService();
            _sysDeptmentService = new SysDeptmentService();

        }
        protected virtual HttpCookie GetUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[UserCookieName];
        }

        public virtual void ClearCurrentUser()
        {
            _httpContext.Response.Cookies[UserCookieName].Expires= DateTime.Now.AddMonths(-1);
            
        }
        protected virtual void SetUserCookie(Users user)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(UserCookieName);
                cookie.HttpOnly = true;
                cookie.Value = user.UserId.ToString();

                if (user.UserId == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
               
                //else
                //{
                //    int cookieExpires = 24 * 365; //TODO make configurable
                //    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                //}

                _httpContext.Response.Cookies.Remove(UserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
                #region 为了验证流程易用户与CZMS用户是不一致，添加UserName
                var UNameCookie = new HttpCookie("UNameCookie");
                UNameCookie.HttpOnly = true;
                UNameCookie.Value = user.UserName;
                _httpContext.Response.Cookies.Add(UNameCookie);
                #endregion
            }
        }


        #region Properties

        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        public virtual Users CurrentUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;

                Users sysUser = null;

                var userCookie = GetUserCookie();
                if (userCookie != null && !String.IsNullOrEmpty(userCookie.Value))
                {
                    Guid userGuid;
                    if (Guid.TryParse(userCookie.Value, out userGuid))
                    {
                        var userByCookie = _userService.FindById(userGuid);
                        //var dept = _sysDeptmentService.FindById(userByCookie.DpId);
                        //userByCookie.Dept = dept;
                        if (userByCookie != null)
                            sysUser = userByCookie;
                    }
                }
            

             

                //validation
                if(sysUser!=null)
                { 
                    if (sysUser.Status==0)
                    {
                        SetUserCookie(sysUser);
                        _cachedUser = sysUser;
                    }
                }
                return _cachedUser;
            }
    
            set
            {
                SetUserCookie(value);
                _cachedUser = value;
            }
        }


     

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}
