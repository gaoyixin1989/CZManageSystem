using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-商品维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_ProductController : BaseController
    {
        IMarketOrder_ProductService _productService = new MarketOrder_ProductService();//商品维护
        IMarketOrder_EndTypeService _endTypeService = new MarketOrder_EndTypeService();//终端机型维护
        // GET: MarketOrder_Product
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
        /// <param name="ProductID">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string ProductID = null)
        {
            int count = 0;
            object queryBuilder = new
            {
                ProductID = ProductID
            };
            var modelList = _productService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Product)u).Select(u => new
            {
                u.ID,
                u.ProductID,
                u.ProductName,
                u.ProductTypeID,
                ProductTypeID_Text=_endTypeService.FindById(u.ProductTypeID)?.EndType,
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
            var u = _productService.FindById(id);
            object obj = new
            {
                u.ID,
                u.ProductID,
                u.ProductName,
                u.ProductTypeID,
                ProductTypeID_Text = _endTypeService.FindById(u.ProductTypeID)?.EndType,
                u.Remark
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_Product dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.ProductID == null || string.IsNullOrEmpty(dataObj.ProductID.Trim()))
                tip = "商品序号不能为空";
            else if (!dataObj.ProductTypeID.HasValue)
                tip = "商品机型不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.ProductID = dataObj.ProductID.Trim();
            }

            if (IsUsed_ProductID(dataObj))
            {
                isValid = false;
                tip = "项目编号已存在，不能重复";
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
                result.IsSuccess = _productService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _productService.Update(dataObj);
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

            var objs = _productService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _productService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }

            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 商品编号是否已经被使用
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        private bool IsUsed_ProductID(MarketOrder_Product dataObj)
        {
            var temp = _productService.List().Where(u => u.ProductID == dataObj.ProductID && u.ID != dataObj.ID).Count();
            if (temp >0)
                return true;
            else
                return false;
        }


    }
}