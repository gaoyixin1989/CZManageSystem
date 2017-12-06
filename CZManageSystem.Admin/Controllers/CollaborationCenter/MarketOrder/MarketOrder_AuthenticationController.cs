using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-鉴权方式维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_AuthenticationController : BaseController
    {
        IMarketOrder_AuthenticationService _authenticationService = new MarketOrder_AuthenticationService();//鉴权方式
        // GET: MarketOrder_Authentication
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
        /// <param name="Authentication">鉴权方式</param>
        /// <param name="Order">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string Authentication = null, decimal? Order = null)
        {
            int count = 0;
            object queryBuilder = new
            {
                Authentication = Authentication,
                Order = Order
            };
            var modelList = _authenticationService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Authentication)u).Select(u => new
            {
                u.ID,
                u.Authentication,
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
            var item = _authenticationService.FindById(id);
            object obj = new
            {
                item.ID,
                item.Authentication,
                item.Order
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_Authentication dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Authentication == null || string.IsNullOrEmpty(dataObj.Authentication.Trim()))
                tip = "鉴权方式不能为空";
            else if (!dataObj.Order.HasValue)
                tip = "序号不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.Authentication = dataObj.Authentication.Trim();
            }

            if (IsUsed_Authentication(dataObj))
            {
                isValid = false;
                tip = "鉴权方式已存在，不能重复";
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
                result.IsSuccess = _authenticationService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _authenticationService.Update(dataObj);
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

            var objs = _authenticationService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _authenticationService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }

            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 检查鉴权方式是否已经被使用
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        private bool IsUsed_Authentication(MarketOrder_Authentication dataObj)
        {
            var temp = _authenticationService.List().Where(u => u.Authentication == dataObj.Authentication && u.ID != dataObj.ID).Count();
            if (temp > 0)
                return true;
            else
                return false;
        }

    }
}