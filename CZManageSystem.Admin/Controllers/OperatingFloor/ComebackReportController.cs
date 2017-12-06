using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.OperatingFloor.ComeBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.OperatingFloor
{
    public class ComebackReportController : BaseController
    {
        IComebackChildService _comebackChildService = new ComebackChildService();
        // GET: ComebackReport
        public ActionResult ComebackReport()
        {
            return View();
        }

        public ActionResult GetReportListData(int pageIndex = 1, int pageSize = 5, ComebackReporteQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var queryDatas = _comebackChildService.GetReport(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();


            return Json(new { items = queryDatas, count = count });

        }

    }
}