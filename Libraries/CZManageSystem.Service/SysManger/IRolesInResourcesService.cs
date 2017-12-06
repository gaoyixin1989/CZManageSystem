using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
   public interface IRolesInResourcesService :IBaseService<RolesInResources>
    {
        bool Any(string resourcesId, Guid roleId);
        List<Guid> GetUserIdByroleId(Guid roleId);
    }
}
