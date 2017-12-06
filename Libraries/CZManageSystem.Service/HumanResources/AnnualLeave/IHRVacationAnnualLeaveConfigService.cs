using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.AnnualLeave
{
    public interface IHRVacationAnnualLeaveConfigService : IBaseService<HRVacationAnnualLeaveConfig>
    {
        IList<HRVacationAnnualLeaveConfig> GetForPagingByCondition(out int count, HRVacationAnnualLeaveConfigQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
