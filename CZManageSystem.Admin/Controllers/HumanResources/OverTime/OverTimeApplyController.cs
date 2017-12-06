using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.OverTime;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.OverTime
{
    public class OverTimeApplyController : BaseController
    {
        // GET: OverTime

        ITracking_TodoService tracking_TodoService = new Tracking_TodoService();
        IOverTimeApplyService overtimeApplyService = new OverTimeApplyService();
        ISysDeptmentService sysDeptmentService = new SysDeptmentService();
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        [SysOperation(OperationType.Browse, "访问加班申请页面")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string id = null)
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.OverTimeApply;
            return View();
        }

        public ActionResult ForWF(string id = null, string type = "")
        {
            //有值=》编辑状态
            ViewData["Type"] = type;
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.OverTimeApply;
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, int type = 0, string ApplyTitle = null)
        {
            int count = 0;
            //var modelList = overtimeApplyService.GetForPaging(out count, new { ApplyTitle = ApplyTitle }, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            object obj = WorkContext.CurrentUser.UserName == Base.Admin.UserName ?
            (obj = new { ApplyTitle = ApplyTitle }) :
            (obj = new { ApplyTitle = ApplyTitle, Editor = this.WorkContext.CurrentUser.UserId });
            var modelList= overtimeApplyService.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, obj, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            var items = modelList;
            List<object> listResult = new List<object>();
            foreach (var u in modelList)
            {
                listResult.Add(new
                {
                    ApplyId = u.ApplyID,
                    u.ApplyTitle,
                    u.ApplySn,
                    u.ApplyUserName,
                    StartTime = Convert.ToDateTime(u.StartTime).ToString("yyyy-MM-dd HH:mm"),
                    EndTime = Convert.ToDateTime(u.EndTime).ToString("yyyy-MM-dd HH:mm"),
                    CreateTime = Convert.ToDateTime(u.CreateTime).ToString("yyyy-MM-dd"),
                    u.PeriodTime,
                    u.ApplyPost,
                    u.ManageName,
                    u.ManagePost,
                    u.Address,
                    u.OvertimeType,
                    u.Reason,
                    u.Editor,
                    u.Newpt,
                    u.WorkflowInstanceId,
                    u.TrackingWorkflow?.State,
                    ActorName = string.IsNullOrEmpty(u.WorkflowInstanceId?.ToString()) ? u.ApplyUserName : GetActorName(u.WorkflowInstanceId)
                });
            }
            return Json(new { items = listResult, count = count });
        }


        public ActionResult GetDataByID(string id)
        {
            try
            {
                bool IsAction = false;
                HROverTimeApply model;
                var Dept = sysDeptmentService.FindById(WorkContext.CurrentUser.DpId);
                if (!string.IsNullOrEmpty(id) && id != "0")
                {
                    model = overtimeApplyService.FindById(new Guid(id));
                    IsAction = !string.IsNullOrEmpty(model.WorkflowInstanceId?.ToString()) ? true : false;
                    if (model.Editor != WorkContext.CurrentUser.UserId)
                        IsAction = true;
                }                    
                else
                    model = new HROverTimeApply()
                    {
                        ApplyTitle = WorkContext.CurrentUser.RealName + "的加班申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")",
                        CreateTime = DateTime.Now,
                        ApplyUserName = WorkContext.CurrentUser.RealName,
                        Editor = WorkContext.CurrentUser.UserId
                    };
                #region Select

                var TypeList = GetDictListByDDName("加班类型");
                var result = new
                {
                    Mobile = WorkContext.CurrentUser.Mobile,
                    DeptName = Dept?.DpName,
                    model.ApplyTitle,
                    model.ApplyID,
                    model.ApplyUserName,
                    model.PeriodTime,
                    model.ApplyPost,
                    model.ManageName,
                    model.ManagePost,
                    model.Address,
                    model.OvertimeType,
                    model.Reason,
                    model.Editor,
                    model.ApplySn,
                    CreateTime = Convert.ToDateTime(model.CreateTime).ToString("yyyy-MM-dd"),
                    StartTime = model.StartTime == null ? "" : Convert.ToDateTime(model.StartTime).ToString("yyyy-MM-dd HH:mm"),
                    EndTime = model.EndTime == null ? "" : Convert.ToDateTime(model.EndTime).ToString("yyyy-MM-dd HH:mm"),
                    model.WorkflowInstanceId
                };
                #endregion
                return Json(new { Items = result, TypeList = TypeList, IsAction = IsAction });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [SysOperation(OperationType.Delete, "删除加班申请数据")]
        public ActionResult Delete(Guid[] ids)
        {
            List<HROverTimeApply> list = new List<HROverTimeApply>();
            foreach (Guid id in ids)
            {
                var _obj = overtimeApplyService.FindById(id);
                if(_obj.TrackingWorkflow != null)
                {
                    SystemResult result = new SystemResult() { IsSuccess = false, Message = "已提交，不能进行修改操作！" };
                    return Json(result);
                }
                list.Add(_obj);
            }
            if(overtimeApplyService.DeleteByList(list))
            {
                return Json(new { IsSuccess = true, Message = "删除成功" , SuccessCount = list.Count });
            }
            else
            {
                return Json(new { IsSuccess = false, Message = "删除失败" });
            }
            //var objs = overtimeApplyService.List().Where(u => ids.Contains(u.ApplyID)).ToList();
            //if (objs.Count > 0)
            //{
            //    if (overtimeApplyService.DeleteByList(objs))
            //    {
            //        return Json(new { status = 0, message = "删除成功" });
            //    }
            //    else
            //    {
            //        return Json(new { status = 0, message = "删除失败" });
            //    }
            //}
            //else
            //{
            //    return Json(new { status = 0, message = "没有可删除的数据" });
            //}
        }
        [SysOperation(OperationType.Save, "保存加班申请数据")]
        public ActionResult Save(HROverTimeApply apply, bool isAction = false, string nextActivity = "", string nextActors = "", string nextCC = null)
        {
            HROverTimeApply _apply;
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            string jiaban = "";
            if (DateTime.Compare(apply.StartTime.Value, DateTime.Now) < 0)
            {
                if(string.IsNullOrEmpty(apply.Reason)||apply.Reason=="")
                    tip = "先加班后申请必须填写原因。";
            }

            jiaban = ChkOverTime(WorkContext.CurrentUser.UserId.ToString(), apply.StartTime.Value, Convert.ToDouble(apply.PeriodTime));
            if(jiaban=="1")
                tip = "在排班和正常上班的时间里不能申请加班。";

            if(tip != "")
            {
                result.Message = tip;
                return Json(result);
            }
            if (apply.ApplyID.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                _apply = apply;
                if (DateTime.Compare(_apply.StartTime.Value, DateTime.Now) < 0)
                {
                    _apply.ApplyTitle = WorkContext.CurrentUser.RealName + "的加班补报申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")";
                }
                _apply.ApplyID = Guid.NewGuid();
                _apply.CreateTime = DateTime.Now;
                if (overtimeApplyService.Insert(_apply))
                    result.IsSuccess = true;
            }
            else
            {
                _apply = overtimeApplyService.FindById(apply.ApplyID);
                if (_apply == null)
                {
                    result.Message = "更新对象不存在！";
                    return Json(result);
                }
                if (DateTime.Compare(_apply.StartTime.Value, DateTime.Now) < 0)
                {
                    _apply.ApplyTitle = WorkContext.CurrentUser.RealName + "的加班补报申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")";
                }
                else
                    _apply.ApplyTitle = apply.ApplyTitle;
                _apply.ApplyUserName = apply.ApplyUserName;
                _apply.StartTime = apply.StartTime;
                _apply.PeriodTime = apply.PeriodTime;
                _apply.ApplyPost = apply.ApplyPost;
                _apply.ManageName = apply.ManageName;
                _apply.ManagePost = apply.ManagePost;
                _apply.Address = apply.Address;
                _apply.OvertimeType = apply.OvertimeType;
                _apply.Reason = apply.Reason;
                if (overtimeApplyService.Update(_apply))
                    result.IsSuccess = true;
            }
            if (isAction)
            {
                string workflowType = FlowInstance.WorkflowType.OverTimeApply;
                result.IsSuccess = false;
                Dictionary<string, string> dictField = new Dictionary<string, string>();
                dictField.Add("F1", _apply.ApplyID.ToString());
                var workFlowResult = WorkFlowHelper.ActionFlow(WorkContext.CurrentUser.UserName, _apply.ApplyTitle, dictField, nextActivity, nextActors, workflowType);
                if (workFlowResult.Success > 0)
                {
                    _apply.WorkflowInstanceId = workFlowResult.WorkFlow.WorkflowInstanceId;
                    _apply.ApplySn = workFlowResult.WorkFlow.SheetId;
                    if (overtimeApplyService.Update(_apply))
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

        #region Other Method



        public string GetActorName(Guid? workflowInstanceId)
        {
            var list = tracking_TodoService.List().Where(w => w.WorkflowInstanceId == workflowInstanceId).Select(s => s.ActorName);
            return string.Join(",", list);
        }
        /// <summary>
        /// 获取职务
        /// </summary>
        /// <param name="ManageId"></param>
        /// <returns></returns>
        public ActionResult getManagePost(string ManageId = null)
        {
            var modelList = new EfRepository<string>().Execute<string>(string.Format("select isnull (title,'')  from UUM_USERINFO  where employee in (select EmployeeId from bw_Users where UserId in ('{0}'))", new Guid(ManageId))).ToList()[0];
            return Json(new { Message = modelList });
        }
        /// <summary>
        /// 判断是否可以申请加班,在排班和正常上班的时间里不能申请加班
        /// 返回  0:可以 1:不可以
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="stime"></param>
        /// <param name="shour"></param>
        /// <returns></returns>
        public string ChkOverTime(string user_id, DateTime stime, double shour)
        {
            string jiaban = "0";
            string sql = "";
            string rtime = "";
            for (int i = 0; Convert.ToDateTime(stime).AddDays(i) <= Convert.ToDateTime(stime).AddHours(shour); i++)
            {
                rtime = Convert.ToDateTime(stime).AddDays(i).ToString("yyyy-MM-dd");
                string ryear = rtime.Substring(0, 4);
                string rmonth = rtime.Substring(5, 2);
                string rday = rtime.Substring(8, 2);
                //是否有排班
                sql = " select top 1 StartHour + ':' + StartMinute as fstart,EndHour + ':' + EndMinute as fend, Day" + rday + "  from dbo.ShiftRich  a  "
                   + " left join dbo.ShiftBanci b on a.BanciId=b.Id left join dbo.ShiftZhiban c on b.ZhibanId=c.Id "
                   + " where c.Year = '" + ryear + "' and c.Month = '" + rmonth + "' and Day" + rday + " like '%'+(select RealName  from bw_Users where UserId = '" + user_id + "')+'%' "
                   + " order by StartHour asc,StartMinute asc ";
                DataTable tb = SqlHelper.ExecuteDataTable(sql);
                if(tb.Rows.Count>0)
                {
                    jiaban = "1";
                    return jiaban;
                }
                //是否正常上班时间
                jiaban = GetWeek(rtime);                
                //是否节假日
                sql = " select * from HRHolidays where StartTime <= '" + rtime + "' and EndTime >= '" + rtime + "'";
                DataTable tbv = SqlHelper.ExecuteDataTable(sql);
                if (tbv.Rows.Count > 0)
                {
                    jiaban = "0";
                }                
                //是否有调休需要上班
                sql = " select * from HRHolidayWork  where StartTime <= '" + rtime + "' and EndTime >= '" + rtime + "'";
                DataTable tbhw = SqlHelper.ExecuteDataTable(sql);
                if (tbhw.Rows.Count > 0)
                {
                    jiaban = "1";
                }
                //是否轮休，是的话可以申请加班
                sql = "select id from ShiftLbuser where lunbanid in ( select a.id from ShiftLunban a left join ShiftZhiban b on a.zhibanid = b.id where b.year + '-' + b.month + '-' + a.StartDay <= '" + rtime + "' "
                   + " and b.year + '-' + b.month + '-' + a.EndDay >= '" + rtime + "') and UserId = '" + user_id + "' ";
                DataTable tblx = SqlHelper.ExecuteDataTable(sql);
                if (tblx.Rows.Count > 0)
                {
                    jiaban = "0";
                }
            }
            return jiaban;
        }

        public string GetWeek(string sDate)
        {
            string weekstr = "";
            try
            {
                System.DayOfWeek dw = Convert.ToDateTime(sDate).DayOfWeek;
                switch (dw.ToString("D"))
                {
                    case "0":
                    case "6":
                        weekstr = "0";
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        weekstr = "1";
                        break;                    
                }
            }
            catch { }
            return weekstr;
        }
        #endregion


    }
}