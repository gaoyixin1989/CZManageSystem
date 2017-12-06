using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using CZManageSystem.Service.HumanResources.Attendance;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.Attendance
{
    public class AttendanceController : BaseController
    {
        // GET: Attendance  
        IHRAttendanceConfigService hrAttendanceConfigService = new HRAttendanceConfigService();
        IHRTimeConfigService hrTimeConfigService = new HRTimeConfigService();
        IHRHolidaysService hrHolidaysService = new HRHolidaysService();
        ISysDeptmentService sysDeptmentService = new SysDeptmentService();
        IHRHolidayWorkService hrHolidayWorkService = new HRHolidayWorkService();
        IHRRfsimService hrRfsimService = new HRRfsimService();
        IHRTBcardService hrTBcardService = new HRTBcardService();
        ISysUserService sysUserService = new SysUserService();
        public ActionResult ConfigAllEdit()
        {
            return View();
        }
        #region Config

        public ActionResult ConfigIndex()
        {
            return View();
        }
        public ActionResult ConfigEdit(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }
        public ActionResult GetConfigByID(Guid key)
        {
            var model = hrAttendanceConfigService.FindById(key);

            return Json(new
            {
                model.ID,
                model.DeptIds,
                model.AMOffDuty,
                model.AMOnDuty,
                DeptName = GetDpName(model.DeptIds),// sysDeptmentService.FindByFeldName(f => f.DpId == model.DeptIds)?.DpName,
                model.PMOffDuty,
                model.PMOnDuty
            });
        }
        public ActionResult GetConfig()
        {
            var model = hrAttendanceConfigService.FindByFeldName(f => f.DeptIds == "NULL");
            if (model == null)
            {
                model = new HRAttendanceConfig() { ID = Guid.NewGuid() };
                model.DeptIds = "NULL";
                if (hrAttendanceConfigService.Insert(model))
                    return Json(model);
                return Json(null);
            }
            return Json(new
            {
                model.ID,
                model.DeptIds,
                model.AMOffDuty,
                model.AMOnDuty,
                //DeptName = GetDpName(model.DeptIds), 
                model.PMOffDuty,
                model.PMOnDuty,
                model.LatestTime,
                model.LeadTime,
                model.SpanTime
            });
        }
        public ActionResult GetConfigList(int pageIndex = 1, int pageSize = 5, object queryBuilder = null)
        {
            int count = 0;
            var listResult = hrAttendanceConfigService.GetForPaging(out count, null, pageIndex, pageSize) as List<HRAttendanceConfig>;
            var modelList = listResult.Select(
                s => new
                {
                    s.ID,
                    s.DeptIds,
                    s.AMOffDuty,
                    s.AMOnDuty,
                    DeptName = GetDpName(s.DeptIds ),
                    s.PMOffDuty,
                    s.PMOnDuty
                }
                );
            return Json(new { items = modelList, count = count });
        }

        public ActionResult GetDeptName(string deptId)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var model = hrAttendanceConfigService.FindByFeldName(f => f.DeptIds == deptId);
            if (model != null)
            {
                result.Message = "当前部门已存在设置！只能修改！";
                result.data = new
                {
                    model.ID,
                    model.DeptIds,
                    model.AMOffDuty,
                    model.AMOnDuty,
                    DeptName = GetDpName(model.DeptIds),
                    model.PMOffDuty,
                    model.PMOnDuty
                };
            }
            return Json(result);
        }
        public ActionResult ConfigDelete(Guid[] ids)
        {
            var list = hrAttendanceConfigService.List().Where(f => ids.Contains(f.ID));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (hrAttendanceConfigService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult SaveConfig(HRAttendanceConfig hrAttendanceConfig)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (hrAttendanceConfig == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            if (hrAttendanceConfig.ID == new Guid())
            {
                hrAttendanceConfig.ID = Guid.NewGuid();
                hrAttendanceConfig.Editor = WorkContext.CurrentUser.RealName;
                hrAttendanceConfig.EditorId = WorkContext.CurrentUser.UserId;
                hrAttendanceConfig.EditTime = DateTime.Now;
                if (hrAttendanceConfigService.Insert(hrAttendanceConfig))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            var model = hrAttendanceConfigService.FindById(hrAttendanceConfig.ID);
            if (model == null)
            {
                result.Message = "没有查询到当前记录。";
                return Json(result);

            }

            model.DeptIds = hrAttendanceConfig.DeptIds;
            model.AMOffDuty = hrAttendanceConfig.AMOffDuty;
            model.AMOnDuty = hrAttendanceConfig.AMOnDuty;
            model.PMOffDuty = hrAttendanceConfig.PMOffDuty;
            model.PMOnDuty = hrAttendanceConfig.PMOnDuty;
            model.Editor = WorkContext.CurrentUser.RealName;
            model.EditorId = WorkContext.CurrentUser.UserId;
            model.EditTime = DateTime.Now;
            model.LatestTime = hrAttendanceConfig.LatestTime;
            model.LeadTime = hrAttendanceConfig.LeadTime;
            model.SpanTime = hrAttendanceConfig.SpanTime;
            if (hrAttendanceConfigService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }

        #endregion
        #region Holidays

        public ActionResult HolidaysIndex()
        {
            return View();
        }
        public ActionResult HolidaysEdit(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }
        public ActionResult GetHolidaysByID(Guid key)
        {
            var model = hrHolidaysService.FindById(key);

            return Json(new
            {
                model.ID,
                model.HolidayClass,
                model.HolidayName,
                HolidayYear = CommonConvert.DateTimeToString(model.HolidayYear, "yyyy年"),
                StartTime = CommonConvert.DateTimeToString(model.StartTime),
                EndTime = CommonConvert.DateTimeToString(model.EndTime)
            });
        }
        public ActionResult GetHolidaysList(int pageIndex = 1, int pageSize = 5, AttendanceHolidayQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var listResult = hrHolidaysService.GetForPaging(out count, queryBuilder, pageIndex, pageSize) as List<HRHolidays>;
            var modelList = listResult.Select(
                s => new
                {
                    s.ID,
                    s.EndTime,
                    s.HolidayClass,
                    s.HolidayName,
                    s.HolidayYear,
                    s.Remark,
                    s.StartTime
                }
                );
            return Json(new { items = modelList, count = count });
        }

        public ActionResult HolidaysDelete(Guid[] ids)
        {
            var list = hrHolidaysService.List().Where(f => ids.Contains(f.ID));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (hrHolidaysService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult SaveHolidays(HRHolidays hrHolidays)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (hrHolidays == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            if (hrHolidays.ID == new Guid())
            {
                hrHolidays.ID = Guid.NewGuid();
                hrHolidays.Editor = WorkContext.CurrentUser.RealName;
                hrHolidays.EditorId = WorkContext.CurrentUser.UserId;
                hrHolidays.EditTime = DateTime.Now;
                if (hrHolidaysService.Insert(hrHolidays))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            var model = hrHolidaysService.FindById(hrHolidays.ID);
            if (model == null)
            {
                result.Message = "没有查询到当前记录。";
                return Json(result);

            }

            model.StartTime = hrHolidays.StartTime;
            model.EndTime = hrHolidays.EndTime;
            model.HolidayClass = hrHolidays.HolidayClass;
            model.HolidayName = hrHolidays.HolidayName;
            model.Editor = WorkContext.CurrentUser.RealName;
            model.EditorId = WorkContext.CurrentUser.UserId;
            model.EditTime = DateTime.Now;
            model.HolidayYear = hrHolidays.HolidayYear;
            if (hrHolidaysService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }

        #endregion
        #region TimeConfig

        public ActionResult TimeConfigIndex()
        {
            return View();
        }
        public ActionResult TimeConfigEdit(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }
        public ActionResult GetTimeConfigByID(Guid key)
        {
            var model = hrTimeConfigService.FindById(key);

            return Json(new
            {
                model.ID,
                model.LatestTime,
                model.LeadTime,
                model.SpanTime
                //HolidayYear = CommonConvert.DateTimeToString(model.HolidayYear, "yyyy年"),
                //StartTime = CommonConvert.DateTimeToString(model.StartTime),
                //EndTime = CommonConvert.DateTimeToString(model.EndTime)
            });
        }
        public ActionResult GetTimeConfigList(int pageIndex = 1, int pageSize = 5, object queryBuilder = null)
        {
            int count = 0;
            var listResult = hrTimeConfigService.GetForPaging(out count, null, pageIndex, pageSize) as List<HRTimeConfig>;
            var modelList = listResult.Select(
                s => new
                {
                    s.ID,
                    s.LatestTime,
                    s.LeadTime,
                    s.SpanTime
                }
                );
            return Json(new { items = modelList, count = count });
        }

        public ActionResult TimeConfigDelete(Guid[] ids)
        {
            var list = hrTimeConfigService.List().Where(f => ids.Contains(f.ID));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (hrTimeConfigService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult SaveTimeConfig(HRTimeConfig TimeConfig)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (TimeConfig == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            if (TimeConfig.ID == new Guid())
            {
                TimeConfig.ID = Guid.NewGuid();
                if (hrTimeConfigService.Insert(TimeConfig))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            var model = hrTimeConfigService.FindById(TimeConfig.ID);
            if (model == null)
            {
                result.Message = "没有查询到当前记录。";
                return Json(result);

            }

            model.LatestTime = TimeConfig.LatestTime;
            model.LeadTime = TimeConfig.LeadTime;
            model.SpanTime = TimeConfig.SpanTime;

            if (hrTimeConfigService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }

        #endregion
        #region HolidayWork

        public ActionResult HolidayWorkIndex()
        {
            return View();
        }
        public ActionResult HolidayWorkEdit(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }
        public ActionResult GetHolidayWorkByID(Guid key)
        {
            var model = hrHolidayWorkService.FindById(key);

            return Json(new
            {
                model.ID,
                EndTime = CommonConvert.DateTimeToString(model.EndTime),
                StartTime = CommonConvert.DateTimeToString(model.StartTime)
            });
        }
        public ActionResult GetHolidayWorkList(int pageIndex = 1, int pageSize = 5, object queryBuilder = null)
        {
            int count = 0;
            var listResult = hrHolidayWorkService.GetForPaging(out count, null, pageIndex, pageSize) as List<HRHolidayWork>;
            var modelList = listResult.Select(
                s => new
                {
                    s.ID,
                    s.EndTime,
                    s.StartTime
                }
                );
            return Json(new { items = modelList, count = count });
        }

        public ActionResult HolidayWorkDelete(Guid[] ids)
        {
            var list = hrHolidayWorkService.List().Where(f => ids.Contains(f.ID));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (hrHolidayWorkService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult SaveHolidayWork(HRHolidayWork holidayWork)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (holidayWork == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            if (holidayWork.StartTime > holidayWork.EndTime )
            {
                result.Message = "结束时间必须大于开始时间！";
                return Json(result);
            }
            if (holidayWork.ID == new Guid())
            {
                holidayWork.ID = Guid.NewGuid();
                if (hrHolidayWorkService.Insert(holidayWork))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            var model = hrHolidayWorkService.FindById(holidayWork.ID);
            if (model == null)
            {
                result.Message = "没有查询到当前记录。";
                return Json(result);

            }

            model.StartTime = holidayWork.StartTime;
            model.EndTime = holidayWork.EndTime;

            if (hrHolidayWorkService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }

        #endregion

        #region Rfsim

        public ActionResult RfsimIndex()
        {
            return View();
        }
        public ActionResult RfsimEdit(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }
        public ActionResult GetRfsimByID(Guid key)
        {
            var model = hrRfsimService.FindById(key);

            return Json(new
            {
                model.ActionStatus,
                EndTime = CommonConvert.DateTimeToString(model.CDateTime),
                model.DeviceSysId,
                model.DptId,
                model.EmplyId,
                model.EmplyName,
                model.OperatorId,
                model.RecordId,
                model.RecordType,
                model.Serial,
                model.SysNo
            });
        }
        public ActionResult GetRfsimList(int pageIndex = 1, int pageSize = 5, AttendanceQueryBuilder queryBuilder = null)
        {

            List<string> list = new List<string>();
            int count = 0;
            if (queryBuilder != null)
                list = GetEmployeeId(queryBuilder);
            var listResult = hrRfsimService.GetForPaging(out count, new { CDateTime_end=queryBuilder.SkTime_End,
                CDateTime_start=queryBuilder.SkTime_Start,
                EmplyId = list
            }, pageIndex, pageSize) as List<HRRfsim>;
            var modelList = listResult.Select(
                s => RfsimForUserInfo(s)
                ); 
            return Json(new { items = modelList, count = count });
        }

        public ActionResult RfsimDelete(Guid[] ids)
        {
            var list = hrRfsimService.List().Where(f => ids.Contains(f.RecordId));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (hrRfsimService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult SaveRfsim(HRRfsim rfsim)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (rfsim == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            if (rfsim.RecordId == new Guid())
            {
                rfsim.RecordId = Guid.NewGuid();
                if (hrRfsimService.Insert(rfsim))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            var model = hrRfsimService.FindById(rfsim.RecordId);
            if (model == null)
            {
                result.Message = "没有查询到当前记录。";
                return Json(result);

            }
            model.ActionStatus = rfsim.ActionStatus;
            model.CDateTime = rfsim.CDateTime;
            model.DeviceSysId = rfsim.DeviceSysId;
            model.DptId = rfsim.DptId;
            model.EmplyId = rfsim.EmplyId;
            model.EmplyName = rfsim.EmplyName;
            model.OperatorId = rfsim.OperatorId;
            model.RecordType = rfsim.RecordType;
            model.Serial = rfsim.Serial;
            model.SysNo = rfsim.SysNo;
            if (hrRfsimService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }

        #endregion
        #region TBcard

        public ActionResult TBcardIndex()
        {
            return View();
        }
        public ActionResult TBcardEdit(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }
        public ActionResult GetTBcardByID(Guid key)
        {
            var model = hrTBcardService.FindById(key);

            return Json(new
            {
                model.ActionStatus,
                model.CardNo,
                model.EmployeeId,
                model.EmpNo,
                model.ID,
                model.SkTime
            });
        }
        public ActionResult GetTBcardList(int pageIndex = 1, int pageSize = 5, AttendanceQueryBuilder queryBuilder = null)
        {
            List<string> list = new List<string>();
            int count = 0;
            if (queryBuilder != null)
                list = GetEmployeeId(queryBuilder);
            var listResult = hrTBcardService.GetForPaging(out count, new { queryBuilder.SkTime_End, queryBuilder.SkTime_Start, EmployeeId = list }, pageIndex, pageSize) as List<HRTBcard>;
            var modelList = listResult.Select(
                s => GetTBcardForUserInfo(s)
                );
            return Json(new { items = modelList, count = count });
        }

        public ActionResult TBcardDelete(Guid[] ids)
        {
            var list = hrTBcardService.List().Where(f => ids.Contains(f.ID));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (hrTBcardService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult SaveTBcard(HRTBcard tBcard)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (tBcard == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            if (tBcard.ID == new Guid())
            {
                tBcard.ID = Guid.NewGuid();
                if (hrTBcardService.Insert(tBcard))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            var model = hrTBcardService.FindById(tBcard.ID);
            if (model == null)
            {
                result.Message = "没有查询到当前记录。";
                return Json(result);

            }

            model.ActionStatus = tBcard.ActionStatus;
            model.CardNo = tBcard.CardNo;
            model.EmployeeId = tBcard.EmployeeId;
            model.EmpNo = tBcard.EmpNo;
            model.SkTime = tBcard.SkTime;

            if (hrTBcardService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }

        #endregion
        List<string> GetEmployeeId(AttendanceQueryBuilder queryBuilder)
        {
            List<string> result1 = new List<string>();
            List<string> result2 = new List<string>();
            List<string> resultList = new List<string>();
            if (queryBuilder == null)
                return null;
            if (queryBuilder.DeptIds != null && queryBuilder.DeptIds.Count() > 0)
            {
                result1.AddRange(sysUserService.List().Where(f => queryBuilder.DeptIds.Contains(f.DpId)).Select(s => s.UserName)
                    );
            }
            if (queryBuilder.UserIds != null && queryBuilder.UserIds.Count() > 0)
            {
                result2.AddRange(sysUserService.List().Where(f => queryBuilder.UserIds.Contains(f.UserId.ToString())).Select(s => s.UserName)
                    );
            }
            if (result2.Count > 0 && result1.Count > 0) 
                resultList= result1.Intersect(result2).ToList ();
            else if (result1.Count > 0)
                resultList = result1;
            else if (result2.Count > 0)
                resultList = result2;
            if (resultList == null || resultList.Count < 1)
                return null;
            return resultList;
        }
        dynamic GetTBcardForUserInfo(HRTBcard hrTBcard)
        {
            if (hrTBcard == null)
                return null;
            var model = sysUserService.FindByFeldName(f => f.UserName == hrTBcard.EmployeeId);
            if (model == null)
                return null;
            var dept = sysDeptmentService.FindById(model.DpId);
            return new
            {
                UserName = model.RealName,
                DeptName = dept?.DpName,
                hrTBcard.ActionStatus,
                hrTBcard.CardNo,
                hrTBcard.EmployeeId,
                hrTBcard.EmpNo,
                hrTBcard.ID,
                hrTBcard.SkTime
            };
        }

        dynamic RfsimForUserInfo(HRRfsim hrRfsim)
        {
            if (hrRfsim == null)
                return null;
            var model = sysUserService.FindByFeldName(f => f.UserName == hrRfsim.EmplyId);
            if (model == null)
                return null;
            var dept = sysDeptmentService.FindById(model.DpId);
            return new
            {
                UserName = model.RealName,
                DeptName = dept?.DpName,
                hrRfsim.ActionStatus,
                hrRfsim.Serial ,
                hrRfsim.EmplyId ,
                hrRfsim.SysNo ,
                hrRfsim.RecordId ,
                hrRfsim.CDateTime 
            };
        }


        string GetDpName(string DeptIds)
        {
            var list = sysDeptmentService.List().Where(f => DeptIds.Contains(f.DpId)).Select(q => q.DpName).ToList();
          return   string.Join(",", list);
        }
    }
}