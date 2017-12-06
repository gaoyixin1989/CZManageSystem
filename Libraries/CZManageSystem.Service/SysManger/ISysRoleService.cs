using System.Collections.Generic;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using System.Linq;
using ZManageSystem.Core;
using System;

namespace CZManageSystem.Service.SysManger
{
    public interface ISysRoleService :IBaseService<Roles>
    {
        IList<Roles> GetRolesByPid(Guid Pid);
    }
}