using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Vacation
{
    public interface IHRReVacationApplyService : IBaseService<HRReVacationApply>
    {
        IList<HRReVacationApply> GetForPaging(out int count, ReVacationApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
