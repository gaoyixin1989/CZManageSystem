using CZManageSystem.Data.Domain.HumanResources.OverTime;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.OverTime
{
    public interface IOverTimeStaticService : IBaseService<OverTimeStatic>
    {
        IList<OverTimeStatic> GetForPagingByCondition(out int count, int pageIndex, int pageSize, Users user, OverTimeStaticQueryBuilder objs = null);
    }
}
