using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;
namespace CZManageSystem.Service.Administrative
{
    public interface IBirthControlInfoService : IBaseService<BirthControlInfo>
    {
        IList<BirthControlInfo> GetListByUserid(Guid id);
        dynamic ImportBirthControlInfo(Stream fileStream, Users user);
    }
}
