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
    public interface IConsumable_SporadicDetailService : IBaseService<Consumable_SporadicDetail>
    {
        dynamic ImportSporadicDetail(Stream fileStream, Users user,Guid id);
    }

}
