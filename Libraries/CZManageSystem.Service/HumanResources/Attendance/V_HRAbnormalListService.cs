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
    public class V_HRAbnormalListService : BaseService<V_HRAbnormalList>, IV_HRAbnormalListService
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
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.DoReallyTime  ) : this._entityStore.Table.OrderByDescending(c => c.DoReallyTime).Where(ExpressionFactory(objs));
                PagedList<V_HRAbnormalList> pageList = new PagedList<V_HRAbnormalList>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    AtDate = ChcekingIn.GetDate(s.AtDate),
                    OffDate = ChcekingIn.GetDate(s.OffDate),
                    s.AttendanceId,
                    s.DoFlag,
                    s.DoReallyTime,
                    s.DoTime,
                    s.Minute,
                    s.OffReallyTime,
                    s.OffTime,
                    s.RotateDaysOffFlag,
                    s.UserId,
                    s.RealName,
                    s.DpId,
                    s.EmployeeId,
                    s.DpName,
                    s.DpFullName 
                })
                    ;
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
