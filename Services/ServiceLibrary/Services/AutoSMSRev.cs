using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Service.Administrative.Dinning;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class AutoSMSRev : ServiceJob
    {
        ISysUserService _sysuserservice = new SysUserService();
        IOrderMeal_CommandService _mealcommandservice = new OrderMeal_CommandService();
        IOrderMeal_MenuPackageCommandService _menupackagecommandservice = new OrderMeal_MenuPackageCommandService();
        IOrderMeal_MealOrderService _ordermealservice = new OrderMeal_MealOrderService();        
        IOrderMeal_UserBaseinfoService _userbaseinfoservice = new OrderMeal_UserBaseinfoService();
        string msghead = "[综合管理平台-订餐系统]\r\n";
        /// <summary>
        /// 短信处理
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            #region 查询当前服务策略信息
            string sTemp = "";
            if (!SetStrategyInfo(out sTemp))
            {
                sMessage = sTemp;
                return false;
            }
            #endregion
            AutoSMSRevMessage();
            return true;
        }
        public void AutoSMSRevMessage()
        {
            LogRecord.WriteCanteenServiceLog("=========开始处理回复的短信===============", LogResult.normal);
            SmsDAO.init(ConfigData.SMS_Connection);//初始化短信系统数据库连接字符串
            string SMS_Port = ConfigData.SMS_Port;
            string ids = "";
            string smsquery = "select * from SMSRev where Deal='-1' and DestAddr like '" + SMS_Port + "_' and DATEDIFF(day,revtime,GETDATE())<1";
            DataTable SMSRevDt = SmsDAO.sqlTable(smsquery);
            if(SMSRevDt.Rows.Count>0)
            {
                ids = DealMessage(SMSRevDt); 
            }
            //已处理的短信审批Deal字段标识-1改为1            
            if (ids != null && ids != "")
            {
                string updatesmsstatus = "update SMSRev set Deal='1',DealTime=getdate() where id in(" + ids + ")";
                SmsDAO.sqlExec(updatesmsstatus);
            }
            AddStrategyLog(string.Format("处理短信结束：处理{0}条", SMSRevDt.Rows.Count), true);
            SaveStrategyLog();
            LogRecord.WriteCanteenServiceLog("=========结束处理短信结束===============", LogResult.normal);
        }
        public string DealMessage(DataTable smsrevdt)
        {
            string ids = "";
            try
            {
                foreach (DataRow row in smsrevdt.Rows)
                {
                    #region 声明变量
                    string mobile = row["OrgAddr"].ToString().Trim().Substring(2);      //手机号
                    string iport = row["DestAddr"].ToString();     //端口号+01-99
                    string UserData = row["UserData"].ToString().Trim();    //短信内容
                    string recvtime = row["revtime"].ToString().Trim();
                    string smID = row["id"].ToString();
                    string userName = GetRealName(mobile);
                    string type = iport.Substring(iport.Length - 1);
                    string OriginalUserData = UserData;
                    string PreStr = "";
                    string update = "";
                    int mealtimetype = 0;
                    string WorkingDate = Convert.ToDateTime(row["revtime"].ToString().Trim()).ToString("yyyy-MM-dd 00:00:00");
                    #endregion
                    #region 读取短信内容
                    var D = UserData.Substring(0, 1);
                    if (UserData.Substring(0,1).ToLower() == "p")
                    {
                        UserData = UserData.Substring(1);
                        PreStr = "P";
                        WorkingDate = Convert.ToDateTime(WorkingDate).AddDays(1).ToString("yyyy-MM-dd 00:00:00");
                    }
                    if (UserData.ToLower() == "x")
                    {
                        //取消订餐  1.判断能否成功，成功-改状态-退款-发短信  失败-发短信
                        string context = Backorders(mobile, Convert.ToDateTime(recvtime), type, userName, WorkingDate);
                        LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "     Mobile:" + mobile + "用户退餐！" + context + "\r\n[RecvTime:" + recvtime + ",UserData:" + UserData + "]==========", LogResult.normal);
                        CommonFun.SendSms(mobile, context);
                        ids += smID + ",";
                    }
                    else
                    {
                        //订餐
                        string context = DealSortNameByNum(mobile, Convert.ToDateTime(recvtime), UserData, type, PreStr, WorkingDate, out mealtimetype);
                        LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "     Mobile:" + mobile + "用户订餐！" + context + "\r\n[RecvTime:" + recvtime + ",UserData:" + UserData + "]==========", LogResult.normal);
                        CommonFun.SendSms(mobile, context);
                        ids += smID + ",";
                    }
                    update = string.Format(@"update ordermsg_log set [state]=1 where UserId='{0}' and num={1} and WorkingDate='{2}'", UserBaseinfoID(mobile), mealtimetype, WorkingDate);

                    SqlHelper.ExecuteNonQuery(update);
                    #endregion



                }
            }
            catch (Exception eIds)
            {
                LogRecord.WriteCanteenServiceLog(eIds.ToString(), LogResult.error);
                LogRecord.WriteLog(string.Format("{0}:短信处理出错:{1}", strCurStrategyInfo, eIds.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:短信处理出错:{1}", strCurStrategyInfo, eIds.Message), false);
            }
            ids = ids.Trim(',');
            return ids;
        }
        string Backorders(string num, DateTime RevTime, string type, string username, string WorkingDate)
        {
            decimal yktbalance = 0;
            decimal afterOrderBalance = 0;
            StringBuilder sb = new StringBuilder(GetMsgHeadBynum(num));
            string query = "";
            query = string.Format(@"SELECT a.DinningDate,a.id,a.PackagePrice,a.DinningRoomName,a.MealTimeType,a.PackageName,a.UserBaseinfoID,a.DinningRoomID,a.MealPlaceID,a.PackageID,b.Balance as AfterOrderBalance,b.employid ,a.MealTimeID 
FROM OrderMeal_MealOrder a 
join OrderMeal_UserBaseinfo b ON a.UserBaseinfoID=b.ID 
join bw_Users usr on b.loginname=usr.UserName
JOIN OrderMeal_DinningRoomMealTimeSettings c ON a.DinningRoomID=c.DinningRoomID AND a.MealTimeType=c.MealTimeType
JOIN ordermsg_log d ON d.userId=b.ID AND a.DinningDate=d.WorkingDate 
JOIN OrderMeal_Menu e ON d.roomId=e.ID AND e.DinningRoomID=c.DinningRoomID
WHERE 
DATEDIFF(DAY,a.DinningDate,'{2}')=0 AND 
OrderState=1
  and usr.mobile='{0}' 
 and (
 (ClosePayBackTime>'{1}' and BeginTime < '{1}' ) 
 or  (ClosePayBackTime>BeginTime and '{1}'<ClosePayBackTime and '{1}'>EndTime) 
 or(ClosePayBackTime<BeginTime  
 and ((BeginTime < '{1}') or( '{1}' <ClosePayBackTime)) 
 )
 )", num, RevTime.ToString("HH:mm:ss"), Convert.ToDateTime(WorkingDate), type);
            //查询当日订单
            DataTable dt = SqlHelper.ExecuteDataset(query).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                int ordermealid =Convert.ToInt16( dt.Rows[0]["id"].ToString());
                Guid UserBaseinfoID = Guid.Parse(dt.Rows[0]["UserBaseinfoID"].ToString());
                var _tmpordermeal = _ordermealservice.FindById(ordermealid);
                var _tmpuserbaseinfo = _userbaseinfoservice.FindById(UserBaseinfoID);
                _tmpordermeal.OrderState = 0;
                _tmpordermeal.OrderStateName = "已取消订餐";
                _tmpordermeal.BackOrderTime = DateTime.Now;
                _tmpuserbaseinfo.Balance = _tmpuserbaseinfo.Balance + Convert.ToDecimal(dt.Rows[0]["PackagePrice"].ToString());
                _ordermealservice.Update(_tmpordermeal);
                _userbaseinfoservice.Update(_tmpuserbaseinfo);
                string EmployId= dt.Rows[0]["employid"].ToString();
                IView_EXT_XF_AccountService _balanceservice = new View_EXT_XF_AccountService();
                var _tmpbalance = _balanceservice.FindByFeldName(u => u.JobNumber == EmployId);
                if (_tmpbalance != null)
                    yktbalance = _tmpbalance.BelAmount.Value;
                else
                    yktbalance = 0;
                UpdateBookAfterBalance(dt.Rows[0], yktbalance);
                sb.Append("您在" + dt.Rows[0]["DinningRoomName"] + "食堂" + "订购的" + dt.Rows[0]["MealTimeType"] + "已经退订成功。\r\n");
                sb.Append("您的账户金额" + Convert.ToDouble(yktbalance).ToString("0.00") + "元。");
            }
            else
            {
                sb.Append("您当前没有可以退订的餐次");
            }
            return sb.ToString();
        }

        string DealSortNameByNum(string num, DateTime RevTime, string CD, string type,string PreStr, string WorkingDate,out int mealtimetype)
        {
            string head = GetMsgHeadBynum(num);
            double days = 0;
            mealtimetype = 0;
            //按时间段判断用餐类型
            string meatTimeTypequery = string.Format(@"select e.MealTimeType from orderMeal_Command a
inner join OrderMeal_Package b on a.PackageId=b.ID
inner join OrderMeal_MealPlace c on a.PlaceId=c.ID and b.DinningRoomID=c.DinningRoomID
inner join OrderMeal_DinningRoom d on b.DinningRoomID=d.ID
inner join OrderMeal_DinningRoomMealTimeSettings e on d.ID=e.DinningRoomID and b.MealTimeType=e.MealTimeType
where (
	(SmsTime <'{0}' and LastSmsTime > '{0}' )  
 or (SmsTime < '23:59:59' and LastSmsTime >'00:00:00' and SmsTime>LastSmsTime   and			(
				(SmsTime < '{0}' and '{0}' <'23:59:59')or		('00:00:00' <'{0}' and '{0}' <LastSmsTime))	
				)
 ) and lower(command)='{1}'", RevTime.ToString("HH:mm:ss"), CD.ToLower());
            DataTable dt = SqlHelper.ExecuteDataset(meatTimeTypequery).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["MealTimeType"].ToString())
                {
                    case "早餐": { type = "1"; days = 1; } break;
                    case "中餐": { type = "2"; days = 0; } break;
                    case "晚餐": { type = "3"; days = 0; } break;
                    case "宵夜": { type = "4"; days = 0; } break;
                    default: type = "-1"; break;
                }
                mealtimetype = Convert.ToInt16(type);
                
            }
            var checkcommand = _mealcommandservice.List().Where(u => u.Command == CD).ToList();
            if(checkcommand.Count<=0)
            {
                return head + "对不起，您回复格式不正确,请按指定格式回复";
            }
            
            DataRow row = GetMeal(num, RevTime, CD, type, WorkingDate,days);
            if (row == null)
            {
                return head + "对不起，套餐编号错误，或不在允许订餐的时间范围";
            }
            else
            {
                decimal yktbalance = 0;
                decimal afterOrderBalance = 0;
                string MealTimeType = row["MealTimeType"].ToString();
                DateTime DinningDate=Convert.ToDateTime( row["WorkingDate"].ToString());
                string EmployId = GetEmployId(num);
                string loginname = GetLoginName(num);
                var _checkhasorder = _ordermealservice.List().Where(u => u.LoginName == loginname && u.MealTimeType == MealTimeType && u.DinningDate == DinningDate && u.OrderState == 1).ToList();
                IView_EXT_XF_AccountService _balanceservice = new View_EXT_XF_AccountService();
                var _tmpbalance = _balanceservice.FindByFeldName(u => u.JobNumber == EmployId);
                if (_tmpbalance != null)
                    yktbalance = _tmpbalance.BelAmount.Value;
                else
                    yktbalance = 0;
                if (_checkhasorder.Count>0)
                {
                    return head + "您已经订购了该餐次的套餐,如需重新订购,请先回复" + PreStr + "X退订成功后再订购";
                }
                else
                {
                    if (Convert.ToDouble(yktbalance) - Convert.ToDouble(row["PackagePrice"]) < 0)
                    {
                        return head + string.Format("对不起,您在{0}订购的{1}{2},需要{3}元,您当前余额为{4}元,不足够订餐,请及时充值", row["DinningRoomName"], Convert.ToDateTime(row["WorkingDate"]).ToString("yy-MM-dd"), row["MealTimeType"], Convert.ToDouble(row["PackagePrice"]).ToString("0.00"), Convert.ToDouble(yktbalance).ToString("0.00"));
                    }else
                    {
                        afterOrderBalance = yktbalance - Convert.ToDecimal(row["PackagePrice"]);
                        //订单表插，扣费
                        if (InsertMeal(num, RevTime, CD, row, afterOrderBalance))
                        {
                            StringBuilder sb = new StringBuilder(head);
                            sb.AppendFormat("您已成功预订{1}{2},套餐类型:{3}\r\n用餐地点:{4}\r\n如想退订,请在{5}前回复{7}X进行退订；", row["RealName"], Convert.ToDateTime(row["WorkingDate"]).ToString("yyyy-MM-dd"), row["MealTimeType"], row["PackageName"], row["MealPlaceName"], string.Format("{0:T}", row["ClosePayBackTime"]), row["DinningRoomName"], PreStr);
                            sb.Append("\r\n您的账户金额").Append(Convert.ToDouble(yktbalance).ToString("0.00")).Append("元，您已预订").Append(Convert.ToDouble(row["PackagePrice"]).ToString("0.00")).Append("元的套餐。");
                            return sb.ToString();
                        }else
                        {
                            return head + "系统错误,请与管理员联系";
                        }

                    }
                }
            }

        }
        bool UpdateBookAfterBalance(DataRow row, decimal afterordbalance)
        {
            string sql = string.Format(@"update OrderMeal_BookOrder set AfterOrderBalance={0} where MealTimeType = '{1}' and UserBaseinfoID ='{2}'
  and PackageID='{3}'
 and MealPlaceID='{4}' and '{5}' between StartedDate and EndDate", afterordbalance
                                                                     , row["MealTimeType"]
                                                                     , row["UserBaseinfoID"]
                                                                     , row["PackageID"]
                                                                     , row["MealPlaceID"]
                                                                     , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            int i = SqlHelper.ExecuteNonQuery(sql);
            return i == 0;
        }
        bool InsertMeal(string num, DateTime RevTime, string CD, DataRow row, decimal afterorbalance)
        {
            int i = 0;
            bool IsSuccess = false;
            Guid UserBaseinfoID = Guid.Parse(row["UserBaseinfoID"].ToString());
            string MealTimeType = row["MealTimeType"].ToString();
            DateTime DinningDate = Convert.ToDateTime(row["WorkingDate"].ToString());
            var _checkhasorder = _ordermealservice.List().Where(u => u.UserBaseinfoID == UserBaseinfoID && u.MealTimeType == MealTimeType && u.DinningDate == DinningDate && u.OrderState == 1).ToList();
            //string sqlisominfo = string.Format("select * from OrderMeal_MealOrder where convert(varchar(10),[OrderTime],120)=CONVERT(varchar(10),GETDATE(),120) and [OrderState]<>0 and UserBaseinfoID='{0}' and MealTimeType='{1}'", row["UserBaseinfoID"], row["MealTimeType"]);
            //DataTable isominfo = SqlHelper.ExecuteDataset(sqlisominfo).Tables[0];
            if (_checkhasorder.Count <= 0)
            {
                var _tmpLoginName = row["LoginName"].ToString();
                var _tmpuserbaseinfo = _userbaseinfoservice.FindByFeldName(u => u.LoginName == _tmpLoginName && u.State == 1);
                OrderMeal_MealOrder Obj = new OrderMeal_MealOrder();
                string ordernum = DateTime.Now.ToString("yyyyMMddHHmmssfff") + row["MealCardID"];

                Obj.UserBaseinfoID = Guid.Parse(row["UserBaseinfoID"].ToString());
                Obj.UserName = row["RealName"].ToString();
                Obj.LoginName = row["LoginName"].ToString();
                Obj.MealCardID = row["MealCardID"].ToString();
                Obj.OrderState = 1;
                Obj.OrderStateName = "订餐";
                Obj.Discription = "手机短信订餐";
                Obj.AfterOrderBalance = afterorbalance;
                Obj.DinningDate = Convert.ToDateTime(row["WorkingDate"]);
                Obj.Flag = 0;
                Obj.MealPlaceID = Guid.Parse(row["MealPlaceID"].ToString());
                Obj.MealPlaceName = row["MealPlaceName"].ToString();
                Obj.PackageID = Guid.Parse(row["PackageID"].ToString());
                Obj.PackageName = row["PackageName"].ToString();
                Obj.PackagePrice = Convert.ToDecimal(row["PackagePrice"]);
                Obj.MealTimeType = row["MealTimeType"].ToString();
                Obj.MealTimeID = GetText(row["MealTimeType"].ToString());
                Obj.DinningRoomID = Guid.Parse(row["DinningRoomID"].ToString());
                Obj.DinningRoomName = row["DinningRoomName"].ToString();
                Obj.OrderNum = ordernum;
                Obj.OrderTime = DateTime.Now;
                _tmpuserbaseinfo.Balance = Convert.ToDecimal(afterorbalance);
                IsSuccess = _ordermealservice.Insert(Obj);
                _userbaseinfoservice.Update(_tmpuserbaseinfo);
            }

            return IsSuccess;
        }
        DataRow GetMeal(string num, DateTime RevTime, string CD, string type,string WorkingDate, double days)
        {
            string mtype;            
            switch (type)
            {
                case "1": mtype = "早餐"; break;
                case "2": mtype = "中餐"; break;
                case "3": mtype = "晚餐"; break;
                case "4": mtype = "宵夜"; break;
                default: mtype = ""; break;
            }
            string queryMeal = string.Format(@"SELECT f.id as menuid,d.UserBaseinfoID,e.RealName,e.loginname,e.MealCardID,e.employid,b.ID as DinningRoomID,b.DinningRoomName,
g.ID as PackageID,g.PackageName,g.PackagePrice,g.MealTimeID,g.MealTimeType,
a.ID as MealPlaceID,a.MealPlaceName,(e.Balance-g.PackagePrice) AS AfterOrderBalance
,f.WorkingDate,c.BeginTime,c.EndTime,c.ClosePayBackTime,e.Balance,g.PackagePrice
 FROM OrderMeal_MealPlace a JOIN OrderMeal_DinningRoom b ON a.DinningRoomID=b.ID
JOIN OrderMeal_DinningRoomMealTimeSettings c ON b.id=c.DinningRoomID 
JOIN OrderMeal_UserDinningRoom d ON b.ID=d.DinningRoomID
JOIN OrderMeal_UserBaseinfo e ON d.UserBaseinfoID=e.ID
JOIN bw_Users usr on e.loginname=usr.UserName
JOIN OrderMeal_Menu f ON f.DinningRoomID = b.ID AND f.MealTimeType=c.MealTimeType and  
DATEDIFF(DAY,f.WorkingDate,GETDATE())<=0 AND DATEDIFF(DAY,f.WorkingDate,GETDATE())>=-1
JOIN OrderMeal_Package g ON g.DinningRoomID=a.DinningRoomID AND g.MealTimeType=c.MealTimeType
join OrderMeal_Command coma on g.ID=coma.packageid and a.ID=coma.placeid
join ordermsg_log h on e.id=h.userid and h.roomId=f.id and h.num={4}
WHERE 
usr.mobile='{0}' 
and lower(coma.command)='{1}' 
and 
( 
(SmsTime <'{2}' and LastSmsTime > '{2}' )  
 or (SmsTime < '23:59:59' and LastSmsTime >'00:00:00' and SmsTime>LastSmsTime   and			(
				(SmsTime < '{2}' and '{2}' <'23:59:59')or		('00:00:00' <'{2}' and '{2}' <LastSmsTime))	
				)
 ) 
 and c.MealTimeType='{3}' and  DATEDIFF(DAY,f.WorkingDate,'{5}')=0 
 order by f.WorkingDate desc", num, CD.ToLower(), RevTime.ToString("HH:mm:ss"), mtype, type, Convert.ToDateTime(WorkingDate).AddDays(days));
            DataTable dtmeal = SqlHelper.ExecuteDataset(queryMeal).Tables[0];
            if (dtmeal != null && dtmeal.Rows.Count > 0)
            {
                DataRow row = dtmeal.Rows[0];
                Guid menuid = Guid.Parse(row["menuid"].ToString());
                var _tmplist = _menupackagecommandservice.List().Where(u => u.MenuId == menuid && u.Command.ToLower() == CD.ToLower()).ToList();
                if(_tmplist.Count>0)
                {
                    return row;
                }
                else
                {
                    return null;

                }
            }
            else
            {
               return  null;
            }
        }
        #region 
        public string GetRealName(string num)
        {
            var _tmpuser = _sysuserservice.FindByFeldName(u => u.Mobile == num);
            if (_tmpuser == null)
                return "";
            return _tmpuser.RealName;
        }
        public Guid UserId(string num)
        {
            var _tmpuser = _sysuserservice.FindByFeldName(u => u.Mobile == num);
            if (_tmpuser == null)
                return Guid.Parse("00000000-0000-0000-0000-000000000000");
            return _tmpuser.UserId;
        }
        public Guid UserBaseinfoID(string num)
        {
            var _tmpuser = _userbaseinfoservice.FindByFeldName(u => u.Telephone == num);
            if (_tmpuser == null)
                return Guid.Parse("00000000-0000-0000-0000-000000000000");            
            return _tmpuser.Id;
        }
        
        public string GetEmployId(string num)
        {
            var _tmpuser = _sysuserservice.FindByFeldName(u => u.Mobile == num);
            if (_tmpuser == null)
                return "";
            return _tmpuser.EmployeeId;
        }
        public string GetLoginName(string num)
        {
            var _tmpuser = _sysuserservice.FindByFeldName(u => u.Mobile == num);
            if (_tmpuser == null)
                return "";
            return _tmpuser.UserName;
        }
        string GetMsgHeadBynum(string num)
        {
            return msghead + GetRealName(num) + ",您好\r\n";
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
        #endregion

    }
}
