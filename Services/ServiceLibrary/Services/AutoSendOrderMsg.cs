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
    public class AutoSendOrderMsg : ServiceJob
    {
        IOrderMsg_LogService _ordermsglogservice = new OrderMsg_LogService();
        private delegate void pushOrderMeal(DataTable dt);
        string msghead = "[综合管理平台-订餐系统]\r\n";
        /// <summary>
        /// 发送菜谱短信
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

            AutoSendMessage();            
            return true;
        }
        public void AutoSendMessage()
        {
            try
            {
                LogRecord.WriteCanteenServiceLog("=========开始发送菜谱给未预约用户===============", LogResult.normal);
                //
                SqlHelper.ExecuteNonQuery("update OrderMeal_UserBaseinfo set telephone=a.mobile from OrderMeal_UserBaseinfo b inner join bw_Users a on a.UserName=b.loginname");
                DataTable dt = GetPepeleTable();
                StringBuilder sbmenuid = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    //先禁用当天菜谱和第二天早餐以及可提前一天的套餐的发送，防止重复发送消息
                    SqlHelper.ExecuteNonQuery("update OrderMeal_Menu set CanSendSms=0 where (DATEDIFF(DAY,WorkingDate,GETDATE())=0 or (DATEDIFF(DAY,WorkingDate,GETDATE())=-1 AND MealTimeType='早餐' ) or (DATEDIFF(DAY,WorkingDate,GETDATE())=-1 AND IsPreOrder=1 AND MealTimeType in ('中餐','晚餐')) )");
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
                        DataTable dt2 = GetOrderMeal(UserBaseinfoID, menuid, row["MealTimeType"].ToString(), WorkingDate, out type);
                        if (dt2.Rows.Count == 0) { continue; }

                        DataRow tmpRw = tmpDt.NewRow();
                        string context = GetContent(dt2, row);
                        //tmpRw["telephone"] = row["telephone"];
                        tmpRw["telephone"] = row["telephone"];
                        //tmpRw["sendSms_port"] = sendSms_port + type;mobile
                        //由于在同一个短信端口号进行交互，因此改为按时间段判断用餐类型20150703
                        //tmpRw["sendSms_port"] = sendSms_port + "1";
                        tmpRw["context"] = context;
                        tmpDt.Rows.Add(tmpRw);
                        sbmenuid.Append(row["menuid"]).Append(",");
                    }
                    LogRecord.WriteCanteenServiceLog("推送菜单短信集合==开始异步推送菜谱========", LogResult.normal);
                    pushOrderMeal iv = new pushOrderMeal(InvoteOrderMeal);
                    iv.BeginInvoke(tmpDt, null, null);
                }
                if (sbmenuid.Length > 0)
                {
                    SqlHelper.ExecuteNonQuery(string.Format("update OrderMeal_Menu set flag=1 where id in ('{0}')", sbmenuid.ToString().Substring(0, sbmenuid.Length - 1).Replace(",", "','")));
                }
                //处理完消息后再次启用当天菜谱和第二天早餐的发送
                SqlHelper.ExecuteNonQuery("update OrderMeal_Menu set CanSendSms=1 where (DATEDIFF(DAY,WorkingDate,GETDATE())=0 or (DATEDIFF(DAY,WorkingDate,GETDATE())=-1 AND MealTimeType='早餐' ) or (DATEDIFF(DAY,WorkingDate,GETDATE())=-1 AND IsPreOrder=1 AND MealTimeType in ('中餐','晚餐')) )");
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:发送菜谱短信出错:{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:发送菜谱短信出错:{1}", strCurStrategyInfo, ex.Message), false);
            }            
        }

        string GetContent(DataTable dt1, DataRow row)
        {
            StringBuilder sb = new StringBuilder();
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                sb.Append(msghead);
                string PreStr = "";
                string cuisineStr = GetCuisine(row["menuid"].ToString());
                if(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") == Convert.ToDateTime(row["WorkingDate"]).ToString("yyyy-MM-dd")&& row["MealTimeType"].ToString()!= "早餐")
                {
                    PreStr = "P";
                }
                string commandStr = GetCommand(row["menuid"].ToString(), PreStr);
                sb.Append(row["RealName"]).Append(",您好！\r\n").Append(row["DinningRoomName"]).Append("食堂").Append(Convert.ToDateTime(row["WorkingDate"]).ToString("yyyy-MM-dd")).Append("日").Append(row["MealTimeType"]).Append("菜式如下:").Append(cuisineStr).Append("\r\n您若需要短信订餐,请在今天").Append(string.Format("{0:T}", row["EndTime"])).Append("之前回复指令：\r\n").Append(commandStr);
                //foreach (DataRow row2 in dt1.Rows)
                //{                    
                //    sb.Append(row2["Command"]).Append("（").Append(row2["MealPlaceName"]).Append("，").Append(row2["Description"]).Append("）\r\n");
                //}
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
        public string GetCommand(string menuid,string prestr)
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
        DataTable GetOrderMeal(Guid userId, Guid roomId, string MealTimeType, DateTime WorkingDate, out int type)
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
                //int t = SqlHelper.ExecuteNonQuery(string.Format("insert into OrderMsg_Log (userId,	roomId,	num,	[state],	WorkingDate,MealTimeType) values ('{0}','{1}','{2}','{3}','{4}','{5}');", userId, roomId, type, 0, WorkingDate, MealTimeType));
            }
            //            string query = string.Format(@"SELECT *,g.Discription as Description 
            //FROM  OrderMeal_MealPlace a 
            //JOIN   OrderMeal_DinningRoom b ON a.DinningRoomID=b.ID
            //JOIN OrderMeal_DinningRoomMealTimeSettings c ON b.id=c.DinningRoomID 
            //JOIN OrderMeal_UserDinningRoom d ON b.ID=d.DinningRoomID
            //JOIN OrderMeal_UserBaseinfo e ON d.UserBaseinfoID=e.ID and e.state=1  and d.getsms=1
            //JOIN OrderMeal_Menu f ON f.DinningRoomID = b.ID AND f.MealTimeType=c.MealTimeType  
            //and  (
            //DATEDIFF(DAY,f.WorkingDate,GETDATE())=0 
            //or (DATEDIFF(DAY,f.WorkingDate,GETDATE())=-1 AND f.MealTimeType='早餐' )  
            //or (DATEDIFF(DAY,f.WorkingDate,GETDATE())=-1 AND f.IsPreOrder=1 AND f.MealTimeType in ('中餐','晚餐'))
            //)
            //JOIN OrderMeal_Package g ON g.DinningRoomID = f.DinningRoomID AND g.MealTimeType=f.MealTimeType
            // join OrderMeal_Command coma on g.ID=coma.packageid and a.ID=coma.placeid
            // JOIN ordermsg_log h ON e.ID=h.userId AND f.id=h.roomId AND f.WorkingDate=h.WorkingDate and h.MealTimeType=f.MealTimeType
            //where (
            //(SmsTime <'{0}' and LastSmsTime > '{0}' )  
            //or (SmsTime <'{0}' and SmsTime < '23:59:59' and LastSmsTime >'00:00:00' and SmsTime>LastSmsTime)
            //) and f.id={2}  and f.flag=0  and e.id={1}", DateTime.Now.ToString("HH:mm:ss"), userId, roomId);
            string query = string.Format(@"SELECT *
FROM  OrderMeal_DinningRoom b 
JOIN OrderMeal_DinningRoomMealTimeSettings c ON b.id=c.DinningRoomID 
JOIN OrderMeal_UserDinningRoom d ON b.ID=d.DinningRoomID
JOIN OrderMeal_UserBaseinfo e ON d.UserBaseinfoID=e.ID and e.state=1  and d.getsms=1
JOIN OrderMeal_Menu f ON f.DinningRoomID = b.ID AND f.MealTimeType=c.MealTimeType  
and  (
DATEDIFF(DAY,f.WorkingDate,GETDATE())=0 
or (DATEDIFF(DAY,f.WorkingDate,GETDATE())=-1 AND f.MealTimeType='早餐' )  
or (DATEDIFF(DAY,f.WorkingDate,GETDATE())=-1 AND f.IsPreOrder=1 AND f.MealTimeType in ('中餐','晚餐'))
)
 JOIN ordermsg_log h ON e.ID=h.userId AND f.id=h.roomId AND f.WorkingDate=h.WorkingDate and h.MealTimeType=f.MealTimeType
where (
	(SmsTime <'{0}' and LastSmsTime > '{0}' )  
 or (SmsTime < '23:59:59' and LastSmsTime >'00:00:00' and SmsTime>LastSmsTime   and			(
				(SmsTime < '{0}' and '{0}' <'20:59:59')or		('00:00:00' <'{0}' and '{0}' <LastSmsTime))	
				)
 ) and f.id='{2}'  and f.flag=0  and e.id='{1}'", DateTime.Now.ToString("HH:mm:ss"), userId, roomId);
            DataTable dt = SqlHelper.ExecuteDataset(query).Tables[0];
            return dt;
        }



        /// <summary>
        /// 获取菜谱（没有在该时间段内预约订餐的订餐用户信息）
        /// </summary>
        /// <returns></returns>
        DataTable GetPepeleTable()
        {
            //将前一天未处理的短信标记为已过期
            SqlHelper.ExecuteNonQuery("update OrderMsg_Log set State=2 where DATEDIFF(DAY,InsertDate,GETDATE())>0 and State=0");

            string querysql = string.Format(@"SELECT d.id as menuid,* FROM OrderMeal_UserBaseinfo a 
JOIN OrderMeal_UserDinningRoom b ON a.id=b.UserBaseinfoID
JOIN OrderMeal_DinningRoom c ON b.DinningRoomID=c.ID 
JOIN OrderMeal_Menu d ON d.DinningRoomID = c.ID
JOIN OrderMeal_DinningRoomMealTimeSettings f ON f.MealTimeType=d.MealTimeType AND f.DinningRoomID=b.DinningRoomID   
JOIN (select tbu.UserName,tbu.Status from bw_Users tbu where  exists (select 1 from uum_userinfo tbuum where tbu.UserName=tbuum.UserID))  tu ON a.loginname=tu.UserName   
LEFT JOIN ordermsg_log e ON a.ID=e.userId AND d.id=e.roomId AND d.WorkingDate=e.WorkingDate and e.MealTimeType=f.MealTimeType
 where 
(
	(SmsTime <'{0}' and LastSmsTime > '{0}' )  
 or (SmsTime < '23:59:59' and LastSmsTime >'00:00:00' and SmsTime>LastSmsTime   and			(
				(SmsTime < '{0}' and '{0}' <'20:59:59')or		('00:00:00' <'{0}' and '{0}' <LastSmsTime))	
				)
 ) and 
 a.state=1 and b.GetSms=1  and e.id is null and d.flag=0 
 and 
 (
 DATEDIFF(DAY,d.WorkingDate,GETDATE())=0 
 or 
 (DATEDIFF(DAY,d.WorkingDate,GETDATE())=-1 AND d.MealTimeType='早餐' )  
 or (DATEDIFF(DAY,d.WorkingDate,GETDATE())=-1 AND d.IsPreOrder=1 AND d.MealTimeType in ('中餐','晚餐')) 
 )
 and isnull(CanSendSms,1) = 1  
 and tu.Status=0   
 and not exists (select UserBaseinfoID from OrderMeal_BookOrder bo where f.MealTimeType=bo.MealTimeType
 and bo.UserBaseinfoID=a.ID and bo.DinningRoomID=c.ID and  d.WorkingDate between bo.StartedDate and bo.EndDate and bo.OrderState=1)", DateTime.Now.ToString("HH:mm:ss"));
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
            LogRecord.WriteCanteenServiceLog(string.Format("=========发送菜谱给未预约用户结束,成功发送{0}条，失败{1}条===============", scnt, ecnt), LogResult.normal);
            AddStrategyLog(string.Format("发送菜谱给未预约用户结束：成功{0}条，失败{1}条", scnt, ecnt), true);
            SaveStrategyLog();
            LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "推送菜单短信集合==异步推送菜谱结束========", LogResult.normal);
        }
    }
}
