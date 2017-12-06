using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class OrderMeal_BookOrderService : BaseService<OrderMeal_BookOrder>, IOrderMeal_BookOrderService
    {

        private readonly IRepository<Users> _bw_Users = new EfRepository<Users>();
        public IList<OrderMeal_BookOrder> GetForPagingByCondition(out int count, OrderMeal_BookOrderQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._bw_Users.Table
                          on hrvc.LoginName equals bwu.UserName
                          select new
                          {
                              hrvc.Id,
                              hrvc.DinningRoomName,
                              hrvc.DinningRoomID,
                              hrvc.MealTimeType,
                              hrvc.MealTimeID,
                              hrvc.Discription,
                              hrvc.PackageName,
                              hrvc.PackagePrice,
                              hrvc.LoginName,
                              bwu.RealName,
                              bwu.DpId,
                              hrvc.UserName,
                              hrvc.StartedDate,
                              hrvc.EndDate,
                              hrvc.BookType,
                              hrvc.OrderStateName,
                              hrvc.OrderTime,
                              hrvc.OrderState,
                              hrvc.AfterOrderBalance,
                              hrvc.MealPlaceName
                          };

            if (objs.MealTimeType != null && objs.MealTimeType != "")
                curData = curData.Where(u => u.MealTimeType.Contains(objs.MealTimeType));
            if (objs.MealPlaceName != null && objs.MealPlaceName != "")
                curData = curData.Where(u => u.MealPlaceName.Contains(objs.MealPlaceName));
            if (objs.OrderStateName != null && objs.OrderStateName != "")
                curData = curData.Where(u => u.OrderStateName.Contains(objs.OrderStateName));
            if (objs.DinningRoomName != null&& objs.DinningRoomName !="")
                curData = curData.Where(u => u.DinningRoomName.Contains( objs.DinningRoomName));
            if (objs.OrderTime_Start != null && objs.OrderTime_End != null)
                curData = curData.Where(u => u.OrderTime >= objs.OrderTime_Start && u.OrderTime <= objs.OrderTime_End);



            if (objs.DpId != null)
                curData = curData.Where(u => objs.DpId.Contains(u.DpId));
            if (objs.RealName != null && objs.RealName != "")
                curData = curData.Where(u => u.RealName.Contains(objs.RealName));

            var list = curData.OrderByDescending(p => p.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_BookOrder()
            {
                Id = x.Id,
                MealTimeType = x.MealTimeType,
                DinningRoomName = x.DinningRoomName,
                PackageName = x.PackageName,
                PackagePrice = x.PackagePrice,
                Discription = x.Discription,
                AfterOrderBalance = x.AfterOrderBalance,
                MealPlaceName = x.MealPlaceName,
                OrderTime = x.OrderTime,
                StartedDate = x.StartedDate,
                OrderStateName = x.OrderStateName,
                LoginName = x.LoginName,
                UserName = x.UserName,
                EndDate = x.EndDate,
                DinningRoomID = x.DinningRoomID,
                BookType = x.BookType
            });
            count = curData.Count();
            return list.ToList();
        }
    }
}
