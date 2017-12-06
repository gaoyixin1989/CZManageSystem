using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
   public  class SyncNeedIntegral : ServiceJob
    {
        /// <summary>
        /// 同步用户信息到积分表
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            #region 查询当前服务策略信息
            string sTemp = "";
            bool boolResult = true;
            if (!SetStrategyInfo(out sTemp))
            {
                sMessage = sTemp;
                return false;
            }
            SyncUserNeedIntegral();
            SaveStrategyLog();
            if (boolResult)
                sMessage = "服务执行成功";
            #endregion
            return true;
        }

        public void SyncUserNeedIntegral()
        {
            int errcode = 0;
            string yeardate = DateTime.Now.Year.ToString();
            string needintegral = "";
            string querysql = string.Format(@"select a.userPosiLevel,b.UserId,b.RealName from UUM_USERINFO  a
                                                left join  bw_Users  b on a.employee=b.EmployeeId
                                                where b.UserType=1 and b.Status=0");
            DataTable tbuser = SqlHelper.ExecuteDataset(querysql).Tables[0];
            querysql = "select * from HRNeedIntegral where YearDate='" + yeardate + "'";
            DataTable tbneedintegral = SqlHelper.ExecuteDataset(querysql).Tables[0];
            int i = 0;
            int tt = 0;
            try
            {
                foreach (DataRowView drv in tbuser.DefaultView)
                {
                    string userid = drv["UserId"].ToString();
                    string user_posi_level = drv["userPosiLevel"].ToString();
                    int user_posi_level2 = 0;
                    if (user_posi_level == ""||user_posi_level == null)
                    {
                        needintegral = "0";
                    }
                    else
                    {
                        user_posi_level2 = Convert.ToInt32(user_posi_level);
                    }
                    string sql = string.Format(@"select * from HRRankConfig a where a.SGrade  <='" + user_posi_level2 + "' and a.EGrade >='" + user_posi_level2 + "'");
                    DataRow maprcfig = SqlHelper.ExecuteDataset(sql).Tables[0].Rows[0];
                    if (maprcfig == null)
                    {
                        needintegral = "0";
                    }
                    else
                    {
                        needintegral = maprcfig["Integral"].ToString();
                    }

                    if (tbneedintegral.Select("UserId='" + drv["UserId"].ToString() + "'").Length == 0)
                    {
                        string insertsql = "insert into HRNeedIntegral (Id,UserId,UserName,YearDate,NeedIntegral,DoFlag) values( NEWID(),'" + drv["UserId"].ToString() + "','" + drv["RealName"].ToString() + "','" + yeardate + "','" + needintegral + "',0)";
                        errcode = SqlHelper.ExecuteNonQuery(insertsql);
                        tt++;
                    }
                    if (tbneedintegral.Select("UserId='" + drv["UserId"].ToString() + "' and DoFlag<>1").Length > 0)
                    {
                        string updatesql = "update HRNeedIntegral set NeedIntegral='" + needintegral + "'where userid ='" + userid + "'";
                        errcode = SqlHelper.ExecuteNonQuery(updatesql);
                        i++;
                    }
                }
                tbneedintegral.Clear();
                LogRecord.WriteLog(string.Format("生成要求积分成功:共新生成{0}条信息,更新{0}条信息", tt, i), LogResult.success);
                AddStrategyLog(string.Format("生成要求积分成功:共新生成{0}条信息,更新{0}条信息", tt, i), true);
            }
            catch (Exception edx)
            {
                LogRecord.WriteLog(string.Format("生成要求积分失败,原因：{0}", edx.ToString()), LogResult.fail);
                AddStrategyLog(string.Format("生成要求积分失败,原因：{0}", edx.ToString()), false);
            }
        }
    }
}
