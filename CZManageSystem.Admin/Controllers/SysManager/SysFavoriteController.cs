using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class SysFavoriteController : BaseController
    {
        private static int state;
        ISysFavoriteService _sysFavoriteService = new SysFavoriteService();
        IWorkflowsService _workflowsService = new WorkflowsService();
        // GET: SysFavorite
        [SysOperation(OperationType.Browse, "访问流程收藏页面")]
        public ActionResult Index(int type)
        {
            ViewData["type"] = type;
            if (type == 0)
                ViewBag.Title = "常用流程";
            else if (type == 1)
                ViewBag.Title = "流程收藏";
            else
                ViewBag.Title = "Index";
            state = type;
            return View();

        }
        public ActionResult Edit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "流程收藏新增";
            else
                ViewBag.Title = "流程收藏编辑";
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string workflowName = null, int type = 0)
        {
            int count = 0;
            List<Guid> workflowList = new List<Guid>();

            var modelList = _workflowsService.GetList(
                out count,
                out workflowList,
                new { WorkflowName = workflowName, Enabled = true, IsCurrent = true, IsDeleted = false },
                pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize
                );
            var sysFavoriteList = (type == 0 ?
                _sysFavoriteService.List().Where(
                f => workflowList.Contains(f.WorkflowId) && f.Type == type && f.EnableFlag == true)
                :
                _sysFavoriteService.List().Where(
                f => workflowList.Contains(f.WorkflowId) && f.Type == type && f.EnableFlag == true && f.UserId == WorkContext.CurrentUser.UserId
                )).ToList();
            return Json(
                new
                {
                    items = modelList,
                    sysFavoriteList = sysFavoriteList,
                    count = count
                });
        }

        public ActionResult GetDataByID(int id)
        {
            var favortite = _sysFavoriteService.FindById(id);
            return Json(new
            {
                favortite.FavoriteId,
                favortite.WorkflowName,
                favortite.UserId,
                favortite.EnableFlag,
                favortite.OrderNo,
                favortite.Remark,
                favortite.Type
            });
        }
        [SysOperation(OperationType.Save, "保存流程收藏信息")]
        public ActionResult Save(List<Guid> workIDs, List<Guid> workflowIds)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            Expression<Func<SysFavorite, bool>> expression = f => f.Type == 0&& workflowIds.Contains (f.WorkflowId);
            if (state != 0)
                expression = f => f.Type == 1 && f.UserId == WorkContext.CurrentUser.UserId && workflowIds.Contains(f.WorkflowId);
            var favorList = _sysFavoriteService.List().Where(expression).ToList();//列表中已经存在的
            #region 验证数据是否合法
            if (workIDs == null)
            {
                foreach (var item in favorList)
                {
                    item.EnableFlag = false;
                    result.IsSuccess = _sysFavoriteService.Update(item);
                }
                return Json(new { result.IsSuccess, Type = state });
            }
            #endregion
            foreach (var item in workIDs)
            {
                expression = f => f.WorkflowId == item && f.Type == 0;
                if (state != 0)
                    expression = f => f.WorkflowId == item && f.Type == 1 && f.UserId == WorkContext.CurrentUser.UserId;
                var favor = _sysFavoriteService.FindByFeldName(expression);
                if (favor != null)//编辑
                {
                    favor.EnableFlag = true;
                    result.IsSuccess = _sysFavoriteService.Update(favor);
                }
                else
                {//新增
                    var work = _workflowsService.FindById(item);
                    result.IsSuccess = _sysFavoriteService.Insert(new SysFavorite()
                    {
                        UserId = WorkContext.CurrentUser.UserId,
                        WorkflowId = work.WorkflowId,
                        WorkflowName = work.WorkflowName,
                        EnableFlag = true,
                        Type = state
                    });
                }
            }
            foreach (var item in favorList.FindAll(u => !workIDs.Contains(u.WorkflowId)))
            {
                item.EnableFlag = false;
                result.IsSuccess = _sysFavoriteService.Update(item);
            }
            return Json(new { result.IsSuccess, Type = state });
        }
        [SysOperation(OperationType.Delete, "删除流程收藏页面")]
        public ActionResult Delete(int[] ids)
        {
            foreach (int id in ids)
            {
                var obj = _sysFavoriteService.FindById(id);
                if (obj != null && obj.FavoriteId != 0)
                    _sysFavoriteService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }
    }
}