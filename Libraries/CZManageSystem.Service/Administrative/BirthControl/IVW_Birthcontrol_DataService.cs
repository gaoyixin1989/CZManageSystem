using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
namespace CZManageSystem.Service.Administrative.BirthControl
{
    public interface IVW_Birthcontrol_DataService : IBaseService<VW_Birthcontrol_Data>
    {
        IList<VW_Birthcontrol_Data> GetForPagingByCondition(Users user,out int count, BirthControlInfoBuilder obj =null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
