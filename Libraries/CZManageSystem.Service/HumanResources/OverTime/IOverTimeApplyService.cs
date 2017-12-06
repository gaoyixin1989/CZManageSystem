using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.OverTime
{
    public interface IOverTimeApplyService:IBaseService<HROverTimeApply>
    {
        IList<HROverTimeApply> GetForPagingByCondition(Users user, out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
