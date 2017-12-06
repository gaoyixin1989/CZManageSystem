using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using ServiceLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 推送待阅信息_当前使用
/// </summary>
namespace ServiceLibrary
{
    public class PushPendingMsg : ServiceJob
    {
        private const int tryMaxTime = 3;//数据推送最高尝试次数
        private string dataSource = DataIdToInt.DataSourceType.ToRead;

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

            List<Tracking_ToReview_Detail> list = new List<Tracking_ToReview_Detail>();//待推送的待办信息
            #region 获取需要推送的信息 
            try
            {
                //拼接查询sql，条件如下：
                //1、未曾推送过的(表DataIdToInt和PendingData没有对应数据)
                //2、为推送成功且尝试次数未超过tryMaxTime
                string strSQL = string.Format(@"select a.* from vw_bwwf_Tracking_ToReview_Detail a
                                                left join DataIdToInt b on a.ActivityInstanceId=b.DataId and b.DataSource='{0}'
                                                left join PendingData c on CAST(a.ActivityInstanceId as varchar(100)) =c.DataID and a.UserName=c.Owner and b.ID=c.SendID and c.DataSource=b.DataSource
                                                where a.State=0 and (b.ID is null or c.ID is null or (c.State=1 and c.TryTime<{1}))"
                                                , dataSource, tryMaxTime);

                list = new EfRepository<Tracking_ToReview_Detail>().ExecuteResT<Tracking_ToReview_Detail>(strSQL).ToList();
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}：查询待推送待阅信息失败", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}：查询待推送待阅信息失败", strCurStrategyInfo), true);
                sMessage = "查询待推送待阅信息失败";
                return false;
            }
            #endregion

            SystemResult result = new SystemResult();
            result = SendingData(list);

            SaveStrategyLog();
            //修改待阅为已阅，具体操作未知
            if (!result.IsSuccess)
            {
                sMessage = "推送待阅信息失败：" + result.Message;
                return false;
            }
            else
            {
                sMessage = "推送待阅信息成功";
                return true;
            }

        }

        public SystemResult SendingData(List<Tracking_ToReview_Detail> data)
        {
            SystemResult result = new SystemResult();
            try
            {
                int intSuccess = 0;
                int intError = 0;

                foreach (var item in data)
                {
                    int toReadID = CommonFun.GetId(dataSource, item.ActivityInstanceId.ToString());

                    object[] args = new object[10];
                    args[0] = ConfigData.Portal_SystemId;//appName
                    args[1] = ConfigData.Portal_UserName;//userName
                    args[2] = ConfigData.Portal_PassWord;//password
                    args[3] = item.UserName;//Owner
                    args[4] = toReadID;//toReadID
                    args[5] = item.Title;//todoTitle
                    args[6] = ConfigData.Portal_DealUrlAuthority + "Plugins/EasyFlow/contrib/workflow/pages/workflowview.aspx?type=review&aiid=" + item.ActivityInstanceId.ToString();//url
                    args[7] = item.CreatedTime.ToString("MM/dd/yyyy HH:mm:ss");//createTime
                    args[8] = null;//toReadType
                    args[9] = 1;//type:处理类型(1=新增、2=修改、3＝删除)
                    object obj = WebServicesHelper.InvokeWebService(ConfigData.Portal_PendingUrl, ConfigData.Portal_PendingClassName, "applicationToRead", args);

                    AppResult appResult = new AppResult();
                    appResult = CommonFun.TranObjToObjByJson<AppResult>(obj);

                    //记录推送记录
                    CommonFun.UpdatePendingData(dataSource, item.ActivityInstanceId.ToString(), toReadID, item.UserName, "add", appResult.successful);

                    if (appResult.successful)
                    {
                        intSuccess++;
                    }
                    else
                    {
                        intError++;
                    }

                }

                LogRecord.WriteLog(string.Format("{0}：推送待阅信息：成功{1}条，失败{2}条", strCurStrategyInfo, intSuccess, intError), LogResult.normal);
                AddStrategyLog(string.Format("{0}：推送待阅信息：成功{1}条，失败{2}条", strCurStrategyInfo, intSuccess, intError), true);
                result.IsSuccess = true;
                result.Message = string.Format("成功{0}条，失败{1}条", intSuccess, intError);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
            }

            return result;
        }


    }
}
