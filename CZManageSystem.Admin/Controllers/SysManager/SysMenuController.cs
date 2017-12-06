using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class tempMenu
    {
        public int MenuId { get; set; }

        public string MenuName { get; set; }
        public int? MenuLevel { get; set; }//为空的时候就是0？
        public string MenuFullName { get; set; }
        public string PageUrl { get; set; }
        public string MenuType { get; set; }
        public bool? EnableFlag { get; set; }
        public int? OrderNo { get; set; }
        public List<tempMenu> children { get; set; }
    }
    public class SysMenuController : BaseController
    {
        // private readonly SystemContext syscontext = new SystemContext();
        // GET: SysMenu
        private readonly SysMenuService menudb = new SysMenuService();
        private readonly SysUserService sysUserService = new SysUserService();
        [SysOperation(OperationType.Browse, "访问菜单管理页面")]
        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult Edit(int id = 0)
        {
            ViewData["id"] = id;
            return View();
        }

        #region
        //public ActionResult GetAllMenu(int pageIndex = 1, int pageSize = 5, MenuBuilderModel menuBuilderModel=null)
        //{
        //    ISysMenuService _sysMenuService = new SysMenuService();

        //    int count = 0;
        //    // var modelList = _sysMenuService.GetAllMenu(out count, menuBuilderModel.MenuName, menuBuilderModel.MenuType, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

        //    var modelList = _sysMenuService.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
        //    return Json(new { items = modelList, count = count });

        //}
        #endregion


        /// <summary>
        /// 根据id获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GET(int id)
        {

            ISysMenuService _sysMenuService = new SysMenuService();
            var menu = _sysMenuService.FindById(id);
            return Json(new { menu.Id, menu.MenuName, menu.MenuFullName, menu.Remark, menu.ParentId, menu.ResourceId, menu.MenuType, menu.OrderNo, menu.PageUrl, menu.MenuLevel, menu.EnableFlag });
        }
        private void RemoveMemoryCache()
        {
            var userIdList = sysUserService.List().Select(u => u.UserId);
            System.Threading.Tasks.Parallel.ForEach(userIdList, (item) =>
            {
                if (CacheManagerFactory.MemoryCache.IsSet(item + "MenuList"))
                    CacheManagerFactory.MemoryCache.Remove(item + "MenuList");
            });
        }

        /// <summary>
        /// 新增，修改
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Save, "保存菜单信息")]
        public ActionResult Save(SysMenu menu)
        {
            bool addResource = false;
            ISysMenuService _sysMenuService = new SysMenuService();
            IResourcesService _ResourcesService = new ResourcesService();
            var rs = _ResourcesService.FindById(menu.ResourceId);
            if (rs == null)
            {
                rs = new Resources();
                addResource = true;
            }
            if (menu.ParentId == 0)
            {
                rs.ParentId = "CD0";
            }
            else
            {
                var pmenue = _sysMenuService.FindById((int)menu.ParentId);
                rs.ParentId = pmenue.ResourceId.ToString();
            }
            if (string.IsNullOrEmpty(menu.MenuName))
            {
                return Json(new { status = -1, message = "失败" });
            }
            rs.Name = menu.PageUrl;
            rs.Alias = menu.MenuName;
            rs.CreatedTime = DateTime.Now;
            rs.Enabled = menu.EnableFlag;
            rs.Visible = menu.EnableFlag;
            rs.SortIndex = menu.OrderNo;
            rs.Type = menu.MenuType;
            //id>0为修改，否则新增
            if (menu.Id > 0)
            {
                //_sysMenuService.updateMenu(menu);
                rs.ResourceId = menu.ResourceId;
                _sysMenuService.Update(menu);
                if (addResource)
                    _ResourcesService.Insert(rs);
                else
                    _ResourcesService.Update(rs);
                RemoveMemoryCache();
                return Json(new { status = 0, message = "成功" });
            }
            else
            {
                // _sysMenuService.addMenu(menu);            
                string var = _ResourcesService.GetResourcesMaxId();
                rs.ResourceId = var;
                menu.ResourceId = var;
                bool r = _ResourcesService.Insert(rs);
                bool m = _sysMenuService.Insert(menu);
                if (!r || !m)
                {
                    return Json(new { status = 0, message = "失败" });
                }
                else
                {
                    return Json(new { status = 0, message = "成功" });
                }

            }

        }

        /// <summary>
        /// 根据父级id递归子节点，返回list列表
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        private List<tempMenu> AddList(List<SysMenu> menuList, int Id)
        {
            List<tempMenu> list = new List<tempMenu>();
            tempMenu tMenu = null;
            foreach (var menu in menuList.Where(m => m.ParentId == Id))
            {
                tMenu = new tempMenu()
                {
                    MenuId = menu.Id,
                    MenuName = menu.MenuName,
                    MenuFullName = menu.MenuFullName,
                    MenuLevel = menu.MenuLevel,
                    MenuType = menu.MenuType,
                    PageUrl = menu.PageUrl,
                    EnableFlag = menu.EnableFlag,
                    OrderNo = menu.OrderNo,
                    children = new List<tempMenu>()

                };
                if (menuList.Exists(m => m.ParentId == menu.Id))
                {
                    tMenu.children = AddList(menuList, menu.Id);
                }
                list.Add(tMenu);
            }
            return list;
        }
        /// <summary>
        /// 递归获取菜单层级，并返回jason列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GETList()
        {

            ISysMenuService _sysMenuService = new SysMenuService();
            var listMenu = AddList(_sysMenuService.List().OrderBy(u => u.OrderNo ?? 999999).ToList(), 0);
            var rtn = Json(listMenu);
            return rtn;
        }

        /// <summary>
        /// 通过id删除并递归删除子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SysOperation(OperationType.Delete, "删除菜单")]

        public ActionResult DeleteById(int id)
        {
            var _sysMenuService = new SysMenuService();
            List<Roles> rolesList = new List<Roles>();
            //获取角色对应的用户
            var Menue = _sysMenuService.FindById(id);
            List<SysMenu> MenueList = new List<SysMenu>();
            MenueList.Add(Menue);
            bool b = DeleteByIdList(id, MenueList);
            if (!b)
            {
                return Json(new { status = 0, message = "删除失败" });
            }
            RemoveMemoryCache();
            return Json(new { status = 0, message = "删除成功" });
        }
        [SysOperation(OperationType.Delete, "删除菜单")]

        private bool DeleteByIdList(int id, List<SysMenu> MenueList)
        {
            var _sysMenuService = new SysMenuService();
            IList<SysMenu> menueRoot = _sysMenuService.GetMenuByPid(id);
            if (menueRoot.Count > 0)
            {
                foreach (var item in menueRoot.Where(r => r.ParentId == id))
                {
                    MenueList.Add(item);
                    if (menueRoot.ToList().Exists(m => m.ParentId == item.Id))
                    {
                        DeleteByIdList(item.Id, MenueList);
                    }
                }
            }
            return _sysMenuService.DeleteByList(MenueList);
        }



    }


}
