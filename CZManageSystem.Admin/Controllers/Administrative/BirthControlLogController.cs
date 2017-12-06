using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Service.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class BirthControlLogController : BaseController
    {
        // GET: BirthControlLog
        IBirthControlLogService _birthcontrollogService = new BirthControlLogService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, BirthControlLogQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _birthcontrollogService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }
    }
}