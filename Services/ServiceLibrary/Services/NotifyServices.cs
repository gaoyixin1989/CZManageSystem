using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 发送通知服务
/// 目前只处理短信（不包括回复）
/// </summary>
namespace ServiceLibrary
{
    public class NotifyServices : ServiceJob
    {
        ICzRemindersService _czRemindersService = new CzRemindersService();
        IVwCzRemindersDetailService _czRemindersDetailService = new VwCzRemindersDetailService();
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

            List<VwCzRemindersDetail> list = new List<VwCzRemindersDetail>();//待发送的通知信息
            try
            {
                #region 获取需要发送的通知信息
                //State:1为未发送，2为已发送
                //MsgType:1-电子邮件，2-短信消息，4-可回复的短信消息
                //RetriedTimes:失败重复次数
                //CreatedTime：创建时间
                int[] intMsgType = { 1, 2, 4 };
                DateTime lastTime = DateTime.Now.AddHours(ConfigData.Notify_HourRange * -1);
                list = _czRemindersDetailService.List().Where(u => (u.State ?? 0) == 1
                && intMsgType.Contains(u.MsgType ?? 0)
                && (u.RetriedTimes ?? 0) < ConfigData.Notify_TryTime
                && (u.CreatedTime.HasValue == false || u.CreatedTime.Value >= lastTime)).ToList();
                #endregion
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}：查询需要发送的通知信息失败", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}：查询需要发送的通知信息失败", strCurStrategyInfo), true);
                sMessage = "查询需要发送的通知信息失败";
                return false;
            }

            //需要发送短信消息的数据：MsgType==2
            List<VwCzRemindersDetail> listSMS = list.Where(u => (u.MsgType ?? 0) == 2).ToList();

            SmsDAO.init(ConfigData.SMS_Connection);//初始化短信系统数据库连接字符串

            int intSuccess = 0;
            int intError = 0;
            DealNotify_SMS(listSMS, out intSuccess, out intError);

            LogRecord.WriteLog(string.Format("成功发送短信{0}条，失败{1}条", intSuccess, intError), LogResult.normal);
            AddStrategyLog(string.Format("成功发送短信{0}条，失败{1}条", intSuccess, intError), true);

            SaveStrategyLog();
            sMessage = string.Format("成功发送短信{0}条，失败{1}条", intSuccess, intError);
            return true;

        }

        /// <summary>
        /// 将传入的数据进行发送短信操作
        /// </summary>
        /// <param name="listData"></param>
        public void DealNotify_SMS(List<VwCzRemindersDetail> listData, out int intSuccess, out int intError)
        {
            intSuccess = 0;
            intError = 0;
            foreach (var item in listData)
            {
                CzReminders tempObj = _czRemindersService.FindById(item.ID);
                if (CommonFun.SendSms(item.ReceiverMobile, item.Content))
                {
                    intSuccess++;
                    tempObj.ProcessedTime = DateTime.Now;
                    tempObj.State = 2;
                }
                else
                {
                    intError++;
                    tempObj.ProcessedTime = DateTime.Now;
                    tempObj.RetriedTimes = (tempObj.RetriedTimes ?? 0) + 1;
                    if ((tempObj.RetriedTimes ?? 0) >= ConfigData.Notify_TryTime)
                        tempObj.State = -1;//失败次数达到尝试次数限制后，状态修改为-1
                    LogRecord.WriteLog(string.Format("{0}：发送短信到{1}({2})失败,内容：{3}", strCurStrategyInfo, item.ReceiverEmployeeId, item.ReceiverMobile, item.Content), LogResult.fail);
                    AddStrategyLog(string.Format("{0}：发送短信到{1}({2})失败，内容：{3}", strCurStrategyInfo, item.ReceiverEmployeeId, item.ReceiverMobile, item.Content), false);
                }
                _czRemindersService.Update(tempObj);
            }
        }
        
    }
}
