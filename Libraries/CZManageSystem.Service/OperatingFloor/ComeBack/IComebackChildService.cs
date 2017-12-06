using CZManageSystem.Data.Domain.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZManageSystem.Core;

namespace CZManageSystem.Service.OperatingFloor.ComeBack
{
    public interface IComebackChildService : IBaseService<ComebackChild>
    {
        IEnumerable<dynamic> GetForPaging(out int count, ComebackChildQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IEnumerable<dynamic> GetReport(out int count, ComebackReporteQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        dynamic Import(Users user, Stream fileStream);
    }
}
