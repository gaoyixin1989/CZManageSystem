using CZManageSystem.Service.Administrative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin.Models;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarDispatchController : Controller
    {
        ICarApplyService _carApplyService = new CarApplyService();
        // GET: CarDispatch
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(string id = null, string type = null, string change = "")
        {
            //有值=》编辑状态
            ViewData["id"] = !string.IsNullOrEmpty(id) ? id : "0";
            ViewData["change"] = change;
            ViewData["workflowName"] = Base.FlowInstance.WorkflowType.UsedCarsApply;
            ViewData["type"] = type;
            return View();

        }

        public ActionResult CarDispatchGetListData(int pageIndex = 1, int pageSize = 5,CarDispatchQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _carApplyService.GetCarEvaluation(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            var items = modelList;
            return Json(new { items = items, count = count });
        }
    }
}