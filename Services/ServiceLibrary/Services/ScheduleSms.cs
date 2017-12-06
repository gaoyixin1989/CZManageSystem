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
/// 为日程安排发送短信通知
/// </summary>
namespace ServiceLibrary
{
    public class ScheduleSms : ServiceJob
    {
        IUserConfigService _userConfigService = new UserConfigService();//个人配置信息
        IScheduleService _scheduleService = new ScheduleService();//日程信息

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

            //查询配置了需要发送日程安排提醒的配置信息
            List<UserConfig> listConfigs = _userConfigService.List().Where(u => u.ConfigName == "Schedule" && u.ConfigValue.Contains("State=true")).ToList();

            foreach (var item in listConfigs)
            {
                //提醒发送的时间量：以安排当天0点为原点，根据hour的值选择时间点发送提醒：
                //hour为非负数，表示当天hour点发送当天的提醒信息；
                //hour为负数，表示在0点处提前[hour]个小时发送提醒信息；
                //当hour值大于等于24时，默认为7；
                int hour = 0;
                foreach (var item1 in item.ConfigValue.Split(';'))
                {
                    if (item1.Contains("Hour="))
                        int.TryParse(item1.Split('=')[1], out hour);
                }
                if (hour >= 24) hour = 7;
                List<Schedule> listData = GetDataForSend(item.UserID.Value, hour);
                if (listData.Count > 0)
                    DetalScheduleMsm(item.UserObj, listData);

            }

            SaveStrategyLog();
            return true;
        }

        /// <summary>
        /// 根据配置信息中的hour值或者当前需要发送提醒的日程安排信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public List<Schedule> GetDataForSend(Guid userId, int hour)
        {
            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.AddHours(hour * -1).Date.AddDays(1).AddSeconds(-1);
            List<Schedule> listData = _scheduleService.List().Where(u => u.UserId == userId
             && u.Time >= startDate
             && u.Time <= endDate
             && (u.Sms ?? false) == false).ToList();

            return listData;
        }

        /// <summary>
        /// 处理需要发送通知的数据
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        public void DetalScheduleMsm(Users user, List<Schedule> list)
        {
            list = list.OrderBy(u => u.Time.Value).ToList();
            List<DateTime> listDate = list.GroupBy(u => u.Time.Value.Date).Select(u=>u.Key).ToList();
            foreach (var curDate in listDate) {
                var curList = list.Where(u => u.Time.Value.Date == curDate).ToList();
                string strContent = string.Format("[综合管理平台-日程安排]{0}，您好！您{1}有以下日程安排：",user.RealName,curDate.ToString("yyyy-MM-dd"));
                foreach (var curItem in curList)
                {
                    strContent += string.Format("{0}-{1}；",curItem.Time.Value.ToString("HH:mm"),curItem.Content);
                }
                if (CommonFun.SendSms(user.Mobile, strContent))
                {
                    LogRecord.WriteLog(string.Format("{0}：发送{1}的日程安排通知到{2}({3})成功,内容：{4}", strCurStrategyInfo, curDate.ToString("yyyy-MM-dd"), user.RealName, user.Mobile, strContent), LogResult.success);
                    AddStrategyLog(string.Format("{0}：发送{1}的日程安排通知到{2}({3})成功,内容：{4}", strCurStrategyInfo, curDate.ToString("yyyy-MM-dd"), user.RealName, user.Mobile, strContent), true);
                }
                else {
                    LogRecord.WriteLog(string.Format("{0}：发送{1}的日程安排通知到{2}({3})失败,内容：{4}", strCurStrategyInfo, curDate.ToString("yyyy-MM-dd"), user.RealName, user.Mobile, strContent), LogResult.fail);
                    AddStrategyLog(string.Format("{0}：发送{1}的日程安排通知到{2}({3})失败,内容：{4}", strCurStrategyInfo, curDate.ToString("yyyy-MM-dd"), user.RealName, user.Mobile, strContent), false);
                }

            }

        }

    }
}
