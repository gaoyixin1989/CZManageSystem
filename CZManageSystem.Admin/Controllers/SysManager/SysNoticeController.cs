using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Admin;
using CZManageSystem.Service.SysManger;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Admin.Models;
using System.Collections;

namespace CZManageSystem.Admin.Controllers.SysManager
{
    public class SysNoticeController : BaseController
    {
        ISysNoticeService _noticeService = new SysNoticeService();
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            ViewData["id"] = id;
            if (id == null)
                ViewBag.Title = "公告新增";
            else
                ViewBag.Title = "公告编辑";
            return View();
        }
        public ActionResult ShowNotice(int? id)
        {
            //ViewData["id"] = id;
            ViewBag.Title = "公告";
            var obj = _noticeService.FindById(id);
            if (obj!=null) { 
                ViewBag.Title = obj.Title;
                ViewBag.Createdtime = obj.Createdtime;
                ViewBag.Creator = obj.Creator;
                ViewBag.Content = obj.Content;
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
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, NoticeQueryBuilder queryBuilder = null)
        {
            if (queryBuilder.Createdtime_Start != null)
                queryBuilder.Createdtime_Start = queryBuilder.Createdtime_Start.Value.Date;
            if (queryBuilder.Createdtime_End != null)
                queryBuilder.Createdtime_End = queryBuilder.Createdtime_End.Value.AddDays(1).Date.AddSeconds(-1);

            int count = 0;
            //var condition = queryBuilder.GetType().GetProperties();
            var modelList = _noticeService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : pageIndex, pageSize);

            return Json(new { items = modelList, count = count });
            //List<object> resultList = new List<object>();
            //foreach (var u in modelList)
            //{
            //    resultList.Add(new
            //    {
            //        u.NoticeId,
            //        u.Title,
            //        u.Content,
            //        ValidTime = u.ValidTime == null ? "" : u.ValidTime.ToString(),
            //        u.EnableFlag,
            //        u.OrderNo,
            //        Createdtime = u.Createdtime == null ? "" : u.Createdtime.ToString(),
            //        u.Creator
            //    });
            //}
            //return Json(new { items = resultList, count = count });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(int id)
        {
            var obj = _noticeService.FindById(id);
            return Json(new
            {
                obj.NoticeId,
                obj.Title,
                obj.Content,
                ValidTime = obj.ValidTime == null ? "" : obj.ValidTime.Value.ToString("yyyy-MM-dd"),
                obj.EnableFlag,
                obj.OrderNo,
                Createdtime = obj.Createdtime == null ? "" : obj.Createdtime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                obj.Creator
            });
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ActionResult Save(SysNotice dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "公告标题不能为空";
            else if (dataObj.ValidTime == null)
                tip = "有效日期不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "公告内容不能为空";
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

            if (dataObj.NoticeId == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.Createdtime = DateTime.Now;
                result.IsSuccess = _noticeService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _noticeService.Update(dataObj);
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
        public ActionResult Delete(int[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            //foreach (int id in ids)
            //{
            //    var obj = _noticeService.FindById(id);
            //    if (obj != null && obj.NoticeId != 0)
            //    {
            //        if (_noticeService.Delete(obj))
            //        {
            //            isSuccess = true;
            //            successCount++;
            //        }
            //    }
            //}

            var objs = _noticeService.List().Where(u => ids.Contains(u.NoticeId)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _noticeService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        public ActionResult SetTop(int[] ids, bool isTop)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            int maxOrderNo = 0;
            if (isTop)
            {
                maxOrderNo = _noticeService.List().Select(u => u.OrderNo ?? 0).Max();
                maxOrderNo++;
            }
            var objs = _noticeService.List().Where(u => ids.Contains(u.NoticeId)).ToList();
            foreach (var obj in objs)
            {
                obj.OrderNo = maxOrderNo;
            }
            if (objs.Count > 0)
            {
                _noticeService.UpdateByList(objs);
            }

            return Json(result);
        }

    }
}