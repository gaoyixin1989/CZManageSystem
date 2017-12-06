using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Administrative
{
    public class BoardroomApplyService : BaseService<BoardroomApply>, IBoardroomApplyService
    {
        public IQueryable<BoardroomApply> GetForPaging(out int count, BoardroomApplyQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var curTable = GetQueryTable(objs);
            count = curTable.Count();
            var list = curTable.OrderByDescending(p => p.AppTime).Skip(pageIndex * pageSize).Take(pageSize);
            return list;
        }

        /// <summary>
        /// 编辑查询条件
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        private IQueryable<BoardroomApply> GetQueryTable(BoardroomApplyQueryBuilder obj = null)
        {
            var curTable = this._entityStore.Table;
            if (obj != null)
            {
                BoardroomApplyQueryBuilder obj2 = (BoardroomApplyQueryBuilder)CloneObject(obj);

                if (obj.Room != null && obj.Room.Length > 0)
                {//会议室
                    curTable = curTable.Where(u => obj.Room.Contains(u.Room ?? 0));
                    obj2.Room = null;
                }
                if (obj.State != null && obj.State.Length > 0)
                {//状态
                    curTable = curTable.Where(u => obj.State.Contains(u.State ?? 0));
                    obj2.State = null;
                }
                if (obj.State_without != null && obj.State_without.Length > 0)
                {//不包含的状态
                    curTable = curTable.Where(u => !obj.State_without.Contains(u.State ?? 0));
                    obj2.State_without = null;
                }
                if (obj.AppPerson != null && obj.AppPerson.Length > 0)
                {//申请人
                    curTable = curTable.Where(u => obj.AppPerson.Contains(u.AppPerson));
                    obj2.AppPerson = null;
                }
                if (obj.JudgeState != null && obj.JudgeState.Length > 0)
                {//评价状态
                    curTable = curTable.Where(u => obj.JudgeState.Contains(u.JudgeState ?? 0));
                    obj2.JudgeState = null;
                }

                #region 比较申请日期
                if (obj.AppTime_start.HasValue)
                {//申请时间开始
                    DateTime tempDate1 = obj.AppTime_start.Value.Date;
                    curTable = curTable.Where(u => u.AppTime >= tempDate1);
                    obj2.AppTime_start = null;
                }
                if (obj.AppTime_end.HasValue)
                {//申请时间结束
                    DateTime tempDate1 = obj.AppTime_end.Value.Date.AddDays(1);
                    curTable = curTable.Where(u => u.AppTime < tempDate1);
                    obj2.AppTime_end = null;
                }
                #endregion
                #region 比较会议日期
                if (obj.MeetingDate_start.HasValue && !obj.MeetingDate_end.HasValue)
                {//只有开始日期,查询会议结束日期大于等于这个日期的
                    DateTime tempDate1 = obj.MeetingDate_start.Value.Date;
                    curTable = curTable.Where(u => u.EndDate_Real >= tempDate1);
                    obj2.MeetingDate_start = null;
                    obj2.MeetingDate_end = null;
                }
                else if (!obj.MeetingDate_start.HasValue && obj.MeetingDate_end.HasValue)
                {//只有结束日期,查询会议开始日期小于等于这个日期的
                    DateTime tempDate1 = obj.MeetingDate_end.Value.Date.AddDays(1);
                    curTable = curTable.Where(u => u.MeetingDate < tempDate1);
                    obj2.MeetingDate_start = null;
                    obj2.MeetingDate_end = null;
                }
                else if (obj.MeetingDate_start.HasValue && obj.MeetingDate_end.HasValue)
                {//开始结束日期有，查询“会议结束日期处于该时间段”或“会议开始日期处于该时间段”
                    DateTime tempDate1 = obj.MeetingDate_start.Value.Date;
                    DateTime tempDate2 = obj.MeetingDate_end.Value.Date.AddDays(1);
                    //curTable = curTable.Where(u => (u.EndDate_Real >= tempDate1) && (u.MeetingDate < tempDate2));
                    curTable = curTable.Where(u => (u.MeetingDate >= tempDate1 && u.MeetingDate < tempDate2) || (u.EndDate_Real >= tempDate1 && u.EndDate_Real < tempDate2));
                    obj2.MeetingDate_start = null;
                    obj2.MeetingDate_end = null;
                }
                #endregion
                #region 比较会议时间
                if (obj.MeetingTime_start.HasValue && !obj.MeetingTime_end.HasValue)
                {//只有开始时间,查询会议结束时间大于等于这个时间的
                    DateTime tempDate1 = obj.MeetingTime_start.Value;
                    curTable = curTable.Where(u => u.EndDate_Real >= tempDate1);
                    obj2.MeetingTime_start = null;
                    obj2.MeetingTime_end = null;
                }
                else if (!obj.MeetingTime_start.HasValue && obj.MeetingTime_end.HasValue)
                {//只有结束时间,查询会议开始时间小于等于这个时间的
                    DateTime tempDate1Date = obj.MeetingTime_end.Value.Date;
                    string tempDate1Time = obj.MeetingTime_end.Value.ToString("HH:mm");
                    curTable = curTable.Where(u => u.MeetingDate < tempDate1Date || (u.MeetingDate == tempDate1Date && u.StartTime.CompareTo(tempDate1Time) <= 0 /*u.StartTime<= tempDate1Time*/));
                    obj2.MeetingTime_start = null;
                    obj2.MeetingTime_end = null;
                }
                else if (obj.MeetingTime_start.HasValue && obj.MeetingTime_end.HasValue)
                {//开始结束时间有，查询“会议开始时间处于该时间段中间”或“会议结束时间处于该时间段中间”
                    DateTime tempDate1 = obj.MeetingTime_start.Value;
                    DateTime tempDate1Date = obj.MeetingTime_end.Value.Date;
                    string tempDate1Time = obj.MeetingTime_end.Value.ToString("HH:mm");
                    DateTime tempDate2 = obj.MeetingTime_end.Value;
                    DateTime tempDate2Date = obj.MeetingTime_end.Value.Date;
                    string tempDate2Time = obj.MeetingTime_end.Value.ToString("HH:mm");
                    curTable = curTable.Where(u => (u.EndDate_Real >= tempDate1 && u.EndDate_Real <= tempDate2) || ((u.MeetingDate.Value < tempDate2Date || (u.MeetingDate == tempDate2Date && u.StartTime.CompareTo(tempDate2Time) <= 0)) && (u.MeetingDate > tempDate1Date || (u.MeetingDate == tempDate1Date && u.StartTime.CompareTo(tempDate1Time) >= 0))));
                    obj2.MeetingTime_start = null;
                    obj2.MeetingTime_end = null;
                }
                #endregion

                var exp = ExpressionFactory(obj2);
                if (exp != null)
                    curTable = curTable.Where(exp);

            }
            return curTable;
        }
    }
}
