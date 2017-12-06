using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_DinningRoomService : IBaseService<OrderMeal_DinningRoom>
    {
        IList<OrderMeal_DinningRoom> GetForPagingByCondition(out int count, OrderMeal_DinningRoomQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
