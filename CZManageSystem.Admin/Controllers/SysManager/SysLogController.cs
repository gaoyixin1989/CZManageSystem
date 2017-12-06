using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Service.SysManger;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class SysLogController: BaseController
    {
       
        public ActionResult ErrorLog()
        {
            return View();
        }
        public ActionResult OperationLog()
        {
            return View();
        }
        public ActionResult GetOperationLogList(int pageIndex, int pageSize, string OperationType, string UserName, string startTime, string endTime)
        {
            Int64 count = 0;
           
            var modelList = _sysLogService.GetOperationLogList(out count, OperationType,UserName, startTime,endTime,pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }
        public ActionResult GetErrorLogList(int pageIndex, int pageSize, string UserName, string startTime, string endTime)
        {
            Int64 count = 0;

            var modelList = _sysLogService.GetErrorLogList(out count, UserName, startTime, endTime, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }
        /// <summary>
        /// 获取服务策略日志
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="ServiceStrategyId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public ActionResult GetServiceStrategyLogList(int pageIndex, int pageSize, string ServiceStrategyId, string startTime, string endTime)
        {
            Int64 count = 0;

            var modelList = _sysLogService.GetServiceStrategyLogList(out count, (string.IsNullOrEmpty(ServiceStrategyId)?0:Convert.ToInt32(ServiceStrategyId)), startTime, endTime, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }
    }
}