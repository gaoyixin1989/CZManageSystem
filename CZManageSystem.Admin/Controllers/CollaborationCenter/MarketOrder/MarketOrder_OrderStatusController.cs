using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-受理单状态维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_OrderStatusController : BaseController
    {
        IMarketOrder_OrderStatusService _statusService = new MarketOrder_OrderStatusService();//受理单状态
        // GET: MarketOrder_OrderStatus
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
        /// <param name="OrderStatus">受理单状态</param>
        /// <param name="Order">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string OrderStatus = null, int? Order = null)
        {
            int count = 0;
            object queryBuilder = new
            {
                OrderStatus = OrderStatus,
                Order = Order
            };
            var modelList = _statusService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_OrderStatus)u).Select(u => new
            {
                u.ID,
                u.OrderStatus,
                u.Order,
                u.Remark
            }).ToList();

            return Json(new { items = modelList, count = count });
        }

        public ActionResult GetAllData(string OrderStatus = null, int? Order = null)
        {
            int pageIndex = 0;
            int pageSize = int.MaxValue;
            int count = 0;
            object queryBuilder = new
            {
                OrderStatus = OrderStatus,
                Order = Order
            };
            var modelList = _statusService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_OrderStatus)u).Select(u => new
            {
                u.ID,
                u.OrderStatus,
                u.Order,
                u.Remark
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
            var item = _statusService.FindById(id);
            object obj = new
            {
                item.ID,
                item.OrderStatus,
                item.Order,
                item.Remark
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_OrderStatus dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.OrderStatus == null || string.IsNullOrEmpty(dataObj.OrderStatus.Trim()))
                tip = "受理单状态不能为空";
            else if (!dataObj.Order.HasValue)
                tip = "序号不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.OrderStatus = dataObj.OrderStatus.Trim();
                dataObj.Remark = dataObj.Remark.Trim();
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
                result.IsSuccess = _statusService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _statusService.Update(dataObj);
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

            var objs = _statusService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _statusService.DeleteByList(objs);
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