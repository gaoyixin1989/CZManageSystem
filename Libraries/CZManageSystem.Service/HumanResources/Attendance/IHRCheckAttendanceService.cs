
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System;
using System.Collections.Generic;
using ZManageSystem.Core;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public interface IHRCheckAttendanceService : IBaseService<HRCheckAttendance>
    {
        IEnumerable<dynamic> GetForPaging(out int count, HRCheckAttendanceQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue, bool isDesc = true, bool isDow = false);

        IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue);
        IEnumerable<HRStatistics> GetStatistics(Guid userId);
        IEnumerable<Summarizing> GetSummarizing(params object[] parameters);
        IEnumerable<ProvisionsOfAttendance> GetProvisionsOfAttendance(params object[] parameters);
        IEnumerable<SummarizingList> GetSummarizingList(params object[] parameters);
        IEnumerable<dynamic> GetOnAndOffDuty(out int count, HRCheckAttendanceQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue, int type = 0);

    }
}