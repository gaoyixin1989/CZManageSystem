using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Service.Administrative.Dinning;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class AutoSendBookOrderMsg : ServiceJob
    {
        IOrderMsg_LogService _ordermsglogservice = new OrderMsg_LogService();
        IOrderMeal_MenuPackageCommandService _ordermealmenupackagecommandservice = new OrderMeal_MenuPackageCommandService();
        IView_EXT_XF_AccountService _balanceservice = new View_EXT_XF_AccountService();
        IOrderMeal_DinningRoomMealTimeSettingsService _settingservice = new OrderMeal_DinningRoomMealTimeSettingsService();
        IOrderMeal_PackageService _mealpackageservice = new OrderMeal_PackageService();
        IOrderMeal_MealOrderService _ordermealservice = new OrderMeal_MealOrderService();
        IOrderMeal_UserBaseinfoService _userbaseinfoservice = new OrderMeal_UserBaseinfoService();
        private delegate void pushOrderMeal(DataTable dt);
        string msghead = "[综合管理平台-订餐系统]\r\n";
        /// <summary>
        /// 短信推送菜谱信息给已经预约订餐
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
            AutoSendBookMessage();

            
            return true;
        }
        public void AutoSendBookMessage()
        {
            try
            {
                LogRecord.WriteCanteenServiceLog("=========开始发送菜谱信息给预约订餐用户===============", LogResult.normal);
                //
                SqlHelper.ExecuteNonQuery("update OrderMeal_UserBaseinfo set telephone=a.mobile from OrderMeal_UserBaseinfo b inner join bw_Users a on a.UserName=b.loginname");
                DataTable dt = GetBookPepeleTable();
                StringBuilder sbmenuid = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    DataTable tmpDt = new DataTable();
                    tmpDt.Columns.Add("telephone");
                    tmpDt.Columns.Add("sendSms_port");
                    tmpDt.Columns.Add("context");
                    foreach (DataRow row in dt.Rows)
                    {
                        int type;
                        Guid UserBaseinfoID = Guid.Parse(row["UserBaseinfoID"].ToString());
                        Guid menuid = Guid.Parse(row["menuid"].ToString());
                        DateTime WorkingDate = Convert.ToDateTime(row["WorkingDate"].ToString());
                        DataTable dt2 = GetBookOrderMeal(UserBaseinfoID, menuid, row["MealTimeType"].ToString(), WorkingDate, out type);
                        if (dt2.Rows.Count == 0) { continue; }

                        DataRow tmpRw = tmpDt.NewRow();
                        string context = GetBookContent(dt2, row);
                        if (string.IsNullOrEmpty(context)) { continue; }
                        tmpRw["telephone"] = row["telephone"];
                        //tmpRw["telephone"] = row["telephone"];
                        //tmpRw["sendSms_port"] = sendSms_port + type;mobile
                        //由于在同一个短信端口号进行交互，因此改为按时间段判断用餐类型20150703
                        //tmpRw["sendSms_port"] = sendSms_port + "1";
                        tmpRw["context"] = context;
                        tmpDt.Rows.Add(tmpRw);
                        sbmenuid.Append(row["menuid"]).Append(",");
                    }
                    LogRecord.WriteCanteenServiceLog("向预约订餐用户发送菜谱信息集合==开始异步推送菜谱========", LogResult.normal);
                    pushOrderMeal iv = new pushOrderMeal(InvoteOrderMeal);
                    iv.BeginInvoke(tmpDt, null, null);
                }
                if (sbmenuid.Length > 0)
                {
                    SqlHelper.ExecuteNonQuery(string.Format("update OrderMeal_Menu set bookflag=1 where id in ('{0}')", sbmenuid.ToString().Substring(0, sbmenuid.Length - 1).Replace(",", "','")));
                }
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:短信推送菜谱信息给已经预约订餐出错:{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:短信推送菜谱信息给已经预约订餐出错:{1}", strCurStrategyInfo, ex.Message), false);
            }
        }
        string GetBookContent(DataTable dt1, DataRow row)
        {
            StringBuilder sb = new StringBuilder();
            decimal yktbalance = 0;
            string PreStr = "";
            DataTable dt;
            Guid menuid = Guid.Parse(row["menuid"].ToString());
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                sb.Append(msghead);
                string cuisineStr = GetCuisine(row["menuid"].ToString());
                string commandStr = GetCommand(row["menuid"].ToString(), PreStr);
                sb.Append(row["RealName"]).Append(",");                
                foreach (DataRow row2 in dt1.Rows)
                {
                    //string type;
                    //switch (row2["MealTimeType"].ToString())
                    //{
                    //    case "早餐": type = "1"; break;
                    //    case "中餐": type = "2"; break;
                    //    case "晚餐": type = "3"; break;
                    //    case "宵夜": type = "4"; break;
                    //    default: type = ""; break;
                    //}
                    if (DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") == Convert.ToDateTime(row["WorkingDate"]).ToString("yyyy-MM-dd") && row["MealTimeType"].ToString() != "早餐")
                    {
                        PreStr = "P";
                    }
                    //获取一卡通余额
                    string EmployId = row["EmployId"].ToString();
                    var _tmpbalance = _balanceservice.FindByFeldName(u => u.JobNumber == EmployId);
                    if (_tmpbalance != null)
                        yktbalance = _tmpbalance.BelAmount.Value;
                    else
                        yktbalance = 0;
                    Guid PackageID= Guid.Parse(row2["PackageID"].ToString());
                    Guid DinningRoomID = Guid.Parse(row2["DinningRoomID"].ToString());
                    string MealTimeType = row2["MealTimeType"].ToString();
                    var list = _ordermealmenupackagecommandservice.List().Where(u => u.MenuId == menuid && u.PackageId == PackageID).ToList();
                    if(list.Count > 0)
                    {
                        double afterordbalance = Convert.ToDouble(yktbalance);
                        var _tmppackageinfo = _mealpackageservice.FindById(PackageID);
                        var _tmpdrinordertime = _settingservice.List().Where(u => u.DinningRoomID == DinningRoomID && u.MealTimeType == MealTimeType).ToList()[0];
                        if (yktbalance >= _tmppackageinfo.PackagePrice)
                        {
                            sb.Append("您好！\r\n" + row["DinningRoomName"]).Append("食堂").Append(Convert.ToDateTime(row["WorkingDate"]).ToString("yyyy-MM-dd")).Append("日").Append(row["MealTimeType"]).Append("菜式如下:");
                            sb.Append(cuisineStr).Append("\r\n您的账户金额" + Convert.ToDouble(yktbalance).ToString("0.00") + "元，您已预订金额为" + Convert.ToDouble(_tmppackageinfo.PackagePrice).ToString("0.00") + "的套餐，").Append(string.Format("{0:T}", _tmpdrinordertime.ClosePayBackTime)).Append("之前回复指令").Append(PreStr + "X退餐\r\n");
                            sb.AppendFormat("（您已预定{0}-{1}在{3}{4}的{2}）", Convert.ToDateTime(row2["StartedDate"]).ToString("MM月dd日"), Convert.ToDateTime(row2["EndDate"]).ToString("MM月dd日"), row2["PackageName"], row2["DinningRoomName"], row2["MealPlaceName"]);
                            InsertMeal("", DateTime.Now, "", row2, afterordbalance);
                            UpdateBookAfterBalance(row2, afterordbalance);
                        }
                        else
                        {
                            sb.AppendFormat("对不起！\r\n您在{0}订购的{1}{2},需要{3}元,您当前余额为{4}元,不足够订餐,请及时充值", row["DinningRoomName"], Convert.ToDateTime(row["WorkingDate"]).ToString("yyyy-MM-dd"), row["MealTimeType"], Convert.ToDouble(_tmppackageinfo.PackagePrice).ToString("0.00"), Convert.ToDouble(yktbalance).ToString("0.00"));
                        }
                    }
                    else
                    {
                        sb.Append("很抱歉！\r\n由于今日没有您之前预订的套餐").AppendFormat("（已预定{0}-{1}在{3}{4}的{2}）", Convert.ToDateTime(row2["StartedDate"]).ToString("MM月dd日"), Convert.ToDateTime(row2["EndDate"]).ToString("MM月dd日"), row2["PackageName"], row2["DinningRoomName"], row2["MealPlaceName"]).Append("，无法订餐！您可通过回复此短信或登录系统重新预订。\r\n").Append(row["DinningRoomName"]).Append("食堂").Append(Convert.ToDateTime(row["WorkingDate"]).ToString("yyyy-MM-dd")).Append("日").Append(row["MealTimeType"]).Append("菜式如下:").Append(cuisineStr).Append("\r\n您若需要短信订餐,请在今天").Append(string.Format("{0:T}", row["EndTime"])).Append("之前回复指令：\r\n").Append(commandStr);
                    }
                }
            }
            if (row["GetSms"].ToString() == "0")
            {
                return "";
            }
            else
            {
                return sb.ToString();
            }
        }
        bool InsertMeal(string num, DateTime RevTime, string CD, DataRow row, double afterorbalance)
        {
            int i = 0;
            bool IsSuccess = false;
            Guid UserBaseinfoID = Guid.Parse(row["UserBaseinfoID"].ToString());
            string MealTimeType = row["MealTimeType"].ToString();
            DateTime DinningDate = Convert.ToDateTime(row["WorkingDate"].ToString());
            var _checkhasorder = _ordermealservice.List().Where(u => u.UserBaseinfoID == UserBaseinfoID && u.MealTimeType == MealTimeType && u.DinningDate == DinningDate).ToList();
            //string sqlisominfo = string.Format("select * from OrderMeal_MealOrder where convert(varchar(10),[OrderTime],120)=CONVERT(varchar(10),GETDATE(),120) and [OrderState]<>0 and UserBaseinfoID='{0}' and MealTimeType='{1}'", row["UserBaseinfoID"], row["MealTimeType"]);
            //DataTable isominfo = SqlHelper.ExecuteDataset(sqlisominfo).Tables[0];
            if (_checkhasorder.Count <= 0)
            {
                var _tmpLoginName = row["LoginName"].ToString();
                var _tmpuserbaseinfo = _userbaseinfoservice.FindByFeldName(u => u.LoginName == _tmpLoginName && u.State == 1);
                OrderMeal_MealOrder Obj = new OrderMeal_MealOrder();
                string ordernum = DateTime.Now.ToString("yyyyMMddHHmmssfff") + row["MealCardID"];

                Obj.UserBaseinfoID = Guid.Parse(row["UserBaseinfoID"].ToString());
                Obj.UserName = row["UserName"].ToString();
                Obj.LoginName = row["LoginName"].ToString();
                Obj.MealCardID = row["MealCardID"].ToString();
                Obj.OrderState = 1;
                Obj.OrderStateName = "订餐";
                Obj.Discription = "手机短信订餐";
                Obj.AfterOrderBalance = Convert.ToDecimal(afterorbalance);
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
        bool UpdateBookAfterBalance(DataRow row, double afterordbalance)
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
        DataTable GetMeal(string num, DateTime RevTime, string CD, string type)
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
            string query = string.Format(@"SELECT d.UserBaseinfoID,e.UserName,e.loginname,e.MealCardID,e.employid,b.ID as did,b.DinningRoomName,
g.ID as PackageID,g.PackageName,g.PackagePrice,g.MealTimeID,g.MealTimeType,a.ID as MealPlaceID,a.MealPlaceName,(e.Balance-g.PackagePrice) AS AfterOrderBalance
,f.WorkingDate,c.BeginTime,c.EndTime,c.ClosePayBackTime,e.Balance,g.PackagePrice
 FROM tb_OrderMeal_MealPlace a JOIN tb_OrderMeal_DinningRoom b ON a.DinningRoomID=b.ID
JOIN tb_OrderMeal_DinningRoomMealTimeSettings c ON b.id=c.DinningRoomID JOIN tb_OrderMeal_UserDinningRoom d ON b.ID=d.DinningRoomID
JOIN tb_OrderMeal_UserBaseinfo e ON d.UserBaseinfoID=e.ID
JOIN tb_user usr on e.loginname=usr.loginname
JOIN tb_OrderMeal_Menu f ON f.DinningRoomID = b.ID AND f.MealTimeType=c.MealTimeType and  
DATEDIFF(DAY,f.WorkingDate,GETDATE())<=0 AND DATEDIFF(DAY,f.WorkingDate,GETDATE())>=-1
JOIN tb_OrderMeal_Package g ON g.DinningRoomID=a.DinningRoomID AND g.MealTimeType=c.MealTimeType
 join tb_OrderMeal_Command coma on g.ID=coma.packageid and a.ID=coma.placeid
join tb_ordermsg_log h on e.id=h.userid and h.roomId=f.id and h.num={4}
WHERE usr.mobile='{0}' and lower(coma.command)='{1}' and 
( (c.BeginTime<'{2}' and  c.EndTime>'{2}' )  or (c.BeginTime<'{2}' and c.EndTime <'23:59:59' and c.BeginTime>c.EndTime)
 or (c.EndTime>'{2}' and c.EndTime >'00:00:00' and c.BeginTime>c.EndTime)) and c.MealTimeType='{3}' order by f.WorkingDate desc", num, CD.ToLower(), RevTime.ToString("HH:mm:ss"), mtype, type);
            //DataRow row = DAO.sqlRow(strSql);
            DataTable dt = SqlHelper.ExecuteDataset(query).Tables[0];
            return dt;

        }
        public string GetCommand(string menuid, string prestr)
        {
            string querymenu = string.Format(@" select * from OrderMeal_MenuPackageCommand
  where MenuID='{0}' ", menuid);
            DataTable dt = SqlHelper.ExecuteDataset(querymenu).Tables[0];
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append(prestr + row["Command"]).Append("（").Append(row["PackageName"]).Replace("-", "，").Append("）\r\n");
            }
            return sb.ToString();
        }
        public string GetCuisine(string menuid)
        {
            string querymenu = string.Format(@" SELECT a.CuisineName,a.CuisineType FROM OrderMeal_Cuisine a JOIN OrderMeal_MenuCuisine b ON a.ID=b.CuisineID  where b.MenuID='{0}'
 ORDER BY a.CuisineType ", menuid);
            DataTable dt = SqlHelper.ExecuteDataset(querymenu).Tables[0];
            StringBuilder sb = new StringBuilder();
            string cType = null;
            foreach (DataRow row in dt.Rows)
            {
                if (cType != row["CuisineType"].ToString())
                {
                    cType = row["CuisineType"].ToString();
                    sb.Append("\r\n");
                    sb.Append(cType).Append(":").Append(row["CuisineName"]);
                }
                else
                {
                    sb.Append(",").Append(row["CuisineName"]);
                }
            }
            return sb.ToString();
        }
        DataTable GetBookOrderMeal(Guid userId, Guid roomId, string MealTimeType, DateTime WorkingDate, out int type)
        {
            switch (MealTimeType)
            {
                case "早餐": type = 1; break;
                case "中餐": type = 2; break;
                case "晚餐": type = 3; break;
                default: type = 4; break;
            }
            var list = _ordermsglogservice.List().Where(u => u.UserId == userId && u.RoomId == roomId && u.MealTimeType == MealTimeType && u.WorkingDate == WorkingDate).ToList();
            //string record = DAO.sqlValue(string.Format("select id from OrderMsg_Log where userid='{0}' and roomid='{1}' and MealTimeType='{2}' and WorkingDate='{3}'", userId, roomId, MealTimeType, WorkingDate));
            if (list.Count == 0)//去重
            {
                OrderMsg_Log _tmpordermsglog = new OrderMsg_Log();
                _tmpordermsglog.MealTimeType = MealTimeType;
                _tmpordermsglog.Num = type;
                _tmpordermsglog.UserId = userId;
                _tmpordermsglog.WorkingDate = WorkingDate;
                _tmpordermsglog.RoomId = roomId;
                _tmpordermsglog.State = 0;
                _tmpordermsglog.InsertDate = DateTime.Now;
                _ordermsglogservice.Insert(_tmpordermsglog);               
            }           
            string query = string.Format(@"SELECT top 1 e.MealCardID,bo.UserBaseinfoID,UserName,bo.LoginName,bo.DinningRoomID,bo.DinningRoomName,bo.PackageID,bo.PackageName,bo.PackagePrice,bo.MealTimeType,bo.MealPlaceID,bo.MealPlaceName,f.WorkingDate,StartedDate,EndDate
from OrderMeal_DinningRoom b
JOIN OrderMeal_DinningRoomMealTimeSettings c ON b.id=c.DinningRoomID 
JOIN OrderMeal_UserDinningRoom d ON b.ID=d.DinningRoomID
JOIN OrderMeal_UserBaseinfo e ON d.UserBaseinfoID=e.ID and e.state=1 
JOIN OrderMeal_Menu f ON f.DinningRoomID = b.ID AND f.MealTimeType=c.MealTimeType  
and  (
DATEDIFF(DAY,f.WorkingDate,GETDATE())=0 
or (DATEDIFF(DAY,f.WorkingDate,GETDATE())=-1 AND f.MealTimeType='早餐' ) 
or (DATEDIFF(DAY,f.WorkingDate,GETDATE())=-1 AND f.IsPreOrder=1 AND f.MealTimeType in ('中餐','晚餐'))
)
 JOIN ordermsg_log h ON e.ID=h.userId AND f.id=h.roomId AND f.WorkingDate=h.WorkingDate and h.MealTimeType=f.MealTimeType
join OrderMeal_BookOrder bo on f.MealTimeType=bo.MealTimeType and e.ID=bo.UserBaseinfoID
 and f.WorkingDate between bo.StartedDate and bo.EndDate
where 
(
	(SmsTime <'{0}' and LastSmsTime > '{0}' )  
 or (SmsTime < '23:59:59' and LastSmsTime >'00:00:00' and SmsTime>LastSmsTime   and			(
				(SmsTime < '{0}' and '{0}' <'20:59:59')or		('00:00:00' <'{0}' and '{0}' <LastSmsTime))	
				)
 )
and f.id='{2} ' 
and isnull(f.bookflag,0)=0 
and e.id='{1}' order by bo.ordertime desc
", DateTime.Now.ToString("HH:mm:ss"), userId, roomId);
            DataTable dt = SqlHelper.ExecuteDataset(query).Tables[0];
            return dt;
        }
        DataTable GetBookPepeleTable()
        {
            //将前一天未处理的短信标记为已过期
            SqlHelper.ExecuteNonQuery("update OrderMsg_Log set State=2 where DATEDIFF(DAY,InsertDate,GETDATE())>0 and State=0");

            string querysql = string.Format(@"SELECT d.id as menuid,* FROM OrderMeal_UserBaseinfo
a JOIN OrderMeal_UserDinningRoom b ON a.id=b.UserBaseinfoID
JOIN OrderMeal_DinningRoom c ON b.DinningRoomID=c.ID JOIN OrderMeal_Menu d ON d.DinningRoomID = c.ID
join OrderMeal_DinningRoomMealTimeSettings f ON f.MealTimeType=d.MealTimeType AND f.DinningRoomID=b.DinningRoomID  
join (select tbu.UserName,tbu.Status from bw_Users tbu where  exists (select 1 from uum_userinfo tbuum where tbu.UserName=tbuum.UserID))  tu ON a.loginname=tu.UserName  
LEFT JOIN ordermsg_log e ON a.ID=e.userId AND d.id=e.roomId AND d.WorkingDate=e.WorkingDate and e.MealTimeType=f.MealTimeType
 where 
(
	(SmsTime <'{0}' and LastSmsTime > '{0}' )  
 or (SmsTime < '23:59:59' and LastSmsTime >'00:00:00' and SmsTime>LastSmsTime   and			(
				(SmsTime < '{0}' and '{0}' <'20:59:59')or		('00:00:00' <'{0}' and '{0}' <LastSmsTime))	
				)
 ) and 
 a.state=1 
 and  isnull(d.bookflag,0)=0 
 and (
 DATEDIFF(DAY,d.WorkingDate,GETDATE())=0 
 or (DATEDIFF(DAY,d.WorkingDate,GETDATE())=-1 AND d.MealTimeType='早餐' )
 or (DATEDIFF(DAY,d.WorkingDate,GETDATE())=-1 AND d.IsPreOrder=1 AND d.MealTimeType in ('中餐','晚餐'))
 )
 and tu.Status=0     
 and exists (select UserBaseinfoID from OrderMeal_BookOrder bo where f.MealTimeType=bo.MealTimeType
 and bo.UserBaseinfoID=a.ID and bo.DinningRoomID=c.ID and  d.WorkingDate  between bo.StartedDate and bo.EndDate and bo.OrderState=1)", DateTime.Now.ToString("HH:mm:ss"));
            DataTable dt = SqlHelper.ExecuteDataset(querysql).Tables[0];
            return dt;
        }
        public void InvoteOrderMeal(DataTable dt)
        {
            int scnt = 0, ecnt = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (CommonFun.SendSms(row["telephone"].ToString(), row["context"].ToString()))
                {
                    scnt++;
                    LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "短信发送成功！==" + row["telephone"] + "========", LogResult.success);
                }
                else
                {
                    ecnt++;
                    LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "短信发送失败！==" + row["telephone"] + "========", LogResult.fail);
                }
            }
            LogRecord.WriteCanteenServiceLog(string.Format("=========发送预约订餐用户发送菜谱结束,成功发送{0}条，失败{1}条===============", scnt, ecnt), LogResult.normal);
            AddStrategyLog(string.Format("{0}：发送菜谱给预约订餐用户结束：成功{1}条，失败{2}条", strCurStrategyInfo, scnt, ecnt), true);
            SaveStrategyLog();
            LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "推送预约订餐用户发送菜谱信息集合==异步推送结束========", LogResult.normal);
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
