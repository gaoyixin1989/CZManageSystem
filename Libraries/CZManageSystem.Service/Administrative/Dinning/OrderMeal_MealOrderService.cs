using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public class OrderMeal_MealOrderService : BaseService<OrderMeal_MealOrder>, IOrderMeal_MealOrderService
    {
        private readonly IRepository<Users> _bw_Users = new EfRepository<Users>();
        public IList<OrderMeal_MealOrder> GetForPagingByCondition(out int count, OrderMeal_BookOrderQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
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
                              hrvc.DinningDate,
                              hrvc.UserName,
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
                curData = curData.Where(u => u.OrderStateName == objs.OrderStateName);
            if (objs.DinningRoomName != null && objs.DinningRoomName != "")
                curData = curData.Where(u => u.DinningRoomName.Contains(objs.DinningRoomName));
            if (objs.OrderTime_Start != null && objs.OrderTime_End != null)
                curData = curData.Where(u => u.OrderTime >= objs.OrderTime_Start && u.OrderTime <= objs.OrderTime_End);


            if (objs.DinningRoomID != Guid.Empty)
                curData = curData.Where(u => u.DinningRoomID == objs.DinningRoomID);
            if (objs.LoginName != null && objs.LoginName != "")
                curData = curData.Where(u => u.LoginName == objs.LoginName);

            if (objs.DpId != null)
                curData = curData.Where(u => objs.DpId.Contains(u.DpId));
            if (objs.RealName != null && objs.RealName != "")
                curData = curData.Where(u => u.RealName.Contains(objs.RealName));

            var list = curData.OrderByDescending(p => p.OrderTime).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_MealOrder()
            {
                Id = x.Id,
                MealTimeType = x.MealTimeType,
                DinningRoomName = x.DinningRoomName,
                PackageName = x.PackageName,
                PackagePrice = x.PackagePrice,
                DinningDate=x.DinningDate,
                Discription = x.Discription,
                AfterOrderBalance = x.AfterOrderBalance,
                MealPlaceName = x.MealPlaceName,
                OrderTime = x.OrderTime,
                UserName=x.UserName,
                OrderStateName = x.OrderStateName,
                LoginName = x.LoginName,
                DinningRoomID = x.DinningRoomID
            });
            count = curData.Count();
            return list.ToList();
        }


        public IList<OrderMeal_MealOrderStatic> GetForStaticPagingByCondition(out int count, OrderMeal_BookOrderQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var curData = from hrvc in this._entityStore.Table
                          where hrvc.OrderState == 1
                          group hrvc by new {
                              hrvc.DinningRoomName ,
                              hrvc.MealTimeType ,
                              hrvc.PackageName,
                              hrvc.MealPlaceName,
                              hrvc.DinningDate
                          }
                          into s
                          select new
                          {
                              s.Key.DinningRoomName,
                              s.Key.MealTimeType,
                              s.Key.PackageName,
                              s.Key.DinningDate,
                              s.Key.MealPlaceName,
                              DinningRoomOrderSum = s.Count()
                          };

            if (objs.MealTimeType != null && objs.MealTimeType != "")
                curData = curData.Where(u => u.MealTimeType.Contains(objs.MealTimeType));
            if (objs.DinningRoomName != null && objs.DinningRoomName != "")
                curData = curData.Where(u => u.DinningRoomName.Contains(objs.DinningRoomName));
            if (objs.MealPlaceName != null && objs.MealPlaceName != "")
                curData = curData.Where(u => u.MealPlaceName.Contains(objs.MealPlaceName));
            if (objs.OrderStateName != null && objs.OrderStateName != "")
                curData = curData.Where(u => u.DinningRoomName.Contains(objs.DinningRoomName));
            if (objs.OrderTime_Start != null && objs.OrderTime_End != null)
                curData = curData.Where(u => u.DinningDate >= objs.OrderTime_Start && u.DinningDate <= objs.OrderTime_End);

            var list = curData.OrderByDescending(p => p.DinningDate).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new OrderMeal_MealOrderStatic()
            {
                MealTimeType = x.MealTimeType,
                DinningRoomName = x.DinningRoomName,
                PackageName = x.PackageName,
                MealPlaceName = x.MealPlaceName,
                DinningDate = x.DinningDate,
                DinningRoomOrderSum = x.DinningRoomOrderSum
            });
            count = curData.Count();
            return list.ToList();
        }



    }
}
