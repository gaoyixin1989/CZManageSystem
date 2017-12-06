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
    public interface IOGSMInfoService : IBaseService<OGSMInfo>
    {
        IList<object> GetForPagingByCondition
            (out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMInfoQueryBuilder objs = null);
        IList<object> FindOGSMInfoById(int id = 0);
        object ImportOGSMPInfo(Stream fileStream, Users user);
        object ImportOGSMInfo(Stream fileStream, Users user);
    }
}
