using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.SysManger
{
    public interface IWorkflowsService : IBaseService<Workflows>
    {
        IEnumerable<dynamic> GetList(out int count, out List<Guid> list, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}