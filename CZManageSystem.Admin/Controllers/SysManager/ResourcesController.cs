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
    public class noss
    {
        Resources notice { set; get; }
        string ParentId { set; get; }
    }
    public class ResourcesController : BaseController
    {
        IResourcesService _resourcesService = new ResourcesService();
        // GET: Resources
        public ActionResult Index()
        {
            return View();
        }
        public string Command
        {
            get;
            set;
        }
        public ActionResult Edit(string id)
        {
            ViewData["id"] = id;
            if (id == null)
            {
                ViewBag.Title = "资源新增";
                Command = "Insert";
            }
            else
            {

                ViewBag.Title = "资源编辑";
                Command = "Edit";
            }
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, ResourcesQueryBuilder queryBuilder = null)
        {
            int count = 0;
            //var condition = queryBuilder.GetType().GetProperties();
            var modelList = _resourcesService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });

        }
        public ActionResult GetDataByID(string id)
        {


            var dept = _resourcesService.FindById(id);
            var depts = _resourcesService.FindById(dept.ParentId);
            return Json(new { dept = dept, Alias = depts == null ? "" : depts.Alias });
        }


        public ActionResult GetAllSysDeptment()
        {

            int count = 0;

            var modelList = _resourcesService.List();// (out count,null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            return Json(new
            {
                items = modelList.Select(u => new { u.ResourceId, u.ParentId, u.Alias }),
                count = modelList.Count()
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Save(Resources dept,string type)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法

            #endregion

            if (type == "add")//新增
            {
                var objs = _resourcesService.List().Where(u => u.ResourceId == dept.ResourceId);
                if (objs != null && objs.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = "资源编号已经被占用";
                    return Json(result);
                }
                dept.CreatedTime = DateTime.Now;
                result.IsSuccess = _resourcesService.Insert(dept);
            }
            else
            {//编辑
             //dept.CreatedTime = DateTime.Now;
                result.IsSuccess = _resourcesService.Update(dept);
            }
            if (!result.IsSuccess)
                result.Message = "保存失败";
            
            return Json(result);
        }

        public ActionResult Delete(string[] ids)
        {
            foreach (var id in ids)
            {
                var obj = _resourcesService.FindById(id);
                if (obj != null && obj.ResourceId != null)
                    _resourcesService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }
    }
}