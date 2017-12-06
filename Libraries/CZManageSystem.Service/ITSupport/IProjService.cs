using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.ITSupport
{
    public interface IProjService : IBaseService<Proj>
    {
        dynamic ImportProj(Stream fileStream, Users user);
    }
}
