using CZManageSystem.Data.Domain.HumanResources.Integral;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Integral
{
    public interface IHRNeedIntegralService : IBaseService<HRNeedIntegral>
    {
        IList<HRNeedIntegral> GetForPagingByCondition(Users user, out int count, HRNeedIntegralQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
