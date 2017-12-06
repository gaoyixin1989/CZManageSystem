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
    public class V_HRCheckAttendanceAbnormalService : BaseService<V_HRCheckAttendanceAbnormal>, IV_HRCheckAttendanceAbnormalService
    {
        IHRUnattendLinkService hrUnattendLinkService = new HRUnattendLinkService();
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetForPaging(out int count, AttendanceListQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue, bool isDow = false)
        {
            try
            {
                var curTable = GetQueryTable(objs);
                count = curTable.Count();
                var list = new List<V_HRCheckAttendanceAbnormal>();
                if (isDow)
                {
                    pageSize = 5000;
                    //list = curTable.OrderBy(p => "").ToList();
                    if (count <= pageSize)
                    {
                        list = curTable.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        double d = count / (double)pageSize;
                        for (int i = 0; i < (int)Math.Ceiling(d); i++)
                        {
                            var result = curTable.OrderBy(p => "").Skip(i * pageSize).Take(pageSize).ToList();
                            list.AddRange(result);
                        }
                    }

                }
                else
                    list = curTable.OrderBy(p => "").Skip(pageIndex * pageSize).Take(pageSize).ToList();

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
                    s.RealName,
                    s.EmployeeId,
                    s.DpName,
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
        private IQueryable<V_HRCheckAttendanceAbnormal> GetQueryTable(AttendanceListQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                AttendanceListQueryBuilder obj2 = (AttendanceListQueryBuilder)CloneObject(obj);

                if (obj.UserId != null && obj.UserId.Count > 0)
                    curTable = curTable.Where(u => obj.UserId.Contains(u.UserId.Value));
                obj2.UserId = null;
                if (obj.DpId != null && obj.DpId.Count > 0)
                    curTable = curTable.Where(u => obj.DpId.Contains(u.DpId));
                obj2.DpId = null;
                //if (obj.DpName != null && obj.DpName.Count > 0)
                //{
                //    curTable = curTable.Where(u => obj.DpName.Contains(u.DpName));
                //    obj2.DpName = null;
                //}
                //if (obj.RealName != null && obj.RealName.Count > 0)
                //{
                //    curTable = curTable.Where(u => obj.RealName.Contains(u.RealName));
                //    obj2.RealName = null;
                //}



                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }

            return curTable;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue, bool isHave = false)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.AtDate) : this._entityStore.Table.OrderByDescending(c => c.AtDate).Where(ExpressionFactory(objs));
                if (isHave)
                    source = source.Where(w => w.DoFlag != null);
                else
                    source = source.Where(w => w.DoFlag == null);
                PagedList<V_HRCheckAttendanceAbnormal> pageList = new PagedList<V_HRCheckAttendanceAbnormal>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.ToList().Select(s => new
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
                    s.RealName,
                    s.EmployeeId,
                    s.DpName,
                    State = ChcekingIn.CheckDuty(s.AtDate, s.DoTime, s.OffTime, s.DoReallyTime, s.OffReallyTime, s.Minute, s.DoFlag, s.RotateDaysOffFlag),
                    WorkflowInstanceId = GetWorkflowInstanceId(s.AttendanceId),
                    ApplyID = hrUnattendLinkService.FindByFeldName(f => f.AttendanceId == s.AttendanceId)?.ApplyRecordId.ToString ()
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #region 方法
        string GetWorkflowInstanceId(Guid? attendanceId)
        {
            if (attendanceId == null)
                return "";
            var unattend = new HRUnattendApplyService().FindByFeldName(f => f.AttendanceIds.Contains(attendanceId.ToString()));
            return unattend?.WorkflowInstanceId?.ToString();
        }
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
