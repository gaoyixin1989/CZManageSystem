
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public interface IHRCheckAttendanceHistoryNo1Service : IBaseService<HRCheckAttendanceHistoryNo1>
    {
        IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}