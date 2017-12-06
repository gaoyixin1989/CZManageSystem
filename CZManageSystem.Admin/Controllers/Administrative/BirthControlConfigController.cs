using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Service.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class BirthControlConfigController : BaseController
    {
        // GET: BirthControlConfig
        IBirthControlConfigService _birthcontrolconfigService = new BirthControlConfigService();
        IBirthControlLogService _birthcontrollogService = new BirthControlLogService();
        BirthControlLog objLog = new BirthControlLog();
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult GetDataByID()
        {
            var obj = _birthcontrolconfigService.FindById(1);
            object birthcontrolconfig = null;
            if(obj!=null)
            {
                birthcontrolconfig = new
                {
                    obj.id,
                    ConfirmStartdate = obj.ConfirmStartdate == null ? "" : obj.ConfirmStartdate.Value.ToString("yyyy-MM-dd"),
                    ConfirmEnddate = obj.ConfirmEnddate == null ? "" : obj.ConfirmEnddate.Value.ToString("yyyy-MM-dd"),
                    obj.ManAge,
                    obj.WomenAge
                };
                return Json(new
                {
                    obj.id,
                    ConfirmStartdate = obj.ConfirmStartdate == null ? "" : obj.ConfirmStartdate.Value.ToString("yyyy-MM-dd"),
                    ConfirmEnddate = obj.ConfirmEnddate == null ? "" : obj.ConfirmEnddate.Value.ToString("yyyy-MM-dd"),
                    obj.ManAge,
                    obj.WomenAge
                });
            }
            else
            {
                birthcontrolconfig = new
                {
                    id="",
                    ConfirmStartdate = "",
                    ConfirmEnddate =  "",
                    ManAge = "",
                    WomenAge = ""
                };
                return Json(new
                {
                    id = "",
                    ConfirmStartdate = "",
                    ConfirmEnddate = "",
                    ManAge = "",
                    WomenAge = ""
                });
            }
            
        }

        public ActionResult Save(BirthControlConfig dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            bool isValid = false;//是否验证通过  
            #region 验证数据是否合法
            if (dataObj.ConfirmEnddate == null && dataObj.ConfirmStartdate == null && dataObj.ManAge == null && dataObj.WomenAge == null)
            {
                SqlHelper.ExecuteNonQuery("truncate table BirthControlConfig");
                result.IsSuccess = true;
            }
            else
            {

                if (dataObj.ConfirmEnddate == null)
                {
                    tip = "推送结束时间不能为空";
                }
                else if (dataObj.ConfirmStartdate == null)
                {
                    tip = "推送开始时间不能为空";
                }
                else if (dataObj.ManAge == null)
                {
                    tip = "男员工年龄不能为空";
                }
                else if (dataObj.WomenAge == null)
                {
                    tip = "女员工年龄不能为空";
                }
                if (tip == "")
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
                if (!isValid)
                {
                    result.IsSuccess = false;
                    result.Message = tip;
                    return Json(result);
                }
                if (dataObj.id == 0)//新增
                {
                    dataObj.IsPush = "0";
                    result.IsSuccess = _birthcontrolconfigService.Insert(dataObj);
                    objLog.UserId = this.WorkContext.CurrentUser.UserId;
                    objLog.UserName = this.WorkContext.CurrentUser.RealName;
                    objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                    objLog.OpTime = DateTime.Now;
                    objLog.OpType = "编辑";
                    objLog.Description = "编辑计划生育管理设置信息成功";
                    _birthcontrollogService.Insert(objLog);
                }
                else
                {//编辑
                    dataObj.IsPush = "0";
                    result.IsSuccess = _birthcontrolconfigService.Update(dataObj);
                    objLog.UserId = this.WorkContext.CurrentUser.UserId;
                    objLog.UserName = this.WorkContext.CurrentUser.RealName;
                    objLog.UserIp = Request.ServerVariables[Request.ServerVariables["HTTP_VIA"] != null ? "HTTP_X_FORWARDED_FOR" : "REMOTE_ADDR"];
                    objLog.OpTime = DateTime.Now;
                    objLog.OpType = "编辑";
                    objLog.Description = "编辑计划生育管理设置信息成功";
                    _birthcontrollogService.Insert(objLog);
                }
            }
            
            #endregion            
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
    }
}