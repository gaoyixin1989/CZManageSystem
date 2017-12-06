using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IOrderMeal_UserBaseinfoService : IBaseService<OrderMeal_UserBaseinfo>
    {
        IList<OrderMeal_UserBaseinfoTmp> GetForPagingByCondition(out int count, OrderMeal_UserBaseinfoQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IList<OrderMeal_UserDinningRoom> GetForBelongPagingByCondition(out int count, OrderMeal_UserDinningRoomQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        dynamic ImportOrderMealUserBaseinfo(string filename, Stream fileStream);
    }
}
