using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class OrderMeal_MenuService : BaseService<OrderMeal_Menu>, IOrderMeal_MenuService
    {
        private readonly IRepository<OrderMeal_DinningRoom> _dinningroom = new EfRepository<OrderMeal_DinningRoom>();
        private readonly IRepository<OrderMeal_DinningRoomMealTimeSettings> _dinningtimeroom = new EfRepository<OrderMeal_DinningRoomMealTimeSettings>();
        public IList<OrderMeal_Menu> GetForPagingByCondition(out int count, OrderMeal_MenuQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._dinningroom.Table
                          on hrvc.DinningRoomID equals bwu.Id
                          select new
                          {
                              hrvc.Id,
                              hrvc.CreateTime,
                              hrvc.DinningRoomID,
                              hrvc.MenuName,
                              hrvc.MealTimeType,
                              bwu.DinningRoomName,
                              hrvc.WorkingDate
                          };

            if (objs.MealTimeType != null && objs.MealTimeType != "")
                curData = curData.Where(u => u.MealTimeType.Contains(objs.MealTimeType));
            if (objs.MenuName != null && objs.MenuName != "")
                curData = curData.Where(u => u.MenuName.Contains(objs.MenuName));
            if (objs.DinningRoomName != null && objs.DinningRoomName != "")
                curData = curData.Where(u => u.DinningRoomName.Contains(objs.DinningRoomName));
            if (objs.DinningRoomID != null && objs.DinningRoomID != "")
            {
                Guid _tmp = Guid.Parse(objs.DinningRoomID);
                curData = curData.Where(u => u.DinningRoomID == _tmp);
            }
            var list = curData.OrderByDescending(p => p.CreateTime).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_Menu()
            {
                Id = x.Id,
                MenuName = x.MenuName,
                MealTimeType = x.MealTimeType,
                DinningRoomID = x.DinningRoomID,
                WorkingDate = x.WorkingDate,
                CreateTime = x.CreateTime
            });
            count = curData.Count();
            return list.ToList();
        }
    }
}
