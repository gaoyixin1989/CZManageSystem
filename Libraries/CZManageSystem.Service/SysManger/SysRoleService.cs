using System;
using System.Collections.Generic;
using System.Linq;
using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Service.SysManger
{
    public partial class SysRoleService : BaseService<Roles>, ISysRoleService 
    {
        private readonly IRepository<Roles> _SysRolesRepository;
        #region 实例化

        public SysRoleService()
        {
            this._SysRolesRepository = new EfRepository<Roles>();
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
            return new PagedList<Roles>(this._entityStore.Table.OrderBy(c => c.RoleId), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count)
                .Select(u => new
                {
                    u.RoleId,
                    u.RoleName,
                    u.Comment
                });
        }
        public IList<Roles> GetRolesByPid(Guid Pid)
        {
            return this._SysRolesRepository.Table.Where(t => t.ParentId == Pid).OrderBy(t => t.SortOrder).ToList<Roles>();
        }
    }
}