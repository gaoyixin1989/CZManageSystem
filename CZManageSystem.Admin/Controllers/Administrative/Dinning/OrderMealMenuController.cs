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
    public class OrderMealMenuController : BaseController
    {
        // GET: OrderMealMenu
        IOrderMeal_DinningRoomService _diningroomservice = new OrderMeal_DinningRoomService();
        IOrderMeal_DinningRoomMealTimeSettingsService _diningroomtimeservice = new OrderMeal_DinningRoomMealTimeSettingsService();
        IOrderMeal_MenuService _mealmenuservice = new OrderMeal_MenuService();
        IOrderMeal_MenuCuisineService _menucuisineservice= new OrderMeal_MenuCuisineService();
        IOrderMeal_MenuPackageCommandService _menupackagecommandservice = new OrderMeal_MenuPackageCommandService();
        IOrderMeal_CommandService _mealcommandservice = new OrderMeal_CommandService();
        IOrderMeal_CuisineService _mealcuisineservice = new OrderMeal_CuisineService();
        IOrderMeal_PackageService _mealpackageservice = new OrderMeal_PackageService();
        IOrderMeal_MealPlaceService _mealplaceservice = new OrderMeal_MealPlaceService();
        [SysOperation(OperationType.Browse, "访问食堂菜谱信息页面")]
        public ActionResult Index(string DinningRoomID,string type = "NJUMP")
        {
            ViewData["Type"] = type;
            ViewData["DinningRoomID"] = DinningRoomID;
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OrderMeal_MenuQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _mealmenuservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _tmpClosePayBackTime = "";
                var _tmpdingningroomname = _diningroomservice.List().Where(u => u.Id == item.DinningRoomID).Select(u => u.DinningRoomName).ToList();
                var _tmpdinningtimesetting = _diningroomtimeservice.FindByFeldName(u => u.DinningRoomID == item.DinningRoomID && u.MealTimeType == item.MealTimeType);
                if (_tmpdinningtimesetting != null)
                    if (_tmpdinningtimesetting.ClosePayBackTime.HasValue)
                        _tmpClosePayBackTime = _tmpdinningtimesetting.ClosePayBackTime.Value.ToString();
                listResult.Add(new
                {
                    item.Id,
                    item.MenuName,
                    item.MealTimeType,
                    WorkingDate = item.WorkingDate.HasValue ? Convert.ToDateTime(item.WorkingDate).ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CreateTime = item.CreateTime.HasValue ? Convert.ToDateTime(item.CreateTime).ToString("yyyy-MM-dd HH:mm:ss") : "",
                    BeginTime= _tmpdinningtimesetting.BeginTime.Value,
                    EndTime = _tmpdinningtimesetting.EndTime.Value,
                    ClosePayBackTime = _tmpClosePayBackTime,
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
            var CuisineInfo = new object();
            var PackageInfo = new object();
            var BaseInfo = new object();
            var obj = _mealmenuservice.FindById(id);
            var _tmppackagemname = _menupackagecommandservice.List().Where(u => u.MenuId == obj.Id).Select(u => u.PackageName).ToList();
            var _tmpcommandid = _menupackagecommandservice.List().Where(u => u.MenuId == obj.Id).Select(u => u.CommandId).ToList();
            BaseInfo = new
            {
                obj.Id,
                obj.DinningRoomID,
                obj.MealTimeType,
                obj.MealTimeID,
                WorkingDate = Convert.ToDateTime(obj.WorkingDate.Value).ToString("yyyy-MM-dd"),
                obj.MenuName,
                obj.CreateTime,
                obj.IsPreOrder,
                obj.Flag,
                obj.SendTimes,
                obj.CanSendSms,
                obj.Bookflag,
                obj.IsCompleted
            };
            //var _tmpdinningtimesetting = _menucuisineservice.FindByFeldName(u => u.DinningRoomID == item.DinningRoomID);
            var _tmpcuisinemname = _menucuisineservice.List().Where(u => u.MenuId == obj.Id).Select(u => u.CuisineName).ToList();
            var _tmpcuisineid = _menucuisineservice.List().Where(u => u.MenuId == obj.Id).Select(u => u.CuisineId).ToList();
            PackageInfo = new {
                PackageName = string.Join(",", _tmppackagemname),
                CommandId = string.Join(",", _tmpcommandid)
            };
            CuisineInfo = new
            {
                CuisineName = string.Join(",", _tmpcuisinemname),
                CuisineId = string.Join(",", _tmpcuisineid)
            };
            return Json(new
            {
                baseinfo = BaseInfo,
                cuisineinfo = CuisineInfo,
                packageinfo = PackageInfo
            });
            //return Json(new
            //{
            //    obj.Id,
            //    obj.DinningRoomID,
            //    obj.MealTimeType,
            //    obj.MealTimeID,
            //    obj.WorkingDate,
            //    obj.MenuName,
            //    obj.CreateTime,
            //    obj.IsPreOrder
            //});
        }


        [SysOperation(OperationType.Save, "保存食堂菜谱信息")]
        public ActionResult Save(OrderMeal_Menu dataBase,string CuisineId ,string CommandId)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            
            try
            {
                #region 验证数据是否合法
                string tip = "";
                bool isValid = false;//是否验证通过 
                string[] CuisineIds = CuisineId.Split(',');
                SysOperationLog _log = new SysOperationLog();
                string[] CommandIds = CommandId.Split(',');
                var cuisinelist = _mealcuisineservice.List();
                var list = _mealmenuservice.List().Where(u => dataBase.DinningRoomID == u.DinningRoomID && dataBase.MenuName == u.MenuName && u.Id != dataBase.Id && dataBase.MealTimeType == u.MealTimeType).ToList();
                var packagelist = _mealpackageservice.List();

                var placelist = _mealplaceservice.List();
                if (list.Count > 0)
                {
                    tip = "食堂对应的菜谱名称重复！";
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
                    _log.Operation = OperationType.Add;
                    dataBase.Id = Guid.NewGuid();
                    dataBase.IsCompleted = 0;
                    dataBase.CanSendSms = 1;
                    dataBase.CreateTime = DateTime.Now;
                    dataBase.MealTimeID = GetText(dataBase.MealTimeType);
                    result.IsSuccess = _mealmenuservice.Insert(dataBase);
                    if (CuisineIds.Length > 0)
                    {
                        foreach (var i in CuisineIds)
                        {
                            OrderMeal_MenuCuisine _tmp1 = new OrderMeal_MenuCuisine();
                            _tmp1.Id = Guid.NewGuid();
                            _tmp1.MenuId = dataBase.Id;
                            _tmp1.CuisineId = Guid.Parse(i.ToString());
                            var _cuisinename = cuisinelist.Where(u => u.Id == _tmp1.CuisineId).Select(u => u.CuisineName).ToList()[0];
                            _tmp1.CuisineName = _cuisinename;
                            _menucuisineservice.Insert(_tmp1);
                        }
                    }
                    if (CommandIds.Length > 0)
                    {
                        foreach (var i in CommandIds)
                        {
                            OrderMeal_MenuPackageCommand _tmp1 = new OrderMeal_MenuPackageCommand();
                            var _i = Guid.Parse(i);

                            var _tmpcommand = _mealcommandservice.FindById(_i);
                            _tmp1.Id = Guid.NewGuid();
                            _tmp1.MenuId = dataBase.Id;
                            _tmp1.PackageId = _tmpcommand.PackageId;
                            _tmp1.Command = _tmpcommand.Command;
                            _tmp1.PackageName = placelist.Where(u => u.Id == _tmpcommand.PlaceId).Select(u => u.MealPlaceName).ToList()[0] + "-" + packagelist.Where(u => u.Id == _tmp1.PackageId).Select(u => u.PackageName).ToList()[0];
                            _tmp1.CommandId = _i;
                            _menupackagecommandservice.Insert(_tmp1);
                        }
                    }
                }
                else
                {//编辑
                    result.IsSuccess = _mealmenuservice.Update(dataBase);
                    _log.Operation = OperationType.Edit;
                    if (CuisineIds.Length > 0)
                    {
                        var objs = _menucuisineservice.List().Where(u => u.MenuId == dataBase.Id).ToList();
                        if (objs.Count > 0)
                            _menucuisineservice.DeleteByList(objs);
                        foreach (var i in CuisineIds)
                        {
                            OrderMeal_MenuCuisine _tmp1 = new OrderMeal_MenuCuisine();
                            _tmp1.Id = Guid.NewGuid();
                            _tmp1.MenuId = dataBase.Id;
                            _tmp1.CuisineId = Guid.Parse(i.ToString());
                            var _cuisinename = cuisinelist.Where(u => u.Id == _tmp1.CuisineId).Select(u => u.CuisineName).ToList()[0];
                            _tmp1.CuisineName = _cuisinename;
                            _menucuisineservice.Insert(_tmp1);
                        }
                    }
                    if (CommandIds.Length > 0)
                    {
                        var objs = _menupackagecommandservice.List().Where(u => u.MenuId == dataBase.Id).ToList();
                        if (objs.Count > 0)
                            _menupackagecommandservice.DeleteByList(objs);
                        foreach (var i in CommandIds)
                        {
                            OrderMeal_MenuPackageCommand _tmp1 = new OrderMeal_MenuPackageCommand();
                            var _i = Guid.Parse(i);

                            var _tmpcommand = _mealcommandservice.FindById(_i);
                            _tmp1.Id = Guid.NewGuid();
                            _tmp1.MenuId = dataBase.Id;
                            _tmp1.PackageId = _tmpcommand.PackageId;
                            _tmp1.Command = _tmpcommand.Command;
                            _tmp1.PackageName = placelist.Where(u => u.Id == _tmpcommand.PlaceId).Select(u => u.MealPlaceName).ToList()[0] + "-" + packagelist.Where(u => u.Id == _tmp1.PackageId).Select(u => u.PackageName).ToList()[0];
                            _tmp1.CommandId = _i;
                            _menupackagecommandservice.Insert(_tmp1);

                        }
                    }
                }
                result.Message = tip;
                _log.OperationDesc = "保存成功！";
                _log.OperationPage = Request.RawUrl;
                _log.RealName = this.WorkContext.CurrentUser.RealName;
                _log.UserName = this.WorkContext.CurrentUser.UserName;
                _sysLogService.WriteSysLog<SysOperationLog>(_log);
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

        [SysOperation(OperationType.Delete, "删除食堂菜谱数据")]
        public ActionResult Delete(Guid[] ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var objs = _mealmenuservice.List().Where(u => ids.Contains(u.Id)).ToList();
                if (objs.Count > 0)
                {
                    if (_mealmenuservice.DeleteByList(objs))
                    {
                        var m1 = _menupackagecommandservice.List().Where(m => ids.Contains(m.MenuId)).ToList();
                        _menupackagecommandservice.DeleteByList(m1);
                        var m2 = _menucuisineservice.List().Where(m => ids.Contains(m.MenuId)).ToList();
                        result.IsSuccess = _menucuisineservice.DeleteByList(m2);
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

        public ActionResult SelectCuisine(string selectedId, Guid? DinningRoomId)
        {
            ViewData["DinningRoomId"] = DinningRoomId;
            ViewData["selectedId"] = selectedId;
            return View();
        }


        public ActionResult SelectPackage(string selectedId, Guid? DinningRoomId,string MealTimeType)
        {
            ViewData["DinningRoomId"] = DinningRoomId;
            ViewData["selectedId"] = selectedId;
            ViewData["MealTimeType"] = MealTimeType;
            return View();
        }

        public ActionResult GetDataStartFromID(Guid id)
        {
            IOrderMeal_CuisineService _mealcuisineservice = new OrderMeal_CuisineService();
            List<OrderMeal_CuisineTreeData> listData = new List<OrderMeal_CuisineTreeData>();
            IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
            var modelList = new List<DataDictionary>();
            int count = 0;
            modelList = _dataDictionaryService.QueryDataByPage(out count, 0, int.MaxValue, "菜式类型", null).ToList();
            modelList = modelList.Where(u => (u.EnableFlag ?? false)).OrderBy(u => u.OrderNo).ToList();

            listData = _mealcuisineservice.GetDictNameGroup(id).ToList();
            OrderMeal_CuisineTreeData _tmp2 = new OrderMeal_CuisineTreeData();
            //_tmp2.CuisineType = "全部";
            _tmp2.CuisineName = "全部";
            _tmp2.ParentId = "-1";
            _tmp2.Id = Guid.Parse("00000000-0000-0000-0000-000000000000");
            listData.Add(_tmp2);
            foreach (var it in modelList)
            {
                OrderMeal_CuisineTreeData _tmp = new OrderMeal_CuisineTreeData();
                _tmp.ParentId = _tmp2.Id.ToString();
                // _tmp.CuisineType = "全部";
                _tmp.CuisineName = it.DDText;
                _tmp.Id = it.DDId;
                listData.Add(_tmp);
            }
            return Json(new { items = listData, count = listData.Count() }, JsonRequestBehavior.AllowGet);
        }




        public ActionResult GetDataByIds(Guid[] ids)
        {
            IOrderMeal_CuisineService _mealcuisineservice = new OrderMeal_CuisineService();
            List<OrderMeal_CuisineTreeData> listData = new List<OrderMeal_CuisineTreeData>();
            if (ids != null && ids.Length > 0)
                listData = _mealcuisineservice.GetDictNameGroup(Guid.Parse("00000000-0000-0000-0000-000000000000")).Where(u => ids.Contains(u.Id)).ToList();
            return Json(listData, JsonRequestBehavior.AllowGet);
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