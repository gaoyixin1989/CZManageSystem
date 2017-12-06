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
    public interface IOGSMMonthService :IBaseService<OGSMMonth>
    {
        IList<OGSMMonth> GetForPagingByCondition(out int count, int pageIndex = 0, int pageSize = int.MaxValue, OGSMMonthQueryBuilder objs = null);
        dynamic ImportOGSMMonth(Stream fileStream, Users user);
        IList<object> GetForExportData(OGSMMonthQueryBuilder objs = null);
    }
}
