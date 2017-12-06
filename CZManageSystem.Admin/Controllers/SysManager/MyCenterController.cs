using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Admin.Base;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class MyCenterController : BaseController
    {
        // GET: MyCenter
        [SysOperation(OperationType.Browse, "访问个人中心")]
        public ActionResult Edit()
        {
            return View();
        }
        public ActionResult GET()
        {
            //users.UserName = this.WorkContext.CurrentUser.UserName;
            //ISysUserService _userService = new SysUserService();
            //Expression<Func<Users, bool>> _Expression = u => u.UserName == users.UserName;
            //var user = _userService.FindByFeldName(_Expression);
            //return Json(new { user.UserId, user.Email, user.RealName, user.Tel, user.UserName, user.Status, user.Mobile, user.Ext_Str2 });
            //return Json(this.WorkContext.CurrentUser);

            var user = new
            {
                UserId = this.WorkContext.CurrentUser.UserId,
                UserName = this.WorkContext.CurrentUser.UserName,
                RealName = this.WorkContext.CurrentUser.RealName,
                Mobile = this.WorkContext.CurrentUser.Mobile,
                Ext_Str2 = this.WorkContext.CurrentUser.Ext_Str2,
                Email = this.WorkContext.CurrentUser.Email,
                EmployeeId = this.WorkContext.CurrentUser.EmployeeId
            };
            List<string> _tmpdplist = CommonFunction.GetDepartMent(this.WorkContext.CurrentUser.DpId);
            return Json(new { user = user, DpName = _tmpdplist[0] , DpMName= _tmpdplist[1] });
        }
        [SysOperation(OperationType.Save, "保存个人中心数据")]
        public ActionResult Save(Users user)
        {
            ISysUserService _userService = new SysUserService();
            var userUp = _userService.FindById(user.UserId);
            //userUp.UserName = user.UserName;
            //userUp.RealName = user.RealName;
            userUp.Mobile = user.Mobile;
            userUp.Email = user.Email;
            userUp.Ext_Str2 = user.Ext_Str2;
            userUp.LastModTime = DateTime.Now;
            _userService.Update(userUp);
            return Json(new { status = 0, message = "成功" });

        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Passwd()
        {
            @ViewBag.UserName = this.WorkContext.CurrentUser.UserName;
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [SysOperation(OperationType.Edit, "修改密码")]
        public ActionResult ChangePassword(Passwd user)
        {

            user.UserName = this.WorkContext.CurrentUser.UserName;

            ISysUserService _sysUserService = new SysUserService();
            Expression<Func<Users, bool>> _Expression = u => u.UserName == user.UserName;
            Users sysUser = _sysUserService.FindByFeldName(_Expression);
            sysUser.Password = EncryptHelper.Decrypt(sysUser.Password);
            if (sysUser.Password == user.Password)
            {
                if (sysUser.Password != user.NewPassword)
                {
                    if (user.NewPassword == user.ConfirmPassword)
                    {
                        sysUser.Password = EncryptHelper.Encrypt(user.NewPassword);
                        _sysUserService.Update(sysUser);
                    }
                    else
                    {
                        return Json(new { status = -1, message = "确认新密码不正确" });
                    }
                }
                else
                { return Json(new { status = -1, message = "新旧密码不能相同" }); }
            }
            else
            {
                return Json(new { status = -1, message = "原始密码不正确" });
            }
            return Json(new { status = 0, message = "成功" });
            //return Json();
        }
    }
}