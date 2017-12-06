using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.SysManger
{

    public partial class RolesInResourcesService: BaseService<RolesInResources>, IRolesInResourcesService
    {
        private readonly IRepository<RolesInResources> _rolesInResources;
        private readonly IRepository<UsersInRoles> _URolesRepository;//

        public RolesInResourcesService()
        {
            // this._SysMenuRepository = new EfRepository<SysMenu>();
            //_dbContext = new SystemContext<SysMenu>();
            this._rolesInResources = new EfRepository<RolesInResources>();
            this._URolesRepository = new EfRepository<UsersInRoles>();
        }

        public bool Any(string resourcesId, Guid roleId)
        {
            return _rolesInResources.Contains(x=>x.ResourceId == resourcesId&&x.RoleId == roleId);
        }
        public List<Guid> GetUserIdByroleId(Guid roleId)
        {
            return this._URolesRepository.Table.Where(r=>r.RoleId==roleId).Select(r=>r.UserId).ToList();
        }
      


    }
}
