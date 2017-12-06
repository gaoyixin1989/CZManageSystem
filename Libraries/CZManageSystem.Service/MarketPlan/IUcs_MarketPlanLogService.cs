using CZManageSystem.Data.Domain.MarketPlan;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.MarketPlan
{
    public interface IUcs_MarketPlanLogService : IBaseService<Ucs_MarketPlanLog>
    {
        IEnumerable<dynamic> GetForPaging(out int count, MarketPlanLogQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
