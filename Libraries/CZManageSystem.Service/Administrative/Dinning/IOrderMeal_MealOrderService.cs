using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;
using CZManageSystem.Data.Domain.Administrative.Dinning;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_MealOrderService : IBaseService<OrderMeal_MealOrder>
    {
        IList<OrderMeal_MealOrder> GetForPagingByCondition(out int count, OrderMeal_BookOrderQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IList<OrderMeal_MealOrderStatic> GetForStaticPagingByCondition(out int count, OrderMeal_BookOrderQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
