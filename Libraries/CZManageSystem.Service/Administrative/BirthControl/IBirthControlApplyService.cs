using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Administrative.BirthControl
{
    public interface IBirthControlApplyService : IBaseService<BirthControlApply>
    {
        IList<BirthControlApply> GetProcessingList();
    }
}
