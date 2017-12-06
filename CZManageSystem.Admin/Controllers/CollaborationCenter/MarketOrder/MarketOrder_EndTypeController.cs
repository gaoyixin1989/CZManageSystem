using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-终端机型维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_EndTypeController : BaseController
    {
        IMarketOrder_EndTypeService _endTypeService = new MarketOrder_EndTypeService();//终端机型维护
        IMarketOrder_MarketService _marketService = new MarketOrder_MarketService();//营销方案维护
        // GET: MarketOrder_EndType
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
        /// <param name="EndType">机型名称</param>
        /// <param name="Order">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string EndType = null, decimal? Order = null)
        {
            int count = 0;
            object queryBuilder = new
            {
                EndType = EndType,
                Order = Order,
            };
            var modelList = _endTypeService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_EndType)u).Select(u => new
            {
                u.ID,
                u.EndType,
                u.Order,
                u.Remark,
                u.MarketID,
                MarketText = _marketService.FindById(u.MarketID)?.Market
            }).ToList();

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllData(Guid? MarketID = null)
        {
            int pageIndex = 1;
            int pageSize = int.MaxValue;
            int count = 0;
            var query = new { MarketID = MarketID };
            var modelList = _endTypeService.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_EndType)u).Select(u => new
            {
                u.ID,
                u.EndType,
                u.Order,
                u.Remark,
                u.MarketID,
                MarketText = _marketService.FindById(u.MarketID)?.Market
            }).ToList();
            modelList = modelList.OrderBy(u => u.Order).ToList();
            return Json(new { items = modelList, count = count });
        }


        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var item = _endTypeService.FindById(id);
            object obj = new
            {
                item.ID,
                item.EndType,
                item.Order,
                item.Remark,
                item.MarketID,
                MarketText = _marketService.FindById(item.MarketID)?.Market
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_EndType dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.EndType == null || string.IsNullOrEmpty(dataObj.EndType.Trim()))
                tip = "机型名称不能为空";
            else if (!dataObj.Order.HasValue)
                tip = "序号不能为空";
            else if (!dataObj.MarketID.HasValue)
                tip = "所属方案不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.EndType = dataObj.EndType.Trim();
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
                result.IsSuccess = _endTypeService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _endTypeService.Update(dataObj);
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

            var objs = _endTypeService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _endTypeService.DeleteByList(objs);
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