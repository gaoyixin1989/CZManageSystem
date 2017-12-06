using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.ReturnPremium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.ReturnPremium
{
    public class ReturnsReasonController : BaseController
    {
        IReturnsReasonService _returnsReasonService = new ReturnsReasonService();
        // GET: ReturnsReason
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid? ID, string type)
        {
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            var queryDatas = _returnsReasonService.GetForPaging(out count, null, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();


            return Json(new { items = queryDatas, count = count });

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _returnsReasonService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _returnsReasonService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult GetByID(Guid ID)
        {
            var list = _returnsReasonService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.Order,
                list.Reason
            });
        }
        public ActionResult Save(ReturnsReason curObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";

            if (curObj.ID == null || curObj.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                curObj.ID = Guid.NewGuid();
                result.IsSuccess = _returnsReasonService.Insert(curObj);
            }
            else
            {//编辑
                result.IsSuccess = _returnsReasonService.Update(curObj);
            }

            result.Message = tip;
            return Json(result);
        }
    }
}