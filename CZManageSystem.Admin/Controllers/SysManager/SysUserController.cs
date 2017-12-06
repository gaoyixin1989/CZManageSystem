using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin;
using CZManageSystem.Admin.Models;
using System.Threading.Tasks;
using CZManageSystem.Admin.Base;

namespace CZManageSystem.Admin.Controllers.SysManager
{

    public class SysUserController : BaseController
    {
        ISysUserService _userService = new SysUserService();
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        // GET: Users
        [SysOperation(OperationType.Browse, "访问用户管理页面")]

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// test
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetAllUsers(int pageIndex = 1, int pageSize = int.MaxValue, UserQueryBuilder queryBuilder = null)
        {

            int count = 0;
            queryBuilder.Status = 0;//只显示未删除用户
            //var modelList = _userService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);


            var modelList = _userService.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var s in modelList)
            {
                List<string> _tmpdplist = CommonFunction.GetDepartMent(s.DpId);
                listResult.Add(new
                {
                    s.UserId,
                    s.UserName,
                    s.RealName,
                    s.Mobile,
                    CreatedTime = s.CreatedTime.ToString(),
                    s.Email,
                    s.EmployeeId,
                    DpName = _tmpdplist[0],
                    DpMName = _tmpdplist[1]
                });
            }
            return Json(new { items = listResult, count = count });

        }

        public ActionResult GetAllUsers_shortData(int pageIndex = 1, int pageSize = int.MaxValue, UserQueryBuilder queryBuilder = null)
        {

            int count = 0;
            queryBuilder.Status = 0;//只显示未删除用户
            var modelList = _userService.GetForPaging_shortData(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }

        public ActionResult GetshortUserData_Inner(int pageIndex = 1, int pageSize = int.MaxValue)
        {

            int count = 0;
            //queryBuilder.Status = 0;//只显示未删除用户
            //var modelList = _userService.GetForPaging_shortData(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);

            var modelList = _userService.List().Where(u => u.Status.HasValue && u.Status == 0 && (u.UserType ?? 0) != 88).Select(u => new
            {
                u.UserId,
                u.UserName,
                u.RealName,
                u.DpId,
                u.Mobile
            });
            return Json(new { items = modelList, count = count });

        }

        /// <summary>
        /// 根据部门id获取其下所有用户的手机号码
        /// </summary>
        /// <param name="listDpId"></param>
        /// <returns></returns>
        public ActionResult GetUserMobileByDeptID(List<string> listDpId)
        {
            List<string> listMobile = new List<string>();
            if (listDpId.Count > 0)
            {
                var allDpIds = _sysDeptmentService.List().Where(u => listDpId.Contains(u.DpId)).Select(u => u.DpId).ToList();
                allDpIds = GetDpIdFromIds(allDpIds);
                listMobile = _userService.List().Where(u => allDpIds.Contains(u.DpId)).Select(u => u.Mobile).ToList();

            }
            return Json(new { items = listMobile });
        }
        /// <summary>
        /// 根据部门组织Ids获取包括自身的所有子节点的id
        /// </summary>
        /// <param name="ids">组织Ids</param>
        /// <returns></returns>
        private List<string> GetDpIdFromIds(List<string> ids)
        {
            List<string> listResult = new List<string>();
            if (ids == null || ids.Count == 0)
                return listResult;
            ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
            List<string> temp = ids;
            while (temp != null && temp.Count > 0)
            {
                listResult.AddRange(temp);
                temp = _sysDeptmentService.List().Where(u => temp.Contains(u.ParentDpId)).Select(u => u.DpId).ToList();
            }
            listResult = listResult.Distinct().ToList();
            return listResult;
        }
        [SysOperation(OperationType.Edit, "编辑用户管理")]
        public ActionResult Edit(string id = "00000000-0000-0000-0000-000000000000")
        {
            ViewData["id"] = Guid.Parse(id);
            return View();
        }

        public ActionResult GET(string id = null)
        {
            Guid guid = new Guid();
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out guid))
                return View("Index");

            var user = _userService.FindById(guid);
            var dept = _sysDeptmentService.FindById(user.DpId);
            string Dpname = dept == null ? "" : dept.DpName;
            return Json(new { user.UserId, user.Email, user.RealName, user.Tel, user.UserName, user.Status, user.Mobile, user.Ext_Str2, user.DpId, Dpname, user.UserType, JoinTime = (user.JoinTime == null ? "" : user.JoinTime.Value.ToString("yyyy-MM-dd")), user.CheckIP });
        }
        /// <summary>
        /// 更加Ids获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult GetDataByIds(Guid[] ids)
        {
            List<Users> listData = new List<Users>();
            if (ids != null && ids.Length > 0)
                listData = _userService.List().Where(u => ids.Contains(u.UserId)).ToList();
            return Json(listData.Select(u => new
            {
                u.UserId,
                u.UserName,
                u.Password,
                u.Email,
                u.Mobile,
                u.Tel,
                u.EmployeeId,
                u.RealName,
                u.Type,
                u.Status,
                u.DpId,
                u.Ext_Int,
                u.Ext_Decimal,
                u.Ext_Str1,
                u.Ext_Str2,
                u.Ext_Str3,
                u.CreatedTime,
                u.LastModTime,
                u.Creator,
                u.LastModifier,
                u.SortOrder,
                u.JoinTime,
                u.UserType,
                u.CheckIP
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login(string id)
        {
            string url = "";
            try
            {
                Guid guid = new Guid();
                if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out guid))
                    return Json(new { status = 1, message = "请选择用户" });

                var user = _userService.FindById(guid);
                if (user != null)
                {

                    this.WorkContext.CurrentUser = user;

                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 1, message = ex.Message });
            }
            return Json(new { status = 0, message = "登陆成功" });
        }
        [SysOperation(OperationType.Delete, "删除用户信息")]

        public ActionResult Delete(Guid[] ids)
        {
            //Delete
            try
            {
                var objs = _userService.List().Where(u => ids.Contains(u.UserId)).ToList();
                if (objs.Count > 0)
                {
                    foreach (var obj in objs)
                    {
                        obj.Status = 1;
                        if (_userService.Update(obj))
                        {
                            return Json(new { status = 0, message = "删除成功" });
                        }
                        else
                        {
                            return Json(new { status = 0, message = "删除失败" });
                        }
                    }

                }
                else
                {
                    return Json(new { status = 0, message = "没有可删除的数据" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "删除失败:" + ex.Message });
            }
            return Json(new { status = 0, message = "删除失败" });
        }
        [SysOperation(OperationType.Save, "保存用户信息")]
        public ActionResult Save(Users user)
        {
            if (string.IsNullOrEmpty(user.UserName))
            {
                return Json(new { status = -1, message = "失败" });
            }

            //用户名不能相同
            var objs = _userService.List().Where(u => u.UserName == user.UserName && u.UserId != user.UserId);
            if (objs.Count() > 0)
            {
                return Json(new { status = -1, message = "该用户名已经被使用，请更换！" });
            }

            if (user.UserId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                var userUp = _userService.FindById(user.UserId);
                userUp.UserName = user.UserName;
                userUp.RealName = user.RealName;
                userUp.Mobile = user.Mobile;
                userUp.Tel = user.Tel;
                userUp.Email = user.Email;
                userUp.Ext_Str2 = user.Ext_Str2;
                userUp.LastModTime = DateTime.Now;
                userUp.UserType = user.UserType;
                userUp.JoinTime = user.JoinTime;
                //userUp .LastModifier =//
                userUp.DpId = user.DpId;
                _userService.Update(userUp);
                return Json(new { status = 0, message = "成功" });
            }
            user.UserId = Guid.NewGuid();
            user.CreatedTime = DateTime.Now;
            //user .Creator =//创建者
            user.Password = Core.Helpers.EncryptHelper.Encrypt("123456");
            _userService.InsertAsync(user);
            return Json(new { status = 0, message = "成功" });
        }

        public ActionResult AssigningRoles(Guid id)
        {
            ViewData["id"] = id;
            return View();
        }
        public async Task<ActionResult> GetAssigningRoles(string id = null)
        {
            Guid guid = new Guid();
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out guid))
                return View("Index");

            var user = await _userService.FindByFeldNameAsync(u => u.UserId == guid);
            ISysRoleService _sysRoleService = new SysRoleService();
            var roles = await _sysRoleService.ListAsync();
            return Json(new { user = new { user.UserId, user.UserName }, UsersInRoles = user.UsersInRoles.Select(r => r.RoleId), roles = roles.Select(r => new { r.RoleId, r.RoleName, r.ParentId }) });
        }
        [SysOperation(OperationType.Setting, "配置用户角色信息")]
        public async Task<ActionResult> SetAssigningRoles(Guid UserId, List<Guid> RoleList)
        {

            UsersInRoles usersInRoles;
            var user = await _userService.FindByFeldNameAsync(u => u.UserId == UserId);
            user.UsersInRoles.Clear();
            if (RoleList != null && RoleList.Count > 0)
            {
                foreach (var item in RoleList)
                {
                    if (user.UsersInRoles.FirstOrDefault(u => u.RoleId == item) == null)
                    {
                        usersInRoles = new UsersInRoles() { UserId = user.UserId, RoleId = item };
                        user.UsersInRoles.Add(usersInRoles);
                    }
                }
            }
            await _userService.UpdateAsync(user);

            if (CacheManagerFactory.MemoryCache.IsSet(user.UserId + "MenuList"))
                CacheManagerFactory.MemoryCache.Remove(user.UserId + "MenuList");

            return Json(new { status = 0, message = "成功" });
            //return Json(new { user = new { user.UserId, user.UserName }, UsersInRoles = user.UsersInRoles.Select(r => r.RoleId), roles = roles.Select(r => new { r.RoleId, r.RoleName }) });
        }
    }
}