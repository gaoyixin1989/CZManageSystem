using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.Composite;
using System.Linq.Expressions;
using ZManageSystem.Core;
using System.IO;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Service.Composite
{
    public interface IOGSMService : IBaseService<OGSM>
    {
        dynamic ImportOGSMBase(Stream fileStream, Users user);
        IList<OGSM> GetForData(out int count, OGSMQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IList<object> GetBaseStationListForPagingByCondition(out int count, string BaseStation = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
