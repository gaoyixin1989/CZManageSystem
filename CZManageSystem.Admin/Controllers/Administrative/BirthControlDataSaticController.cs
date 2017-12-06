using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Service.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class BirthControlDataSaticController : BaseController
    {
        // GET: BirthControlDataSatic
        IBirthControlStaticService _birthcontrolService = new BirthControlStaticService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListData(string DpId=null,string StartTime=null,string EndTime=null)
        {
            var modelList = _birthcontrolService.GetForPagingByCondition(this.WorkContext.CurrentUser, DpId, StartTime, EndTime).ToList()[0];
            return Json(new { items = modelList });
        }
    }
}