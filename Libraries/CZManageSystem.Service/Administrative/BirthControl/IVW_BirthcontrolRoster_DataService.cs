using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public interface IVW_BirthcontrolRoster_DataService : IBaseService<VW_BirthcontrolRoster_Data>
    {
        IList<VW_BirthcontrolRoster_Data> GetForPagingByCondition(Users user, out int count, BirthControlRosterBuilder obj = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
