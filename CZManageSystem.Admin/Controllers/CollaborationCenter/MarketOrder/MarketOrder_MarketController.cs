using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-营销方案维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_MarketController : BaseController
    {
        IMarketOrder_MarketService _marketService = new MarketOrder_MarketService();//营销方案维护
        // GET: MarketOrder_Market
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Edit(Guid? ID)
        {
            ViewData["ID"] = ID;
            return View();
        }

        //选择营销方案
        public ActionResult SelectMarket()
        {
            return View();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="ProjectID">项目编号</param>
        /// <param name="Order">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, MarketOrder_MarketQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _marketService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Market)u).Select(u => new
            {
                u.ID,
                u.Market,
                u.Order,
                u.AbleTime,
                u.DisableTime,
                u.Remark,
                u.PlanPay,
                u.MustPay,
                u.isJK
            }).ToList();

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllData(MarketOrder_MarketQueryBuilder queryBuilder = null)
        {
            int pageIndex = 1;
            int pageSize = int.MaxValue;
            int count = 0;
            var modelList = _marketService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Market)u).Select(u => new
            {
                u.ID,
                u.Market,
                u.Order,
                u.AbleTime,
                u.DisableTime,
                u.Remark,
                u.PlanPay,
                u.MustPay,
                u.isJK
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
            var u = _marketService.FindById(id);
            object obj = new
            {
                u.ID,
                u.Market,
                u.Order,
                u.AbleTime,
                u.DisableTime,
                u.Remark,
                u.PlanPay,
                u.MustPay,
                u.isJK
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_Market dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Market == null || string.IsNullOrEmpty(dataObj.Market.Trim()))
                tip = "营销方案名称不能为空";
            if (dataObj.Order == null || string.IsNullOrEmpty(dataObj.Order.Trim()))
                tip = "营销方案编号不能为空";
            else if (!dataObj.AbleTime.HasValue)
                tip = "生效时间不能为空";
            else if (!dataObj.DisableTime.HasValue)
                tip = "失效时间不能为空";
            if (dataObj.Remark == null || string.IsNullOrEmpty(dataObj.Remark.Trim()))
                tip = "营销方案说明不能为空";
            else if (!dataObj.PlanPay.HasValue)
                tip = "优惠费用不能为空";
            else if (!dataObj.MustPay.HasValue)
                tip = "实收费用不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.Market = dataObj.Market.Trim();
                dataObj.Order = dataObj.Order.Trim();
                dataObj.Remark = dataObj.Remark.Trim();
                dataObj.isJK = dataObj.isJK ?? false;
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
                result.IsSuccess = _marketService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _marketService.Update(dataObj);
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

            var objs = _marketService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _marketService.DeleteByList(objs);
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