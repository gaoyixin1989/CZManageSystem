using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class OperateLogController : BaseController
    {
        IOperateLogService _operateLogService = new OperateLogService();
       // GET: OperateLog
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetListData(int pageIndex, int pageSize,string portalID, DateTime? Createdtime_Start,DateTime? Createdtime_End)
        {            
            return Json(_operateLogService.GetUserIdByroleId(pageIndex, pageSize, portalID, Createdtime_Start, Createdtime_End));

        }
    }
}