using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_PackageService : IBaseService<OrderMeal_Package>
    {
        IList<OrderMeal_Package> GetForPagingByCondition(out int count, OrderMeal_PackageQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
