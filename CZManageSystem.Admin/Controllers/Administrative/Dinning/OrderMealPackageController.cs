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
    public class OrderMealPackageController : BaseController
    {
        // GET: OrderMeal_Package
        IOrderMeal_DinningRoomService _diningroomservice = new OrderMeal_DinningRoomService();
        IOrderMeal_PackageService _mealpackageservice = new OrderMeal_PackageService();
        IOrderMeal_MealPlaceService _mealplaceservice = new OrderMeal_MealPlaceService();
        IOrderMeal_CommandService _mealcommandservice = new OrderMeal_CommandService();
        [SysOperation(OperationType.Browse, "访问食堂套餐信息页面")]
        public ActionResult Index(string DinningRoomID,string type = "NJUMP")
        {
            ViewData["Type"] = type;
            ViewData["DinningRoomID"] = DinningRoomID;
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OrderMeal_PackageQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _mealpackageservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _tmpcommand = "";
                var _tmpdingningroomname = _diningroomservice.List().Where(u => u.Id == item.DinningRoomID).Select(u => u.DinningRoomName).ToList();
                var _tmpmealplace  = _mealplaceservice.List().Where(u => u.DinningRoomID == item.DinningRoomID).ToList();
                foreach(var it in _tmpmealplace)
                {
                    var mealplaceid = it.Id;
                    var _tmp = _mealcommandservice.List().Where(u => u.PlaceId == mealplaceid && u.PackageId == item.Id).Select(u => u.Command);
                    _tmpcommand +=  "   " + _tmpmealplace.ToList()[0].MealPlaceName + "：" + string.Join(",", _tmp);
                }
                listResult.Add(new
                {
                    item.Id,
                    item.PackageName,
                    item.PackagePrice,
                    item.MealTimeType,
                    item.Discription,
                    Command = _tmpcommand,
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
            var obj = _mealpackageservice.FindById(id);
            var commandobj = _mealcommandservice.List().Where(u => u.PackageId == id).ToList();
            return Json(new
            {
                obj.Id,
                obj.DinningRoomID,
                obj.Discription,
                obj.MealTimeType,
                obj.PackageName,
                obj.PackagePrice,
                obj.MealTimeID,
                obj.CreatedTime,
                obj.Creator
            });
        }
        [SysOperation(OperationType.Save, "保存食堂套餐信息")]
        public ActionResult Save(OrderMeal_Package dataBase, List<OrderMeal_Command> dataCommandList)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                #region 验证数据是否合法
                string tip = "";
                bool isValid = false;//是否验证通过 

                var list = _mealpackageservice.List().Where(u => dataBase.DinningRoomID == u.DinningRoomID && dataBase.PackageName == u.PackageName && u.MealTimeType == dataBase.MealTimeType && u.Id != dataBase.Id).ToList();
                if (list.Count > 0)
                {
                    tip = "食堂对应餐时的套餐名重复！";
                }

                foreach (var item in dataCommandList)
                {
                    var _tmplist = _mealcommandservice.List().Where(u => u.Command == item.Command && u.Id != item.Id).ToList();
                    if (_tmplist.Count > 0 || item.Command == "P" || item.Command == "X")
                    {
                        var tmpplacename = _mealplaceservice.List().Where(u => u.Id == item.PlaceId).Select(u => u.MealPlaceName).ToList();
                        tip = string.Join(",", tmpplacename) + "订餐命令已被使用了";
                        continue;
                    }
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
                if (dataBase.Id == Guid.Empty)//新增
                {
                    dataBase.CreatedTime = DateTime.Now;
                    dataBase.Creator = this.WorkContext.CurrentUser.UserName;
                    dataBase.Id = Guid.NewGuid();
                    dataBase.MealTimeID = GetText(dataBase.MealTimeType);
                    foreach (var item in dataCommandList)
                    {
                        var tmp1 = item.Command;
                        var tmp2 = item.Id;
                        item.PackageId = dataBase.Id;
                        item.Creator = this.WorkContext.CurrentUser.RealName;
                        item.CreatedTime = DateTime.Now;
                        _mealcommandservice.Insert(item);
                    }
                    result.IsSuccess = _mealpackageservice.Insert(dataBase);
                }
                else
                {//编辑
                    dataBase.LastModifier = this.WorkContext.CurrentUser.UserName;
                    dataBase.LastModTime = DateTime.Now;
                    foreach (var item in dataCommandList)
                    {
                        var tmp1 = item.Command;
                        var tmp2 = item.Id;
                        item.PackageId = dataBase.Id;
                        item.LastModifier = this.WorkContext.CurrentUser.RealName;
                        item.LastModTime = DateTime.Now;
                        _mealcommandservice.Update(item);
                    }
                    result.IsSuccess = _mealpackageservice.Update(dataBase);
                }
                result.Message = tip;
                //记录操作日志
                _sysLogService.WriteSysLog<SysOperationLog>(new SysOperationLog()
                {
                    Operation = OperationType.Edit,
                    OperationDesc = "保存成功！",
                    OperationPage = Request.RawUrl,
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
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

        [SysOperation(OperationType.Delete, "删除食堂套餐数据")]
        public ActionResult Delete(Guid[] ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var objs = _mealpackageservice.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    if (_mealpackageservice.DeleteByList(objs))
                    {
                        var m1 = _mealcommandservice.List().Where(m => ids.Contains(m.PackageId)).ToList();
                        
                        result.IsSuccess= _mealcommandservice.DeleteByList(m1);
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

        public ActionResult GetPlaceListData(Guid DinningRoomID,Guid PackageId)
        {
            List<object> listResult = new List<object>();
            if (PackageId == Guid.Empty)
            {
                var modelList = _mealplaceservice.List().Where(u => u.DinningRoomID == DinningRoomID);
                foreach (var item in modelList)
                {
                    listResult.Add(new
                    {
                        PlaceId = item.Id,
                        item.MealPlaceName,
                        Id = Guid.NewGuid(),
                        Command = ""
                    });
                }
            }
            else
            {
                var modelList = _mealcommandservice.List().Where(u => u.PackageId == PackageId );
                foreach (var item in modelList)
                {
                    var _tmpplacename = _mealplaceservice.List().Where(u => u.Id == item.PlaceId).Select(u => u.MealPlaceName).ToList();
                    listResult.Add(new
                    {
                        PlaceId = item.PlaceId,
                        MealPlaceName = _tmpplacename.Count > 0 ? string.Join(",", _tmpplacename) : "",
                        Id = item.Id,
                        Command = item.Command
                    });
                }
            }

            return Json(new { items = listResult });
        }

        public static int GetText(string type)
        {
            int strResult = 0;
            if (type != null)
            {
                switch (type)
                {
                    case "早餐": strResult = 1; break;
                    case "中餐": strResult = 2; break;
                    case "晚餐": strResult = 3; break;
                    case "宵夜": strResult = 4; break;
                    default: break;
                }
            }
            return strResult;
        }
    }
}