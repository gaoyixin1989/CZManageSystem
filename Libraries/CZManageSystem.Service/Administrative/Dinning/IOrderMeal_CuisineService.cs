using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_CuisineService : IBaseService<OrderMeal_Cuisine>
    {
        IList<OrderMeal_Cuisine> GetForPagingByCondition(out int count, OrderMeal_CuisineQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IList<OrderMeal_CuisineTreeData> GetDictNameGroup(Guid DinningRoomId);
    }
}
