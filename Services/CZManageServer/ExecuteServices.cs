using CZManageSystem.Core;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CZManageServer
{
    public class ExecuteServices
    {
        #region 变量
        private int timeForUpdateServiceStrategyInfo = 10 * 60 * 1000;//从数据库更新服务策略信息的时间间隔（秒），默认10分钟
        private int timeForExecuteServiceStrategyInfo = 1 * 1000;//执行服务策略信息的时间间隔（秒），默认1秒
        System.Timers.Timer timer_UpdateServiceStrategyInfo;  //计时器,用于从数据库更新服务策略信息
        System.Timers.Timer timer_Execute;  //计时器,用于循环判断执行服务策略
        ISysServiceStrategyService _serviceStrategyService = new SysServiceStrategyService();//服务策略
        //ISysServiceStrategyLogService _sysServiceStrategyLogService = new SysServiceStrategyLogService();//服务策略日志
        SysLogService _sysLogService = new SysLogService(); //服务策略日志
        private List<SysServiceStrategy> listAll_Strategy = new List<SysServiceStrategy>();//服务策略信息
        private List<SysServiceStrategy> listExecuting_Strategy = new List<SysServiceStrategy>();//正在执行中的服务策略   
        private Dictionary<string, Assembly> dictAssembly = new Dictionary<string, Assembly>();
        #endregion

        #region 构造函数
        public ExecuteServices()
        {
            GetConfigValue();
            timer_UpdateServiceStrategyInfo = new System.Timers.Timer();
            timer_UpdateServiceStrategyInfo.Interval = timeForUpdateServiceStrategyInfo;  //设置计时器事件间隔执行时间
            timer_UpdateServiceStrategyInfo.Elapsed += new System.Timers.ElapsedEventHandler(timer_UpdateServiceStrategyInfo_Elapsed);
            timer_UpdateServiceStrategyInfo.Enabled = true;
            timer_UpdateServiceStrategyInfo_Elapsed(null, null);

            timer_Execute = new System.Timers.Timer();
            timer_Execute.Interval = timeForExecuteServiceStrategyInfo;  //设置计时器事件间隔执行时间
            timer_Execute.Elapsed += new System.Timers.ElapsedEventHandler(timer_Execute_Elapsed);
            timer_Execute.Enabled = true;
            timer_Execute_Elapsed(null, null);
        }
        #endregion
        #region 配置信息
        /// <summary>
        /// 读取设置配置信息
        /// </summary>
        private void GetConfigValue()
        {
            string s1 = System.Configuration.ConfigurationManager.AppSettings["timeForUpdateServiceStrategyInfo"];
            string s2 = System.Configuration.ConfigurationManager.AppSettings["timeForExecuteServiceStrategyInfo"];
            int intTemp = 0;
            if (int.TryParse(s1, out intTemp))
                timeForUpdateServiceStrategyInfo = intTemp;
            if (int.TryParse(s2, out intTemp))
                timeForExecuteServiceStrategyInfo = intTemp;
        }
        #endregion

        #region 计时器触发的方法
        /// <summary>
        /// 定时更新服务策略信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_UpdateServiceStrategyInfo_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GetServiceStrategyInfo();
        }

        /// <summary>
        /// 判断是否执行服务策略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Execute_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<SysServiceStrategy> listWillExecute = new List<SysServiceStrategy>();//需要被执行的策略
            listWillExecute = listAll_Strategy.Where(u => (u.EnableFlag ?? false) == true//启用的
                                                    && u.ValidTime.Value.Date >= DateTime.Now.Date//有效时间大于等于当前时间的
                                                    && u.NextRunTime.Value <= DateTime.Now//下次执行时间小于等于当前时间
                                                    && (!listExecuting_Strategy.Select(t => t.ServiceId).Contains(u.ServiceId))//对应服务非正在执行
                                                    && u.SysServices != null
                                                    ).ToList();
            foreach (SysServiceStrategy curItem in listWillExecute)
            {
                Thread t1 = new Thread(ExecuteThread);//执行服务策略任务的线程
                t1.IsBackground = true;
                t1.Start(curItem);
            }

        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 获取更新服务策略信息
        /// </summary>
        private void GetServiceStrategyInfo()
        {
            try
            {
                listAll_Strategy = _serviceStrategyService.GetValidServiceStrategyData().ToList();
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(ex.Message, LogResult.error);
            }
        }

        /// <summary>
        /// 执行服务策略任务的线程
        /// </summary>
        /// <param name="task"></param>
        private void ExecuteThread(object task)
        {
            SysServiceStrategy curTask = (SysServiceStrategy)task;
            DateTime startTime = DateTime.Now;//记录开始时间
            DateTime endTime = DateTime.Now;//记录结束时间
            List<SysServiceStrategyLog> listLog = new List<SysServiceStrategyLog>();
            bool result = false;//执行结果
            string strMsg = "";//执行结果描述
            listExecuting_Strategy.Add(curTask);//将当前任务添加到正在执行的列表中
            try
            {
                _sysLogService.WriteSysLog(new SysServiceStrategyLog()
                {
                    LogTime = DateTime.Now,
                    ServiceStrategyId = curTask.Id,
                    Result = true,
                    Description = "开始执行"
                });

                if (curTask.SysServices != null)
                {
                    LogRecord.WriteLog(string.Format("服务策略编号({0})-名称({1})：开始执行", curTask.Id, curTask.SysServices.ServiceName), LogResult.normal);

                    #region 执行服务

                    result = ExecuteTask(curTask, out strMsg);
                    LogRecord.WriteLog(string.Format("服务策略编号({0})-名称({1})：{2}", curTask.Id, curTask.SysServices.ServiceName, strMsg), result ? LogResult.success : LogResult.fail);
                    #endregion

                    LogRecord.WriteLog(string.Format("服务策略编号({0})-名称({1})：执行结束", curTask.Id, curTask.SysServices.ServiceName), LogResult.normal);

                    endTime = DateTime.Now;
                    SetStrategyToNext(curTask, endTime);//更新服务策略下次运行时间
                }
                else
                {
                    LogRecord.WriteLog(string.Format("服务策略编号({0})不存在服务", curTask.Id), LogResult.error);
                }

                GetServiceStrategyInfo();//更新信息
            }
            catch (Exception ex)
            {
                strMsg = "程序出错:" + ex.Message;
                LogRecord.WriteLog(ex.Message, LogResult.error);
            }
            finally
            {
                listExecuting_Strategy.Remove(curTask);//将当前任务从正在执行的列表中移除
                listLog.Add(new SysServiceStrategyLog()
                {
                    LogTime = DateTime.Now,
                    ServiceStrategyId = curTask.Id,
                    Result = result,
                    Description = strMsg
                });
                listLog.Add(new SysServiceStrategyLog()
                {
                    LogTime = DateTime.Now,
                    ServiceStrategyId = curTask.Id,
                    Result = true,
                    Description = "执行结束"
                });
            }

            try
            {
                if (listLog.Count > 0)
                    _sysLogService.WriteSysLog(listLog);//记录日志
               
            }
            catch
            {
                LogRecord.WriteLog(string.Format("服务策略编号({0})记录日志到数据库失败", curTask.Id), LogResult.error);
            }

            Thread.CurrentThread.Abort();//终止线程
        }

        /// <summary>
        /// 执行服务策略任务
        /// </summary>
        /// <param name="curTask">服务策略</param>
        /// <param name="strMsg">结果信息</param>
        /// <returns>执行结果</returns>
        private bool ExecuteTask(SysServiceStrategy curTask, out string strMsg)
        {
            strMsg = "";
            //string strDllPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + curTask.SysServices.AssemblyName + ".dll";
            //if (!File.Exists(strDllPath))
            //{
            //    strMsg = "目标程序集不存在";
            //    return false;
            //}
            //Assembly assembly = Assembly.LoadFile(strDllPath); // 加载程序集
            Assembly assembly = null;
            try
            {
                if (dictAssembly.ContainsKey(curTask.SysServices.AssemblyName))
                    assembly = dictAssembly[curTask.SysServices.AssemblyName];
                else
                {
                    assembly = Assembly.Load(curTask.SysServices.AssemblyName);
                    if (!dictAssembly.ContainsKey(curTask.SysServices.AssemblyName))
                        dictAssembly.Add(curTask.SysServices.AssemblyName, assembly);
                }
            }
            catch (Exception ex)
            {
                strMsg = "加载目标程序集失败";
                return false;
            }

            object obj = assembly.CreateInstance(curTask.SysServices.ClassName, true); // 创建类的实例 
            if (obj == null)
            {
                strMsg = "目标类名不存在";
                return false;
            }

            PropertyInfo property_ServiceStrategyID = obj.GetType().GetProperty("sServiceStrategyID");
            if (property_ServiceStrategyID == null)
            {
                strMsg = "目标类中不存在公共属性sServiceStrategyID";
                return false;
            }
            property_ServiceStrategyID.SetValue(obj, curTask.Id.ToString());

            PropertyInfo property_Message = obj.GetType().GetProperty("sMessage");
            if (property_Message == null)
            {
                strMsg = "目标类中不存在公共属性sMessage";
                return false;
            }

            MethodInfo methodExecute = obj.GetType().GetMethod("Execute");
            if (methodExecute == null)
            {
                strMsg = "目标类中不存在公共方法Execute";
                return false;
            }
            if (methodExecute.ReturnType.ToString() != "System.Boolean")
            {
                strMsg = "目标类中方法Execute的返回类型不为布尔型";
                return false;
            }

            bool result = (bool)methodExecute.Invoke(obj, null);
            //strMsg = result ? "执行目标任务成功" : "执行目标任务失败";
            strMsg = property_Message.GetValue(obj).ToString();
            return result;
        }

        /// <summary>
        /// 更新服务策略下次运行时间
        /// </summary>
        /// <param name="item"></param>
        /// <param name="lastTime"></param>
        private void SetStrategyToNext(SysServiceStrategy item, DateTime lastTime)
        {
            var obj = _serviceStrategyService.FindById(item.Id);
            if ((obj.PeriodNum ?? 0) <= 0)
            {//周期小于等于0，表示只执行这一次，将EnableFlag置为false
                obj.EnableFlag = false;
            }
            else
            {
                switch (obj.PeriodType)
                {
                    case "年": obj.NextRunTime = lastTime.AddYears(obj.PeriodNum.Value); break;
                    case "月": obj.NextRunTime = lastTime.AddMonths(obj.PeriodNum.Value); break;
                    case "周": obj.NextRunTime = lastTime.AddDays(obj.PeriodNum.Value * 7); break;
                    case "天": obj.NextRunTime = lastTime.AddDays(obj.PeriodNum.Value); break;
                    case "小时": obj.NextRunTime = lastTime.AddHours(obj.PeriodNum.Value); break;
                    case "分钟": obj.NextRunTime = lastTime.AddMinutes(obj.PeriodNum.Value); break;
                    case "秒": obj.NextRunTime = lastTime.AddSeconds(obj.PeriodNum.Value); break;
                    default: obj.EnableFlag = false; break;//PeriodType未能识别同样认为值执行这一次，将EnableFlag置为false
                }
            }
            _serviceStrategyService.Update(obj);
        }
        #endregion


    }



}
