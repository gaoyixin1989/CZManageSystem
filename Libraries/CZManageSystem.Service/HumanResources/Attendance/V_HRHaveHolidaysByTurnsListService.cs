using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using CZManageSystem.Service.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public class V_HRHaveHolidaysByTurnsListService : BaseService<V_HRHaveHolidaysByTurnsList>, IV_HRHaveHolidaysByTurnsListService
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.AtDate) : this._entityStore.Table.OrderByDescending(c => c.AtDate).Where(ExpressionFactory(objs));
                PagedList<V_HRHaveHolidaysByTurnsList> pageList = new PagedList<V_HRHaveHolidaysByTurnsList>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    AtDate = ChcekingIn.GetDate(s.AtDate),
                    s.AttendanceId,
                    s.EmployeeId,
                    s.DoFlag,
                    s.DoReallyTime,
                    s.DoTime,
                    s.FlagOff,
                    s.FlagOn,
                    s.IpOff,
                    s.IpOn,
                    s.Minute,
                    s.OffReallyTime,
                    s.OffTime,
                    s.RotateDaysOffFlag,
                    s.UserId,
                    s.RealName, 
                    s.DpName,
                    s.DpFullName
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #region 方法
        string GetDate(string date)
        {
            if (date.Length != 6)
                return "";
            DateTime dt = DateTime.ParseExact(date, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
            return dt.ToString("yyyy年M月");
        }
        #endregion
    }
}
