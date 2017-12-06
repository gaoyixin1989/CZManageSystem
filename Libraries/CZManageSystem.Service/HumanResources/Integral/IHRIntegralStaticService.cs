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
    public interface IHRIntegralStaticService : IBaseService<HRIntegralStatic>
    {
        IList<HRIntegralStatic> GetForPagingByCondition(out int count, int pageIndex, int pageSize, Users user, IntegralStaticQueryBuilder objs = null);
    }
}
