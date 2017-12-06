using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-满意度维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_SatController : BaseController
    {
        IMarketOrder_SatService _satService = new MarketOrder_SatService();//满意度维护
        // GET: MarketOrder_Sat
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
        /// <param name="Sat">满意度</param>
        /// <param name="Order">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string Sat = null, decimal? Order = null)
        {
            int count = 0;
            object queryBuilder = new
            {
                Sat = Sat,
                Order = Order
            };
            var modelList = _satService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Sat)u).Select(u => new
            {
                u.ID,
                u.Sat,
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
            var item = _satService.FindById(id);
            object obj = new
            {
                item.ID,
                item.Sat,
                item.Order
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_Sat dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Sat == null || string.IsNullOrEmpty(dataObj.Sat.Trim()))
                tip = "满意度不能为空";
            else if (!dataObj.Order.HasValue)
                tip = "序号不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.Sat = dataObj.Sat.Trim();
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
                result.IsSuccess = _satService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _satService.Update(dataObj);
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

            var objs = _satService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _satService.DeleteByList(objs);
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