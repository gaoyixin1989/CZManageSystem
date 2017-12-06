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
    public interface IBirthControlRosterService : IBaseService<BirthControlRoster>
    {
        dynamic ImportBirthControlRosterInfo(Stream fileStream, Users user);
    }
}
