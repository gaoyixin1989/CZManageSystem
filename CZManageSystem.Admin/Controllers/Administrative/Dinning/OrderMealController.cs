using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative.Dinning;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative.Dinning
{
    public class OrderMealController : BaseController
    {
        // GET: OrderMeal
        IOrderMeal_UserBaseinfoService _userbaseinfoservice = new OrderMeal_UserBaseinfoService();
        IOrderMeal_UserDinningRoomService _userdinningroomservice = new OrderMeal_UserDinningRoomService();
        IOrderMeal_DinningRoomService _diningroomservice = new OrderMeal_DinningRoomService();
        IOrderMeal_DinningRoomMealTimeSettingsService _settingservice = new OrderMeal_DinningRoomMealTimeSettingsService();
        IOrderMeal_DinningRoomMealBookSettingsService _bookservice = new OrderMeal_DinningRoomMealBookSettingsService();
        IOrderMeal_MealPlaceService _mealplaceservice = new OrderMeal_MealPlaceService();
        IOrderMeal_PackageService _mealpackageservice = new OrderMeal_PackageService();
        IView_EXT_XF_AccountService _balanceservice = new View_EXT_XF_AccountService();
        IOrderMeal_MealOrderService _ordermealservice = new OrderMeal_MealOrderService();
        IOrderMeal_BookOrderService _orderbookmealservice = new OrderMeal_BookOrderService();
        IOrderMeal_UserAccountBalanceService _useraccountbalanceservice = new OrderMeal_UserAccountBalanceService();
        ISysUserService _userService = new SysUserService();
        IOrderMeal_DinningRoomAdminService _adminservice = new OrderMeal_DinningRoomAdminService();

        [SysOperation(OperationType.Browse, "访问订餐明细页面")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OrderMeal_BookOrderQueryBuilder queryBuilder = null, string DpId = null)
        {
            int count = 0;
            if (queryBuilder.OrderTime_Start != null)
                queryBuilder.OrderTime_Start = queryBuilder.OrderTime_Start.Value.Date;
            if (queryBuilder.OrderTime_End != null)
                queryBuilder.OrderTime_End = queryBuilder.OrderTime_End.Value.AddDays(1).Date.AddSeconds(-1);
            if (DpId != "")
                queryBuilder.DpId = Get_Subdept_ByDept(DpId);
            else
                queryBuilder.DpId = null;

            if (_adminservice.List().Where(u => u.Loginname == WorkContext.CurrentUser.UserName).ToList().Count>0)
            {
                var _tmpadminobj = _adminservice.FindByFeldName(u => u.Loginname == WorkContext.CurrentUser.UserName);
                queryBuilder.DinningRoomID = _tmpadminobj.DinningRoomID.Value;
            }
            else if( WorkContext.CurrentUser.UserName != Base.Admin.UserName)
            {
                queryBuilder.LoginName = WorkContext.CurrentUser.UserName;

            }
            var modelList = _ordermealservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _tmpClosePayBackTime = "";
                var _userobj = _userService.FindByFeldName(u => u.UserName == item.LoginName);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                var _tmpdinningtimesetting = _settingservice.FindByFeldName(u => u.DinningRoomID == item.DinningRoomID && u.MealTimeType == item.MealTimeType);
                if(_tmpdinningtimesetting != null)
                    if (_tmpdinningtimesetting.ClosePayBackTime.HasValue)
                        _tmpClosePayBackTime = _tmpdinningtimesetting.ClosePayBackTime.Value.ToString();


                listResult.Add(new
                {
                    item.Id,
                    item.MealPlaceName,
                    item.MealTimeType,
                    item.DinningRoomName,
                    item.AfterOrderBalance,
                    item.PackagePrice,
                    item.PackageName,
                    item.OrderStateName,
                    item.UserName,
                    ClosePayBackTime = _tmpClosePayBackTime,
                    DinningDate = item.DinningDate.ToString("yyyy-MM-dd"),
                    OrderTime = item.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    DpName = _tmpdplist[0]//CommonFunction.getDeptNamesByIDs(_userobj.DpId)
                });
            }
            return Json(new { items = listResult, count = count });
        }

        [SysOperation(OperationType.Browse, "访问我要订餐页面")]
        public ActionResult Edit()
        {
            var CurrentUser= this.WorkContext.CurrentUser.UserName;
            var CurrentUserId = this.WorkContext.CurrentUser.UserId;
            var _tmpuserbaseinfo = _userbaseinfoservice.FindByFeldName(u => u.LoginName == CurrentUser && u.State == 1);           
            if (_tmpuserbaseinfo == null)
            {
                ViewData["type"] = "show";
                ViewData["UserBaseinfoId"] = Guid.Parse("00000000-0000-0000-0000-000000000000");
                ViewData["UserBalance"] = 0.00;
                ViewData["UserRealName"] = "";
            }                
            else
            {
                var _tmpbalance = _balanceservice.FindByFeldName(u => u.JobNumber == _tmpuserbaseinfo.EmployId);
                if(_tmpbalance != null)
                    ViewData["UserBalance"] = _tmpbalance.BelAmount;
                else
                    ViewData["UserBalance"] = 0.00;
                ViewData["UserRealName"] = _tmpuserbaseinfo.RealName;
                ViewData["type"] = "edit";
                ViewData["UserBaseinfoId"] = _tmpuserbaseinfo.Id;
            }
            return View();
        }

        [SysOperation(OperationType.Browse, "访问我要订餐页面")]
        public ActionResult Order()
        {
            var CurrentUser = this.WorkContext.CurrentUser.UserName;
            var CurrentUserId = this.WorkContext.CurrentUser.UserId;
            var _tmpuserbaseinfo = _userbaseinfoservice.FindByFeldName(u => u.LoginName == CurrentUser && u.State == 1);
            if (_tmpuserbaseinfo == null)
            {
                ViewData["type"] = "show";
                ViewData["UserBaseinfoId"] = Guid.Parse("00000000-0000-0000-0000-000000000000");
                ViewData["UserBalance"] = 0.00;
                ViewData["UserRealName"] = "";
            }
            else
            {
                var _tmpbalance = _balanceservice.FindByFeldName(u => u.JobNumber == _tmpuserbaseinfo.EmployId);
                if (_tmpbalance != null)
                    ViewData["UserBalance"] = _tmpbalance.BelAmount;
                else
                    ViewData["UserBalance"] = 0.00;
                ViewData["UserRealName"] = _tmpuserbaseinfo.RealName;
                ViewData["type"] = "edit";
                ViewData["UserBaseinfoId"] = _tmpuserbaseinfo.Id;
            }
            return View();
        }
        public ActionResult GetMealOrderData()
        {
            List<object> OrderMealList = new List<object>();
            List<object> SubscribeOrderMeal = new List<object>();
            //object SubscribeOrderMeal = null;
            DateTime dtnow = DateTime.Now;
            DateTime dtstart = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
            DateTime dtend = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59").AddDays(1);
            var _tmp = _orderbookmealservice.List().Where(u => u.LoginName == this.WorkContext.CurrentUser.UserName && u.OrderState == 1 && u.EndDate >= dtnow).ToList();
            var _tmp2 = _ordermealservice.List().Where(u => u.LoginName == this.WorkContext.CurrentUser.UserName && u.OrderState == 1 && u.DinningDate >= dtstart && u.DinningDate <= dtend).ToList();
            foreach (var item in _tmp)
            {
                SubscribeOrderMeal.Add(new
                {
                    item.Id,
                    item.MealPlaceName,
                    item.MealTimeType,
                    item.PackagePrice,
                    item.PackageName,
                    item.OrderStateName,
                    PeriodTime = item.StartedDate.ToString("yyyy-MM-dd") + "到" + item.EndDate.ToString("yyyy-MM-dd")
                });
            }
            foreach (var item in _tmp2)
            {
                OrderMealList.Add(new
                {
                    item.Id,
                    item.MealPlaceName,
                    item.DinningDate,
                    item.MealTimeType,
                    item.PackagePrice,
                    item.PackageName,
                    item.OrderStateName
                });
            }
            return Json(new { SubscribeOrderMeal = SubscribeOrderMeal, SubscribeOrderMealCount = SubscribeOrderMeal.Count, OrderMealCount = OrderMealList.Count, OrderMealList = OrderMealList });
        }

        [SysOperation(OperationType.Save, "保存订餐")]
        public ActionResult Save(OrderMeal_MealOrderTmp dataObj)
        {
            bool isValid = false;//是否验证通过 
            SystemResult result = new SystemResult() { IsSuccess = false };
            OrderMeal_MealOrder Obj = new OrderMeal_MealOrder();
            OrderMeal_BookOrder BObj = new OrderMeal_BookOrder();
            OrderMeal_UserAccountBalance UBObj = new OrderMeal_UserAccountBalance();

            SysOperationLog _log = new SysOperationLog();
            try
            {
                string tip = "";
                string ordernum = "";
                string message = "";
                decimal yktbalance = 0;
                decimal afterOrderBalance = 0;

                var CurrentUser = this.WorkContext.CurrentUser.UserName;
                var _tmpuserbaseinfo = _userbaseinfoservice.FindByFeldName(u => u.LoginName == CurrentUser && u.State == 1);
                var _tmpdiningroom = _diningroomservice.FindById(dataObj.DinningRoomID);
                var _tmppackageinfo = _mealpackageservice.FindById(dataObj.MealPackage);
                var _mealplaceinfo = _mealplaceservice.FindById(dataObj.MealPlace);
                var _tmpbalance = _balanceservice.FindByFeldName(u => u.JobNumber == _tmpuserbaseinfo.EmployId);
                var _tmpdrinordertime = _settingservice.List().Where(u => u.DinningRoomID == dataObj.DinningRoomID && u.MealTimeType == dataObj.MealTime).ToList()[0];


                if (_tmpbalance != null)
                    yktbalance = _tmpbalance.BelAmount.Value;
                else
                    yktbalance = 0;
                afterOrderBalance = yktbalance - _tmppackageinfo.PackagePrice;
                ordernum = DateTime.Now.ToString("yyyyMMddHHmmssfff") + _tmpuserbaseinfo.MealCardID;

                Obj.UserBaseinfoID = _tmpuserbaseinfo.Id;
                Obj.UserName = _tmpuserbaseinfo.RealName;
                Obj.LoginName = _tmpuserbaseinfo.LoginName;
                Obj.MealCardID = _tmpuserbaseinfo.MealCardID;
                Obj.OrderState = 1;
                Obj.OrderStateName = "订餐";
                Obj.Discription = "";
                Obj.AfterOrderBalance = afterOrderBalance;
                if (dataObj.MealTime == "早餐" && dataObj.OrderType != "P")//如果为早餐，则天数加1
                {
                    Obj.DinningDate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
                }
                else
                {
                    Obj.DinningDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                }
                Obj.Flag = 0;
                Obj.MealPlaceID = dataObj.MealPlace;
                Obj.MealPlaceName = _mealplaceinfo.MealPlaceName;
                Obj.PackageID = dataObj.MealPackage;
                Obj.PackageName = _tmppackageinfo.PackageName;
                Obj.PackagePrice = _tmppackageinfo.PackagePrice;
                Obj.MealTimeType = dataObj.MealTime;
                Obj.MealTimeID = GetText(dataObj.MealTime);
                Obj.DinningRoomID = dataObj.DinningRoomID;
                Obj.DinningRoomName = _tmpdiningroom.DinningRoomName;

                UBObj.UserBaseinfoID = _tmpuserbaseinfo.Id;
                UBObj.Balance = afterOrderBalance;
                UBObj.UserName = _tmpuserbaseinfo.RealName;
                UBObj.LoginName = _tmpuserbaseinfo.LoginName;
                UBObj.MealCardID = _tmpuserbaseinfo.MealCardID;
                UBObj.Payments = _tmppackageinfo.PackagePrice;
                UBObj.Reason = _tmppackageinfo.PackageName;
                UBObj.RecordType = "订餐扣费";



                if (dataObj.OrderType == "P")//用户是批量订餐的情况
                {
                    string typeDescription = "";
                    decimal totalPrice = 0;
                    int bType = 0;
                    DateTime beginDT = Convert.ToDateTime(dataObj.StartDate.Value.ToString("yyyy-MM-dd") + " 00:00:00");
                    DateTime DTNow = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00");
                    DateTime endDT = DateTime.Now;
                    switch (dataObj.OrderBook)
                    {
                        case "Week":
                            endDT = beginDT.AddDays(7);
                            typeDescription = "一周内";
                            bType = 1;
                            break;
                        case "Month":
                            endDT = beginDT.AddMonths(1);
                            typeDescription = "一月内";
                            bType = 2;
                            break;
                    }
                    totalPrice = _mealpackageservice.FindById(dataObj.MealPackage).PackagePrice * (endDT - beginDT).Days;
                    BObj.OrderNum = ordernum;
                    BObj.UserBaseinfoID = _tmpuserbaseinfo.Id;
                    BObj.UserName = _tmpuserbaseinfo.RealName;
                    BObj.LoginName = _tmpuserbaseinfo.LoginName;
                    BObj.MealCardID = _tmpuserbaseinfo.MealCardID;
                    BObj.MealPlaceID = dataObj.MealPlace;
                    BObj.MealPlaceName = _mealplaceinfo.MealPlaceName;
                    BObj.PackageID = dataObj.MealPackage;
                    BObj.PackageName = _tmppackageinfo.PackageName;
                    BObj.PackagePrice = _tmppackageinfo.PackagePrice;
                    BObj.MealTimeType = dataObj.MealTime;
                    BObj.MealTimeID = GetText(dataObj.MealTime);
                    BObj.DinningRoomID = dataObj.DinningRoomID;
                    BObj.DinningRoomName = _tmpdiningroom.DinningRoomName;
                    BObj.OrderState = 1;
                    BObj.OrderStateName = "预定" + dataObj.MealTime;
                    BObj.Discription = "预定" + typeDescription;
                    BObj.BookType = bType;
                    BObj.Flag = 0;
                    BObj.AfterOrderBalance = yktbalance;
                    BObj.DinningDate = Convert.ToDateTime(beginDT.ToString("yyyy-MM-dd 00:00:00"));
                    BObj.StartedDate = Convert.ToDateTime(beginDT.ToString("yyyy-MM-dd 00:00:00"));
                    BObj.EndDate = Convert.ToDateTime(endDT.ToString("yyyy-MM-dd 23:59:59"));
                    SqlParameter[] param = { new SqlParameter("@UserBaseinfoID",_tmpuserbaseinfo.Id)
                                   ,new SqlParameter("@MealTimeType",dataObj.MealTime)
                                   ,new SqlParameter("@DinningRoomID",dataObj.DinningRoomID)
                                   ,new SqlParameter("@StartedDate",beginDT.ToString("yyyy-MM-dd 00:00:00"))
                                   ,new SqlParameter("@EndDate",endDT.ToString("yyyy-MM-dd 23:59:59"))};
                    string bookSql = @"SELECT [ID],[OrderNum],[UserBaseinfoID] ,[UserName],[loginname],[MealCardID],[DinningRoomName],[PackageID],[PackageName],[PackagePrice],[MealTimeType],[MealPlaceName],[OrderTime],[OrderState],[OrderStateName],[Discription],[DinningDate],[StartedDate],[EndDate],[BookType],[flag]
  FROM [dbo].[OrderMeal_BookOrder] where UserBaseinfoID=@UserBaseinfoID and MealTimeType=@MealTimeType and DinningRoomID=@DinningRoomID and (@StartedDate between StartedDate and EndDate or @EndDate between StartedDate and EndDate) and OrderState=1";
                    DataTable bookDT = SqlHelper.ExecuteDataset(CommandType.Text, bookSql, param).Tables[0];
                    if (bookDT.Rows.Count > 0)
                    {
                        tip = "该时间段内您已经预定过套餐了。";
                    }
                    else
                    {
                        BObj.OrderTime = DateTime.Now;
                        _orderbookmealservice.Insert(BObj);
                        if (Math.Round(yktbalance, 2) < Math.Round(totalPrice, 2))
                        {
                            message = message + "预订餐成功" + "(您当前的的余额为" + Math.Round(yktbalance, 2) + "元，不足以支付预定费用,请及时充值。)";
                        }
                        else
                        {
                            message = message + "预订餐成功。";
                        }

                        if (beginDT == DTNow && dataObj.MealTime != "早餐")//如果用户在批量订餐时，是在截止订餐前且开始时间是当天的话，自动加入今天的对应餐时订餐
                        {
                            //获取所选食堂餐段的订餐时间
                            string sqlinordertime = string.Format(@"select convert(varchar(12) , getdate(), 108),tomdrmts.BeginTime,tomdrmts.EndTime,dr.DinningRoomName,tomdrmts.MealTimeType,* from 
OrderMeal_UserBaseinfo tomub join OrderMeal_UserDinningRoom tomudr on tomub.ID=tomudr.UserBaseinfoID join
OrderMeal_DinningRoomMealTimeSettings tomdrmts on tomudr.DinningRoomID=tomdrmts.DinningRoomID 
left join OrderMeal_DinningRoom dr on tomudr.DinningRoomID=dr.Id
where 
tomub.ID='{0}' and tomdrmts.MealTimeType='{1}' and tomudr.DinningRoomID='{2}' and ((tomdrmts.BeginTime<convert(varchar(12) , getdate(), 108) and tomdrmts.EndTime>convert(varchar(12) , getdate(), 108)) or(tomdrmts.BeginTime>tomdrmts.EndTime and tomdrmts.BeginTime>convert(varchar(12) , getdate(), 108) and tomdrmts.EndTime>convert(varchar(12) , getdate(), 108)))", _tmpuserbaseinfo.Id, dataObj.MealTime, dataObj.DinningRoomID);
                            DataTable inordertime = SqlHelper.ExecuteDataset(sqlinordertime).Tables[0];
                            if (inordertime.Rows.Count > 0)
                            {
                                //判断是否为已定餐时
                                string sqlisominfo = string.Format("select * from OrderMeal_MealOrder where convert(varchar(10),[DinningDate],120)=CONVERT(varchar(10),GETDATE(),120) and [OrderState]<>0 and UserBaseinfoID='{0}' and MealTimeType='{1}'", _tmpuserbaseinfo.Id, dataObj.MealTime);
                                DataTable isominfo = SqlHelper.ExecuteDataset(sqlisominfo).Tables[0];
                                if (isominfo.Rows.Count > 0)
                                {
                                    //message = message + "您已经订购了[" + _tmppackageinfo.PackageName + "]的[" + dataObj.MealTime + "]套餐。";
                                }
                                else
                                {
                                    if (yktbalance >= _tmppackageinfo.PackagePrice)
                                    {
                                        Obj.Discription = "系统即时订餐";
                                        Obj.OrderTime = DateTime.Now;
                                        Obj.OrderNum = DateTime.Now.ToString("yyyyMMddHHmmssfff") + _tmpuserbaseinfo.MealCardID;
                                        UBObj.RecordTime = Obj.OrderTime;
                                        _tmpuserbaseinfo.Balance = afterOrderBalance;
                                        _ordermealservice.Insert(Obj);
                                        _useraccountbalanceservice.Insert(UBObj);
                                        _userbaseinfoservice.Update(_tmpuserbaseinfo);
                                        message = message + string.Format("订餐成功！[{0}]的订餐前余额[{1}]（元），订餐扣费金额[{2}]（元），餐卡所剩余额为：[{3}]（元）。", this.WorkContext.CurrentUser.RealName, yktbalance, Convert.ToDouble(_tmppackageinfo.PackagePrice).ToString("0.00"), _tmpuserbaseinfo.Balance);
                                        #region 短信发送逻辑
                                        //短信内容
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendFormat("{0},您好\r\n您已成功预订{1}{2},套餐类型:{3}\r\n用餐地点:{6}食堂{4}\r\n如想退订,请在{5}前回复X进行退订;",
                                            this.WorkContext.CurrentUser.RealName, Obj.DinningDate.ToString("yyyy-MM-dd"), dataObj.MealTime, _tmppackageinfo.PackageName, _mealplaceinfo.MealPlaceName, _tmpdrinordertime.ClosePayBackTime, _tmpdiningroom.DinningRoomName);
                                        sb.Append("\r\n您本次订餐花费").Append(Convert.ToDouble(_tmppackageinfo.PackagePrice)).Append("元,当次订餐后余额为").Append(Convert.ToDouble(afterOrderBalance)).Append("元");
                                        CommonFunction.SendSms(this.WorkContext.CurrentUser.Mobile.Split(',').ToList(), sb.ToString());
                                        #endregion
                                    }
                                    else
                                    {
                                        message = message + "订餐余额不足。今日[" + dataObj.MealTime + "]的预订即时无法生效！";
                                    }
                                }
                            }
                            else
                            {
                                message = message + "您不在[" + _tmpdiningroom.DinningRoomName + "]的[" + dataObj.MealTime + "]的订购时间内。今日[" + dataObj.MealTime + "]的预订即时无法生效！";
                            }
                        }


                    }
                }
                else//用户是单次订餐的情况
                {
                    //判断是否在订餐时间
                    string sqlinordertime = string.Format(@"select convert(varchar(12) , getdate(), 108),tomdrmts.BeginTime,tomdrmts.EndTime,dr.DinningRoomName,tomdrmts.MealTimeType,* from 
OrderMeal_UserBaseinfo tomub join OrderMeal_UserDinningRoom tomudr on tomub.ID=tomudr.UserBaseinfoID join
OrderMeal_DinningRoomMealTimeSettings tomdrmts on tomudr.DinningRoomID=tomdrmts.DinningRoomID 
left join OrderMeal_DinningRoom dr on tomudr.DinningRoomID=dr.Id
where 
tomub.ID='{0}' and tomdrmts.MealTimeType='{1}' and tomudr.DinningRoomID='{2}' and ((tomdrmts.BeginTime<convert(varchar(12) , getdate(), 108) and tomdrmts.EndTime>convert(varchar(12) , getdate(), 108)) or(tomdrmts.BeginTime>tomdrmts.EndTime and tomdrmts.BeginTime>convert(varchar(12) , getdate(), 108) and tomdrmts.EndTime>convert(varchar(12) , getdate(), 108)))", _tmpuserbaseinfo.Id, dataObj.MealTime, dataObj.DinningRoomID);
                    DataTable inordertime = SqlHelper.ExecuteDataset(sqlinordertime).Tables[0];
                    if (inordertime.Rows.Count == 0)
                    {
                        tip = "您不在[" + _tmpdiningroom.DinningRoomName + "]的[" + dataObj.MealTime + "]的订购时间内。请于" + _tmpdrinordertime.BeginTime + "至" + _tmpdrinordertime.EndTime + "时段订购。";
                    }
                    else
                    {
                        //判断是否为已定餐时
                        string sqlisominfo = string.Format("select * from OrderMeal_MealOrder where convert(varchar(10),[OrderTime],120)=CONVERT(varchar(10),GETDATE(),120) and [OrderState]<>0 and UserBaseinfoID='{0}' and MealTimeType='{1}'", _tmpuserbaseinfo.Id, dataObj.MealTime);
                        DataTable isominfo = SqlHelper.ExecuteDataset(sqlisominfo).Tables[0];
                        if (isominfo.Rows.Count > 0)
                        {
                            tip = tip + "您已经订购了[" + _tmppackageinfo.PackageName + "]的[" + dataObj.MealTime + "]套餐，请不要重复订购[" + dataObj.MealTime + "]餐时的同类套餐。";
                        }
                        else
                        {
                            if (yktbalance >= _tmppackageinfo.PackagePrice)
                            {
                                Obj.OrderNum = DateTime.Now.ToString("yyyyMMddHHmmssfff") + _tmpuserbaseinfo.MealCardID;
                                Obj.OrderTime = DateTime.Now;
                                UBObj.RecordTime = Obj.OrderTime;
                                _tmpuserbaseinfo.Balance = afterOrderBalance;
                                _ordermealservice.Insert(Obj);
                                _useraccountbalanceservice.Insert(UBObj);
                                _userbaseinfoservice.Update(_tmpuserbaseinfo);
                                message = string.Format("订餐成功！[{0}]的订餐前余额[{1}]（元），订餐扣费金额[{2}]（元），餐卡所剩余额为：[{3}]（元）。", this.WorkContext.CurrentUser.RealName, yktbalance, Convert.ToDouble(_tmppackageinfo.PackagePrice).ToString("0.00"), _tmpuserbaseinfo.Balance);
                                #region 短信发送逻辑
                                //短信内容
                                StringBuilder sb = new StringBuilder();
                                sb.AppendFormat("{0},您好\r\n您已成功预订{1}{2},套餐类型:{3}\r\n用餐地点:{6}食堂{4}\r\n如想退订,请在{5}前回复X进行退订;",
                                    this.WorkContext.CurrentUser.RealName, Obj.DinningDate, dataObj.MealTime, _tmppackageinfo.PackageName, _mealplaceinfo.MealPlaceName, _tmpdrinordertime.ClosePayBackTime, _tmpdiningroom.DinningRoomName);
                                sb.Append("\r\n您本次订餐花费").Append(Convert.ToDouble(_tmppackageinfo.PackagePrice)).Append("元,当次订餐后余额为").Append(Convert.ToDouble(afterOrderBalance)).Append("元");
                                CommonFunction.SendSms(this.WorkContext.CurrentUser.Mobile.Split(',').ToList(), sb.ToString());
                                #endregion
                            }
                            else
                            {
                                tip = tip + "订餐余额不足。";
                            }
                        }
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
                result.IsSuccess = true;
                result.Message = message;
                _log.Operation = OperationType.Add;
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
        [SysOperation(OperationType.Save, "退订订餐")]
        public ActionResult BackOrder(int[] Ids)
        {
            SysOperationLog _log = new SysOperationLog();
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                bool isSuccess = false;
                string message = "";
                decimal beforeOrderBalance = 0, afterOrderBalance = 0;
                List<OrderMeal_MealOrder> list = new List<OrderMeal_MealOrder>();
                foreach (int id in Ids)
                {
                    var _obj = _ordermealservice.FindById(id);
                    var _timesetobj = _settingservice.FindByFeldName(u => u.DinningRoomID == _obj.DinningRoomID && u.MealTimeType == _obj.MealTimeType);
                    var _tmpuserbaseinfo = _userbaseinfoservice.FindByFeldName(u => u.LoginName == _obj.LoginName && u.State == 1);

                    var _tmppackageinfo = _mealpackageservice.FindById(_obj.PackageID);
                    var _tmpbalance = _balanceservice.FindByFeldName(u => u.JobNumber == _tmpuserbaseinfo.EmployId);
                    TimeSpan _doTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    if (DateTime.Now.ToString("yyyyMMdd") == _obj.OrderTime.ToString("yyyyMMdd"))
                    {
                        if(_timesetobj != null)
                        {
                            if (_doTime.CompareTo(_timesetobj.ClosePayBackTime) < 0 )
                            {
                                OrderMeal_UserAccountBalance UBObj = new OrderMeal_UserAccountBalance();
                                UBObj.UserBaseinfoID = _tmpuserbaseinfo.Id;
                                UBObj.Balance = afterOrderBalance;
                                UBObj.UserName = _tmpuserbaseinfo.RealName;
                                UBObj.LoginName = _tmpuserbaseinfo.LoginName;
                                UBObj.MealCardID = _tmpuserbaseinfo.MealCardID;
                                UBObj.Payments = _tmppackageinfo.PackagePrice;
                                UBObj.Reason = _tmppackageinfo.PackageName;
                                UBObj.RecordType = "取消订餐并退款";
                                UBObj.RecordTime = DateTime.Now;

                                beforeOrderBalance = _tmpuserbaseinfo.Balance.Value;
                                afterOrderBalance = _tmpuserbaseinfo.Balance.Value + _obj.PackagePrice;
                                _tmpuserbaseinfo.Balance = afterOrderBalance;
                                _obj.OrderState = 0;
                                _obj.OrderStateName = "已取消订餐";
                                _obj.BackOrderTime = DateTime.Now;
                                _obj.AfterOrderBalance = afterOrderBalance;
                                _ordermealservice.Update(_obj);
                                _userbaseinfoservice.Update(_tmpuserbaseinfo);
                                _useraccountbalanceservice.Insert(UBObj);
                                message = "您已经成功退订[" + _obj.PackageName + "]套餐。\n您的退餐前餐卡余额为：" + Convert.ToDouble(beforeOrderBalance).ToString("0.00") + "（元），退餐后餐卡余额为：" + Convert.ToDouble(afterOrderBalance).ToString("0.00") + "（元），返还金额为：" + Convert.ToDouble(_obj.PackagePrice).ToString("0.00") + "（元）。";
                                #region 短信发送逻辑
                                //短信内容
                                string msg = "您在" + _obj.DinningRoomName + "食堂" + "订购的[" + _obj.MealTimeType + "] [" + _obj.PackageName + "]已经退订成功。\r\n";
                                msg += Convert.ToDouble(_obj.PackagePrice).ToString("0.00") + "元餐费,已经返还到您的账户。";
                                //发送短信
                                CommonFunction.SendSms(this.WorkContext.CurrentUser.Mobile.Split(',').ToList(), msg.ToString());
                                #endregion 短信发送流程
                                result.IsSuccess = true;
                                result.Message = message;
                                result.data = list.Count;
                                //return Json(new { IsSuccess = true, Message = message, SuccessCount = list.Count });
                            }
                            else
                            {
                                result.IsSuccess = false;
                                result.Message = "您已经超过了[最晚退餐时间：" + _timesetobj.ClosePayBackTime + "]！";
                                //SystemResult result = new SystemResult() { IsSuccess = false, Message = "您已经超过了[最晚退餐时间：" + _timesetobj.ClosePayBackTime + "]！" };
                                //return Json(result);
                            }
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "您已经超过了[最晚退餐时间：]！";
                            //SystemResult result = new SystemResult() { IsSuccess = false, Message = "您已经超过了[最晚退餐时间：" + _timesetobj.ClosePayBackTime + "]！" };
                            //return Json(result);
                        }


                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "您不能退订超过一天以上的订餐！";
                        //SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };
                        //return Json(result);
                    }

                    //if (_doTime.CompareTo(_timesetobj.ClosePayBackTime) >= 0)
                    //{
                    //    SystemResult result = new SystemResult() { IsSuccess = false, Message = "已提交，不能进行修改操作！" };
                    //    return Json(result);
                    //}
                }
                _log.OperationDesc = "退订成功！";
                _log.OperationPage = Request.RawUrl;
                _log.Operation = OperationType.Edit;
                _log.RealName = this.WorkContext.CurrentUser.RealName;
                _log.UserName = this.WorkContext.CurrentUser.UserName;
                _sysLogService.WriteSysLog<SysOperationLog>(_log);
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = "退订失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "退订失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }
        }
        [SysOperation(OperationType.Save, "退订预订餐")]
        public ActionResult BackSubscribeOrder(int[] Ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            SysOperationLog _log = new SysOperationLog();
            try
            {
                bool isSuccess = false;
                int successCount = 0;
                List<OrderMeal_BookOrder> list = new List<OrderMeal_BookOrder>();
                foreach (int id in Ids)
                {
                    var _obj = _orderbookmealservice.FindById(id);
                    var _timesetobj = _settingservice.FindByFeldName(u => u.DinningRoomID == _obj.DinningRoomID);
                    TimeSpan _doTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    if (DateTime.Now.CompareTo(_obj.EndDate) >= 0)
                    {
                        result.IsSuccess = false;
                        result.Message = "所选的数据中有已过了退订时间，不能进行退订操作！";
                        //SystemResult result = new SystemResult() { IsSuccess = false, Message = "所选的数据中有已过了退订时间，不能进行退订操作！" };
                        //return Json(result);
                    }
                    //if (_doTime.CompareTo(_timesetobj.ClosePayBackTime) >= 0)
                    //{
                    //    SystemResult result = new SystemResult() { IsSuccess = false, Message = "已提交，不能进行修改操作！" };
                    //    return Json(result);
                    //}
                    _obj.OrderState = 0;
                    _obj.OrderStateName = "已取消订餐";
                    list.Add(_obj);
                }
                if (_orderbookmealservice.UpdateByList(list))
                {
                    result.IsSuccess = true;
                    result.Message = "退订成功";
                    result.data = list.Count;
                    //return Json(new { IsSuccess = true, Message = "退订成功", SuccessCount = list.Count });
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "退订失败";
                    //return Json(new { IsSuccess = false, Message = "退订失败" });
                }
                _log.OperationDesc = "退订成功！";
                _log.Operation = OperationType.Edit;
                _log.OperationPage = Request.RawUrl;
                _log.RealName = this.WorkContext.CurrentUser.RealName;
                _log.UserName = this.WorkContext.CurrentUser.UserName;
                _sysLogService.WriteSysLog<SysOperationLog>(_log);
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = "退订失败！";// ex.Message;
                //记录错误日志
                _sysLogService.WriteSysLog<SysErrorLog>(new SysErrorLog()
                {
                    ErrorDesc = ex.ToString(),
                    ErrorPage = Request.RawUrl,
                    ErrorTitle = "退订失败",
                    RealName = this.WorkContext.CurrentUser.RealName,
                    UserName = this.WorkContext.CurrentUser.UserName
                });
                return Json(result);
            }





            
        }

        [SysOperation(OperationType.Browse, "访问订餐汇总页面")]
        public ActionResult Static()
        {
            return View();
        }
        public ActionResult GetStaticListData(int pageIndex = 1, int pageSize = 5, OrderMeal_BookOrderQueryBuilder queryBuilder = null)
        {
            int count = 0;
            if (queryBuilder.OrderTime_Start != null)
                queryBuilder.OrderTime_Start = queryBuilder.OrderTime_Start.Value.Date;
            if (queryBuilder.OrderTime_End != null)
                queryBuilder.OrderTime_End = queryBuilder.OrderTime_End.Value.AddDays(1).Date.AddSeconds(-1);
            
            var modelList = _ordermealservice.GetForStaticPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.MealPlaceName,
                    item.MealTimeType,
                    item.DinningRoomName,
                    item.DinningRoomOrderSum,
                    item.PackageName,
                    DinningDate = item.DinningDate.ToString("yyyy-MM-dd")
                });
            }
            return Json(new { items = listResult, count = count });
        }
        [SysOperation(OperationType.Browse, "访问批量订餐明细页面")]
        public ActionResult Subscribe()
        {
            return View();
        }
        public ActionResult GetSubscribeListData(int pageIndex = 1, int pageSize = 5, OrderMeal_BookOrderQueryBuilder queryBuilder = null,string DpId = null)
        {
            int count = 0;
            if (queryBuilder.OrderTime_Start != null)
                queryBuilder.OrderTime_Start = queryBuilder.OrderTime_Start.Value.Date;
            if (queryBuilder.OrderTime_End != null)
                queryBuilder.OrderTime_End = queryBuilder.OrderTime_End.Value.AddDays(1).Date.AddSeconds(-1);
            if (DpId != "")
                queryBuilder.DpId = Get_Subdept_ByDept(DpId);
            else
                queryBuilder.DpId = null;
            var modelList = _orderbookmealservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _userobj = _userService.FindByFeldName(u => u.UserName == item.LoginName);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                listResult.Add(new
                {
                    item.Id,
                    item.MealPlaceName,
                    item.MealTimeType,
                    item.DinningRoomName,
                    item.AfterOrderBalance,
                    item.PackagePrice,
                    item.PackageName,
                    item.OrderStateName,
                    item.UserName,
                    BookType = item.BookType == 1? "一周内（7天）" : "一月内",
                    OrderTime = item.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    PeriodTime= item.StartedDate.ToString("yyyy-MM-dd HH:mm:ss")+ "到" + item.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    DpName = _tmpdplist[0]//DpName = CommonFunction.getDeptNamesByIDs(_userobj.DpId)
                });
            }
            return Json(new { items = listResult, count = count });
        }




        [SysOperation(OperationType.Export, "导出订餐明细")]
        public ActionResult Export(string queryBuilder = null, string DpId = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OrderMeal_BookOrderQueryBuilder>(queryBuilder);
                if (DpId != "")
                    QueryBuilder.DpId = Get_Subdept_ByDept(DpId);
                else
                    QueryBuilder.DpId = null;
                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;


                if (QueryBuilder.OrderTime_Start != null)
                    QueryBuilder.OrderTime_Start = QueryBuilder.OrderTime_Start.Value.Date;
                if (QueryBuilder.OrderTime_End != null)
                    QueryBuilder.OrderTime_End = QueryBuilder.OrderTime_End.Value.AddDays(1).Date.AddSeconds(-1);

                if (_adminservice.List().Where(u => u.Loginname == WorkContext.CurrentUser.UserName).ToList().Count > 0)
                {
                    var _tmpadminobj = _adminservice.FindByFeldName(u => u.Loginname == WorkContext.CurrentUser.UserName);
                    QueryBuilder.DinningRoomID = _tmpadminobj.DinningRoomID.Value;
                }
                else if (WorkContext.CurrentUser.UserName != Base.Admin.UserName)
                {
                    QueryBuilder.LoginName = WorkContext.CurrentUser.UserName;
                }

                var modelList = _ordermealservice.GetForPagingByCondition(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    var _userobj = _userService.FindByFeldName(u => u.UserName == item.LoginName);
                    List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                    var _tmpdinningtimesetting = _settingservice.FindByFeldName(u => u.DinningRoomID == item.DinningRoomID);
                    listResult.Add(new
                    {
                        item.Id,
                        item.MealPlaceName,
                        item.MealTimeType,
                        item.DinningRoomName,
                        item.AfterOrderBalance,
                        item.PackagePrice,
                        item.PackageName,
                        item.OrderStateName,
                        item.UserName,
                        DinningDate = item.DinningDate.ToString("yyyy-MM-dd"),
                        ClosePayBackTime = _tmpdinningtimesetting.ClosePayBackTime.Value,
                        OrderTime = item.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        DpName = _tmpdplist[0]//DpName = CommonFunction.getDeptNamesByIDs(_userobj.DpId)
                    });
                }
                
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.Detail + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();


                //打开模板
                designer.Open(ExportTempPath.Detail);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Detail, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        [SysOperation(OperationType.Export, "导出预订餐明细")]
        public ActionResult SubscribeExport(string queryBuilder = null, string DpId = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OrderMeal_BookOrderQueryBuilder>(queryBuilder);
                if (DpId != "")
                    QueryBuilder.DpId = Get_Subdept_ByDept(DpId);
                else
                    QueryBuilder.DpId = null;
                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;


                if (QueryBuilder.OrderTime_Start != null)
                    QueryBuilder.OrderTime_Start = QueryBuilder.OrderTime_Start.Value.Date;
                if (QueryBuilder.OrderTime_End != null)
                    QueryBuilder.OrderTime_End = QueryBuilder.OrderTime_End.Value.AddDays(1).Date.AddSeconds(-1);

                var modelList = _orderbookmealservice.GetForPagingByCondition(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    var _userobj = _userService.FindByFeldName(u => u.UserName == item.LoginName);
                    List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                    listResult.Add(new
                    {
                        item.Id,
                        item.MealPlaceName,
                        item.MealTimeType,
                        item.DinningRoomName,
                        item.AfterOrderBalance,
                        item.PackagePrice,
                        item.PackageName,
                        item.OrderStateName,
                        item.UserName,
                        BookType = item.BookType == 1 ? "一周内（7天）" : "一月内",
                        OrderTime = item.OrderTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        PeriodTime = item.StartedDate.ToString("yyyy-MM-dd HH:mm:ss") + "到" + item.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        DpName = _tmpdplist[0]//CommonFunction.getDeptNamesByIDs(_userobj.DpId)
                    });
                }

                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.SubscribeDetail + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();


                //打开模板
                designer.Open(ExportTempPath.SubscribeDetail);
                //设置集合变量
                designer.SetDataSource(ImportFileType.SubscribeDetail, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }

        [SysOperation(OperationType.Export, "导出订餐汇总")]
        public ActionResult StaticExport(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OrderMeal_BookOrderQueryBuilder>(queryBuilder);
                
                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;


                if (QueryBuilder.OrderTime_Start != null)
                    QueryBuilder.OrderTime_Start = QueryBuilder.OrderTime_Start.Value.Date;
                if (QueryBuilder.OrderTime_End != null)
                    QueryBuilder.OrderTime_End = QueryBuilder.OrderTime_End.Value.AddDays(1).Date.AddSeconds(-1);

                var modelList = _ordermealservice.GetForStaticPagingByCondition(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    listResult.Add(new
                    {
                        item.MealPlaceName,
                        item.MealTimeType,
                        item.DinningRoomName,
                        item.DinningRoomOrderSum,
                        item.PackageName,
                        DinningDate = item.DinningDate.ToString("yyyy-MM-dd")
                    });
                }
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.Static + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();


                //打开模板
                designer.Open(ExportTempPath.Static);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Static, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }


        #region 其他方法
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
        public ActionResult getDinningRoomAsDict(string UserBaseinfoId)
        {
            var _tmpUserBaseinfoId = Guid.Parse(UserBaseinfoId);
            var objList = _userdinningroomservice.List().Where(u => u.UserBaseinfoID == _tmpUserBaseinfoId).Select(u => u.DinningRoomID).ToList();
            var objs = _diningroomservice.List().Where(u => objList.Contains(u.Id)).ToList();
            List<object> itemResult = new List<object>();
            foreach (var model in objs)
            {
                itemResult.Add(new
                {
                    DinningRoomID = model.Id,
                    DinningRoomName = model.DinningRoomName
                });
            }
            var defaultValue = objs[0].Id;
            return Json(new { items = itemResult, defaultvalue = defaultValue }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getMealTimeAsDict(string DinningRoomID)
        {
            var _tmpDinningRoomID = Guid.Parse(DinningRoomID);
            var objList = _settingservice.List().Where(u => u.DinningRoomID == _tmpDinningRoomID && u.State == 1).OrderBy(u=>u.MealTimeID).Select(u => new { u.MealTimeType}).ToList();
            var defaultValue = getDefalut(Guid.Parse(DinningRoomID));
            return Json(new { items = objList, defaultvalue = defaultValue }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getMealPlaceAsDict(string DinningRoomID)
        {
            var _tmpDinningRoomID = Guid.Parse(DinningRoomID);
            var defaultValue = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var objList = _mealplaceservice.List().Where(u => u.DinningRoomID == _tmpDinningRoomID).Select(u => new { u.MealPlaceName, MealPlaceId = u.Id }).ToList();
            if(objList.Count>0)
                defaultValue = objList[0].MealPlaceId;
            return Json(new { items = objList, defaultvalue = defaultValue }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getMealPackageAsDict(string DinningRoomID, string MealTimeType)
        {
            var _tmpDinningRoomID = Guid.Parse(DinningRoomID);
            var objList = _mealpackageservice.List().Where(u => u.DinningRoomID == _tmpDinningRoomID && u.MealTimeType == MealTimeType).OrderBy(u => u.CreatedTime).ToList();
            List<object> itemResult = new List<object>();
            foreach (var model in objList)
            {
                itemResult.Add(new
                {
                    PackageId = model.Id,
                    PackageName = model.PackageName + "  (价格 : " + model.PackagePrice+ "元 )"
                });
            }
            var defaultValue = objList[0].Id;
            return Json(new { items = itemResult, defaultvalue = defaultValue }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getOrderBookAsDict(string DinningRoomID,string MealTimeType)
        {
            var _tmpDinningRoomID = Guid.Parse(DinningRoomID);
            var objList = _bookservice.List().Where(u => u.DinningRoomID == _tmpDinningRoomID && u.MealTimeType == MealTimeType).ToList();
            List<object> itemResult = new List<object>();
            foreach (var model in objList)
            {
                if(model.Week == 1)
                {
                    itemResult.Add(new
                    {
                        BookId = "Week",
                        BookName = "一周内（7天）"
                    });
                }
                if (model.Month == 1)
                {
                    itemResult.Add(new
                    {
                        BookId = "Month",
                        BookName = "一月内"
                    });
                }
            }
            var defaultValue = "Week";
            return Json(new { items = itemResult, defaultvalue = defaultValue }, JsonRequestBehavior.AllowGet);
        }

        public string getDefalut(Guid DinningRoomID)
        {
            DateTime time1 = Convert.ToDateTime("0:00:00");
            DateTime time2 = Convert.ToDateTime(DateTime.Now.ToString());
            TimeSpan TS = new TimeSpan(time2.Ticks - time1.Ticks);
            int Time = (int)TS.TotalHours;
            string str = "早餐";
            if (Time < 12)
            {
                str = "中餐";
            }
            else if (Time >= 12 && Time < 18)
            {
                str = "晚餐";
            }
            else if (Time >= 18 && Time < 23)
            {
                str = "早餐";
            }
            var checklist=_settingservice.List().Where(u => u.DinningRoomID == DinningRoomID && u.State == 1&&u.MealTimeType== str).ToList();
            if(checklist.Count<=0)
                str = "早餐";
            return str;
        }
        public ActionResult getBookOrderStateAsDict()
        {
            var objList = _orderbookmealservice.List().Select(u => new { u.OrderStateName }).Distinct().ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getOrderStateAsDict()
        {
            var objList = _ordermealservice.List().Select(u => new { u.OrderStateName }).Distinct().ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 根据部门组织Ids获取包括自身的所有子节点的id
        /// </summary>
        /// <param name="ids">组织Ids</param>
        /// <returns></returns>
        private List<string> Get_Subdept_ByDept(string ids)
        {
            List<string> listResult = new List<string>();
            if (ids == null)
                return listResult;
            string[] temp = ids.Split(',');
            for (int i = 0; i < temp.Length; i++)
            {
                var mm = new EfRepository<string>().Execute<string>(string.Format("select * from  dbo.Get_Subdept_ByDept ('{0}')", temp[i].ToString())).ToList();
                listResult.AddRange(mm);
            }
            return listResult;
        }
        #endregion
    }
}