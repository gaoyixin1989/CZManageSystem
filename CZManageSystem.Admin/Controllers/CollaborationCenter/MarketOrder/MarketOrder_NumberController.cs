using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-号码段维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_NumberController : BaseController
    {
        IMarketOrder_NumberService _numberService = new MarketOrder_NumberService();//号码段
        // GET: MarketOrder_Number
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? ID)
        {
            ViewData["ID"] = ID;
            return View();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="Number">号码段</param>
        /// <param name="Order">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string Number = null, decimal? Order = null)
        {
            int count = 0;
            object queryBuilder = new
            {
                Number = Number,
                Order = Order
            };
            var modelList = _numberService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Number)u).Select(u => new
            {
                u.ID,
                u.Number,
                u.Order
            }).ToList();

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var item = _numberService.FindById(id);
            object obj = new
            {
                item.ID,
                item.Number,
                item.Order
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_Number dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Number == null || string.IsNullOrEmpty(dataObj.Number.Trim()))
                tip = "号码段不能为空";
            else if (!dataObj.Order.HasValue)
                tip = "序号不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.Number = dataObj.Number.Trim();
            }
            
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == Guid.Empty)
            {//新增
                dataObj.ID = Guid.NewGuid();
                result.IsSuccess = _numberService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _numberService.Update(dataObj);
            }

            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;

            var objs = _numberService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _numberService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }

            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }
        

    }
}