
using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Employees
{
    public interface IGdPayIdService : IBaseService<GdPayId>
    {
          IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}