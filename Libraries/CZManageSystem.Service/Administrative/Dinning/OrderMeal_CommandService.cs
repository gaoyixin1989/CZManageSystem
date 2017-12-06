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
    public  class OrderMeal_CommandService : BaseService<OrderMeal_Command>, IOrderMeal_CommandService
    {
        private readonly IRepository<OrderMeal_MealPlace> _mealplace = new EfRepository<OrderMeal_MealPlace>();
        private readonly IRepository<OrderMeal_Package> _package = new EfRepository<OrderMeal_Package>();
        public IList<object> GetForPagingByCondition(out int count, OrderMeal_CommandQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._mealplace.Table
                          on hrvc.PlaceId equals bwu.Id
                          join bpak in this._package.Table
                          on hrvc.PackageId equals bpak.Id
                          select new
                          {
                              hrvc.Id,
                              hrvc.Command,
                              bwu.MealPlaceName,
                              bpak.MealTimeType,
                              bpak.PackageName,
                              bpak.PackagePrice,
                              bpak.DinningRoomID,
                              bpak.Discription
                          };

            if (objs.MealTimeType != null && objs.MealTimeType != "")
                curData = curData.Where(u => u.MealTimeType.Contains(objs.MealTimeType));
            if (objs.PackageName != null && objs.PackageName != "")
                curData = curData.Where(u => u.PackageName.Contains(objs.PackageName));
            if (objs.DinningRoomId != null && objs.DinningRoomId !=Guid.Empty)
                curData = curData.Where(u => u.DinningRoomID == objs.DinningRoomId);
            List<object> resultList = new List<object>();

            //var list = curData.OrderByDescending(p => p.Id).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new 
            //{
            //    x.Id,
            //    x.MealTimeType,
            //    x.PackageName,
            //    x.PackagePrice,
            //    x.Command
            //});
            foreach (var x in curData)
            {
                resultList.Add(new {
                    x.Id,
                    x.MealPlaceName,
                    x.MealTimeType,
                    x.PackageName,
                    x.PackagePrice,
                    x.Command,
                    x.Discription
                });
            }
            count = curData.Count();
            return resultList;
        }
    }
}
