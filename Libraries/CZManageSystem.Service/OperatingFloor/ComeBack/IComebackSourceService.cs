using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.OperatingFloor.ComeBack
{
    public interface IComebackSourceService : IBaseService<ComebackSource>
    {
        IEnumerable<dynamic> GetReport(out int count, ComebackReporteQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
