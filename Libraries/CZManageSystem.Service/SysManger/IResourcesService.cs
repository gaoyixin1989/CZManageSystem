using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using ZManageSystem.Core;


namespace CZManageSystem.Service.SysManger
{
    public interface IResourcesService : IBaseService<Resources>
    {
        string GetResourcesMaxId();
      IList<Resources> GetResourcesByPid(string Pid);
    }
}
