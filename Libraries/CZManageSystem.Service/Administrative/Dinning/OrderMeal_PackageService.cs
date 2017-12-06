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
    public class OrderMeal_PackageService : BaseService<OrderMeal_Package>, IOrderMeal_PackageService
    {
        private readonly IRepository<OrderMeal_DinningRoom> _dinningroom = new EfRepository<OrderMeal_DinningRoom>();
        public IList<OrderMeal_Package> GetForPagingByCondition(out int count, OrderMeal_PackageQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._dinningroom.Table
                          on hrvc.DinningRoomID equals bwu.Id
                          select new
                          {
                              hrvc.Id,
                              hrvc.DinningRoomID,
                              hrvc.MealTimeType,
                              hrvc.MealTimeID,
                              hrvc.Discription,
                              hrvc.PackageName,
                              hrvc.PackagePrice,
                              bwu.DinningRoomName
                          };

            if (objs.MealTimeType != null && objs.MealTimeType != "")
                curData = curData.Where(u => u.MealTimeType.Contains(objs.MealTimeType));
            if (objs.PackageName != null && objs.PackageName != "")
                curData = curData.Where(u => u.PackageName.Contains(objs.PackageName));
            if (objs.DinningRoomName != null && objs.DinningRoomName != "")
                curData = curData.Where(u => u.DinningRoomName.Contains(objs.DinningRoomName));
            if (objs.DinningRoomID != null && objs.DinningRoomID != "")
            {
                Guid _tmp = Guid.Parse(objs.DinningRoomID);
                curData = curData.Where(u => u.DinningRoomID == _tmp);
            }
            var list = curData.OrderByDescending(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_Package()
            {
                Id = x.Id,
                MealTimeType=x.MealTimeType,
                PackageName = x.PackageName,
                PackagePrice = x.PackagePrice,
                Discription = x.Discription,
                DinningRoomID = x.DinningRoomID
            });
            count = curData.Count();
            return list.ToList();
        }
    }
}
