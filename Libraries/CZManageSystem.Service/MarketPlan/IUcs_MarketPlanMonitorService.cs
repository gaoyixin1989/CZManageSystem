using CZManageSystem.Data.Domain.MarketPlan;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.MarketPlan
{
    public interface IUcs_MarketPlanMonitorService : IBaseService<Ucs_MarketPlanMonitor>
    {
        IEnumerable<dynamic> GetForPaging(out int count, MarketPlanMonitorQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
