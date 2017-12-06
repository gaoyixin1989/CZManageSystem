using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Models;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class SysVersionController : BaseController
    {
        ISysVersionService _sysVersionService = new SysVersionService();
        // GET: SysVersion
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

        /// <summary>
        /// 版本信息展示
        /// </summary>
        public ActionResult Info()
        {
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, SysVersionQueryBuilder queryBuilder = null)
        {
            int count = 0;
            //var condition = queryBuilder.GetType().GetProperties();
            var modelList = _sysVersionService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }


        public ActionResult GetDataByID(int id)
        {

            
            var dept = _sysVersionService.FindById(id);
            return Json(dept);
        }

        public ActionResult Save(SysVersion notice)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法

            #endregion

            if (notice.VerId == 0)//新增
            {
                result.IsSuccess = _sysVersionService.Insert(notice);
            }
            else
            {//编辑
                result.IsSuccess = _sysVersionService.Update(notice);
            }
            return Json(result);
        }

        public ActionResult Delete(int[] ids)
        {
            foreach (int id in ids)
            {
                var obj = _sysVersionService.FindById(id);
                if (obj != null && obj.VerId != 0)
                    _sysVersionService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }
    }
}