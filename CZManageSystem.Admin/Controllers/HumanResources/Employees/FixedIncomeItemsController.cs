using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using CZManageSystem.Service.HumanResources.Employees;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.Employees
{
    public class FixedIncomeItemsController : BaseController
    {
        // GET: FixedIncome 
        IGdPayService gdPayService = new GdPayService();
        IGdPayIdService gdPayIdService = new GdPayIdService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(int id = 0)
        {
            ViewData["id"] = id;
            return View();
        }
        public ActionResult GetDataByID(int id)
        {
            var model = gdPayIdService.FindByFeldName(f => f.payid == id);

            return Json(new
            {
                model.payid,
                model.payname,
                model.pid,
                model.bz,
                model.sort,
                model.RowExclusive,
                model.Inherit,
                model.DataType
            });
        }



        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string payName = null)
        {
            int count = 0;
            var modelList = gdPayIdService.GetForPaging_(out count, new { payname = payName }, pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }
        public ActionResult Delete(int[] ids)
        {
            try
            {
                SystemResult result = new SystemResult() { IsSuccess = false };
                if (gdPayService.Contains(c => ids.Contains(c.payid)))
                {
                    result.Message = "选择的记录中存在固定收入明细，只能删除没有固定收入明细的记录！";
                    return Json(result);
                }
                var list = gdPayIdService.List().Where(f => ids.Contains(f.payid));
                var models = list.ToList();
                result.data = new { successCount = models.Count };
                if (models.Count <= 0)
                {
                    result.Message = "该记录不存在！";
                    return Json(result);
                }
                if (gdPayIdService.DeleteByList(models))
                    result.IsSuccess = true;
                return Json(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult Save(GdPayId gdPayId)
        {
            try
            {
                SystemResult result = new SystemResult() { IsSuccess = false };
                if (gdPayId == null)
                {
                    result.Message = "保存对象对Null！";
                    return Json(result);
                }
                if (!(gdPayId.payid > 0))
                {
                    var Contains = gdPayIdService.Contains(c => c.payname == gdPayId.payname);
                    if (Contains)
                    {
                        result.Message = "保存 " + gdPayId.payname + " 失败，已存在相同名称。";
                        return Json(result);
                    }
                    if (gdPayIdService.Insert(gdPayId))
                    {
                        result.IsSuccess = true;
                        return Json(result);
                    }
                }

                var model = gdPayIdService.FindById(gdPayId.payid);
                if (model==null)
                {
                    result.Message = "编辑对象不存在！";
                    return Json(result);
                }
                model.bz = gdPayId.bz;
                model.DataType = gdPayId.DataType;
                model.payname = gdPayId.payname;
                model.pid = gdPayId.pid;
                model.sort = gdPayId.sort;
                model.RowExclusive = gdPayId.RowExclusive;
                model.Inherit = gdPayId.Inherit;
                if (gdPayIdService.Update(model))
                    result.IsSuccess = true;

                return Json(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public ActionResult GetIncomeTypeP()
        {

            var modelList = gdPayIdService.List().Where(w => w.pid == 0).Select(
                s => new
                {
                    PayId = s.payid,
                    Pid = s.pid,
                    PayName = s.payname
                }
                ).ToList();

            return Json(modelList);
        }


    }
}