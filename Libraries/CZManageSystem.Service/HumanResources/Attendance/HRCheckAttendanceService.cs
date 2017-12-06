using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using CZManageSystem.Service.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public class HRCheckAttendanceService : BaseService<HRCheckAttendance>, IHRCheckAttendanceService
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetForPaging(out int count, HRCheckAttendanceQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue, bool isDesc = true, bool isDow = false)
        {
            try
            {
                //var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.AtDate) : this._entityStore.Table.OrderByDescending(c => c.AtDate).Where(ExpressionFactory(objs));
                //PagedList<HRCheckAttendance> pageList = new PagedList<HRCheckAttendance>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                //count = pageList.TotalCount;

                var curTable = GetQueryTable(objs);
                count = curTable.Count();
                var list = new List<HRCheckAttendance>();
                if (isDow)
                {
                    pageSize = 5000;
                    //list = curTable.OrderBy(p => "").ToList();
                    if (count <= pageSize)
                    {
                        list = (isDesc ? curTable.OrderByDescending(p => p.AtDate) : curTable.OrderBy(p => p.AtDate)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        double d = count / (double)pageSize;
                        for (int i = 0; i < (int)Math.Ceiling(d); i++)
                        {
                            var result = (isDesc ? curTable.OrderByDescending(p => p.AtDate) : curTable.OrderBy(p => p.AtDate)).Skip(i * pageSize).Take(pageSize).ToList();
                            list.AddRange(result);
                        }
                    }

                }
                else
                    list = (isDesc ? curTable.OrderByDescending(p => p.AtDate) : curTable.OrderBy(p => p.AtDate)).Skip((pageIndex <= 0 ? 0 : (pageIndex - 1)) * pageSize).Take(pageSize).ToList();

                return list.Select(s => new
                {
                    AtDate = ChcekingIn.GetDate(s.AtDate),
                    s.AttendanceId,
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
                    s.Users?.RealName,
                    s.Users?.EmployeeId,
                    s.Users?.Dept?.DpName,

                    State = ChcekingIn.CheckDuty(s.AtDate, s.DoTime, s.OffTime, s.DoReallyTime, s.OffReallyTime, s.Minute, s.DoFlag, s.RotateDaysOffFlag)
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<HRCheckAttendance> GetQueryTable(HRCheckAttendanceQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                HRCheckAttendanceQueryBuilder obj2 = (HRCheckAttendanceQueryBuilder)CloneObject(obj);

                if (obj.UserId != null && obj.UserId.Count > 0)
                    curTable = curTable.Where(u => obj.UserId.Contains(u.UserId.Value));
                obj2.UserId = null;
                if (obj.DpId != null && obj.DpId.Count > 0)
                    curTable = curTable.Where(u => obj.DpId.Contains(u.Users.DpId));
                obj2.DpId = null;
                if (obj.EmployeeId != null && !string.IsNullOrEmpty(obj.EmployeeId))
                    curTable = curTable.Where(u => u.Users.EmployeeId.Contains(obj.EmployeeId));
                obj2.EmployeeId = null;


                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }



        public IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.AtDate) : this._entityStore.Table.OrderByDescending(c => c.AtDate).Where(ExpressionFactory(objs));
                //source.Where(w => w.DoReallyTime != null || w.OffReallyTime != null);
                PagedList<HRCheckAttendance> pageList = new PagedList<HRCheckAttendance>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    AtDate = ChcekingIn.GetDate(s.AtDate),
                    s.AttendanceId,
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
                    s.Users?.RealName,
                    s.Users?.EmployeeId,
                    s.Users?.Dept?.DpName,
                    State = ChcekingIn.CheckDuty(s.AtDate, s.DoTime, s.OffTime, s.DoReallyTime, s.OffReallyTime, s.Minute, s.DoFlag, s.RotateDaysOffFlag)
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HRStatistics> GetStatistics(Guid userId)
        {
            try
            {
                return this._entityStore.ExecuteResT<HRStatistics>("EXEC dbo.usp_Statistics @userId='" + userId + "'");

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 汇总表头
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Summarizing> GetSummarizing(params object[] parameters)
        {
            try
            {
                return this._entityStore.ExecuteResT<Summarizing>("EXEC [dbo].[usp_UnionVavation] @UserIdToQuery, @YEAR , @MONTH , @DpId, @UserType, @EmployeeId, @UserId ", parameters);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 表头中的规定考勤天数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<ProvisionsOfAttendance> GetProvisionsOfAttendance(params object[] parameters)
        {
            try
            { 
                return this._entityStore.ExecuteResT<ProvisionsOfAttendance>("EXEC dbo.usp_GetProvisionsOfAttendance @UserIdToQuery, @YEAR , @MONTH , @DpId, @UserType, @EmployeeId, @UserId ", parameters);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 汇总列表 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SummarizingList> GetSummarizingList(params object[] parameters)
        {
            try
            {


                return this._entityStore.ExecuteResT<SummarizingList>(" EXEC[dbo].[ups_GetSummarizing] @UserIdToQuery,  @YEAR , @MONTH , @DpId, @UserType, @EmployeeId, @UserId ,@PageIndex,@PageSize ,@Count OUT", parameters);/*+ ",@PageIndex = 1,@PageSize = 5,@Count = 0"*/
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public IEnumerable<dynamic> GetOnAndOffDuty(out int count, HRCheckAttendanceQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue, int type = 0)
        {
            try
            {

                var atDate = Convert.ToDateTime(DateTime.Now.ToLongDateString());
                var source = this._entityStore.Table.Where(w => w.AtDate == atDate && w.DoFlag == null && w.DoTime != null);
                switch (type)
                {
                    case 0:
                        source = source.Where(w => w.DoReallyTime != null && w.OffReallyTime == null);
                        break;
                    case 1:
                        source = source.Where(w => w.OffReallyTime != null);
                        break;
                    default:
                        break;
                }
                if (objs != null && objs.DpId.Count > 0)
                    source = source.Where(w => objs.DpId.Contains(w.Users.DpId));

                if (objs != null && objs.UserId != null && objs.UserId.Count > 0)
                    source = source.Where(w => objs.UserId.Contains(w.Users.UserId));

                if (objs != null && !string.IsNullOrEmpty(objs.EmployeeId))
                    source = source.Where(w => objs.EmployeeId.Contains(w.Users.EmployeeId));

                var child = from a in source
                            group new { a.UserId, a.DoTime, a.AttendanceId } by a.UserId into g
                            select new
                            {
                                g.Key,
                                l = from c in source where c.DoTime == g.Max(m => m.DoTime) && c.UserId == g.Key select new { AttendanceId = c.AttendanceId }
                            };
                var list = (from o in child select o.l.FirstOrDefault().AttendanceId).ToList();
                if (list.Count > 0)
                    source = from c in source where list.Contains(c.AttendanceId) select c;
                source = source.OrderByDescending(c => c.DoReallyTime);
                PagedList<HRCheckAttendance> pageList = new PagedList<HRCheckAttendance>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    AtDate = ChcekingIn.GetDate(s.AtDate),
                    OffDate = ChcekingIn.GetDate(s.OffDate),
                    s.AttendanceId,
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
                    s.Users?.RealName,
                    s.Users?.DpId,
                    s.Users?.EmployeeId,
                    s.Users?.Dept?.DpName,
                    State = ChcekingIn.CheckDuty(s.AtDate, s.DoTime, s.OffTime, s.DoReallyTime, s.OffReallyTime, s.Minute, s.DoFlag, s.RotateDaysOffFlag)
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
