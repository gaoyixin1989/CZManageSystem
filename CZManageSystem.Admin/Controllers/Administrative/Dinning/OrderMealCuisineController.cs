using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative.Dinning;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative.Dinning
{
    public class OrderMealCuisineController : BaseController
    {
        // GET: OrderMeal_Cuisine
        IOrderMeal_DinningRoomService _diningroomservice = new OrderMeal_DinningRoomService();
        IOrderMeal_CuisineService _mealcuisineservice = new OrderMeal_CuisineService();
        [SysOperation(OperationType.Browse, "访问菜式页面")]
        public ActionResult Index(string DinningRoomID, string type = "NJUMP")
        {
            ViewData["Type"] = type;
            ViewData["DinningRoomID"] = DinningRoomID;
            return View();
        }
        
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OrderMeal_CuisineQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _mealcuisineservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _tmpdingningroomname = _diningroomservice.List().Where(u => u.Id == item.DinningRoomID).Select(u => u.DinningRoomName).ToList();
                listResult.Add(new
                {
                    item.Id,
                    item.CuisineName,
                    item.CuisineType,
                    DinningRoomName = _tmpdingningroomname.Count > 0 ? string.Join(",", _tmpdingningroomname) : ""
                });
            }
            return Json(new { items = listResult, count = count });
        }


        public ActionResult Edit(Guid? id, string DinningRoomID)
        {
            if (id == null)
                id = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewData["id"] = id;
            ViewData["DinningRoomID"] = DinningRoomID;
            return View();
        }
        public ActionResult GetDataByID(Guid id)
        {
            var obj = _mealcuisineservice.FindById(id);
            return Json(new
            {
                obj.Id,
                obj.DinningRoomID,
                obj.CuisineName,
                obj.CuisineType,
                obj.CreatedTime,
                obj.Creator
            });
        }
        [SysOperation(OperationType.Save, "保存菜式")]
        public ActionResult Save(OrderMeal_Cuisine dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            SysOperationLog _log = new SysOperationLog();
            try
            {
                #region 验证数据是否合法
                string tip = "";
                bool isValid = false;//是否验证通过 

                var list = _mealcuisineservice.List().Where(u => dataObj.DinningRoomID == u.DinningRoomID && dataObj.CuisineName == u.CuisineName && u.Id != dataObj.Id).ToList();
                if (list.Count > 0)
                {
                    tip = "食堂对应的菜式名称重复！";
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
                #endregion
                if (dataObj.Id == Guid.Empty)//新增
                {
                    _log.Operation = OperationType.Add;
                    dataObj.CreatedTime = DateTime.Now;
                    dataObj.Creator = this.WorkContext.CurrentUser.UserName;
                    dataObj.Id = Guid.NewGuid();
                    result.IsSuccess = _mealcuisineservice.Insert(dataObj);
                }
                else
                {//编辑
                    _log.Operation = OperationType.Edit;
                    dataObj.LastModifier = this.WorkContext.CurrentUser.UserName;
                    dataObj.LastModTime = DateTime.Now;
                    result.IsSuccess = _mealcuisineservice.Update(dataObj);
                }
                result.Message = tip;
                _log.OperationDesc = "保存成功！";
                _log.OperationPage = Request.RawUrl;
                _log.RealName = this.WorkContext.CurrentUser.RealName;
                _log.UserName = this.WorkContext.CurrentUser.UserName;
                _sysLogService.WriteSysLog<SysOperationLog>(_log);
                result.Message = tip;
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = "保存失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "保存失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }
        }

        [SysOperation(OperationType.Delete, "删除食堂菜式数据")]
        public ActionResult Delete(Guid[] ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var objs = _mealcuisineservice.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    if (_mealcuisineservice.DeleteByList(objs))
                    {
                        result.IsSuccess = true;
                        result.Message = "删除成功";
                        result.data = objs.Count;
                        //return Json(new { IsSuccess = true, Message = "删除成功", SuccessCount = objs.Count });
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "删除失败";
                        //return Json(new { IsSuccess = false, Message = "删除失败" });
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "没有可删除的数据";
                    //return Json(new { IsSuccess = false, Message = "没有可删除的数据" });
                }
                _sysLogService.WriteSysLog<SysOperationLog>(new SysOperationLog()
                {
                    Operation = OperationType.Delete,
                    OperationDesc = "删除成功！",
                    OperationPage = Request.RawUrl,
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = "删除失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "删除失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }
        }
    }
}