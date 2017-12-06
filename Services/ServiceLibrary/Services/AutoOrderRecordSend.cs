using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public class AutoOrderRecordSend : ServiceJob
    {
        /// <summary>
        /// 推送订单信息到一卡通
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
            OrderRecordSend();
            SaveStrategyLog();
            return true;
        }
        public void OrderRecordSend()
        {
            try
            {
                string update = "";
                int i = 0;
                DateTime datenow = DateTime.Now;
                string date = DateTime.Now.ToString("HH:mm:ss");
                string query = string.Format(@"SELECT
  null,a.UserName,DpName,MealCardID, a.DinningDate, a.MealTimeType,a. DinningRoomName,a. MealPlaceName,
  PackageName, PackagePrice,null,null
FROM
	OrderMeal_MealOrder a
	JOIN (select tbu.* from bw_Users tbu where  exists (select 1 from uum_userinfo tbuum where tbu.UserName=tbuum.UserID))  b ON a.loginname=b.UserName
	JOIN bw_Depts c ON b.DpId=c.DpId
	JOIN OrderMeal_DinningRoomMealTimeSettings d ON a.DinningRoomID=d.DinningRoomID
	AND a.MealTimeType=d.MealTimeType   
WHERE b.Status=0 and (DATEDIFF(DAY,a.DinningDate,'{1}')=0 or (DATEDIFF(DAY,a.DinningDate,'{1}')=-1 AND a.MealTimeType='早餐' )) AND a.OrderState=1 and a.OrderStateName<>'补订餐' and a.flag=0 AND d.ClosePayBackTime<'{0}' and   OrderMealRecordSendTime<'{0}' and LastOrderMealRecordSendTime>'{0}'", datenow.ToString("HH:mm:ss"), datenow.ToString("yyyy-MM-dd"));
                DataTable tb = SqlHelper.ExecuteDataset(query).Tables[0];
                if (tb == null || tb.Rows.Count == 0)
                {
                    LogRecord.WriteCanteenServiceLog(date + "没有可以推送的信息。 ==========", LogResult.normal);
                    AddStrategyLog(string.Format(date + "没有可以推送的信息："), true);
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(ConfigData.Card))
                    {

                        if (conn.State == ConnectionState.Closed)
                        {
                            conn.Open();
                        }
                        try
                        {
                            using (SqlBulkCopy copySql = new SqlBulkCopy(conn))
                            {
                                copySql.BatchSize = tb.Rows.Count;
                                copySql.BulkCopyTimeout = 600;
                                copySql.NotifyAfter = 10000;
                                copySql.DestinationTableName = "tbcz_OrderMeal_MealOrder";
                                copySql.WriteToServer(tb);
                            }
                            update = string.Format(@"update OrderMeal_MealOrder set flag=1 where  id in (SELECT
 a.id
FROM
	OrderMeal_MealOrder a JOIN bw_Users b ON a.loginname=b.UserName
	JOIN bw_Depts c ON b.DpId=c.DpId
	JOIN OrderMeal_DinningRoomMealTimeSettings d ON a.DinningRoomID=d.DinningRoomID
	AND a.MealTimeType=d.MealTimeType   
WHERE   b.Status=0  and (DATEDIFF(DAY,a.DinningDate,'{1}')=0 or (DATEDIFF(DAY,a.DinningDate,'{1}')=-1 AND a.MealTimeType='早餐' ))  AND a.OrderState=1 and a.OrderStateName<>'补订餐' and a.flag=0 AND d.ClosePayBackTime<'{0}')", datenow.ToString("HH:mm:ss"), datenow.ToString("yyyy-MM-dd"));
                            i = SqlHelper.ExecuteNonQuery(update);
                            LogRecord.WriteCanteenServiceLog(date + "推送订单信息成功,共推送。" + tb.Rows.Count + "条订单 ==========", LogResult.normal);
                            AddStrategyLog(string.Format(date + "推送订单信息成功,共推送：" + tb.Rows.Count + "条订单"), true);
                        }
                        catch (Exception ex)
                        {
                            LogRecord.WriteCanteenServiceLog(date + "推送订单信息时出现异常。 " + ex.ToString() + "==========", LogResult.error);
                            AddStrategyLog(string.Format(date + "推送订单信息时出现异常：", ex.ToString()), true);
                        }
                        finally
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:推送订单信息到一卡通出错:{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:推送订单信息到一卡通出错:{1}", strCurStrategyInfo, ex.Message), false);
            }


        }

    }
}
