using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin.Models;
using System.Collections;
using CZManageSystem.Service.HumanResources.Knowledge;
using CZManageSystem.Data.Domain.HumanResources.Knowledge;
using CZManageSystem.Service.Composite;

/// <summary>
/// 知识普及
/// </summary>
namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class SysKnowledgeController : BaseController
    {
        ISysKnowledgeService _knowledgeService = new SysKnowledgeService();
        IAdmin_AttachmentService _attchmentService = new Admin_AttachmentService();//附件
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string type)
        {
            ViewData["type"] = type;
            return View();
        }

        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid? id)
        {
            ViewData["id"] = id;
            ViewData["newID"] = Guid.NewGuid();
            if (id == null)
                ViewBag.Title = "知识普及新增";
            else
                ViewBag.Title = "知识普及编辑";
            return View();
        }
        public ActionResult ShowKnowledge(Guid? id)
        {
            ViewData["id"] = id;
            ViewBag.Title = "知识普及";
            var obj = _knowledgeService.FindById(id);
            if (obj != null)
            {
                ViewBag.Title = obj.Title;
            }
            return View();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, KnowledgeQueryBuilder queryBuilder = null)
        {
            if (queryBuilder.Createdtime_Start != null)
                queryBuilder.Createdtime_Start = queryBuilder.Createdtime_Start.Value.Date;
            if (queryBuilder.Createdtime_End != null)
                queryBuilder.Createdtime_End = queryBuilder.Createdtime_End.Value.AddDays(1).Date.AddSeconds(-1);

            int count = 0;
            var modelList = _knowledgeService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize).Select(u => (SysKnowledge)u).Select(u => new
            {
                u.ID,
                u.Title,
                u.Content,
                u.Createdtime,
                u.CreatorObj.RealName,
                u.OrderNo
            });

            return Json(new { items = modelList, count = count });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var obj = _knowledgeService.FindById(id);
            return Json(new
            {
                obj.ID,
                obj.Title,
                obj.Content,
                obj.OrderNo,
                Createdtime = obj.Createdtime == null ? "" : obj.Createdtime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                obj.CreatorID,
                obj.CreatorObj.RealName,
                Attachments = _attchmentService.GetAllAttachmentList(Guid.Parse(obj.ID.ToString()))
            });
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ActionResult Save(SysKnowledge dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "内容不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
                dataObj.Content = dataObj.Content.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == null || dataObj.ID == Guid.Empty || _knowledgeService.List().Where(u => u.ID == dataObj.ID).Count() <= 0)//新增
            {
                if (dataObj.ID == null || dataObj.ID == Guid.Empty)
                    dataObj.ID = Guid.NewGuid();
                dataObj.CreatorID = this.WorkContext.CurrentUser.UserId;
                dataObj.Createdtime = DateTime.Now;
                dataObj.OrderNo = 0;
                result.IsSuccess = _knowledgeService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _knowledgeService.Update(dataObj);
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

            var objs = _knowledgeService.List().Where(u => ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _knowledgeService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 置顶和取消置顶
        /// </summary>
        /// <param name="ids">数据id</param>
        /// <param name="isTop">是否置顶</param>
        /// <returns></returns>
        public ActionResult SetTop(Guid[] ids, bool isTop)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            int maxOrderNo = 0;
            if (isTop)
            {
                maxOrderNo = _knowledgeService.List().Select(u => u.OrderNo ?? 0).Max();
                maxOrderNo++;
            }
            var objs = _knowledgeService.List().Where(u => ids.Contains(u.ID)).ToList();
            foreach (var obj in objs)
            {
                obj.OrderNo = maxOrderNo;
            }
            if (objs.Count > 0)
            {
                _knowledgeService.UpdateByList(objs);
            }

            return Json(result);
        }

    }
}