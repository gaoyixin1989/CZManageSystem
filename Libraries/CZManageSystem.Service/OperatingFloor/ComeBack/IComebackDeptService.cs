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
    public interface IComebackDeptService : IBaseService<ComebackDept>
    {
        IEnumerable<dynamic> GetForPaging(out int count, ComebackQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);

        dynamic Import(Users user, Stream fileStream);
    }
}
