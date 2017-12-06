using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class AutoSendToManager : ServiceJob
    {
        private delegate void pushOrderMeal(DataTable dt);
        string msghead = "[综合管理平台-订餐系统]\r\n";
        /// <summary>
        /// 推送订餐统计信息给食堂管理员
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
            AutoSendMessageToManager();
            return true;
        }
        public void AutoSendMessageToManager()
        {
            try
            {
                LogRecord.WriteCanteenServiceLog("=========开始向食堂管理员发送订餐结构信息===============", LogResult.normal);
                string sql = @"select count(od.DinningRoomID) as DinningRoomOrderSum,DinningRoomName,od.MealTimeType,PackageID,PackageName,MealPlaceName,DinningDate,od.DinningRoomID
from OrderMeal_MealOrder od
inner join OrderMeal_Menu tomm 
on od.DinningRoomID=tomm.DinningRoomID and od.MealTimeID=tomm.MealTimeID
and od.DinningDate=tomm.WorkingDate 
inner join OrderMeal_DinningRoomMealTimeSettings tomdrmts
on od.DinningRoomID=tomdrmts.DinningRoomID and tomdrmts.MealTimeType=od.MealTimeType 
inner join OrderMeal_DinningRoomAdmin tomad 
on tomdrmts.DinningRoomID=tomad.DinningRoomID
where tomdrmts.ClosePayBackTime<'{1}'
and (
 DATEDIFF(DAY,tomm.WorkingDate,'{0}')=0 
 or 
 (DATEDIFF(DAY,tomm.WorkingDate,'{0}')=-1 AND tomm.MealTimeType='早餐' )  
 or (DATEDIFF(DAY,tomm.WorkingDate,'{0}')=-1 AND tomm.IsPreOrder=1 AND tomm.MealTimeType in ('中餐','晚餐')) 
)
 and isnull(IsCompleted,0)=0 and tomm.ID='{2}' and tomad.loginname='{3}' and od.OrderState=1
group by od.DinningRoomID,DinningRoomName,PackageName,MealPlaceName,
DinningDate,od.MealTimeType,od.MealTimeID ,od.DinningRoomID,PackageID";
                DateTime date = DateTime.Now;
                DataTable dtManager = GetDinningRoomManager(date);
                StringBuilder sbmenuid = new StringBuilder();
                DataTable tmpDt = new DataTable();
                tmpDt.Columns.Add("telephone");
                tmpDt.Columns.Add("sendSms_port");
                tmpDt.Columns.Add("context");
                foreach (DataRow dwManager in dtManager.Rows)
                {
                    string staticquery = string.Format(sql, date.ToString("yyyy-MM-dd"), date.ToString("HH:mm:ss"), dwManager["menuid"], dwManager["UserName"]);
                    DataTable staticdt = SqlHelper.ExecuteDataset(staticquery).Tables[0];
                    if (staticdt.Rows.Count > 0)
                    {
                        string mobile = dwManager["mobile"].ToString();
                        StringBuilder sb = new StringBuilder(GetMsgHead(dwManager["realname"].ToString()));
                        sb.Append("截至" + date.ToString("yyyy-MM-dd HH:mm:ss") + "，您管理的食堂[" + dwManager["DinningRoomName"] + "][" + Convert.ToDateTime(dwManager["WorkingDate"].ToString()).ToString("yyyy-MM-dd") + "]的[" + dwManager["MealTimeType"] + "]订餐结果如下：\r\n");
                        foreach (DataRow dw in staticdt.Rows)
                        {
                            sb.Append("用餐地点：" + dw["MealPlaceName"] + "，套餐：" + dw["PackageName"] + "，订餐人数：" + dw["DinningRoomOrderSum"] + "人。\r\n");
                        }
                        DataRow tmpRw = tmpDt.NewRow();
                        string context = sb.ToString();
                        tmpRw["telephone"] = mobile;
                        //tmpRw["sendSms_port"] = sendSms_port;
                        tmpRw["context"] = context;
                        tmpDt.Rows.Add(tmpRw);
                        sbmenuid.Append(dwManager["menuid"]).Append(",");
                        LogRecord.WriteCanteenServiceLog(date.ToString("yyyy-MM-dd HH:mm:ss") + "[telephone：" + dwManager["mobile"] + "][memuid：" + dwManager["menuid"] + "]" + sb.ToString() + " ==========", LogResult.normal);
                    }
                    else
                    {
                        LogRecord.WriteCanteenServiceLog(date.ToString("yyyy-MM-dd HH:mm:ss") + "[telephone：" + dwManager["mobile"] + "][memuid：" + dwManager["menuid"] + "]" + "没有订餐统计信息。 ==========", LogResult.normal);
                    }

                }
                LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "推送订餐统计短信集合==开始异步推送订餐统计信息========", LogResult.normal);
                pushOrderMeal iv = new pushOrderMeal(InvoteOrderMeal);
                iv.BeginInvoke(tmpDt, null, null);
                if (sbmenuid.Length > 0)
                {
                    SqlHelper.ExecuteNonQuery(string.Format("update OrderMeal_Menu set IsCompleted=1 where id in ('{0}')", sbmenuid.ToString().Substring(0, sbmenuid.Length - 1).Replace(",", "','")));
                }
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:推送订餐统计信息给食堂管理员出错:{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:推送订餐统计信息给食堂管理员出错:{1}", strCurStrategyInfo, ex.Message), false);
            }            
        }
        DataTable GetDinningRoomManager(DateTime time)
        {
            string querysql = string.Format(@"select u.realname,u.mobile,u.UserName,tomad.DinningRoomID,r.DinningRoomName,tomdrmts.MealTimeType,m.ID as menuid,m.workingdate
from (select tbu.* from bw_Users tbu where  exists (select 1 from uum_userinfo tbuum where tbu.UserName=tbuum.UserID))  u
inner join OrderMeal_DinningRoomAdmin tomad 
on u.UserName=tomad.loginname
inner join OrderMeal_DinningRoom r
on tomad.DinningRoomID=r.ID
inner join OrderMeal_DinningRoomMealTimeSettings tomdrmts
on r.ID=tomdrmts.DinningRoomID 
inner join OrderMeal_Menu m on r.ID=m.DinningRoomID and tomdrmts.MealTimeType=m.MealTimeType
and (DATEDIFF(DAY,m.WorkingDate,'{0}')=0 or (DATEDIFF(DAY,m.WorkingDate,'{0}')=-1 AND m.MealTimeType='早餐' ))
where u.Status=0 and tomdrmts.ClosePayBackTime<'{1}'  and isnull(IsCompleted,0)=0", time.ToString("yyyy-MM-dd"), time.ToString("HH:mm:ss"));
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
            LogRecord.WriteCanteenServiceLog(string.Format("=========发送推送订餐统计信息结束,成功发送{0}条，失败{1}条===============", scnt, ecnt), LogResult.normal);
            AddStrategyLog(string.Format("发送推送订餐统计信息结束：成功{0}条，失败{1}条", scnt, ecnt), true);
            SaveStrategyLog();
            LogRecord.WriteCanteenServiceLog(DateTime.Now.ToString() + "推送推送订餐统计信息集合==异步异步推送订餐统计信息结束========", LogResult.normal);
        }
        string GetMsgHead(string username)
        {
            return msghead + username + ",您好\r\n";
        }
    }
}
