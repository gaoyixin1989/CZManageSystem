using CZManageSystem.Core;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using CZManageSystem.Service.HumanResources.Attendance;
using CZManageSystem.Service.SysManger;
using ServiceLibrary.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 考勤数据同步，考勤数据处理。
/// </summary>
namespace ServiceLibrary
{
    public class HrSyncManager : ServiceJob
    {
        IHRAttendanceConfigService hrAttendanceConfigService = new HRAttendanceConfigService();
        ISysUserService _userService = new SysUserService();
        IHRCheckAttendanceService _checkService = new HRCheckAttendanceService();
        IHRCheckAttendanceHistoryNo1Service _checkHistoryService = new HRCheckAttendanceHistoryNo1Service();

        private IFingerprintDataService fingerRfsimService = new FingerprintDataService();
        private IHRRfsimService hrRfsimService = new HRRfsimService();
        private IHRTBcardService hrTBcardService = new HRTBcardService();
        private IHRYKTDataService yktService = new HRYKTDataService();
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

            SystemResult _result = new SystemResult();
            bool boolResult = true;

            LogRecord.WriteLog(string.Format("{0}:开始同步考勤记录信息", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:开始同步考勤记录信息", strCurStrategyInfo), true);
            YKTData();//一卡通考勤同步考勤结果数据到本系统中
            SyncHrData();//指纹考勤同步考勤结果数据到本系统中
            RFSIMData();//RFSIM考勤同步考勤结果数据到本系统中
            TBKData();//通宝卡考勤同步考勤结果数据到本系统中
            HrDataAction();//处理同步过来的考勤数据，与排班表关联：问题：暂时指纹机的数据没有办法与用户数据做关联
            LogRecord.WriteLog(string.Format("{0}:考勤记录信息同步结束", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:考勤记录信息同步结束", strCurStrategyInfo), true);

            SaveStrategyLog();//保存日志到数据库
            if (boolResult)
                sMessage = "服务执行成功";
            return boolResult;
        }

        /// <summary>
        /// 一卡通考勤同步考勤结果数据到本系统中
        /// </summary>
        private void YKTData()
        {
            if (!ConfigData.IsGet_HrData)
                return;
            LogRecord.WriteLog(string.Format("{0}:开始同步一卡通考勤信息到本系统", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:开始同步一卡通考勤信息到本系统", strCurStrategyInfo), true);

            try
            {
                while (true)
                {
                    var tid = yktService.List().Max(u => u.Tid);//已经读取到本系统数据库中的最大tid
                    tid = tid ?? 0;
                    string strSQL = string.Format(@"select top {1} * from view_CardReader_Log where ID>'{0}' order by ID", tid.Value, ConfigData.KQ_GetCount);
                    KaoQinDAO.init(ConfigData.YKT_Connect);
                    DataTable tb = KaoQinDAO.sqlTable(strSQL);
                    if (tb == null || tb.Rows.Count == 0)
                        break;

                    //写入数据;
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        DateTime tempDate = new DateTime();
                        long tempLong = 0;
                        string sqlInsert = string.Empty;
                        List<HRYKTData> listNew = new List<HRYKTData>();
                        foreach (DataRow row in tb.Rows)
                        {
                            HRYKTData temp = new HRYKTData();
                            temp.ID = Guid.NewGuid();
                            temp.ActionStatus = 0;
                            temp.Tid = int.Parse(row["ID"].ToString());
                            if (long.TryParse(row["CardID"].ToString(), out tempLong))
                                temp.CardID = tempLong;
                            if (long.TryParse(row["BusinessCardID"].ToString(), out tempLong))
                                temp.BusinessCardID = tempLong;
                            temp.loginId = row["loginId"].ToString();
                            temp.Employee = row["Employee"].ToString();
                            temp.Name = row["Name"].ToString();
                            temp.DepartmentName = row["DepartmentName"].ToString();
                            temp.DoorName = row["DoorName"].ToString();
                            temp.Path = row["Path"].ToString();
                            if (DateTime.TryParse(row["ReaderTime"].ToString(), out tempDate))
                                temp.ReaderTime = tempDate;

                            listNew.Add(temp);
                        }
                        if (yktService.InsertByList(listNew))
                        {
                            LogRecord.WriteLog(string.Format("{0}:成功同步{1}条一卡通考勤信息到本系统", strCurStrategyInfo, listNew.Count), LogResult.success);
                            AddStrategyLog(string.Format("{0}:成功同步{1}条一卡通考勤信息到本系统", strCurStrategyInfo, listNew.Count), true);
                        }
                        else
                        {
                            LogRecord.WriteLog(string.Format("{0}:保存一卡通考勤信息到本系统失败", strCurStrategyInfo), LogResult.error);
                            AddStrategyLog(string.Format("{0}:保存一卡通考勤信息到本系统失败", strCurStrategyInfo), false);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:一卡通考勤信息到本系统同步出错:{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:一卡通考勤信息到本系统同步出错:{1}", strCurStrategyInfo, ex.Message), false);
            }

            LogRecord.WriteLog(string.Format("{0}:同步一卡通考勤信息到本系统结束", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:同步一卡通考勤信息到本系统结束", strCurStrategyInfo), true);
        }

        /// <summary>
        /// 指纹考勤同步考勤结果数据到本系统中
        /// </summary>
        private void SyncHrData()
        {
            if (!ConfigData.IsGet_HrData)
                return;
            LogRecord.WriteLog(string.Format("{0}:开始同步指纹考勤信息到本系统", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:开始同步指纹考勤信息到本系统", strCurStrategyInfo), true);

            try
            {
                while (true)
                {
                    var tid = fingerRfsimService.List().Max(u => u.Tid);//已经读取到本系统数据库中的最大tid
                    tid = tid ?? 0;
                    string strSQL = string.Format(@"select top {1} * from AccessData where id>'{0}' order by id", tid.Value, ConfigData.KQ_GetCount);
                    KaoQinDAO.init(ConfigData.SyncHrData_Connect);
                    DataTable tb = KaoQinDAO.sqlTable(strSQL);
                    if (tb == null || tb.Rows.Count == 0)
                        break;
                    //写入数据;
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        DateTime tempDate = new DateTime();
                        int tempInt = 0;
                        string sqlInsert = string.Empty;
                        List<FingerprintData> listNew = new List<FingerprintData>();
                        foreach (DataRow row in tb.Rows)
                        {
                            FingerprintData temp = new FingerprintData();
                            temp.ID = Guid.NewGuid();
                            temp.ActionStatus = 0;
                            temp.Tid = int.Parse(row["id"].ToString());
                            temp.DevicePhyAddr = row["DevicePhyAddr"].ToString();
                            if (int.TryParse(row["RecType"].ToString(), out tempInt))
                                temp.RecType = tempInt;
                            if (int.TryParse(row["RecStatus"].ToString(), out tempInt))
                                temp.RecStatus = tempInt;
                            temp.RecTypes = row["RecTypes"].ToString();
                            temp.RecStatuss = row["RecStatuss"].ToString();
                            temp.CardSerno = row["CardSerno"].ToString();
                            temp.EmpNumber = row["cardserNo"].ToString();
                            if (DateTime.TryParse(row["RecTime"].ToString(), out tempDate))
                                temp.Rectime = tempDate;
                            //temp.EnsureTime =
                            temp.Operate = row["Operate"].ToString();
                            if (int.TryParse(row["Status"].ToString(), out tempInt))
                                temp.Status = tempInt;
                            if (DateTime.TryParse(row["OpDateTime"].ToString(), out tempDate))
                                temp.OpDatetime = tempDate;
                            temp.DoorName = row["DoorName"].ToString();
                            temp.DeviceserNo = row["DeviceSerno"].ToString();
                            if (int.TryParse(row["DoorID"].ToString(), out tempInt))
                                temp.DoorId = tempInt;
                            if (int.TryParse(row["EmpID"].ToString(), out tempInt))
                                temp.EmpId = tempInt;

                            listNew.Add(temp);
                        }
                        if (fingerRfsimService.InsertByList(listNew))
                        {
                            LogRecord.WriteLog(string.Format("{0}:成功同步{1}条指纹考勤信息到本系统", strCurStrategyInfo, listNew.Count), LogResult.success);
                            AddStrategyLog(string.Format("{0}:成功同步{1}条指纹考勤信息到本系统", strCurStrategyInfo, listNew.Count), true);
                        }
                        else
                        {
                            LogRecord.WriteLog(string.Format("{0}:保存指纹考勤信息到本系统失败", strCurStrategyInfo), LogResult.error);
                            AddStrategyLog(string.Format("{0}:保存指纹考勤信息到本系统失败", strCurStrategyInfo), false);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:指纹考勤信息到本系统同步出错:{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:指纹考勤信息到本系统同步出错:{1}", strCurStrategyInfo, ex.Message), false);
            }

            LogRecord.WriteLog(string.Format("{0}:同步指纹考勤信息到本系统结束", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:同步指纹考勤信息到本系统结束", strCurStrategyInfo), true);
        }
        /// <summary>
        /// RFSIM考勤同步考勤结果数据到本系统中
        /// </summary>
        private void RFSIMData()
        {
            LogRecord.WriteLog(string.Format("{0}:开始同步RFSIM考勤信息到本系统", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:开始同步RFSIM考勤信息到本系统", strCurStrategyInfo), true);
            try
            {
                while (true)
                {
                    var tid = hrRfsimService.List().Max(u => u.Tid);//已经读取到本系统数据库中的最大tid
                    tid = tid ?? 0;
                    string strSQL = string.Format(@"select top {1} * from kq where recordid>{0} order by recordid ", tid.Value, ConfigData.KQ_GetCount);
                    KaoQinDAO.init(ConfigData.RFSIMData_Connect);
                    DataTable tb = KaoQinDAO.sqlTable(strSQL);
                    if (tb == null || tb.Rows.Count == 0)
                        break;
                    //写入数据;
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        string sqlInsert = string.Empty;
                        List<HRRfsim> listNew = new List<HRRfsim>();

                        DateTime tempDate = new DateTime();
                        int tempInt = 0;

                        foreach (DataRowView drv in tb.DefaultView)
                        {
                            HRRfsim temp = new HRRfsim();
                            temp.RecordId = Guid.NewGuid();
                            temp.ActionStatus = 0;
                            temp.Tid = int.Parse(drv["recordid"].ToString());
                            temp.SysNo = drv["sysno"].ToString();
                            temp.Serial = drv["serial"].ToString();
                            //temp.CDateTime = DateTime.Parse(drv["cdatetime"].ToString());
                            if (DateTime.TryParse(drv["cdatetime"].ToString(), out tempDate))
                                temp.CDateTime = tempDate;
                            //temp.DeviceSysId = int.Parse(drv["devicesysid"].ToString());
                            if (int.TryParse(drv["devicesysid"].ToString(), out tempInt))
                                temp.DeviceSysId = tempInt;
                            temp.RecordType = drv["recordtype"].ToString();
                            temp.OperatorId = drv["operatorid"].ToString();
                            temp.EmplyName = drv["EmplyName"].ToString();
                            temp.DptId = drv["DptId"].ToString();
                            temp.EmplyId = drv["EmplyId"].ToString();
                            listNew.Add(temp);
                        }

                        if (hrRfsimService.InsertByList(listNew))
                        {
                            LogRecord.WriteLog(string.Format("{0}:成功同步{1}条RFSIM考勤信息到本系统", strCurStrategyInfo, listNew.Count), LogResult.success);
                            AddStrategyLog(string.Format("{0}:成功同步{1}条RFSIM考勤信息到本系统", strCurStrategyInfo, listNew.Count), true);
                        }
                        else
                        {
                            LogRecord.WriteLog(string.Format("{0}:保存RFSIM考勤信息到本系统失败", strCurStrategyInfo), LogResult.error);
                            AddStrategyLog(string.Format("{0}:保存RFSIM考勤信息到本系统失败", strCurStrategyInfo), false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:RFSIM考勤信息到本系统同步出错：{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:RFSIM考勤信息到本系统同步出错：{1}", strCurStrategyInfo, ex.Message), false);
            }


            LogRecord.WriteLog(string.Format("{0}:同步RFSIM考勤信息到本系统结束", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:同步RFSIM考勤信息到本系统结束", strCurStrategyInfo), true);
        }
        /// <summary>
        /// 通宝卡考勤同步考勤结果数据到本系统中
        /// </summary>
        private void TBKData()
        {
            LogRecord.WriteLog(string.Format("{0}:开始同步通宝卡考勤信息到本系统", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:开始同步通宝卡考勤信息到本系统", strCurStrategyInfo), true);

            try
            {
                while (true)
                {
                    var tid = hrTBcardService.List().Max(u => u.Tid);//已经读取到本系统数据库中的最大tid
                    tid = tid ?? 0;
                    string strSQL = string.Format(@"select top {1} * from V_KQTIME where id>{0} order by id ", tid.Value, ConfigData.KQ_GetCount);
                    KaoQinDAO.init(ConfigData.TBKData_Connect);
                    DataTable tb = KaoQinDAO.sqlTable(strSQL);
                    if (tb == null || tb.Rows.Count == 0)
                        break;
                    //写入数据;
                    if (tb != null && tb.Rows.Count > 0)
                    {
                        DateTime tempDate = new DateTime();
                        string sqlInsert = string.Empty;
                        List<HRTBcard> listNew = new List<HRTBcard>();
                        foreach (DataRowView drv in tb.DefaultView)
                        {
                            HRTBcard temp = new HRTBcard();
                            temp.ID = Guid.NewGuid();
                            temp.Tid = int.Parse(drv["id"].ToString());
                            temp.EmployeeId = drv["employeeid"].ToString();
                            temp.SkTime = DateTime.ParseExact(drv["sktime"].ToString(), "yyyyMMddHHmmss", new System.Globalization.CultureInfo("zh-CN", true));
                            //if (DateTime.TryParse(drv["sktime"].ToString(), out tempDate))
                            //    temp.SkTime = tempDate;
                            temp.ActionStatus = 0;
                            temp.EmpNo = drv["empno"].ToString();
                            temp.CardNo = drv["cardno"].ToString();
                            listNew.Add(temp);
                        }
                        if (hrTBcardService.InsertByList(listNew))
                        {
                            LogRecord.WriteLog(string.Format("{0}:成功同步{1}条通宝卡考勤信息到本系统", strCurStrategyInfo, listNew.Count), LogResult.success);
                            AddStrategyLog(string.Format("{0}:成功同步{1}条通宝卡考勤信息到本系统", strCurStrategyInfo, listNew.Count), true);
                        }
                        else
                        {
                            LogRecord.WriteLog(string.Format("{0}:保存通宝卡考勤信息到本系统失败", strCurStrategyInfo), LogResult.error);
                            AddStrategyLog(string.Format("{0}:保存通宝卡考勤信息到本系统失败", strCurStrategyInfo), false);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:通宝卡考勤信息到本系统同步出错：{1}", strCurStrategyInfo, ex.Message), LogResult.error);
                AddStrategyLog(string.Format("{0}:通宝卡考勤信息到本系统同步出错：{1}", strCurStrategyInfo, ex.Message), false);
            }

            LogRecord.WriteLog(string.Format("{0}:同步通宝卡考勤信息到本系统结束", strCurStrategyInfo), LogResult.normal);
            AddStrategyLog(string.Format("{0}:同步通宝卡考勤信息到本系统结束", strCurStrategyInfo), true);
        }
        /// <summary>
        /// 处理同步过来的考勤数据，与排班表关联
        /// 问题：暂时指纹机的数据没有办法与用户数据做关联
        /// </summary>
        public void HrDataAction()
        {
            try
            {
                LogRecord.WriteLog(string.Format("{0}:开始处理考勤记录与排班表的关系", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}:开始处理考勤记录与排班表的关系", strCurStrategyInfo), true);

                var list1 = fingerRfsimService.List().Where(u => (u.ActionStatus ?? 0) == 0).OrderBy(u => u.Tid).Take(ConfigData.KQ_GetCount).ToList();
                var list2 = hrRfsimService.List().Where(u => (u.ActionStatus ?? 0) == 0).OrderBy(u => u.Tid).Take(ConfigData.KQ_GetCount).ToList();
                var list3 = hrTBcardService.List().Where(u => (u.ActionStatus ?? 0) == 0).OrderBy(u => u.Tid).Take(ConfigData.KQ_GetCount).ToList();
                var list4 = yktService.List().Where(u => (u.ActionStatus ?? 0) == 0).OrderBy(u => u.Tid).Take(ConfigData.KQ_GetCount).ToList();


                var list2temp = list2.Select(u => new KaoQinClass
                {
                    Dataid = u.RecordId,
                    Actiontype = 2,
                    UserID = _userService.FindByFeldName(m => m.UserName == u.EmplyId)?.UserId,
                    Rectime = u.CDateTime
                }).ToList();
                var list3temp = list3.Select(u => new KaoQinClass
                {
                    Dataid = u.ID,
                    Actiontype = 3,
                    UserID = _userService.FindByFeldName(m => m.UserName == u.EmployeeId)?.UserId,
                    Rectime = u.SkTime
                }).ToList();
                var list4temp = list4.Select(u => new KaoQinClass
                {
                    Dataid = u.ID,
                    Actiontype = 4,
                    UserID = _userService.FindByFeldName(m => m.UserName == u.loginId)?.UserId,
                    Rectime = u.ReaderTime
                }).ToList();

                List<KaoQinClass> listData = new List<KaoQinClass>();
                listData.AddRange(list2temp);
                listData.AddRange(list3temp);
                listData.AddRange(list4temp);

                if (ConfigData.IsGet_HrData)
                {//判定是否需要指纹考勤的数据
                    var list1temp = list1.Select(u => new KaoQinClass
                    {
                        Dataid = u.ID,
                        Actiontype = 1,
                        UserID = _userService.FindByFeldName(m => m.EmployeeId == (u.EmpId ?? 0).ToString())?.UserId,
                        Rectime = u.Rectime
                    }).ToList();
                    listData.AddRange(list1temp);
                }
                var OrginallistData = listData;
                listData = listData.Where(u => u.Rectime.HasValue && u.UserID.HasValue).OrderBy(u => u.Rectime).ToList();

                int sbtime = 0;//定义整数型上班刷卡可提前刷卡限制时间：单位分钟
                int xbtime = 0;//定义整数型下班刷卡可推迟刷卡限制时间：单位分钟
                var model = hrAttendanceConfigService.FindByFeldName(f => f.DeptIds == "NULL");
                if (model != null)
                {
                    sbtime = model.LeadTime ?? 0;
                    xbtime = model.LatestTime ?? 0;
                }
                foreach (var item in listData)
                {
                    DateTime curDate = item.Rectime.Value.Date;
                    if (curDate > DateTime.Now.Date)
                        continue;
                    if (curDate >= DateTime.Now.Date.AddDays(-30).Date)
                    {
                        #region 信息解析关联
                        //DateTime temp1 = curDate.AddDays(-3);
                        //DateTime temp2 = curDate.AddDays(3);
                        var listCurDate = _checkService.List().Where(u => u.UserId == item.UserID && u.AtDate == curDate).ToList();
                        var curData1 = listCurDate.Where(u => u.AtDate.HasValue);
                        curData1 = curData1.Where(u => (u.AtDate + (u.DoTime.HasValue ? u.DoTime : new TimeSpan())).Value.AddMinutes(sbtime * -1) <= item.Rectime);
                        curData1 = curData1.Where(u => ((u.OffDate.HasValue ? u.OffDate : u.AtDate) + (u.OffTime.HasValue ? u.OffTime : new TimeSpan())).Value.AddMinutes(xbtime) >= item.Rectime);
                        var curData = curData1.FirstOrDefault();
                        if (curData != null)
                        {
                            var varAtDate = curData.AtDate.Value;
                            var varOffDate = (curData.OffDate ?? curData.AtDate).Value;
                            var varDoTime = curData.DoTime ?? new TimeSpan();
                            var varOffTime = curData.OffTime ?? new TimeSpan();

                            if (!curData.DoReallyTime.HasValue
                                && item.Rectime.Value.CompareTo(varOffDate + varOffTime) <= 0)
                            {//未登记上班时间，且在下班时间之前，则登记为上班打卡
                                curData.FlagOn = item.Actiontype;
                                curData.DoReallyDate = item.Rectime.Value.Date;
                                curData.DoReallyTime = item.Rectime.Value.TimeOfDay;
                                _checkService.Update(curData);
                            }
                            else if (curData.DoReallyTime.HasValue
                                && item.Rectime > (varAtDate + varDoTime)
                                && item.Rectime > (curData.DoReallyDate + curData.DoReallyTime)
                                && (!curData.OffReallyTime.HasValue || item.Rectime > (curData.OffReallyDate + curData.OffReallyTime)))
                            {//已经登记上班时间，且打卡时间大于上班登记时间；并且下班未登记或者大于原来的登记时间，则更新为下班登记时间
                                curData.FlagOff = item.Actiontype;
                                curData.OffReallyDate = item.Rectime.Value.Date;
                                curData.OffReallyTime = item.Rectime.Value.TimeOfDay;
                                _checkService.Update(curData);
                            }
                            else if (item.Rectime >= (varOffDate + varOffTime)
                                && (!curData.OffReallyTime.HasValue || item.Rectime > (curData.OffReallyDate + curData.OffReallyTime)))
                            {//打卡时间在下班后，且时间大于下班登记时间，则登记为下班打卡
                                curData.FlagOff = item.Actiontype;
                                curData.OffReallyDate = item.Rectime.Value.Date;
                                curData.OffReallyTime = item.Rectime.Value.TimeOfDay;
                                _checkService.Update(curData);
                            }

                            //switch (item.Actiontype.Value)
                            //{
                            //    case 1:
                            //        {
                            //            var temp = fingerRfsimService.FindById(item.Dataid);
                            //            if (temp != null)
                            //            {
                            //                temp.ActionStatus = 1;
                            //                fingerRfsimService.Update(temp);
                            //            }
                            //        }
                            //        break;
                            //    case 2:
                            //        {
                            //            var temp = hrRfsimService.FindById(item.Dataid);
                            //            if (temp != null)
                            //            {
                            //                temp.ActionStatus = 1;
                            //                hrRfsimService.Update(temp);
                            //            }
                            //        }
                            //        break;
                            //    case 3:
                            //        {
                            //            var temp = hrTBcardService.FindById(item.Dataid);
                            //            if (temp != null)
                            //            {
                            //                temp.ActionStatus = 1;
                            //                hrTBcardService.Update(temp);
                            //            }
                            //        }
                            //        break;
                            //    case 4:
                            //        {
                            //            var temp = yktService.FindById(item.Dataid);
                            //            if (temp != null)
                            //            {
                            //                temp.ActionStatus = 1;
                            //                yktService.Update(temp);
                            //            }
                            //        }
                            //        break;
                            //    default: break;
                            //}
                        }
                        #endregion
                    }
                    else
                    {//需要去history表
                        #region 信息解析关联
                        //DateTime temp1 = curDate.AddDays(-3);
                        //DateTime temp2 = curDate.AddDays(3);
                        var listCurDate = _checkHistoryService.List().Where(u => u.UserId == item.UserID && u.AtDate == curDate).ToList();
                        var curData = listCurDate.Where(u => u.AtDate.HasValue)
                            .Where(u => (u.AtDate + (u.DoTime.HasValue ? u.DoTime : new TimeSpan())).Value.AddMinutes(sbtime * -1) <= item.Rectime)
                            .Where(u => ((u.OffDate.HasValue ? u.OffDate : u.AtDate) + (u.OffTime.HasValue ? u.OffTime : new TimeSpan())).Value.AddMinutes(xbtime) >= item.Rectime).FirstOrDefault();

                        if (curData != null)
                        {
                            #region
                            var varAtDate = curData.AtDate.Value;
                            var varOffDate = (curData.OffDate ?? curData.AtDate).Value;
                            var varDoTime = curData.DoTime ?? new TimeSpan();
                            var varOffTime = curData.OffTime ?? new TimeSpan();

                            if (!curData.DoReallyTime.HasValue
                                && item.Rectime.Value.CompareTo(varOffDate + varOffTime) <= 0)
                            {//未登记上班时间，且在下班时间之前，则登记为上班打卡
                                curData.FlagOn = item.Actiontype;
                                curData.DoReallyDate = item.Rectime.Value.Date;
                                curData.DoReallyTime = item.Rectime.Value.TimeOfDay;
                                _checkHistoryService.Update(curData);
                            }
                            else if (curData.DoReallyTime.HasValue
                                && item.Rectime > (varAtDate + varDoTime)
                                && item.Rectime > (curData.DoReallyDate + curData.DoReallyTime)
                                && (!curData.OffReallyTime.HasValue || item.Rectime > (curData.OffReallyDate + curData.OffReallyTime)))
                            {//已经登记上班时间，且打卡时间大于上班登记时间；并且下班未登记或者大于原来的登记时间，则更新为下班登记时间
                                curData.FlagOff = item.Actiontype;
                                curData.OffReallyDate = item.Rectime.Value.Date;
                                curData.OffReallyTime = item.Rectime.Value.TimeOfDay;
                                _checkHistoryService.Update(curData);
                            }
                            else if (item.Rectime >= (varOffDate + varOffTime)
                                && (!curData.OffReallyTime.HasValue || item.Rectime > (curData.OffReallyDate + curData.OffReallyTime)))
                            {//打卡时间在下班后，且时间大于下班登记时间，则登记为下班打卡
                                curData.FlagOff = item.Actiontype;
                                curData.OffReallyDate = item.Rectime.Value.Date;
                                curData.OffReallyTime = item.Rectime.Value.TimeOfDay;
                                _checkHistoryService.Update(curData);
                            }
                            #endregion
                        }
                        //switch (item.Actiontype.Value)
                        //{
                        //    case 1:
                        //        {
                        //            var temp = fingerRfsimService.FindById(item.Dataid);
                        //            if (temp != null)
                        //            {
                        //                temp.ActionStatus = 1;
                        //                fingerRfsimService.Update(temp);
                        //            }
                        //        }
                        //        break;
                        //    case 2:
                        //        {
                        //            var temp = hrRfsimService.FindById(item.Dataid);
                        //            if (temp != null)
                        //            {
                        //                temp.ActionStatus = 1;
                        //                hrRfsimService.Update(temp);
                        //            }
                        //        }
                        //        break;
                        //    case 3:
                        //        {
                        //            var temp = hrTBcardService.FindById(item.Dataid);
                        //            if (temp != null)
                        //            {
                        //                temp.ActionStatus = 1;
                        //                hrTBcardService.Update(temp);
                        //            }
                        //        }
                        //        break;
                        //    case 4:
                        //        {
                        //            var temp = yktService.FindById(item.Dataid);
                        //            if (temp != null)
                        //            {
                        //                temp.ActionStatus = 1;
                        //                yktService.Update(temp);
                        //            }
                        //        }
                        //        break;
                        //    default: break;
                        //}
                        #endregion
                    }

                    switch (item.Actiontype.Value)
                    {
                        case 1:
                            {
                                var temp = fingerRfsimService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    fingerRfsimService.Update(temp);
                                }
                            }
                            break;
                        case 2:
                            {
                                var temp = hrRfsimService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    hrRfsimService.Update(temp);
                                }
                            }
                            break;
                        case 3:
                            {
                                var temp = hrTBcardService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    hrTBcardService.Update(temp);
                                }
                            }
                            break;
                        case 4:
                            {
                                var temp = yktService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    yktService.Update(temp);
                                }
                            }
                            break;
                        default: break;
                    }
                }


                foreach (var item in OrginallistData)
                {
                    switch (item.Actiontype.Value)
                    {
                        case 1:
                            {
                                var temp = fingerRfsimService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    fingerRfsimService.Update(temp);
                                }
                            }
                            break;
                        case 2:
                            {
                                var temp = hrRfsimService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    hrRfsimService.Update(temp);
                                }
                            }
                            break;
                        case 3:
                            {
                                var temp = hrTBcardService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    hrTBcardService.Update(temp);
                                }
                            }
                            break;
                        case 4:
                            {
                                var temp = yktService.FindById(item.Dataid);
                                if (temp != null)
                                {
                                    temp.ActionStatus = 1;
                                    yktService.Update(temp);
                                }
                            }
                            break;
                        default: break;
                    }
                }


                LogRecord.WriteLog(string.Format("{0}:处理考勤记录与排班表的关系结束", strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}:处理考勤记录与排班表的关系结束", strCurStrategyInfo), true);
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}:处理考勤记录与排班表的关系出错:{1}", strCurStrategyInfo, ex.Message), LogResult.normal);
                AddStrategyLog(string.Format("{0}:处理考勤记录与排班表的关系出错:{1}", strCurStrategyInfo, ex.Message), true);
            }

        }


    }


    public class KaoQinClass
    {
        public Guid? Dataid { get; set; }
        public int? Actiontype { get; set; }
        public Guid? UserID { get; set; }
        public DateTime? Rectime { get; set; }
    }



}
