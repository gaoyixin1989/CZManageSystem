using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.AnnualLeave
{
    public interface IHRAnnualLeaveService : IBaseService<HRAnnualLeave>
    {
        IList<HRAnnualLeave> GetForPagingByCondition(Users user, out int count, HRAnnualLeaveQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        dynamic ImportHRAnnualLeave(string filename, Stream fileStream, Users user);
    }
}
