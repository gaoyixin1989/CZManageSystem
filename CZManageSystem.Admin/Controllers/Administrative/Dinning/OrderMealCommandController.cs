using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Service.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative.Dinning
{
    public class OrderMealCommandController :  BaseController
    {
        // GET: OrderMeal_Command
        IOrderMeal_CommandService _ordermealcommandservice = new OrderMeal_CommandService();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult GetSelectListData(int pageIndex = 1, int pageSize = 5, OrderMeal_CommandQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ordermealcommandservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();            
            return Json(new { items = modelList, count = count });
        }
    }
}