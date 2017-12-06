using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{

    public class SysFavoriteLinkController : BaseController
    {
        ISysFavoriteLinkService _sysFavoriteLinkService = new SysFavoriteLinkService();
        // GET: SysFavoriteLink
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Edit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "新增收藏链接";
            else
                ViewBag.Title = "编辑收藏链接";
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string favoriteLinkName = null)
        {
            int count = 0;
            //var condition = queryBuilder.GetType().GetProperties();
            var modelList = _sysFavoriteLinkService.GetForPaging(out count, new { FavoriteLinkName = favoriteLinkName, UserId = WorkContext.CurrentUser.UserId }, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }

        public ActionResult GetDataByID(int id)
        {


            var sysFavoriteLink = _sysFavoriteLinkService.FindById(id);
            return Json(new
            {
                sysFavoriteLink.FavoriteLinkId,
                sysFavoriteLink.UserId,
                sysFavoriteLink.FavoriteLinkName,
                sysFavoriteLink.FavoriteLinkUrl,
                ValidTime = sysFavoriteLink.CreateTime == null ? "" : sysFavoriteLink.CreateTime.Value.ToString("yyyy-MM-dd"),
                sysFavoriteLink.EnableFlag,
                sysFavoriteLink.OrderNo,
                sysFavoriteLink.CreateTime,
                sysFavoriteLink.Remark
            });
        }
        public ActionResult Save(SysFavoriteLink notice)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法

            #endregion

            if (notice.FavoriteLinkId == 0)//新增
            {
                notice.UserId = WorkContext.CurrentUser.UserId;
                notice.CreateTime = DateTime.Now;
                result.IsSuccess = _sysFavoriteLinkService.Insert(notice);
            }
            else
            {//编辑
                result.IsSuccess = _sysFavoriteLinkService.Update(notice);
            }
            return Json(result);
        }
        public ActionResult Delete(int[] ids)
        {
            foreach (int id in ids)
            {
                var obj = _sysFavoriteLinkService.FindById(id);
                if (obj != null && obj.FavoriteLinkId != 0)
                    _sysFavoriteLinkService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }

    }
}