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
    public interface IComebackTypeService : IBaseService<ComebackType>
    {
        IEnumerable<dynamic> GetForPaging(out int count, ComebackTypeQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
         IEnumerable<dynamic> CheckChildRemain(Guid PID);

        dynamic Import(Users user, Stream fileStream);
    }
}
