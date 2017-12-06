using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class OrderMeal_DinningRoomService : BaseService<OrderMeal_DinningRoom>, IOrderMeal_DinningRoomService
    {
        public IList<OrderMeal_DinningRoom> GetForPagingByCondition(out int count, OrderMeal_DinningRoomQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curData = objs == null ? this._entityStore.Table.OrderByDescending(c => c.Id) : this._entityStore.Table.OrderByDescending(c => c.Id).Where(ExpressionFactory(objs));
            count = curData.Count();
            var list = curData.OrderByDescending(c => c.Id).Skip(pageIndex * pageSize).Take(pageSize);
            return list.ToList();
        }
    }
}
