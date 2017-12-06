using CZManageSystem.Core.Helpers;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 系统服务接口
/// </summary>
namespace ServiceLibrary
{
    public abstract class ServiceJob
    {
        #region 实现接口
        private string message = "";
        SysLogService _sysLogService = new SysLogService();
        public string sServiceStrategyID { get; set; }//服务策略ID
        public string sMessage
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }//执行信息
        public abstract bool Execute();//执行服务
        #endregion

        protected int intRetriedTimes = 3;//数据执行任务失败允许尝试次数
        protected SysServiceStrategy curStrategy = new SysServiceStrategy();//当前的服务策略信息
        protected string strCurStrategyInfo = "";//用于记录日志的当前服务策略信息
        protected List<SysServiceStrategyLog> listStrategyLog = new List<SysServiceStrategyLog>();

        public ServiceJob()
        {
            string s1 = System.Configuration.ConfigurationManager.AppSettings["retriedTimes"];
            int.TryParse(s1, out intRetriedTimes);
            intRetriedTimes = intRetriedTimes > 0 ? intRetriedTimes : 3;
        }

        //~ServiceJob()
        //{
        //    SaveStrategyLog();
        //}
        /// <summary>
        /// 设置当前服务策略信息
        /// </summary>
        /// <returns></returns>
        protected bool SetStrategyInfo(out string strMsg)
        {
            strMsg = "";
            try
            {
                curStrategy = new SysServiceStrategyService().FindById(int.Parse(sServiceStrategyID));
                if (curStrategy == null || curStrategy.SysServices == null)
                {
                    strMsg = "服务策略信息或服务信息不存在";
                    return false;
                }
            }
            catch (Exception ex)
            {
                strMsg = "查询服务策略信息失败";
                return false;
            }
            strCurStrategyInfo = string.Format("服务策略“{0}”-服务“{1}”", curStrategy.Id, curStrategy.SysServices.ServiceName);
            return true;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        protected void AddStrategyLog(string strMsg, bool result)
        {
            if (curStrategy.LogFlag ?? false)
            {
                listStrategyLog.Add(new SysServiceStrategyLog()
                {
                    LogTime = DateTime.Now,
                    ServiceStrategyId = DbHelper.ToInt32(sServiceStrategyID),
                    Result = result,
                    Description = strMsg
                });
            }
        }
        /// <summary>
        /// 保存日志
        /// </summary>
        protected void SaveStrategyLog()
        {
            try
            {
                if ((curStrategy.LogFlag ?? false) && listStrategyLog.Count > 0)
                { 
                     if(_sysLogService.WriteSysLog(listStrategyLog).Result)  //服务策略日志
                        listStrategyLog.Clear();
                }
            }
            catch (Exception ex) { }
        }
    }
}
