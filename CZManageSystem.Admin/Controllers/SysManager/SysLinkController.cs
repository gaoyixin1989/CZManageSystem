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
    
    public class SysLinkController : BaseController
    {
        ISysLinkService _sysLinkService = new SysLinkService();
        // GET: SysLink
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Edit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "服务信息新增";
            else
                ViewBag.Title = "服务信息编辑";
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, SysLinkQueryBuilder queryBuilder = null)
        {
            int count = 0;
            //var condition = queryBuilder.GetType().GetProperties();
            var modelList = _sysLinkService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }

        public ActionResult GetDataByID(int id)
        {


            var dept = _sysLinkService.FindById(id);
            return Json(new
            {
                dept.LinkId,
                dept.LinkName,
                dept.LinkUrl,
                ValidTime = dept.ValidTime == null ? "" : dept.ValidTime.Value.ToString("yyyy-MM-dd"),
                dept.EnableFlag,
                dept.OrderNo,
                dept.Remark
            });
        }
        public ActionResult Save(SysLink notice)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法

            #endregion

            if (notice.LinkId == 0)//新增
            {
                result.IsSuccess = _sysLinkService.Insert(notice);
            }
            else
            {//编辑
                result.IsSuccess = _sysLinkService.Update(notice);
            }
            return Json(result);
        }
        public ActionResult Delete(int[] ids)
        {
            foreach (int id in ids)
            {
                var obj = _sysLinkService.FindById(id);
                if (obj != null && obj.LinkId != 0)
                    _sysLinkService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }

    }
}