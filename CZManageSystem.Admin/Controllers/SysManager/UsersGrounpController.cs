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
    public class MemberType
    {
        public const string user = "user";
        public const string dept = "dept";
        public const string role = "role";
    }
    public class UsersGrounpController : BaseController
    {
        IUsersGrounpService _groupService = new UsersGrounpService();
        IUsersGrounp_MemberService _menberService = new UsersGrounp_MemberService();
        // GET: UsersGrounp
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "新增群组";
            else
                ViewBag.Title = "编辑群组";
            return View();
        }

        public ActionResult SelectUsers(string selectedId,string startDeptId)
        {
            ViewData["selectedId"] = selectedId;
            ViewData["startDeptId"] = startDeptId;
            return View();
        }
        public ActionResult SelectDepts(string selectedId, string startId)
        {
            ViewData["selectedId"] = selectedId;
            ViewData["startId"] = startId;
            return View();
        }
        public ActionResult SelectRoles(string selectedId)
        {
            ViewData["selectedId"] = selectedId;
            return View();
        }

        /// <summary>
        /// 通过群组选择用户
        /// </summary>
        /// <param name="selectedId"></param>
        /// <returns></returns>
        public ActionResult SelectGroupToUsers(string selectedId) {
            ViewData["selectedId"] = selectedId;
            return View();
        }

        /// <summary>
        /// 获取群组信息列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="GroupName"></param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string GroupName = null)
        {
            var condition = new
            {
                GroupName = GroupName,
                UserId = this.WorkContext.CurrentUser.UserId
            };
            int count = 0;
            var modelList = _groupService.GetForPaging(out count, condition, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (UsersGrounp)u);
            List<object> result = new List<object>();

            foreach (var item in modelList)
            {
                result.Add(new
                {
                    Id = item.Id,
                    GroupName = item.GroupName,
                    CreatedTime = item.CreatedTime.HasValue ? item.CreatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    UserId = item.UserId,
                    Remark = item.Remark,
                    Members = GetMemberInfoByGroupId(item.Id)
                });
            }
            return Json(new { items = result, count = count });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(int id)
        {
            var obj = _groupService.FindById(id);
            return Json(new
            {
                Id = obj.Id,
                GroupName = obj.GroupName,
                CreatedTime = obj.CreatedTime.HasValue ? obj.CreatedTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                UserId = obj.UserId,
                Remark = obj.Remark,
                Members = GetMemberInfoByGroupId(obj.Id)
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据群组Id获取群组成员信息
        /// </summary>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        public List<object> GetMemberInfoByGroupId(int GroupId)
        {
            List<object> result = new List<object>();
            var members = _menberService.List().Where(u => u.GroupId == GroupId).ToList();
            var userMemberIds = members.Where(u => u.MemberType == MemberType.user).Select(u => Guid.Parse(u.MemberId)).ToList();//用户
            var deptMemberIds = members.Where(u => u.MemberType == MemberType.dept).Select(u => u.MemberId).ToList();//部门
            var roleMemberIds = members.Where(u => u.MemberType == MemberType.role).Select(u => Guid.Parse(u.MemberId)).ToList();//角色
            //部门
            if (deptMemberIds.Count > 0)
            {
                var users = new SysDeptmentService().List().Where(u => deptMemberIds.Contains(u.DpId));
                foreach (var item in users)
                {
                    result.Add(new
                    {
                        type = MemberType.dept,
                        id = item.DpId,
                        text = item.DpName
                    });
                }
            }
            //角色
            if (roleMemberIds.Count > 0)
            {
                var users = new SysRoleService().List().Where(u => roleMemberIds.Contains(u.RoleId));
                foreach (var item in users)
                {
                    result.Add(new
                    {
                        type = MemberType.role,
                        id = item.RoleId,
                        text = item.RoleName
                    });
                }
            }
            //用户
            if (userMemberIds.Count > 0)
            {
                var users = new SysUserService().List().Where(u => userMemberIds.Contains(u.UserId));
                foreach (var item in users)
                {
                    result.Add(new
                    {
                        type = MemberType.user,
                        id = item.UserId,
                        userName=item.UserName,
                        text = item.RealName
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ActionResult Save(UsersGrounp dataObj, string[] userIds, string[] deptIds, string[] roleIds)
        {
            string[] mm = { "1", "2" };
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.GroupName == null || string.IsNullOrEmpty(dataObj.GroupName.Trim()))
                tip = "群组名称不能为空";
            else
            {
                isValid = true;
                dataObj.GroupName = dataObj.GroupName.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.Id == 0)//新增
            {
                dataObj.UserId = this.WorkContext.CurrentUser.UserId;
                dataObj.CreatedTime = DateTime.Now;
                result.IsSuccess = _groupService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _groupService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                //保存群组成员信息
                var members_old = _menberService.List().Where(u => u.GroupId == dataObj.Id).ToList();//旧成员列表
                List<UsersGrounp_Member> members_new = new List<UsersGrounp_Member>();//新成员列表
                #region 添加新成员信息
                if (userIds != null)
                    foreach (var id in userIds)
                    {
                        if (!string.IsNullOrEmpty(id))
                            members_new.Add(new UsersGrounp_Member()
                            {
                                GroupId = dataObj.Id,
                                MemberType = MemberType.user,
                                MemberId = id
                            });
                    }
                if (deptIds != null)
                    foreach (var id in deptIds)
                    {
                        if (!string.IsNullOrEmpty(id))
                            members_new.Add(new UsersGrounp_Member()
                            {
                                GroupId = dataObj.Id,
                                MemberType = MemberType.dept,
                                MemberId = id
                            });
                    }
                if (roleIds != null)
                    foreach (var id in roleIds)
                    {
                        if (!string.IsNullOrEmpty(id))
                            members_new.Add(new UsersGrounp_Member()
                            {
                                GroupId = dataObj.Id,
                                MemberType = MemberType.role,
                                MemberId = id
                            });
                    }
                members_new = members_new.Distinct().ToList();
                #endregion

                var members_ToDel = members_old.Where(u => !(members_new.Select(m => m.MemberType + m.MemberId).ToList()).Contains(u.MemberType + u.MemberId)).ToList(); //待删除的成员
                var members_Add = members_new.Where(u => !(members_old.Select(m => m.MemberType + m.MemberId).ToList()).Contains(u.MemberType + u.MemberId)).ToList();//新增加的群组成员

                _menberService.DeleteByList(members_ToDel);
                _menberService.InsertByList(members_Add);

            }

            return Json(result);
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(int[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;

            foreach (int id in ids)
            {
                var obj = _groupService.FindById(id);
                if (_groupService.Delete(obj))
                {//删除群组成员信息
                    isSuccess = true;
                    successCount++;
                    var members = _menberService.List().Where(u => u.GroupId == id).ToList();
                    _menberService.DeleteByList(members);
                }
            }
            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }
        public ActionResult GetUsersGroup()
        {
            var modelList = _groupService.List().Where(s => s.UserId == WorkContext.CurrentUser.UserId).ToList();
            var result = modelList.Select(l => new
            {
                l.Id,
                l.GroupName,
                Members = GetMembers(l.Id)
            });

            return Json(result);
        }

        public IEnumerable<dynamic> GetMembers(int GroupId)
        {
            var members = _menberService.List().Where(u => u.GroupId == GroupId && u.MemberType == MemberType.user).ToList().Select(u => Guid.Parse(u.MemberId));
            //用户
            var users = new SysUserService().List().Where(u => members.Contains(u.UserId)).Select(u => new
            {
                u.UserId,
                u.RealName
            });
            return users;
        }
    }
}