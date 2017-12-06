using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public interface IBirthControlStaticService : IBaseService<BirthControlStatic>
    {
        IList<object> GetForPagingByCondition(Users user, string DpId = null, string StartTime = null, string EndTime = null);
    }
}
