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
    public class SysServicesController : BaseController
    {
        ISysServicesService _sysServicesService = new SysServicesService();
        // GET: SysServices
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
                ViewBag.Title = "服务信息新增";
            else
                ViewBag.Title = "服务信息编辑";
            return View();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string ServiceName = null)
        {
            int count = 0;
            var modelList = _sysServicesService.GetForPaging(out count, new { ServiceName = ServiceName }, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var obj in modelList)
            {
                listResult.Add(new
                {
                    obj.ServiceId,
                    obj.ServiceName,
                    obj.AssemblyName,
                    obj.ClassName,
                    obj.ServiceDesc,
                    obj.Remark,
                    obj.Creator,
                    obj.Createdtime,
                    obj.LastModifier,
                    obj.LastModTime
                });
            }
            return Json(new { items = modelList, count = count });

        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(int id)
        {
            var obj = _sysServicesService.FindById(id);
            return Json(new
            {
                obj.ServiceId,
                obj.ServiceName,
                obj.AssemblyName,
                obj.ClassName,
                obj.ServiceDesc,
                obj.Remark,
                obj.Creator,
                obj.Createdtime,
                obj.LastModifier,
                obj.LastModTime
            }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ActionResult Save(SysServices dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.ServiceName == null || string.IsNullOrEmpty(dataObj.ServiceName.Trim()))
                tip = "服务名称不能为空";
            else if (dataObj.AssemblyName == null || string.IsNullOrEmpty(dataObj.AssemblyName.Trim()))
                tip = "程序集不能为空";
            else if (dataObj.ClassName == null || string.IsNullOrEmpty(dataObj.ClassName.Trim()))
                tip = "类名不能为空";
            else
            {
                isValid = true;
                dataObj.ServiceName = dataObj.ServiceName.Trim();
                dataObj.AssemblyName = dataObj.AssemblyName.Trim();
                dataObj.ClassName = dataObj.ClassName.Trim();
            }

            //服务名称不能重复
            var objs = _sysServicesService.List().Where(u => u.ServiceName == dataObj.ServiceName && u.ServiceId != dataObj.ServiceId);
            if (objs.Count() > 0)
            {
                isValid = false;
                tip = "服务名称已经被使用";
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ServiceId == 0)//新增
            {
                dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                dataObj.Createdtime = DateTime.Now;
                result.IsSuccess = _sysServicesService.Insert(dataObj);
            }
            else
            {//编辑
                dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                dataObj.LastModTime = DateTime.Now;
                if (dataObj.ServiceDesc == null) dataObj.ServiceDesc = "";
                if (dataObj.Remark == null) dataObj.Remark = "";
                result.IsSuccess = _sysServicesService.Update(dataObj);
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
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";
            var sysServicesList = _sysServicesService.List().Where(u => ids.Contains(u.ServiceId)).ToList();//服务信息
            allCount = sysServicesList.Count;
            sysServicesList = sysServicesList.Where(u => u.SysServiceStrategy.Count == 0).ToList();
            if (sysServicesList.Count > 0)
            {
                isSuccess = _sysServicesService.DeleteByList(sysServicesList);
                successCount = isSuccess ? sysServicesList.Count() : 0;
            }
            else if (allCount > 0)
            {
                isSuccess = false;
                strMsg = "存在对应的服务策略信息，请先删除对应的服务策略信息";
            }
            else
            {
                isSuccess = false;
                strMsg = "删除失败";
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });
        }

        public ActionResult GetServiceNameAsDict()
        {
            var objList = _sysServicesService.List().Select(u => new { u.ServiceId, u.ServiceName }).ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }

    }
}