using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Service.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class BirthControlChildrenInfoController : BaseController
    {
        // GET: BirthControlChildrenInfo
        IBirthControlChildrenInfoService _birthcontrolchildreninfoService = new BirthControlChildrenInfoService();
        IBirthControlLogService _birthcontrollogService = new BirthControlLogService();
        BirthControlLog objLog = new BirthControlLog();
        public ActionResult Edit(int? id,string UserId)
        {
            //有值=》编辑状态
            ViewData["id"] = id;
            ViewData["UserId"] = UserId;
            return View();
        }

        public ActionResult GetDataByID(int id)
        {
            var obj = _birthcontrolchildreninfoService.FindById(id);
            return Json(new
            {
                obj.id,
                obj.UserId,
                obj.Name,
                obj.Treatment,
                obj.PolicyPostiton,
                obj.CISingleChildNum,
                obj.CISingleChildren,
                obj.Sex,
                Birthday = obj.Birthday == null ? "" : obj.Birthday.ToString("yyyy-MM-dd"),
                obj.remark
            });
        }
        public ActionResult GetChildrenDataByUserId(Guid id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var modelList = _birthcontrolchildreninfoService.GetAllChildrenList(id);
            result.data = modelList;
            return Json(new
            {
                data = _birthcontrolchildreninfoService.GetAllChildrenList(id)
            });
        }
        public ActionResult deleteChildren(int Id)
        {
            bool IsSuccess = false;
            int successCount = 0;
            var objs = _birthcontrolchildreninfoService.List().Where(u => u.id == Id).ToList();
            var Upguid = _birthcontrolchildreninfoService.FindById(Id).UserId.ToString();
            //var modelList = _birthcontrolchildreninfoService.GetAllChildrenList(new Guid(Upguid));
            if (objs.Count > 0)
            {
                IsSuccess = _birthcontrolchildreninfoService.DeleteByList(objs);
                successCount = IsSuccess ? objs.Count() : 0;
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "删除";
                objLog.Description = "删除" + CommonFunction.getUserRealNamesByIDs(objs[0].UserId.ToString()) + "的子女情况成功";
                _birthcontrollogService.Insert(objLog);
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                data = _birthcontrolchildreninfoService.GetAllChildrenList(new Guid(Upguid))
            });

        }
        public ActionResult Save(BirthControlChildrenInfo dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            dataObj.CreatedTime = DateTime.Now;
            dataObj.Creator = this.WorkContext.CurrentUser.UserName;
            if (dataObj.id == 0)//新增
            {
                result.IsSuccess = _birthcontrolchildreninfoService.Insert(dataObj);
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "新增";
                objLog.Description = "新增" + CommonFunction.getUserRealNamesByIDs(dataObj.UserId.ToString()) + "的子女情况成功";
                _birthcontrollogService.Insert(objLog);
            }
            else
            {//编辑
                result.IsSuccess = _birthcontrolchildreninfoService.Update(dataObj);
                objLog.UserId = this.WorkContext.CurrentUser.UserId;
                objLog.UserName = this.WorkContext.CurrentUser.RealName;
                objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                objLog.OpTime = DateTime.Now;
                objLog.OpType = "编辑";
                objLog.Description = "编辑" + CommonFunction.getUserRealNamesByIDs(dataObj.UserId.ToString()) + "的子女情况成功";
                _birthcontrollogService.Insert(objLog);
            }
            result.Message = tip;
            return Json(result);
        }
    }
}