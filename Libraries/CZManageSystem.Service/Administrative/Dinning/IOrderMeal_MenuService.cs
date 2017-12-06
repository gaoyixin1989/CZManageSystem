using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_MenuService : IBaseService<OrderMeal_Menu>
    {
        IList<OrderMeal_Menu> GetForPagingByCondition(out int count, OrderMeal_MenuQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
