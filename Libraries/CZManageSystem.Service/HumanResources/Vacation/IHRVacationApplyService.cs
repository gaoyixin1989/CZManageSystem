using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Vacation
{
    public interface IHRVacationApplyService : IBaseService<HRVacationApply>
    {
        IList<HRVacationApply> GetForPaging(out int count, VacationApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
