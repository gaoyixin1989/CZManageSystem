using CZManageSystem.Admin.Models;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Service.HumanResources.Vacation;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.Vacation
{
    public class VacationHolidayController : BaseController
    {
        IHRVacationHolidayService _holidayService = new HRVacationHolidayService();//其他休假明细管理
        IHRVacationTeachingService _teachingService = new HRVacationTeachingService();//内部讲师授课
        IHRVacationCoursesService _coursesService = new HRVacationCoursesService();//假期培训
        IHRVacationMeetingService _meetingService = new HRVacationMeetingService();//假期会议
        IHRVacationOtherService _otherService = new HRVacationOtherService();//其他休假
        ISysUserService _sysUserService = new SysUserService();
        // GET: Holidays
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid? ID, string type)
        {
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }

        #region 其他休假明细管理
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HolidayQueryBuilder queryBuilder = null)
        {
            int count = 0;
           var source= _holidayService.List().Where(w=>1==1);
            if (queryBuilder.UserId !=null )
            {
                source = source.Where(w=>w.UserId == queryBuilder.UserId);
            }
            if (queryBuilder.YearDate != null)
            {
                source = source.Where(w => w.YearDate == queryBuilder.YearDate);
            }
            if (!string .IsNullOrEmpty ( queryBuilder.EmployeeId ))
            {
                source=source.Where(w => w.UserObj.EmployeeId.Contains(queryBuilder.EmployeeId));
            }
            if (!string.IsNullOrEmpty(queryBuilder.DpId))
            {
                source = source.Where(w => w.UserObj.DpId == queryBuilder.DpId);
            }
            var pageList = new PagedList<HRVacationHoliday>().QueryPagedList(source.OrderByDescending (o=>o.ID ), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
            var queryDatas = pageList.ToList().Select(u => new
            {
                u.ID,
                EndTime = u.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                u.PeriodTime,
                u.Reason,
                StartTime = u.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                UserName = u.UserObj.RealName,
                u.UserId,
                u.VacationType,
                u.VacationClass,
                u.YearDate,
                EmployeeId=u.UserObj.EmployeeId,
                DpId = u.UserObj.DpId,
                DpName = u.UserObj.Dept.DpName
            });

            return Json(new { items = queryDatas, count = count });

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _holidayService.List().Where(u => Ids.Contains(u.ID)).ToList();
            var m = _meetingService.List().Where(u => Ids.Contains(u.VacationID.Value)).ToList();
            var c = _coursesService.List().Where(u => Ids.Contains(u.VacationID.Value)).ToList();
            var t = _teachingService.List().Where(u => Ids.Contains(u.VacationID.Value)).ToList();
            var o = _otherService.List().Where(u => Ids.Contains(u.VacationID.Value)).ToList();
          
            if (objs.Count > 0)
            {
                isSuccess = _holidayService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
                _meetingService.DeleteByList(m);
                _coursesService.DeleteByList(c);
                _teachingService.DeleteByList(t);
                _otherService.DeleteByList(o);
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult GetByID(Guid ID)
        {
            var holiday = _holidayService.FindById(ID);
            var m = _meetingService.FindByFeldName(s => s.VacationID == ID);
            var c = _coursesService.FindByFeldName(s => s.VacationID == ID);
            var t = _teachingService.FindByFeldName(s => s.VacationID == ID);
            var o = _otherService.FindByFeldName(s => s.VacationID == ID);
            object list = new
            {
                holiday.ID,
                EndTime= holiday.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                holiday.PeriodTime,
                holiday.Reason,
                StartTime= holiday.StartTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                UserName = holiday.UserObj.RealName,
                holiday.UserId,
                holiday.VacationType,
                holiday.VacationClass,
                holiday.YearDate
            };

            return Json(new {list=list,m=m,c=c,t=t,o=o });
        }
        public ActionResult Save(HRVacationHoliday holiday, HRVacationMeeting meeting, HRVacationCourses courses, HRVacationTeaching teaching, HRVacationOther other)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            if (holiday.ID == null || holiday.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                holiday.ID = Guid.NewGuid();
              var v=  _holidayService.List().Where(s => s.UserId == holiday.UserId&&s.StartTime<holiday.StartTime&&s.EndTime>holiday.EndTime).ToList();
                if (v.Count > 0)
                {
                    tip = "已有该记录";
                }
                else
                {
                    result.IsSuccess = _holidayService.Insert(holiday);
                }
               
            }
            else
            {//编辑
                result.IsSuccess = _holidayService.Update(holiday);
               var m= _meetingService.FindByFeldName(s => s.VacationID == holiday.ID);
                var c = _coursesService.FindByFeldName(s => s.VacationID == holiday.ID);
                var t = _teachingService.FindByFeldName(s => s.VacationID == holiday.ID);
                var o = _otherService.FindByFeldName(s => s.VacationID == holiday.ID);
                _meetingService.Delete(m);
                _coursesService.Delete(c);
                _teachingService.Delete(t);
                _otherService.Delete(o);
            }
            if (holiday.VacationType == "公假")
            {
                if (holiday.VacationClass == "开会")
                {
                    if (string.IsNullOrEmpty(meeting.MeetingName))
                    {
                        tip = "会议名称不能为空";
                    }
                    else
                    {
                        meeting.ID = Guid.NewGuid();
                        meeting.VacationID = holiday.ID;
                        meeting.StartTime = holiday.StartTime;
                        meeting.EndTime = holiday.EndTime;
                        meeting.PeriodTime = holiday.PeriodTime;
                        result.IsSuccess= _meetingService.Insert(meeting);
                    }
                   

                }
                if (holiday.VacationClass == "培训")
                {
                    if (string.IsNullOrEmpty(courses.CoursesName))
                    {
                        tip = "课程名称不能为空";
                    }
                   else if (string.IsNullOrEmpty(courses.CoursesType))
                    {
                        tip = "课程类别不能为空";
                    }
                    else if (string.IsNullOrEmpty(courses.ProvinceCity))
                    {
                        tip = "主办单位不能为空";
                    }
                    else
                    {
                        courses.VacationID = holiday.ID;
                        courses.CoursesId = Guid.NewGuid();
                        courses.StartTime = holiday.StartTime;
                        courses.EndTime = holiday.EndTime;
                        courses.PeriodTime = holiday.PeriodTime;
                        result.IsSuccess = _coursesService.Insert(courses);
                    }

                }
                if (holiday.VacationClass == "内部讲师授课")
                {
                    if (string.IsNullOrEmpty(teaching.TeachingPlan))
                    {
                        tip = "授课名称不能为空";
                    }
                    else if (string.IsNullOrEmpty(teaching.TeacherType))
                    {
                        tip = "讲师级别不能为空";
                    }
                    else
                    {
                        teaching.VacationID = holiday.ID;
                        teaching.ID = Guid.NewGuid();
                        teaching.StartTime = holiday.StartTime;
                        teaching.EndTime = holiday.EndTime;
                        teaching.PeriodTime = holiday.PeriodTime;
                        result.IsSuccess = _teachingService.Insert(teaching);
                    }

                }
                if (holiday.VacationClass == "其他")
                {
                    other.VacationID = holiday.ID;
                    other.ID = Guid.NewGuid();
                    other.StartTime = holiday.StartTime;
                    other.EndTime = holiday.EndTime;
                    other.PeriodTime = holiday.PeriodTime;
                    result.IsSuccess = _otherService.Insert(other);

                }
            }
        result.Message = tip;
            return Json(result);
        }
        /// <summary>
        /// 获取下拉框休假类型
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetVacationTypeList()
        {
            var VacationList = this.GetDictListByDDName("休假类型");
            return Json(new
            {
                VacationList = VacationList
            });
        }
        /// <summary>
        /// 获取下拉框公假类型
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetVacationClassList()
        {
            var VacationClassList = this.GetDictListByDDName("公假类型");
            return Json(new
            {
                VacationClassList = VacationClassList
            });
        }
        public ActionResult GetCourseList()
        {
            var CoursesList = this.GetDictListByDDName("课程类别");
            var ProvinceCityList = this.GetDictListByDDName("主办单位");
            return Json(new
            {
                CoursesList = CoursesList,
                ProvinceCityList= ProvinceCityList
            });
        }
        public ActionResult GetTeacherList()
        {
            var TeacherTypeList = this.GetDictListByDDName("讲师级别");
            return Json(new
            {
                TeacherTypeList = TeacherTypeList
            });
        }
        public ActionResult GetYearList()
        {
            var YearDateList = this.GetDictListByDDName("年");
            return Json(new
            {
                YearDateList = YearDateList
            });
        }

        #endregion
    }
}