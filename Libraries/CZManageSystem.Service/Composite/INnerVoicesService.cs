using CZManageSystem.Data.Domain.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Composite
{
     public interface INnerVoicesService: IBaseService<InnerVoices>
    {
        IQueryable<InnerVoices> GetForPaging(out int count, InnerVoicesQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
