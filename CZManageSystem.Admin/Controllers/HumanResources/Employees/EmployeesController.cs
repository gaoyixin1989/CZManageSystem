using CZManageSystem.Admin.Models;
using CZManageSystem.Service.HumanResources.Employees;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.Employees
{
    public class EmployeesController : Controller
    {

        IVWHRSumService _vWHRSumService = new VWHRSumService();

        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult EmployeesGetListData(int pageIndex = 1, int pageSize = 5, EmployeesQueryBuilder queryBuilder = null)
        {
            try
            {
                int count = 0;
                var modelList = _vWHRSumService.GetListForPaging(out count, queryBuilder.employerid, queryBuilder.name, queryBuilder.DpId, queryBuilder.billcyc_start, queryBuilder.billcyc_end, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

                return Json(new { items = modelList, count = count });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GetBillcyc()
        {
            try
            {
                var dateList = (from c in _vWHRSumService.List()
                                orderby c.billcyc
                                select c.billcyc)
                                .Distinct().ToList().Select(s => _vWHRSumService.GetBillcyc(s));
                return Json(dateList);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}