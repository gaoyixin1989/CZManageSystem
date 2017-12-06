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
    public interface IBirthControlChildrenInfoService : IBaseService<BirthControlChildrenInfo>
    {
        IList<BirthControlChildrenInfo> GetAllChildrenList(Guid id);
        dynamic ImportBirthControlChildrenInfo(Stream fileStream, Users user);
    }
}
