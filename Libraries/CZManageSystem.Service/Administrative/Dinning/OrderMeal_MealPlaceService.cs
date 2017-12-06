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
    public  class OrderMeal_MealPlaceService : BaseService<OrderMeal_MealPlace>, IOrderMeal_MealPlaceService
    {
        private readonly IRepository<OrderMeal_DinningRoom> _dinningroom = new EfRepository<OrderMeal_DinningRoom>();
        public IList<OrderMeal_MealPlace> GetForPagingByCondition(out int count, OrderMeal_MealPlaceQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._dinningroom.Table
                          on hrvc.DinningRoomID equals bwu.Id
                          select new
                          {
                              hrvc.Id,
                              hrvc.DinningRoomID,
                              hrvc.MealPlaceShortName,
                              hrvc.Discription,
                              hrvc.MealPlaceName,
                              bwu.DinningRoomName
                          };

            if (objs.MealPlaceName != null && objs.MealPlaceName != "")
                curData = curData.Where(u => u.MealPlaceName.Contains(objs.MealPlaceName));
            if (objs.DinningRoomName != null && objs.DinningRoomName != "")
                curData = curData.Where(u => u.DinningRoomName.Contains(objs.DinningRoomName));
            if (objs.DinningRoomID != null && objs.DinningRoomID != "")
            {
                Guid _tmp = Guid.Parse(objs.DinningRoomID);
                curData = curData.Where(u => u.DinningRoomID == _tmp);
            }

            var list = curData.OrderByDescending(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_MealPlace()
            {
                Id = x.Id,
                MealPlaceShortName = x.MealPlaceShortName,
                MealPlaceName = x.MealPlaceName,
                Discription=x.Discription,
                DinningRoomID = x.DinningRoomID
            });
            count = curData.Count();
            return list.ToList();
        }
    }
}
