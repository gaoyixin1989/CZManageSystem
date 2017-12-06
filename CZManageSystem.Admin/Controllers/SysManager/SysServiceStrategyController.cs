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
    public class SysServiceStrategyController : BaseController
    {
        ISysServiceStrategyService _sysServiceStrategyService = new SysServiceStrategyService();
        // GET: SysServiceStrategy
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "服务策略信息新增";
            else
                ViewBag.Title = "服务策略信息编辑";
            return View();
        }

        public ActionResult StrategyLog(int? id)
        {
            ViewData["id"] = id;
            ViewBag.Title = "服务策略日志-" + id;
            return View();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string ServiceName = null)
        {
            int count = 0;
            var modelList = _sysServiceStrategyService.QueryDataByServiceName(out count, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, ServiceName);

            List<object> resultList = new List<object>();
            foreach (var obj in modelList)
            {
                resultList.Add(new
                {
                    obj.Id,
                    obj.ServiceId,
                    ServiceName = obj.SysServices.ServiceName,
                    ValidTime = obj.ValidTime == null ? "" : obj.ValidTime.Value.ToString("yyyy-MM-dd"),
                    NextRunTime = obj.NextRunTime == null ? "" : obj.NextRunTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    obj.PeriodNum,
                    obj.PeriodType,
                    obj.EnableFlag,
                    obj.LogFlag,
                    obj.Remark
                });
            }

            return Json(new
            {
                items = resultList,
                count = count
            });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(int id)
        {
            var mm = Request;
            var obj = _sysServiceStrategyService.FindById(id);
            return Json(new
            {
                obj.Id,
                obj.ServiceId,
                ValidTime = obj.ValidTime == null ? "" : obj.ValidTime.Value.ToString("yyyy-MM-dd"),
                NextRunTime = obj.NextRunTime == null ? "" : obj.NextRunTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                obj.PeriodNum,
                obj.PeriodType,
                obj.EnableFlag,
                obj.LogFlag,
                obj.Remark,
                obj.Creator,
                obj.Createdtime,
                obj.LastModifier,
                obj.LastModTime
            });
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ActionResult Save(SysServiceStrategy dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.ServiceId == null)
                tip = "服务名称不能为空";
            else if (dataObj.ValidTime == null)
                tip = "有效时间不能为空";
            else if (dataObj.NextRunTime == null)
                tip = "下次运行时间不能为空";
            else if (dataObj.PeriodNum == null)
                tip = "周期不能为空";
            else if (dataObj.PeriodType == null || string.IsNullOrEmpty(dataObj.PeriodType.Trim()))
                tip = "周期类型不能为空";
            else
            {
                isValid = true;
                dataObj.PeriodType = dataObj.PeriodType.Trim();
            }

            //一个服务只能有一个策略
            var objs = _sysServiceStrategyService.List().Where(u => u.ServiceId == dataObj.ServiceId && u.Id != dataObj.Id);
            if (objs.Count() > 0)
            {
                isValid = false;
                tip = "该服务已经存在策略";
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.Id == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.Createdtime = DateTime.Now;
                result.IsSuccess = _sysServiceStrategyService.Insert(dataObj);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                if (dataObj.Remark == null) dataObj.Remark = "";
                result.IsSuccess = _sysServiceStrategyService.Update(dataObj);
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
            var objs = _sysServiceStrategyService.List().Where(u => ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _sysServiceStrategyService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        ///// <summary>
        ///// 获取数据列表
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">页数据量</param>
        ///// <param name="queryBuilder">查询条件</param>
        ///// <returns></returns>
        //public ActionResult GetStrategyLogData(int pageIndex = 1, int pageSize = 5, int StrategyId = 0, DateTime? StartTime = null, DateTime? EndTime = null)
        //{
        //    int count = 0;
        //    object condition = new
        //    {
        //        ServiceStrategyId = StrategyId,
        //        LogTime_start = StartTime.HasValue ? StartTime.Value.Date : StartTime,
        //        LogTime_end = EndTime.HasValue ? EndTime.Value.Date.AddDays(1).AddSeconds(-1) : EndTime
        //    };
        //    var modelList = new SysServiceStrategyLogService().GetForPaging(out count, condition, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
        //    List<object> resultList = new List<object>();
        //    //foreach (var obj in modelList)
        //    //{
        //    //    resultList.Add(new
        //    //    {
        //    //        obj.Id,
        //    //        obj.ServiceId,
        //    //        ServiceName = obj.SysServices.ServiceName,
        //    //        ValidTime = obj.ValidTime == null ? "" : obj.ValidTime.Value.ToString("yyyy-MM-dd"),
        //    //        NextRunTime = obj.NextRunTime == null ? "" : obj.NextRunTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
        //    //        obj.PeriodNum,
        //    //        obj.PeriodType,
        //    //        obj.EnableFlag,
        //    //        obj.LogFlag,
        //    //        obj.Remark
        //    //    });
        //    //}

        //    return Json(new
        //    {
        //        items = modelList,
        //        count = count
        //    });

        //}


    }
}