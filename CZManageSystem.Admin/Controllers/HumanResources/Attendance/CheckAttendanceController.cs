using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Common;
using CZManageSystem.Service.HumanResources.Attendance;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.Attendance
{
    //CheckAttendance
    public class CheckAttendanceController : BaseController
    {
        // GET: 
        IHRCheckAttendanceService hrCheckAttendanceService = new HRCheckAttendanceService();
        IHRCheckAttendanceHistoryNo1Service hrCheckAttendanceHistoryNo1Service = new HRCheckAttendanceHistoryNo1Service();
        IHRAttendanceConfigService hrAttendanceConfigService = new HRAttendanceConfigService();
        ISysUserService sysUserService = new SysUserService();
        IHRUnattendApplyService hrUnattendApplyService = new HRUnattendApplyService();
        IV_HRCheckAttendanceAbnormalService v_HRCheckAttendanceAbnormalService = new V_HRCheckAttendanceAbnormalService();
        IV_HRAbnormalListService v_HRAbnormalListService = new V_HRAbnormalListService();
        IV_HRHaveHolidaysByTurnsListService v_HRHaveHolidaysByTurnslListService = new V_HRHaveHolidaysByTurnsListService();
        IV_HRHaveAHolidayListService v_HRHaveAHolidayListService = new V_HRHaveAHolidayListService();
        IV_HROtherListService v_HROtherListService = new V_HROtherListService();
        IHRUnattendLinkService hrUnattendLinkService = new HRUnattendLinkService();
        #region Attendance

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Attendance()
        {
            ViewData["startId"] = WorkContext.CurrentUser.Dept.DpId;
            return View();
        }
        public ActionResult Abnormal()
        {
            ViewData["startId"] = WorkContext.CurrentUser.Dept.DpId;
            return View();
        }

        /// <summary>
        /// 获取当天签到列表 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChckingIn()
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            #region 检查IP有效性
            //if (WorkContext .CurrentUser .CheckIP !=GetUserIp )
            //{
            //    result.Message = "当前IP无法签到！";
            //    return Json(result);
            //} 
            #endregion

            var now = DateTime.Now;
            #region 获取可打卡时间范围
            int LeadTime = 0, LatestTime = 0;
            var config = hrAttendanceConfigService.FindByFeldName(f => f.DeptIds == "NULL");
            if (config != null)
            {
                LeadTime = config.LeadTime ?? 0;
                LatestTime = config.LatestTime ?? 0;
            }

            #endregion
            var date = Convert.ToDateTime(now.ToString("yyyy-MM-dd"));
            var list = hrCheckAttendanceService.List().Where(w => w.AtDate == date && w.UserId == WorkContext.CurrentUser.UserId).OrderBy(o => new { o.AtDate, o.OffDate, o.DoTime, o.OffTime }).ToList().Select(s => new
            {
                s.UserId,
                s.AtDate,
                OffDate = string.IsNullOrEmpty(s.OffDate?.ToString()) ? "---" : s.OffDate.ToString(),
                DoTime = string.IsNullOrEmpty(s.DoTime?.ToString()) ? "" : s.DoTime.ToString(),
                OffTime = string.IsNullOrEmpty(s.OffTime?.ToString()) ? "" : s.OffTime?.ToString(),
                s.RotateDaysOffFlag,
                s.Users.CheckIP,
                s.AttendanceId,
                s.DoFlag,
                IsDoReallyTime = string.IsNullOrEmpty(s.DoReallyTime?.ToString()) ? false : true,
                IsOffReallyTime = string.IsNullOrEmpty(s.OffReallyTime?.ToString()) ? false : true,
                Minute = s.Minute ?? 0,
                TimeQuantum = GetTimeQuantum(s.DoTime),
                LeadTime = LeadTime,
                LatestTime = LatestTime,
                IsDo = !string.IsNullOrEmpty(s.DoReallyTime?.ToString()) ? false : IsTrue(s.AtDate, s.DoTime, LeadTime, s.Minute ?? 0),
                IsOff = !string.IsNullOrEmpty(s.OffReallyTime?.ToString()) ? false : IsTrue(s.OffDate, s.OffTime, LatestTime, s.Minute ?? 0, false)
            }).ToList();
            if (list.Count > 0)
            {
                result.IsSuccess = true;
                result.data = list;
            }
            return Json(result);
        }
        [SysOperation(OperationType.Save, "考勤签到")]
        public ActionResult SaveChckingIn(string id, bool isDoTime = true)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            Guid attendanceId = new Guid();
            if (!Guid.TryParse(id, out attendanceId))
            {
                result.Message = "参数ID非Guid格式！";
                return Json(result);
            }
            var model = hrCheckAttendanceService.FindById(attendanceId);
            if (model == null)
            {
                result.Message = "签到异常，没有可以签到记录！";
                return Json(result);
            }
            var now = DateTime.Now;
            #region 获取可打卡时间范围
            int LeadTime = 0, LatestTime = 0;
            var config = hrAttendanceConfigService.FindByFeldName(f => f.DeptIds == "NULL");
            if (config != null)
            {
                LeadTime = config.LeadTime ?? 0;
                LatestTime = config.LatestTime ?? 0;
            }

            #endregion
            if (isDoTime)//上班签到
            {
                if (!string.IsNullOrEmpty(model?.DoReallyTime.ToString()))
                {
                    result.Message = "已签到！";
                    return Json(result);
                }
                if (!IsTrue(model.AtDate, model.DoTime, LeadTime, model.Minute ?? 0))
                {
                    result.Message = "非签到时间！";
                    return Json(result);
                }
                model.DoReallyTime = new TimeSpan(now.Hour, now.Minute, now.Second);
            }
            else//下班签到
            {
                if (!IsTrue(model.OffDate, model.OffTime, LatestTime, model.Minute ?? 0, false))
                {
                    result.Message = "非签到时间！";
                    return Json(result);
                }
                model.OffReallyTime = new TimeSpan(now.Hour, now.Minute, now.Second);
            }
            model.IpOff = GetUserIp;
            result.IsSuccess = hrCheckAttendanceService.Update(model);
            return Json(result);
        }
        /// <summary>
        /// 异常查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetAbnormalList(int pageIndex = 1, int pageSize = 5, AttendanceListQueryBuilder queryBuilder = null)
        {
            int count = 0;

            //object query2 = new
            //{
            //    queryBuilder.AtDate_Start,
            //    queryBuilder.AtDate_End,
            //    queryBuilder.IpOn,
            //    queryBuilder.IpOff,
            //    queryBuilder.EmployeeId,
            //    queryBuilder.UserId,
            //    queryBuilder.DpId

            //};

            #region MyRegion
            //var listResult = hrCheckAttendanceService.GetForPaging_(out count, new { queryBuilder.AtDate_End, queryBuilder.AtDate_Start, UserId = WorkContext.CurrentUser.UserId }, pageIndex, pageSize);
            //var now = DateTime.Now;
            //var nowD = Convert.ToDateTime(now.ToShortDateString());
            //var nowTimeSpan = new TimeSpan(now.Hour, now.Minute, now.Second);

            //var list = hrCheckAttendanceService.List().Where(w =>
            //  (w.DoFlag == null || w.DoFlag != 2)// 1、已申报；2、休假；3、外出
            //  && w.DoTime != null//需要考勤的
            //  && w.RotateDaysOffFlag != 1//非休假的
            //  && !w.Users.UserName.Contains("admin")//管理员除外
            //  && ((w.AtDate < nowD
            //      && ((w.DoReallyTime == null//没有上班签到记录
            //          || w.DoReallyTime.Value.TotalMinutes > (w.DoTime.Value.TotalMinutes + w.Minute))//或者签到时间晚于最迟上班时间
            //          || (w.OffReallyTime == null
            //          || w.OffReallyTime.Value.TotalMinutes > (w.OffTime.Value.TotalMinutes - w.Minute))
            //      ))
            //    //当天的情况
            //    || (w.AtDate == nowD
            //    && ((nowTimeSpan.TotalMinutes > (w.DoTime.Value.TotalMinutes + w.Minute)
            //    && (w.DoReallyTime == null//没有上班签到记录
            //          || w.DoReallyTime.Value.TotalMinutes > (w.DoTime.Value.TotalMinutes + w.Minute))//或者签到时间晚于最迟上班时间 
            //          )
            //          || ((nowTimeSpan.TotalMinutes < (w.DoTime.Value.TotalMinutes - w.Minute)
            //          && (w.OffReallyTime == null
            //          || w.OffReallyTime.Value.TotalMinutes > (w.OffTime.Value.TotalMinutes - w.Minute))))
            //          ))
            //      )
            //      ).Select(s => new
            //      {
            //          s.AttendanceId,
            //          s.AtDate,
            //          s.DoFlag,
            //          s.DoReallyTime,
            //          s.DoTime,
            //          s.FlagOff,
            //          s.FlagOn,
            //          s.IpOff,
            //          s.IpOn,
            //          s.Minute,
            //          s.OffDate,
            //          s.OffReallyTime,
            //          s.OffTime,
            //          s.RotateDaysOffFlag,
            //          s.Users.RealName,
            //          s.Users.Dept.DpName
            //      }
            //      );




            #endregion
            //, UserId = WorkContext.CurrentUser.UserId 
            var listResult = v_HRCheckAttendanceAbnormalService.GetForPaging(out count, queryBuilder, pageIndex, pageSize).ToList();

            return Json(new { items = listResult, count = count });
        }
        /// <summary>
        /// 考勤查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetAttendanceList(int pageIndex = 1, int pageSize = 5, HRCheckAttendanceQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var listResult = hrCheckAttendanceService.GetForPaging(out count, queryBuilder, pageIndex, pageSize);
            return Json(new { items = listResult, count = count });
        }

        #endregion
        #region CheckAttendance
        public ActionResult HaveBeenDeclared()
        {
            return View();
        }
        public ActionResult ForWF(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }

        public ActionResult Edit(string key = null, bool isTrue = false)
        {
            //有值=》编辑状态
            ViewData["key"] = !string.IsNullOrEmpty(key) ? key : "";
            ViewData["isTrue"] = isTrue;
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.Abnormal;
            return View();
        }
        public ActionResult GetByID(Guid key)
        {
            var model = hrCheckAttendanceService.FindById(key);

            return Json(new
            {
                model.AtDate,
                model.AttendanceId,
                model.DoFlag,
                model.DoReallyTime,
                model.DoTime,
                model.FlagOff,
                model.FlagOn,
                model.IpOff,
                model.IpOn,
                model.Minute,
                model.OffReallyTime,
                model.OffTime,
                model.RotateDaysOffFlag,
                model.UserId,
                model.Users?.RealName
            });
        }
        public ActionResult GetAbnormalByID(string key, bool isTrue = false)
        {
            try
            {
                SystemResult result = new Models.SystemResult() { IsSuccess = false };
                if (isTrue)//仅拿表单
                {
                    var model = hrUnattendApplyService.FindById(new Guid(key));//此时的key为申请单的GUID
                    if (model != null)
                        result.data = model;
                    return Json(result);
                }
                var keys = key.Split(',');
                var gKeys = keys.Select(s => new Guid(s)).ToList();
                var modelList = v_HRCheckAttendanceAbnormalService.List().Where(w => gKeys.Contains((Guid)w.AttendanceId)).ToList();
                if (modelList.Count <= 0)
                {
                    result.Message = "不存在当前记录！";
                    return Json(result);
                }
                var unattendLink = hrUnattendLinkService.List().Where(w => gKeys.Contains((Guid)w.AttendanceId));

                if (unattendLink.Count() > 0)
                {
                    result.Message = "您所选择的记录已经全部申报或者部分申报！";
                    return Json(result);
                }
                var user = WorkContext.CurrentUser;
                string recordContent = "";
                modelList.ForEach(s => recordContent += "出勤日期："
                        + s.AtDate.Value.ToShortDateString()
                        + "  上班时间："
                        + s.DoTime
                        + "  下班时间："
                        + s.OffTime
                        + "  实际上班时间："
                        + s.DoReallyTime
                        + "  实际下班时间："
                        + s.OffReallyTime
                        + "  状态："
                        + ChcekingIn.CheckDuty(s.AtDate, s.DoTime, s.OffTime, s.DoReallyTime, s.OffReallyTime, s.Minute, s.DoFlag, s.RotateDaysOffFlag) + "\n");
                var unattend = new HRUnattendApply()
                {
                    AttendanceIds = string.Join(",", modelList.Select(s => s.AttendanceId.ToString())),
                    ApplyDept = user.Dept?.DpName,
                    ApplyUserName = user.RealName,
                    Mobile = user.Mobile,
                    ApplySn = "提交后自动生成",
                    ApplyTitle = user.RealName + "的考勤异常申报(" + DateTime.Now.ToString("yyyy-MM-dd") + ")",
                    CreateTime = DateTime.Now,
                    RecordContent = recordContent
                };

                result.IsSuccess = true;
                result.data = unattend;
                return Json(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GetAbnormalInfoByID(Guid key)
        {
            SystemResult result = new Models.SystemResult() { IsSuccess = false };

            var unattend = hrUnattendApplyService.FindByFeldName(f => f.ApplyID == key);
            if (unattend == null)
            {
                result.Message = "不存在当前记录！";
                return Json(result);
            }
            result.IsSuccess = true;
            result.data = unattend;
            return Json(result);
        }


        public ActionResult saveResult(string AttendanceIds)
        {
            var keys = AttendanceIds.Split(',');
            var gKeys = keys.Select(s => new Guid(s)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "保存对象不存在！" };
            var hrCheckAttendance = hrCheckAttendanceService.List().Where(w => gKeys.Contains((Guid)w.AttendanceId)).ToList();
            bool isTrue = false, isTrueNo1 = false;
            if (hrCheckAttendance.Count > 0)
            {
                for (int i = 0; i < hrCheckAttendance.Count; i++)
                    hrCheckAttendance[i].DoFlag = 1;//已申报
                isTrue = hrCheckAttendanceService.UpdateByList(hrCheckAttendance);
            }
            var hrCheckAttendanceHistoryNo1 = hrCheckAttendanceHistoryNo1Service.List().Where(w => gKeys.Contains((Guid)w.HistoryId)).ToList();
            if (hrCheckAttendanceHistoryNo1.Count > 0)
            {
                for (int i = 0; i < hrCheckAttendanceHistoryNo1.Count; i++)
                    hrCheckAttendanceHistoryNo1[i].DoFlag = 1;//已申报 
                isTrueNo1 = hrCheckAttendanceHistoryNo1Service.UpdateByList(hrCheckAttendanceHistoryNo1);
            }
            result.Message = "";
            result.IsSuccess = (isTrueNo1 || isTrue);
            return Json(result);
        }

        /// <summary>
        /// 考勤异常申报
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetDataList(int pageIndex = 1, int pageSize = 5, CheckAttendanceQueryBuilder queryBuilder = null, bool isHave = false)
        {
            int count = 0;
            object obj = WorkContext.CurrentUser.UserName == Base.Admin.UserName ?
             (obj = new { queryBuilder.AtDate_End, queryBuilder.AtDate_Start }) :
             (obj = new { queryBuilder.AtDate_End, queryBuilder.AtDate_Start, UserId = WorkContext.CurrentUser.UserId });
            var listResult = v_HRCheckAttendanceAbnormalService.GetForPaging_(out count, obj, pageIndex, pageSize, isHave);
            return Json(new { items = listResult, count = count });
        }
        [SysOperation(OperationType.Delete, "删除考勤记录")]
        public ActionResult Delete(Guid[] ids)
        {
            var list = hrCheckAttendanceService.List().Where(f => ids.Contains(f.AttendanceId));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            result.IsSuccess = hrCheckAttendanceService.DeleteByList(models);
            return Json(result);
        }
        [SysOperation(OperationType.Save, "修正异常考勤")]
        public ActionResult UpdateData(Guid?[] ids)
        {
            var list = hrCheckAttendanceService.List().Where(f => ids.Contains(f.AttendanceId));
            var historyNo1List = hrCheckAttendanceHistoryNo1Service.List().Where(f => ids.Contains(f.HistoryId));
            var models = list.ToList();
            var historyNo1Models = historyNo1List.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (models.Count <= 0 && historyNo1Models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            for (int i = 0; i < models.Count; i++)
            {
                models[i].DoFlag = 1;
            }
            for (int i = 0; i < historyNo1Models.Count; i++)
            {
                historyNo1Models[i].DoFlag = 1;
            }
            int count = 0;
            count += hrCheckAttendanceService.UpdateByList(models) ? models.Count : 0;
            count += hrCheckAttendanceHistoryNo1Service.UpdateByList(historyNo1Models) ? historyNo1Models.Count : 0;
            result.IsSuccess = count > 0;
            result.data = new { successCount = count };
            return Json(result);
        }

        public ActionResult Save(HRCheckAttendance checkAttendance)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (checkAttendance == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            if (checkAttendance.AttendanceId == new Guid())
            {
                checkAttendance.AttendanceId = Guid.NewGuid();
                if (hrCheckAttendanceService.Insert(checkAttendance))
                {
                    result.IsSuccess = true;
                    return Json(result);
                }
            }
            var model = hrCheckAttendanceService.FindById(checkAttendance.AttendanceId);
            if (model == null)
            {
                result.Message = "没有查询到当前记录。";
                return Json(result);

            }

            model.AtDate = checkAttendance.AtDate;
            model.DoFlag = checkAttendance.DoFlag;
            model.DoReallyTime = checkAttendance.DoReallyTime;
            model.DoTime = checkAttendance.DoTime;
            model.FlagOff = checkAttendance.FlagOff;
            model.FlagOn = checkAttendance.FlagOn;
            model.IpOff = checkAttendance.IpOff;
            model.IpOn = checkAttendance.IpOn;
            model.Minute = checkAttendance.Minute;
            model.OffReallyTime = checkAttendance.OffReallyTime;
            model.OffTime = checkAttendance.OffTime;
            model.RotateDaysOffFlag = checkAttendance.RotateDaysOffFlag;
            model.UserId = checkAttendance.UserId;
            if (hrCheckAttendanceService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult SaveAbnormalApply(HRUnattendApply hrUnattendApply, bool isAction = false, string nextActivity = "", string nextActors = "", string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            HRUnattendApply model;
            if (hrUnattendApply == null)
            {
                result.Message = "保存对象对Null！";
                return Json(result);
            }
            var keys = hrUnattendApply.AttendanceIds.Split(',');
            var gKeys = keys.Select(s => new Guid(s)).ToList();
            if (hrUnattendApply.ApplyID == new Guid())
            {
                model = hrUnattendApply;
                model.ApplyID = Guid.NewGuid();
                if (hrUnattendApplyService.Insert(model))
                {
                    #region 将ApplyRecordId、AttendanceId加到关系表HRUnattendLink中
                    List<HRUnattendLink> linkList = new List<HRUnattendLink>();// hrUnattendLinkService.
                    gKeys.ForEach(f =>
                    linkList.Add(
                        new HRUnattendLink()
                        {
                            ID = Guid.NewGuid(),
                            ApplyRecordId = model.ApplyID,
                            AttendanceId = f
                        }));
                    if (linkList.Count > 0)
                        hrUnattendLinkService.InsertByList(linkList);
                    #endregion
                    result.IsSuccess = true;
                }
            }
            else
            {
                model = hrUnattendApplyService.FindById(hrUnattendApply.ApplyID);
                if (model == null)
                {
                    result.Message = "没有查询到当前记录。";
                    return Json(result);

                }

                model.ApplyDept = hrUnattendApply.ApplyDept;
                model.ApplySn = hrUnattendApply.ApplySn;
                model.ApplyTitle = hrUnattendApply.ApplyTitle;
                model.ApplyUserName = hrUnattendApply.ApplyUserName;
                model.AttendanceIds = hrUnattendApply.AttendanceIds;
                model.CreateTime = hrUnattendApply.CreateTime;
                model.Mobile = hrUnattendApply.Mobile;
                model.Reason = hrUnattendApply.Reason;
                model.RecordContent = hrUnattendApply.RecordContent;
                model.Remark = hrUnattendApply.Remark;
                if (hrUnattendApplyService.Update(model))
                    result.IsSuccess = true;
            }
            if (isAction && result.IsSuccess)
            {

                result.IsSuccess = false;
                string workflowType = FlowInstance.WorkflowType.Abnormal;
                result.IsSuccess = false;
                Dictionary<string, string> dictField = new Dictionary<string, string>();
                dictField.Add("F1", model.ApplyID.ToString());
                var workFlowResult = WorkFlowHelper.ActionFlow(WorkContext.CurrentUser.UserName, model.ApplyTitle, dictField, nextActivity, nextActors, workflowType);
                if (workFlowResult.Success > 0)
                {
                    //更改考勤表中的

                    var hrCheckAttendance = hrCheckAttendanceService.List().Where(w => gKeys.Contains((Guid)w.AttendanceId)).ToList();
                    bool isTrue = false, isTrueNo1 = false;
                    if (hrCheckAttendance.Count > 0)
                    {
                        for (int i = 0; i < hrCheckAttendance.Count; i++)
                            hrCheckAttendance[i].DoFlag = 0;//申报中
                        isTrue = hrCheckAttendanceService.UpdateByList(hrCheckAttendance);
                    }
                    var hrCheckAttendanceHistoryNo1 = hrCheckAttendanceHistoryNo1Service.List().Where(w => gKeys.Contains((Guid)w.HistoryId)).ToList();
                    if (hrCheckAttendanceHistoryNo1.Count > 0)
                    {
                        for (int i = 0; i < hrCheckAttendanceHistoryNo1.Count; i++)
                            hrCheckAttendanceHistoryNo1[i].DoFlag = 0;//申报中 
                        isTrueNo1 = hrCheckAttendanceHistoryNo1Service.UpdateByList(hrCheckAttendanceHistoryNo1);
                    }
                    if (!(isTrueNo1 || isTrue))
                        return Json(result);
                    model.WorkflowInstanceId = workFlowResult.WorkFlow.WorkflowInstanceId;
                    model.ApplySn = workFlowResult.WorkFlow.SheetId;
                    if (hrUnattendApplyService.Update(model))
                    {
                        result.IsSuccess = true;
                    }

                    //提交成功后抄送
                    if (!string.IsNullOrEmpty(nextCC))
                        CommonFunction.PendingData(workFlowResult.WorkFlow.WorkflowInstanceId, nextCC);//抄送
                }
                else
                    result.Message += workFlowResult?.Errmsg;

            }
            return Json(result);
        }
        #endregion

        #region 报表

        public ActionResult Statistics()
        {
            return View();
        }

        public ActionResult OnAndOffDuty(string DpId = null, int Type = 0)
        {
            ViewData["DpId"] = DpId;
            ViewData["Type"] = Type;
            return View();
        }
        public ActionResult AbnormalList(string DpId = null)
        {
            ViewData["DpId"] = DpId;
            return View();
        }
        public ActionResult HaveHolidaysByTurnsList(string DpId = null)
        {
            ViewData["DpId"] = DpId;
            return View();
        }
        public ActionResult HaveAHolidayList(string DpId = null)
        {
            ViewData["DpId"] = DpId;
            return View();
        }
        public ActionResult OtherList(string DpId = null)
        {
            ViewData["DpId"] = DpId;
            return View();
        }

        public ActionResult GetOnAndOffDuty(int pageIndex = 1, int pageSize = 5, HRCheckAttendanceQueryBuilder queryBuilder = null, int type = 0)
        {
            if (queryBuilder?.DpId?.Count > 0)
            {
                queryBuilder.DpId = new ChcekingIn().GetDpIdList(queryBuilder?.DpId[0]) as List<string>;
            }
            int count = 0;
            var listResult = hrCheckAttendanceService.GetOnAndOffDuty(out count, queryBuilder, pageIndex, pageSize, type);

            return Json(new { items = listResult, count = count });
        }
        public ActionResult GetHRAbnormalList(int pageIndex = 1, int pageSize = 5, HRCheckAttendanceQueryBuilder queryBuilder = null)
        {
            if (queryBuilder?.DpId?.Count > 0)
            {
                queryBuilder.DpId = new ChcekingIn().GetDpIdList(queryBuilder?.DpId[0]) as List<string>;
            }
            int count = 0;
            var listResult = v_HRAbnormalListService.GetForPaging(out count, queryBuilder, pageIndex, pageSize);

            return Json(new { items = listResult, count = count });
        }
        public ActionResult GetHaveHolidaysByTurnsList(int pageIndex = 1, int pageSize = 5, HRCheckAttendanceQueryBuilder queryBuilder = null)
        {
            if (queryBuilder?.DpId?.Count > 0)
            {
                queryBuilder.DpId = new ChcekingIn().GetDpIdList(queryBuilder?.DpId[0]) as List<string>;
            }
            int count = 0;
            var listResult = v_HRHaveHolidaysByTurnslListService.GetForPaging(out count, queryBuilder, pageIndex, pageSize);

            return Json(new { items = listResult, count = count });
        }

        public ActionResult GetHaveAHolidayList(int pageIndex = 1, int pageSize = 5, HRCheckAttendanceQueryBuilder queryBuilder = null)
        {
            if (queryBuilder?.DpId?.Count > 0)
            {
                queryBuilder.DpId = new ChcekingIn().GetDpIdList(queryBuilder?.DpId[0]) as List<string>;
            }
            int count = 0;
            var listResult = v_HRHaveAHolidayListService.GetForPaging(out count, queryBuilder, pageIndex, pageSize);

            return Json(new { items = listResult, count = count });
        }

        public ActionResult GetOtherList(int pageIndex = 1, int pageSize = 5, HRCheckAttendanceQueryBuilder queryBuilder = null)
        {
            if (queryBuilder?.DpId?.Count > 0)
            {
                queryBuilder.DpId = new ChcekingIn().GetDpIdList(queryBuilder?.DpId[0]) as List<string>;
            }
            int count = 0;
            var listResult = v_HROtherListService.GetForPaging(out count, queryBuilder, pageIndex, pageSize);

            return Json(new { items = listResult, count = count });
        }
        public ActionResult Summarizing()
        {
            ViewData["startId"] = WorkContext.CurrentUser.Dept.DpId;
            return View();
        }
        public ActionResult GetUserType()
        {
            var list = GetDictListByDDName(DataDic.UserType).Select(s => new { s.DDValue, s.DDText });
            return Json(list);
        }

        /// <summary>
        /// 统计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetStatistics()
        {
            //var level = GetLevel();
            //if (level < 0)
            //    return null;
            int count = 0;
            var listResult = hrCheckAttendanceService.GetStatistics(WorkContext.CurrentUser.UserId);
            return Json(new { items = listResult, count = count });
        }
        /// <summary>
        /// 导出汇总
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        [SysOperation(OperationType.Export, "导出考勤汇总")]
        public ActionResult Download(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<SummarizingQueryBuilder>(queryBuilder);
                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                List<Summarizing> headList = new List<Summarizing>();
                var modelResult = GetHeadModel(QueryBuilder);
                headList.Add(modelResult);
                var modelList = GetSummarizingList(out count, pageIndex, pageSize, QueryBuilder);
                var listResult = modelList.Select(s => s).ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Summarizing + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Summarizing);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Summarizing, headList);
                designer.SetDataSource(ImportFileType.SummarizingList, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        /// <summary>
        /// 导出考勤
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        [SysOperation(OperationType.Export, "导出考勤")]
        public ActionResult AttendanceDownload(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<HRCheckAttendanceQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = hrCheckAttendanceService.GetForPaging(out count, QueryBuilder, pageIndex, pageSize, false, true); // 
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Attendance + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Attendance);
                //设置集合变量 
                designer.SetDataSource(ImportFileType.Attendance, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Xlsx, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }
        /// <summary>
        /// 导出异常查询
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        [SysOperation(OperationType.Export, "导出异常查询")]
        public ActionResult AbnormalDownload(string queryBuilder = null)
        {


            //var listResult = v_HRCheckAttendanceAbnormalService.GetForPaging(out count, queryBuilder, pageIndex, pageSize).ToList();

            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<AttendanceListQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = v_HRCheckAttendanceAbnormalService.GetForPaging(out count, QueryBuilder, pageIndex, pageSize, true); // 
                var listResult = modelList.ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Abnormal + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Abnormal);
                //设置集合变量 
                designer.SetDataSource(ImportFileType.Abnormal, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Xlsx, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion

            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        /// <summary>
        /// 汇总列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSummarizing(int pageIndex = 1, int pageSize = 5, SummarizingQueryBuilder queryBuilder = null)
        {
            try
            {
                #region List
                int count = 0;
                var listResult = GetSummarizingList(out count, pageIndex, pageSize, queryBuilder);
                #endregion 
                return Json(new { items = listResult, count = count });
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
        public ActionResult GetHeader(int pageIndex = 1, int pageSize = 5, SummarizingQueryBuilder queryBuilder = null)
        {
            try
            {
                #region Model

                var modelResult = GetHeadModel(queryBuilder);

                #endregion

                return Json(new { model = modelResult });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 汇总-规定考勤天数
        /// </summary>
        /// <returns></returns>
        public ActionResult GetProvisionsOfAttendance(int pageIndex = 1, int pageSize = 5, SummarizingQueryBuilder queryBuilder = null)
        {
            try
            {
                #region Model 
                var modelResult = GetProvisionsOfAttendance(queryBuilder);
                #endregion

                return Json(new { model = modelResult });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion


        #region 方法

        List<SummarizingList> GetSummarizingList(out int count, int pageIndex = 1, int pageSize = 5, SummarizingQueryBuilder queryBuilder = null)
        {
            count = 0;
            #region List
            SqlParameter[] para = new SqlParameter[]{
                    new SqlParameter("@YEAR",System.Data.SqlDbType.Int),
                    new SqlParameter("@MONTH",System.Data.SqlDbType.Int),
                    new SqlParameter("@DpId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserType",System.Data.SqlDbType.Int),
                    new SqlParameter("@EmployeeId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@PageIndex",System.Data.SqlDbType.Int),
                    new SqlParameter("@PageSize",System.Data.SqlDbType.Int),
                    new SqlParameter("@count",System.Data.SqlDbType.Int),
                    new SqlParameter("@UserIdToQuery",System.Data.SqlDbType.UniqueIdentifier )
                };
            para[0].Value = queryBuilder.Year ?? 0;
            para[1].Value = queryBuilder.Month ?? 0;
            para[2].Value = queryBuilder.DeptIds ?? string.Empty;
            para[3].Value = queryBuilder.UserType ?? 0;
            para[4].Value = queryBuilder.EmployeeId ?? string.Empty;
            para[5].Value = queryBuilder.UserIds ?? string.Empty;
            para[6].Value = pageIndex;
            para[7].Value = pageSize;
            para[8].Value = 0;
            para[8].Direction = ParameterDirection.Output;
            para[9].Value = WorkContext.CurrentUser.UserId;
            var listResult = hrCheckAttendanceService.GetSummarizingList(para).ToList();

            #endregion
            count = (int)para[8].Value;
            return listResult;

        }
        Summarizing GetHeadModel(SummarizingQueryBuilder queryBuilder)
        {
            #region Model
            SqlParameter[] para = new SqlParameter[]{
                    new SqlParameter("@YEAR",System.Data.SqlDbType.Int),
                    new SqlParameter("@MONTH",System.Data.SqlDbType.Int),
                    new SqlParameter("@DpId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserType",System.Data.SqlDbType.Int),
                    new SqlParameter("@EmployeeId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserIdToQuery",System.Data.SqlDbType.UniqueIdentifier )
                };
            para[0].Value = queryBuilder.Year ?? 0;
            para[1].Value = queryBuilder.Month ?? 0;
            para[2].Value = queryBuilder.DeptIds ?? string.Empty;
            para[3].Value = queryBuilder.UserType ?? 0;
            para[4].Value = queryBuilder.EmployeeId ?? string.Empty;
            para[5].Value = queryBuilder.UserIds ?? string.Empty;
            para[6].Value = WorkContext.CurrentUser.UserId;
            return hrCheckAttendanceService.GetSummarizing(para).FirstOrDefault();

            #endregion
        }

        decimal GetProvisionsOfAttendance(SummarizingQueryBuilder queryBuilder)
        {
            #region Model
            SqlParameter[] para = new SqlParameter[]{
                    new SqlParameter("@YEAR",System.Data.SqlDbType.Int),
                    new SqlParameter("@MONTH",System.Data.SqlDbType.Int),
                    new SqlParameter("@DpId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserType",System.Data.SqlDbType.Int),
                    new SqlParameter("@EmployeeId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserId",System.Data.SqlDbType.NVarChar),
                    new SqlParameter("@UserIdToQuery",System.Data.SqlDbType.UniqueIdentifier )
                };
            para[0].Value = queryBuilder.Year ?? 0;
            para[1].Value = queryBuilder.Month ?? 0;
            para[2].Value = queryBuilder.DeptIds ?? string.Empty;
            para[3].Value = queryBuilder.UserType ?? 0;
            para[4].Value = queryBuilder.EmployeeId ?? string.Empty;
            para[5].Value = queryBuilder.UserIds ?? string.Empty;
            para[6].Value = WorkContext.CurrentUser.UserId;
            return   hrCheckAttendanceService.GetProvisionsOfAttendance(para).FirstOrDefault().Count;


            #endregion
        }
        string GetTimeQuantum(TimeSpan? doTime)
        {
            if (doTime == null)
                //   早晨：5:00-7:00上午：7:00-11:00中午：11:00-13:00下午：13:00-17:00傍晚：17:00-19:00上半夜：19:00-23:00（一更、二更）午夜：23:00-1:00（三更）下半夜：1:00-5:00（四更、五更）
                return "---";
            if (doTime >= new TimeSpan(1, 0, 0) && doTime < new TimeSpan(5, 0, 0))
                return "下半夜";//：1:00-5:00";
            if (doTime >= new TimeSpan(5, 0, 0) && doTime < new TimeSpan(7, 0, 0))
                return "早晨";//：5:00-7:00";
            if (doTime >= new TimeSpan(7, 0, 0) && doTime < new TimeSpan(11, 0, 0))
                return "上午";//：7:00-11:00";
            if (doTime >= new TimeSpan(11, 0, 0) && doTime < new TimeSpan(13, 0, 0))
                return "中午";//：11:00-13:00";
            if (doTime >= new TimeSpan(13, 0, 0) && doTime < new TimeSpan(17, 0, 0))
                return "下午";//：13:00-17:00";
            if (doTime >= new TimeSpan(17, 0, 0) && doTime < new TimeSpan(19, 0, 0))
                return "傍晚";//：17:00-19:00";
            if (doTime >= new TimeSpan(19, 0, 0) && doTime < new TimeSpan(23, 0, 0))
                return "上半夜";//：19:00-23:00";
            if (doTime >= new TimeSpan(23, 0, 0) && doTime < new TimeSpan(1, 0, 0))
                return "午夜";//：23:00-1:00";

            return "---";
        }

        bool IsTrue(DateTime? date, TimeSpan? time, int span, int minute, bool isDoTime = true)
        {
            var date_ = new DateTime();
            //检查时间date、time是否合法，日期是否为签到当天
            if (time == null || !DateTime.TryParse(date.ToString(), out date_) || date_.Date != DateTime.Now.Date)
                return false;
            var now = DateTime.Now;
            var nowTime = new TimeSpan(now.Hour, now.Minute, now.Second);//当前时间点
            double nowTimeDif = nowTime <= time ? time.Value.TotalMinutes - nowTime.TotalMinutes : nowTime.TotalMinutes - time.Value.TotalMinutes; //1、当前上(下)班时间和当前时间差在0到可提前上(下)班打卡时间之间属于正常  2、当当前时间大于上(下)班时间并且在可允许迟到(推迟)的时间范围内 

            //当前时间早于上班时间LeadTime 或者晚于上班时间span
            if (isDoTime)//上班 
                //1、当前上班时间和当前时间差在0到可提前上班打卡时间之间属于正常
                return nowTime <= time ?
                    (nowTimeDif >= 0 && nowTimeDif <= span)//可提前打卡范围
                     : (nowTimeDif > 0 && nowTimeDif <= minute);//可迟到的范围  
            //以下为下班部分 
            return nowTime <= time ?
                 (nowTimeDif >= 0 && nowTimeDif <= minute) //可提前打卡范围
                 : (nowTimeDif > 0 && nowTimeDif <= span);//可推迟的范围


        }

        int GetLevel()
        {
            var list = GetRight();
            if (list.Contains(Level.HR))
                return 1;
            if (list.Contains(Level.Department))
                return 2;
            if (list.Contains(Level.AdministrativeOffice))
                return 3;
            if (list.Contains(Level.ServiceDepartment))
                return 4;
            return -99;
        }
        List<string> GetRight()
        {
            var list = WorkContext.CurrentUser.UsersInRoles.Select(s => s.Roles.RoleName).ToList();
            return list;
        }

        #endregion
    }
}