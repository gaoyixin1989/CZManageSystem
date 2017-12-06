using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class ScheduleController : BaseController
    {
        IScheduleService _scheduleService = new ScheduleService();
        // GET: Schedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DayInfo(string date)
        {
            ViewData["date"] = date;
            return View();
        }

        /// <summary>
        /// 获取一天的数据列表
        /// </summary>
        /// <param name="curDate">日期，默认当天</param>
        /// <returns></returns>
        public ActionResult GetListDayData(string curDate)
        {
            if (string.IsNullOrEmpty(curDate))
                curDate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime Time_start = new DateTime();
            DateTime Time_end = new DateTime();
            if (!DateTime.TryParse(curDate, out Time_start))
                Time_start = DateTime.Now;
            Time_start = Time_start.Date;
            Time_end = Time_start.AddDays(1).AddSeconds(-1);
            return GetListData(Time_start, Time_end);
        }
        /// <summary>
        /// 获取一个月的数据列表
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <returns></returns>
        public ActionResult GetListMonthData(int? year, int? month)
        {
            year = year ?? DateTime.Now.Year;
            month = month ?? DateTime.Now.Month;
            DateTime Time_start = new DateTime();
            DateTime Time_end = new DateTime();
            Time_start = new DateTime(year.Value, month.Value, 1);
            Time_end = Time_start.AddMonths(1).AddSeconds(-1);
            return GetListData(Time_start, Time_end);
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="Time_start">开始时间</param>
        /// <param name="Time_end">结束时间</param>
        /// <returns></returns>
        private ActionResult GetListData(DateTime Time_start, DateTime Time_end)
        {
            var curUserId = this.WorkContext.CurrentUser.UserId;
            if (curUserId == null || curUserId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return Json(new { items = new List<object>(), count = 0 });
            }

            var condition = new
            {
                UserId = curUserId,
                Time_start = Time_start,
                Time_end = Time_end
            };
            int count = 0;
            var modelList = _scheduleService.GetForPaging(out count, condition);
            modelList = modelList.OrderBy(u => u.Time);
            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(int id)
        {
            var obj = _scheduleService.FindById(id);
            return Json(obj);
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ActionResult Save(Schedule dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            if (dataObj.ScheduleId == 0)//新增
            {
                dataObj.UserId = this.WorkContext.CurrentUser.UserId;
                dataObj.Createdtime = DateTime.Now;
                result.IsSuccess = _scheduleService.Insert(dataObj);
            }
            else
            {//编辑
                var oldObj = _scheduleService.FindById(dataObj.ScheduleId);
                oldObj.Time = dataObj.Time;
                oldObj.Content = dataObj.Content;
                dataObj = oldObj;
                result.IsSuccess = _scheduleService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(int[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _scheduleService.List().Where(u => ids.Contains(u.ScheduleId)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _scheduleService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

    }
}