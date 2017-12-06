using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.AnnualLeave
{
    public interface IHRAnnualleaveStaticService : IBaseService<HRAnnualLeaveStatic>
    {
        IList<HRAnnualLeaveStatic> GetForPagingByCondition(out int count, Users user, int pageIndex = 0, int pageSize = int.MaxValue, HRAnnualLeaveStaticQueryBuilder objs = null);
    }
}
