using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_BookOrderService : IBaseService<OrderMeal_BookOrder>
    {
        IList<OrderMeal_BookOrder> GetForPagingByCondition(out int count, OrderMeal_BookOrderQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
