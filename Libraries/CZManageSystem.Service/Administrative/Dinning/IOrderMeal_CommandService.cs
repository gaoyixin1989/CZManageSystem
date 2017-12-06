using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_CommandService : IBaseService<OrderMeal_Command>
    {
        IList<object> GetForPagingByCondition(out int count, OrderMeal_CommandQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
