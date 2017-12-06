using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.Dinning
{
    public interface IView_XF_AmountLogService : IBaseService<view_XF_AmountLog>
    {
        IList<view_XF_AmountLog> GetForPagingByCondition(out int count, view_XF_AmountLogQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
