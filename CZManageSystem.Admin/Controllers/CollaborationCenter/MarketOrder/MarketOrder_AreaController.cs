using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-配送时限维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_AreaController : BaseController
    {
        IMarketOrder_AreaService _areaService = new MarketOrder_AreaService();//项目编号
        // GET: MarketOrder_Area
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5)
        {
            int count = 0;
            var modelList = _areaService.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Area)u).Select(u => new
            {
                u.ID,
                u.DpCode,
                u.DpName,
                u.LimitTime,
                u.Order
            }).ToList();

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllData()
        {
            int pageIndex = 1;
            int pageSize = int.MaxValue;
            int count = 0;
            var modelList = _areaService.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Area)u).Select(u => new
            {
                u.ID,
                u.DpCode,
                u.DpName,
                u.LimitTime,
                u.Order
            }).ToList();

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="listData"></param>
        /// <returns></returns>
        public ActionResult SaveList(List<MarketOrder_Area> listData)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };

            var list1 = listData.Where(u => u.ID != Guid.Empty).ToList();
            var list2 = listData.Where(u => u.ID == Guid.Empty).ToList();

            if(list1.Count>0)
            result.IsSuccess = _areaService.UpdateByList(list1);
            if (list2.Count > 0) {
                foreach (var item in list2)
                    item.ID = Guid.NewGuid();
                result.IsSuccess = _areaService.InsertByList(list2);
            }

            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

    }
}