using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin;
using CZManageSystem.Core;
using CZManageSystem.Admin.Models;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class tempRole
    {
        public Guid RoleId { get; set; }

        public string RoleName { get; set; }
        // public DateTime EndTime { get; set; }
        public string Comment { get; set; }
        public List<tempRole> children { get; set; }
        public List<Guid> userList { get; set; }
    }
    public class tempResources
    {
        public string ResourceId { get; set; }

        public string ParentId { get; set; }

        public string name { get; set; }

        public List<tempResources> children { get; set; }
    }
    public class SysRoleController : BaseController
    {
        // GET: SysRole
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult Edit(string id = "00000000-0000-0000-0000-000000000000")
        {
            ViewData["id"] = id;
            return View();
        }
        public ActionResult RolesInResources(string id = "00000000-0000-0000-0000-000000000000")
        {
            ViewData["id"] = id;
            return View();
        }
        #region
        /// <summary>
        /// test
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        //public ActionResult GetAllSysRole(int pageIndex = 1, int pageSize = 5, string RoleName = null)
        //{
        //    ISysRoleService _sysRoleService = new SysRoleService();
        //    int count = 0;
        //    var modelList = _sysRoleService.GetForPaging (out count, null, pageIndex <= 0 ? 0 : pageIndex, pageSize);
        //    return Json(new { items = modelList, count = count });
        //}
        #endregion
        public ActionResult GET(Guid id)
        {
            ISysRoleService _sysRoleService = new SysRoleService();
            var role = _sysRoleService.FindById(id);
            var prole = _sysRoleService.FindById(role.ParentId);//根据父级id找父级菜单
            return Json(new
            {
                role.IsInheritable,
                role.RoleId,
                role.RoleName,
                role.ParentId,
                BeginTime = role.BeginTime.HasValue ? role.BeginTime.Value.ToString("yyyy-MM-dd") : "",
                EndTime = role.EndTime.HasValue ? role.EndTime.Value.ToString("yyyy-MM-dd") : "",
                role.Comment,
                role.SortOrder,
                pname = prole.RoleName
            });
        }

        //修改新增
        public ActionResult Save(Roles role)
        {//
            if (string.IsNullOrEmpty(role.RoleName))
            {
                return Json(new { status = -1, message = "失败" });
            }
            ISysRoleService _sysRoleService = new SysRoleService();
            role.LastModTime = DateTime.Now;
            // role.LastModifier = "admin";
            role.LastModifier = this.WorkContext.CurrentUser.UserName;
            if (role.RoleId != Guid.Parse("00000000-0000-0000-0000-000000000000"))//修改
            {
                if (_sysRoleService.Update(role))
                {
                    return Json(new { status = 0, message = "成功" });
                }
                else
                {
                    return Json(new { status = 0, message = "失败" });
                }

            }
            else//新增
            {
                role.RoleId = Guid.NewGuid();
                role.CreatedTime = DateTime.Now;
                //role.Creator = "admin";
                role.Creator = this.WorkContext.CurrentUser.UserName;
                if (_sysRoleService.Insert(role))
                {
                    return Json(new { status = 0, message = "成功" });
                }
                else
                {
                    return Json(new { status = 0, message = "失败" });
                }
            }

        }

        /// <summary>
        /// 根据父级id递归子节点，返回tempRole列表
        /// </summary>
        /// <param name="allRole"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<tempRole> AddList(List<Roles> allRole, Guid id)
        {
            List<tempRole> listRole = new List<tempRole>();
            tempRole tempRole = null;
            foreach (Roles role in allRole.Where(r => r.ParentId == id))
            {//遍历所有子项，没有的话也会执行这一步
                tempRole = new tempRole()
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName,
                    // EndTime = (DateTime)role.EndTime,
                    Comment = role.Comment,
                    userList = role.UsersInRoles.Select(u => u.UserId).ToList(),
                    children = new List<tempRole>()
                };
                if (allRole.Exists(r => r.ParentId == role.RoleId))//存在子项
                {

                    tempRole.children = AddList(allRole, role.RoleId);
                }
                listRole.Add(tempRole);
            }
            return listRole;//
        }
        public ActionResult DeleteById(Guid id)
        {
            var _sysRoleService = new SysRoleService();
            var _sysRolesInResourcesService = new RolesInResourcesService();
            List<Roles> rolesList = new List<Roles>();
            var rolesRoot = _sysRoleService.FindById(id);
            rolesList.Add(rolesRoot);
            //获取角色对应的用户
            var userIdList = _sysRolesInResourcesService.GetUserIdByroleId(id);
            DeleteByIdList(id, rolesList, userIdList);
            return Json(new { status = 0, message = "删除成功" });
        }

        private void DeleteByIdList(Guid id, List<Roles> rolesList, List<Guid> userIdList)
        {
            var _sysRoleService = new SysRoleService();
            var _sysRolesInResourcesService = new RolesInResourcesService();
            IList<Roles> rolesRoot = _sysRoleService.GetRolesByPid(id);
            ////获取角色对应的用户
            //var userIdList = _sysRolesInResourcesService.GetUserIdByroleId(id);
            if (rolesRoot.Count > 0)
            {
                foreach (var item in rolesRoot.Where(r => r.ParentId == id))
                {
                    rolesList.Add(item);
                    //根据roleid查询userid，添加到用户列表
                    var childuserid = _sysRolesInResourcesService.GetUserIdByroleId(item.RoleId);
                    userIdList.AddRange(childuserid);
                    if (rolesRoot.ToList().Exists(m => m.ParentId == item.RoleId))
                    {
                        DeleteByIdList(item.RoleId, rolesList, userIdList);
                    }
                }
            }
            _sysRoleService.DeleteByList(rolesList);
            //移除所有删除的角色及其子节点角色菜单权限缓存
            System.Threading.Tasks.Parallel.ForEach(userIdList, (item) =>
            {
                if (CacheManagerFactory.MemoryCache.IsSet(item + "MenuList"))
                    CacheManagerFactory.MemoryCache.Remove(item + "MenuList");
            });
        }

        /// <summary>
        /// 递归获取菜单层级，并返回jason列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GETList()
        {
            ISysRoleService _sysRoleService = new SysRoleService();
            var listRoles = AddList(_sysRoleService.List().OrderBy(r => r.SortOrder).ToList(), Guid.Parse("00000000-0000-0000-0000-000000000000"));
            var rtn = Json(listRoles);
            return rtn;
        }

        public ActionResult GetAllRoleData()
        {
            ISysRoleService _sysRoleService = new SysRoleService();

            var modelList = _sysRoleService.List().ToList();// (out count,null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    RoleId = item.RoleId,
                    ParentId = item.ParentId,
                    RoleName = item.RoleName
                });
            }

            return Json(new { items = listResult, count = listResult.Count() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetResources(Guid id)
        {
            var _sysRoleService = new SysRoleService();
            var _sysRolesInResourcesService = new RolesInResourcesService();
            var role = _sysRoleService.FindById(id);
            var ResourceId = _sysRolesInResourcesService.List().Where(x => x.RoleId == id).Select(x => x.ResourceId).ToList(); //_sysRolesInResourcesService.FindById(id);
            return Json(new { role.RoleId, role.RoleName, role.ParentId, ResourceIds = ResourceId });

        }

        public ActionResult GetResourcesList()
        {
            IResourcesService _sysResourcesService = new ResourcesService();
            var listResources = AddResourcesList(_sysResourcesService.List().ToList(), "00");///系统菜单资源
            listResources.AddRange(AddResourcesList(_sysResourcesService.List().ToList(), "11"));//流程资源
            return Json(listResources);

        }
        public List<tempResources> AddResourcesList(List<Resources> allResources, string id)
        {
            List<tempResources> listResources = new List<tempResources>();
            tempResources tempResources = null;
            #region
            //var list = allResources.Where(r => r.ParentId == id).ToList();
            //System.Threading.Tasks.Parallel.ForEach(list, (resources)=> {
            //    tempResources = new tempResources()
            //    {
            //        ResourceId = resources.ResourceId,
            //        ParentId = resources.ParentId,
            //        Alias = resources.Alias,
            //        children = new List<tempResources>()
            //    };
            //    {//存在子项

            //        tempResources.children = AddResourcesList(allResources, resources.ResourceId);
            //    }
            //    listResources.Add(tempResources);
            //});
            #endregion

            foreach (Resources resources in allResources.Where(r => r.ParentId == id))
            {//遍历所有子项，没有的话也会执行这一步
                tempResources = new tempResources()
                {
                    ResourceId = resources.ResourceId,
                    ParentId = resources.ParentId,
                    name = resources.Alias,
                    children = new List<tempResources>()
                };
                if (resources != null)
                {//存在子项

                    tempResources.children = AddResourcesList(allResources, resources.ResourceId);
                }
                listResources.Add(tempResources);
            }
            return listResources;//

        }

        public ActionResult SaveRoRe(IList<string> ResourceList, string RoleId)
        {
            if (ResourceList == null || ResourceList.Count == 0)
            {
                return Json(new { status = -1, message = "失败" });
            }
            List<RolesInResources> listRoleInR = new List<RolesInResources>();
            Guid GRoleId = Guid.Parse(RoleId);
            var _sysRolesInResourcesService = new RolesInResourcesService();
            // var resourceIdList = ResourceList.Select(x => x.ResourceId).ToList();
            //与数据库比较,若列表无，数据库有则将数据库这部分数据删除
            var otherList = _sysRolesInResourcesService.List()
                .Where(r => r.RoleId == GRoleId && !ResourceList.Contains(r.ResourceId)).ToList();
            _sysRolesInResourcesService.DeleteByList(otherList);
            //反之则执行新增
            var rolesInResources = default(RolesInResources);
            foreach (var item in ResourceList)
            {
                if (!_sysRolesInResourcesService.Any(item, GRoleId))
                {
                    rolesInResources = new Data.Domain.SysManger.RolesInResources()
                    {
                        ResourceId = item,
                        RoleId = GRoleId
                    };
                    listRoleInR.Add(rolesInResources);
                }
            }
            _sysRolesInResourcesService.InsertByList(listRoleInR);
            //根据roleid查询userid，移除当前角色菜单权限缓存
            var userIdList = _sysRolesInResourcesService.GetUserIdByroleId(GRoleId);
            System.Threading.Tasks.Parallel.ForEach(userIdList, (item) =>
            {
                if (CacheManagerFactory.MemoryCache.IsSet(item + "MenuList"))
                    CacheManagerFactory.MemoryCache.Remove(item + "MenuList");
            });
            return Json(new { status = 0, message = "成功" });
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult SaveRoleUsers(Guid roleId, List<Guid> listUsers)
        {
            if (listUsers == null) listUsers = new List<Guid>();
            SystemResult result = new SystemResult() { IsSuccess = true };
            ISysUsersInRolesService service = new SysUsersInRolesService();

            var oldData = service.List().Where(u => u.RoleId == roleId).ToList();
            if (oldData.Count() > 0)
                service.DeleteByList(oldData);
            List<UsersInRoles> list = new List<UsersInRoles>();
            listUsers = listUsers.Distinct().ToList();
            foreach (var item in listUsers)
            {
                list.Add(new UsersInRoles() { RoleId = roleId, UserId = item });
            }
            if (list.Count > 0)
                service.InsertByList(list);


            return Json(result);
        }


    }
}