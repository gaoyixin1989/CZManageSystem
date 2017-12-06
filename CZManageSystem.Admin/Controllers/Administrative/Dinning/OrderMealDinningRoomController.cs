using CZManageSystem.Admin.Base;
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
    public class OrderMealDinningRoomController : BaseController
    {
        // GET: OrderMeal_DinningRoom
        IOrderMeal_DinningRoomService _diningroomservice = new OrderMeal_DinningRoomService();
        IOrderMeal_DinningRoomMealTimeSettingsService _settingservice = new OrderMeal_DinningRoomMealTimeSettingsService();
        IOrderMeal_DinningRoomMealBookSettingsService _bookservice = new OrderMeal_DinningRoomMealBookSettingsService();
        IOrderMeal_DinningRoomAdminService _adminservice = new OrderMeal_DinningRoomAdminService();
        [SysOperation(OperationType.Browse, "访问食堂基本信息页面")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OrderMeal_DinningRoomQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _diningroomservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _tmpuserid = _adminservice.List().Where(u => u.DinningRoomID == item.Id).Select(u => u.UserId).ToList();
                var _tmprealname = _adminservice.List().Where(u => u.DinningRoomID == item.Id).Select(u => u.RealName).ToList();
                listResult.Add(new
                {
                    item.Id,
                    item.DinningRoomName,
                    item.Discription,
                    UserId = _tmpuserid.Count > 0 ? string.Join(",", _tmpuserid): "",
                    RealName = _tmprealname.Count > 0 ? string.Join(",", _tmprealname) : "该食堂还未设置管理员"
                });
            }




            return Json(new { items = listResult, count = count });
        }
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
                id = Guid.Parse("00000000-0000-0000-0000-000000000000");
            ViewData["id"] = id;
            return View();
        }
        public ActionResult GetDataByID(Guid id)
        {
            //var model = _settingservice.List().Where(u => u.DinningRoomID == id).OrderBy(u=>u.MealTimeID);//FindByFeldName(u => u.DinningRoomID == id);
            var InfoList = new List<OrderMeal_DinningRoomMealTimeSettings>();
            var BookInfoList = new List<OrderMeal_DinningRoomMealBookSettings>();
            var BaseInfo = new OrderMeal_DinningRoom();
            if (id == Guid.Empty)
            {
                IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
                var modelList = new List<DataDictionary>();
                int count = 0;
                modelList = _dataDictionaryService.QueryDataByPage(out count, 0, int.MaxValue, "餐次", null).ToList();
                modelList = modelList.Where(u => (u.EnableFlag ?? false)).OrderBy(u => u.OrderNo).ToList();
                foreach (var item in modelList)
                {
                    OrderMeal_DinningRoomMealTimeSettings _obj = new OrderMeal_DinningRoomMealTimeSettings();
                    _obj.Id = Guid.NewGuid();
                    _obj.MealTimeType = item.DDValue;
                    _obj.MealTimeID = item.OrderNo;
                    InfoList.Add(_obj);

                    OrderMeal_DinningRoomMealBookSettings _objbook = new OrderMeal_DinningRoomMealBookSettings();
                    _objbook.Id = Guid.NewGuid();
                    _objbook.MealTimeType = item.DDValue;
                    _objbook.MealTimeID = item.OrderNo;
                    BookInfoList.Add(_objbook);
                }
            }
            else
            {
                BaseInfo = _diningroomservice.FindById(id);
                InfoList = _settingservice.List().Where(u => u.DinningRoomID == id).OrderBy(u => u.MealTimeID).ToList();
                BookInfoList= _bookservice.List().Where(u => u.DinningRoomID == id).OrderBy(u => u.MealTimeID).ToList();
            }

            return Json(new
            {
                baseinfo = BaseInfo,
                info = InfoList,
                bookinfo = BookInfoList
            });
        }
        [SysOperation(OperationType.Save, "保存食堂管理员信息")]
        public ActionResult SaveAdmin(string  Id,string Userid)
        {

            SystemResult result = new SystemResult() { IsSuccess = false };
            SysOperationLog _log = new SysOperationLog();
            try
            {
                string[] _tempuserid = Userid.Split(',');
                Guid DiningRoomId = new Guid();
                Guid.TryParse(Id, out DiningRoomId);
                var m1 = _adminservice.List().Where(m => m.DinningRoomID == DiningRoomId).ToList();
                _adminservice.DeleteByList(m1);
                foreach (var i in _tempuserid)
                {
                    OrderMeal_DinningRoomAdmin obj = new OrderMeal_DinningRoomAdmin();
                    obj.UserId = new Guid(i);
                    obj.DinningRoomID = DiningRoomId;
                    obj.RealName = CommonFunction.getUserRealNamesByIDs(i);
                    obj.Loginname = _sysUserService.FindById(obj.UserId).UserName;
                    obj.AdminType = "食堂管理员";
                    result.IsSuccess = _adminservice.Insert(obj);                    
                }
                if (!result.IsSuccess && string.IsNullOrEmpty(result.Message))
                {
                    result.Message = "保存失败";
                }
                else
                {
                    _log.Operation = OperationType.Edit;
                    _log.OperationDesc = "保存食堂管理员成功！";
                    _log.OperationPage = Request.RawUrl;
                    _log.RealName = this.WorkContext.CurrentUser.RealName;
                    _log.UserName = this.WorkContext.CurrentUser.UserName;
                    _sysLogService.WriteSysLog<SysOperationLog>(_log);
                }               
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = "保存食堂管理员失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "保存食堂管理员失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }            
        }
        [SysOperation(OperationType.Save, "保存食堂信息")]
        public ActionResult Save(OrderMeal_DinningRoom dataBase, List<OrderMeal_DinningRoomMealTimeSettings> dataList,List<OrderMeal_DinningRoomMealBookSettings> databookList)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            SysOperationLog _log = new SysOperationLog();
            try
            {
                if (dataBase.Id == Guid.Empty)
                {
                    _log.Operation = OperationType.Add;
                    dataBase.Id = Guid.NewGuid();
                    result.IsSuccess = _diningroomservice.Insert(dataBase);
                    foreach (var item in dataList)
                    {
                        var tmp1 = item.BeginTime;
                        var tmp2 = item.Id;
                        item.DinningRoomID = dataBase.Id;
                        item.Creator = this.WorkContext.CurrentUser.RealName;
                        item.CreatedTime = DateTime.Now;
                        _settingservice.Insert(item);
                    }

                    foreach (var item in databookList)
                    {
                        var tmp1 = item.Week;
                        var tmp2 = item.Id;
                        item.DinningRoomID = dataBase.Id;
                        _bookservice.Insert(item);
                    }

                }
                else
                {
                    _log.Operation = OperationType.Edit;
                    result.IsSuccess = _diningroomservice.Update(dataBase);
                    foreach (var item in dataList)
                    {
                        var tmp1 = item.BeginTime;
                        var tmp2 = item.Id;
                        item.LastModifier = this.WorkContext.CurrentUser.RealName;
                        item.LastModTime = DateTime.Now;
                        _settingservice.Update(item);
                    }
                    foreach (var item in databookList)
                    {
                        var tmp1 = item.Week;
                        var tmp2 = item.Id;
                        _bookservice.Update(item);
                    }
                }
                if (!result.IsSuccess && string.IsNullOrEmpty(result.Message))
                {
                    result.Message = "保存失败";
                }
                else
                {
                    _log.OperationDesc = "保存成功！";
                    _log.OperationPage = Request.RawUrl;
                    _log.RealName = this.WorkContext.CurrentUser.RealName;
                    _log.UserName = this.WorkContext.CurrentUser.UserName;
                    _sysLogService.WriteSysLog<SysOperationLog>(_log);
                }
                
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
        [SysOperation(OperationType.Delete, "删除食堂基本信息数据")]
        public ActionResult Delete(Guid[] ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var objs = _diningroomservice.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    if (_diningroomservice.DeleteByList(objs))
                    {
                        var m1 = _settingservice.List().Where(m => ids.Contains(m.DinningRoomID)).ToList();
                        _settingservice.DeleteByList(m1);
                        var m2 = _bookservice.List().Where(m => ids.Contains(m.DinningRoomID)).ToList();
                        _bookservice.DeleteByList(m2);
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

        #region 其他方法
        public ActionResult getDinningRoomAsDict()
        {
            var objList = _diningroomservice.List().Select(u => new { u.DinningRoomName, DinningRoomID = u.Id }).ToList();
            //int count = 0;
            //var modelList = _diningroomservice.GetForPagingByCondition(out count, null, 0, int.MaxValue).ToList();
            //List<object> itemResult = new List<object>();
            //foreach (var model in modelList)
            //{
            //    itemResult.Add(new
            //    {
            //        DinningRoomID = model.Id,
            //        DinningRoomName = model.DinningRoomName
            //    });
            //}

            return Json(objList, JsonRequestBehavior.AllowGet);
        }

        #endregion



    }
}