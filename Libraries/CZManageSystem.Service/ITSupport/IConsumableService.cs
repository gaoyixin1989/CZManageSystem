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
    public interface IConsumableService: IBaseService<Consumable>
    {
        IList<Consumable> GetForPaging(out int count, ConsumableQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        dynamic ImportConsumable(Stream fileStream, Users user);
        dynamic ImportConsumableInput(Stream fileStream, Users user);
    }
}
