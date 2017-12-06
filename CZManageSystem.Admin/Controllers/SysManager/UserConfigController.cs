using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class UserConfigController : BaseController
    {
        IUserConfigService _userConfigService = new UserConfigService();
        // GET: UserConfig
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GET()
        {
            Dictionary<string, string> configDict = new Dictionary<string, string>();
            if (this.WorkContext.CurrentUser.UserId != null && this.WorkContext.CurrentUser.UserId != Guid.Empty)
            {
                List<UserConfig> listConfig = _userConfigService.List().Where(u => u.UserID == this.WorkContext.CurrentUser.UserId).ToList();
                foreach (var item in listConfig)
                {
                    if (!configDict.ContainsKey(item.ConfigName))
                        configDict.Add(item.ConfigName, item.ConfigValue);
                }
            }
            return Json(configDict, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save(List<UserConfig> listConfig)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            if (this.WorkContext.CurrentUser.UserId != null && this.WorkContext.CurrentUser.UserId != Guid.Empty)
            {
                var listOld = _userConfigService.List().Where(u => u.UserID == this.WorkContext.CurrentUser.UserId).ToList();
                if (listOld.Count > 0)
                    _userConfigService.DeleteByList(listOld);

                foreach (var item in listConfig)
                {
                    item.ID = Guid.NewGuid();
                    item.UserID = this.WorkContext.CurrentUser.UserId;
                }

                result.IsSuccess = _userConfigService.InsertByList(listConfig);
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "没有登录用户";
            }

            if (!result.IsSuccess && string.IsNullOrEmpty(result.Message))
            {
                result.Message = "保存失败";
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}