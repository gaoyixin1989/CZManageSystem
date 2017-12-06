using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CZManageSystem.Service.SysManger
{
    public class SysMenuService : BaseService<SysMenu>, ISysMenuService
    {

        //  private readonly IRepository<SysMenu> _SysMenuRepository;
        //private readonly IRepository<RolesInResources> _UResourcesRepository;
        //private readonly IRepository<Users> _userRepository;
        private readonly SysUsersInRolesService _URolesRepository;//IRepository<UsersInRoles>
        private readonly RolesInResourcesService _UResourcesRepository ;
        private readonly SysUserService _userRepository;

        #region 实例化

        public SysMenuService()
        {
            // this._SysMenuRepository = new EfRepository<SysMenu>();
            //_dbContext = new SystemContext<SysMenu>();
            // this._SysMenuRepository = new EfRepository<SysMenu>();
            this._URolesRepository = new SysUsersInRolesService();// new EfRepository<UsersInRoles>();
            this._UResourcesRepository = new RolesInResourcesService();//= new EfRepository<RolesInResources>();
            this._userRepository = new SysUserService();// = new EfRepository<Users>();
        }
        #endregion
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            return new PagedList<SysMenu>(this._entityStore.Table.OrderBy(c => c.Id), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count)
                .Select(m => new
                {
                    m.Id,
                    m.MenuName,
                    m.MenuType,
                    m.PageUrl,
                    m.ParentId,
                    m.ResourceId,
                    m.EnableFlag,
                    m.Remark
                });
        }
        public SysMenu GetMenuById(int MenuId)
        {
            return this._entityStore.GetById(MenuId);
        }
        public IList<SysMenu> GetMenuByPid(int Pid)
        {
            return this._entityStore.Table.Where(t => t.ParentId == Pid && t.EnableFlag == true).OrderBy(t => t.OrderNo).ToList<SysMenu>();
        }

        public IList<SysMenu> getMenuByUser(string username, Guid userId)
        {
            var menuList = new List<SysMenu>();
            //是否有缓存
            var isSet = CacheManagerFactory.MemoryCache.IsSet(userId + "MenuList");
            if (isSet)
            {
                menuList = CacheManagerFactory.MemoryCache.Get<List<SysMenu>>(userId + "MenuList");
            }
            else {
                //var userir = from ur in _URolesRepository.List()
                //             where ur.UserId ==
                //                (from u in _userRepository.List()
                //                 where u.UserName == username
                //                 select u.UserId).FirstOrDefault()
                //             select ur.RoleId;

                var userir = _URolesRepository.List().Where(ur => ur.UserId == userId).Select(ur => ur.RoleId);
                var roleir = _UResourcesRepository.List().Where(r => userir.ToList().Contains(r.RoleId)).Select(r => r.ResourceId);
                menuList = this._entityStore.Table.Where(m => roleir.Contains(m.ResourceId) && m.EnableFlag == true).OrderBy(m => m.OrderNo).ToList();
                CacheManagerFactory.MemoryCache.Set(userId + "MenuList", menuList, int.MaxValue);
            }
            return menuList;
        }
    }
}
