
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public interface IV_HRCheckAttendanceAbnormalService : IBaseService<V_HRCheckAttendanceAbnormal>
    {
        IEnumerable<dynamic> GetForPaging(out int count, AttendanceListQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue, bool isDow = false);
        IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue, bool isHave = false);
    }
}