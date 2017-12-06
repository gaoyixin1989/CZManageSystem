using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-项目编号维护
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_ProjectController : BaseController
    {
        IMarketOrder_ProjectService _projectService = new MarketOrder_ProjectService();//项目编号
        // GET: MarketOrder_Project
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
        /// <param name="ProjectID">项目编号</param>
        /// <param name="Order">序号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string ProjectID = null, decimal? Order = null)
        {
            int count = 0;
            object queryBuilder = new
            {
                ProjectID = ProjectID,
                Order = Order
            };
            var modelList = _projectService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Project)u).Select(u => new
            {
                u.ID,
                u.ProjectID,
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
            var modelList = _projectService.GetForPaging(out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (MarketOrder_Project)u).Select(u => new
            {
                u.ID,
                u.ProjectID,
                u.Order
            }).OrderBy(u=>u.Order).ToList();

            return Json(new { items = modelList, count = count });
        }


        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var item = _projectService.FindById(id);
            object obj = new
            {
                item.ID,
                item.ProjectID,
                item.Order
            };
            return Json(obj);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save(MarketOrder_Project dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.ProjectID == null || string.IsNullOrEmpty(dataObj.ProjectID.Trim()))
                tip = "项目编号不能为空";
            else if (!dataObj.Order.HasValue)
                tip = "序号不能为空";

            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                dataObj.ProjectID = dataObj.ProjectID.Trim();
            }

            if (IsUsed_ProjectID(dataObj))
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
                result.IsSuccess = _projectService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _projectService.Update(dataObj);
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

            var objs = _projectService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _projectService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }

            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 检查项目编号是否已经被使用
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        private bool IsUsed_ProjectID(MarketOrder_Project dataObj)
        {
            var temp = _projectService.List().Where(u => u.ProjectID == dataObj.ProjectID && u.ID != dataObj.ID).Count();
            if (temp >0)
                return true;
            else
                return false;
        }


    }
}