using System;
using ServiceLibrary.Base;
using CZManageSystem.Core;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

using System.Text;
using System.Collections;
using System.Data;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Service.MarketPlan;
using CZManageSystem.Data.Domain.MarketPlan;

namespace ServiceLibrary.Services
{
    public class MarktPlanSeverices : ServiceJob
    {
        private Ucs_MarketPlan1Service _MarketPlan1Service = new Ucs_MarketPlan1Service();
        private Ucs_MarketPlan2_TmpService _MarketPlan2_TmpService = new Ucs_MarketPlan2_TmpService();
        private Ucs_MarketPlan2Service _MarketPlan2Service = new Ucs_MarketPlan2Service();
        private Ucs_MarketPlan3_TmpService _MarketPlan3_TmpService = new Ucs_MarketPlan3_TmpService();
        private Ucs_MarketPlan3Service _MarketPlan3Service = new Ucs_MarketPlan3Service();
        private Ucs_MarketPlanMonitorService _MarketPlanMonitorService = new Ucs_MarketPlanMonitorService();
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

            /////开始清空优惠方案数
            //bool result= _MarketPlan2Service.DeleteByList(_MarketPlan2Service.List().Where(m=>m.EndTime<=DateTime.Now).ToList());
            //if (result)
            //{
            //    LogRecord.WriteLog(string.Format("{0}：删除过期特殊优惠方案办理明细", strCurStrategyInfo), LogResult.normal);
            //    AddStrategyLog(string.Format("{0}：删除过期特殊优惠方案办理明细", strCurStrategyInfo), true);
            //    result = _MarketPlan3Service.DeleteByList(_MarketPlan3Service.List().Where(m => m.EndTime <= DateTime.Now).ToList());
            //    if (result){ 
            //        LogRecord.WriteLog(string.Format("{0}：删除过期特殊优惠方案成功", strCurStrategyInfo), LogResult.normal);
            //        AddStrategyLog(string.Format("{0}：删除过期特殊优惠方案成功", strCurStrategyInfo), true);


            //    }
            //    else
            //    {
            //        LogRecord.WriteLog(string.Format("{0}：删除过期特殊优惠方案失败", strCurStrategyInfo), LogResult.normal);
            //        AddStrategyLog(string.Format("{0}：删除过期特殊优惠方案失败", strCurStrategyInfo), true);
            //    }
            //}
            //else
            //{
            //    LogRecord.WriteLog(string.Format("{0}：删除过期特殊优惠方案办理明细失败", strCurStrategyInfo), LogResult.normal);
            //    AddStrategyLog(string.Format("{0}：删除过期特殊优惠方案办理明细失败", strCurStrategyInfo), true);
            //}
            DateTime dLastTime = DateTime.Now.Date;

            bool bResult = true;

            string sImportDir = ConfigData.ImportDir;
            sImportDir = Regex.Replace(sImportDir, @"\\*$", "");
            if (string.IsNullOrEmpty(sImportDir)) sImportDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + @"\Import";
            string sErrorDir = sImportDir + @"\Error";
            string sHistoryDir = sImportDir + @"\History";

            try
            {
                if (!Directory.Exists(sImportDir))
                {
                    //return false;
                    Directory.CreateDirectory(sImportDir);
                }
                if (!Directory.Exists(sErrorDir))
                {
                    Directory.CreateDirectory(sErrorDir);
                }
                if (!Directory.Exists(sHistoryDir))
                {
                    Directory.CreateDirectory(sHistoryDir);
                }

                _MarketPlan1Service.ExecuteSqlCommand("truncate table Ucs_MarketPlan1");
                _MarketPlan2_TmpService.ExecuteSqlCommand("truncate table UCS_MRKTPLAN2_tmp");
                _MarketPlan3_TmpService.ExecuteSqlCommand("truncate table UCS_MRKTPLAN3_tmp");

                string detail = ConfigData.MRKTPLANDetail_FTPDiscount;
                string MRKTPLAN = ConfigData.MRKTPLAN_FTPDiscount;
                string sRemoteFile = "*.txt";
                ImportFile(detail, sRemoteFile, sHistoryDir, sErrorDir, "");
                ImportFile(MRKTPLAN, sRemoteFile, sHistoryDir, sErrorDir, "");
                RunProcedure();


            }
            catch (Exception ex)
            {
                AddStrategyLog(ex.ToString(), false);
                LogRecord.WriteLog(string.Format("{0}：" + ex.ToString(), strCurStrategyInfo), LogResult.fail);
            }
            return true;
        }


        private void ImportFile(string RemotePath, string sRemoteFile, string sHistoryDir, string sErrorDir, string tableName)
        {


            // 建立FTP对象 和 本地存放的相关信息
            FTPClient ftp = new FTPClient();
            ftp.RemoteHost = ConfigData.RemoteHost_FTPDiscount;
            ftp.RemotePort = ConfigData.RemotePort_FTPDiscount;
            ftp.RemoteUser = ConfigData.RemoteUser_FTPDiscount;
            ftp.RemotePass = ConfigData.RemotePass_FTPDiscount;
            string detail = ConfigData.MRKTPLANDetail_FTPDiscount;
            string MRKTPLAN = ConfigData.MRKTPLAN_FTPDiscount;
            ftp.RemotePath = RemotePath;
            string sLocalPath = ConfigData.LocalPath_FTPDiscount;
            try
            {
                if (Directory.Exists(sLocalPath) == false) Directory.CreateDirectory(sLocalPath);
                else
                {
                    ClearFloder(sLocalPath);
                    if (Directory.Exists(sLocalPath) == false) Directory.CreateDirectory(sLocalPath);
                }
                ftp.Connect();
                try
                {
                    ftp.Get(sRemoteFile, sLocalPath);

                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                    throw;
                }
                string[] arFile = Directory.GetFiles(sLocalPath, sRemoteFile, SearchOption.TopDirectoryOnly);

                foreach (string sFile in arFile)
                {

                    StringBuilder sbLog = new StringBuilder();

                    string sFileName = sLocalPath + "\\" + Path.GetFileName(sFile);
                    string oldFileName = Path.GetFileName(sFile);
                    ArrayList alMessage = new ArrayList();
                    if (File.Exists(sFileName))
                    {
                        //将文件中的数据先导入到临时表
                        if (RemotePath.Equals(detail))//明细
                            tableName = "Ucs_MarketPlan1";
                        else
                        {
                            tableName = "Ucs_MarketPlan2_Tmp";
                        }

                        #region 将文件导入到datatable后再进行入库
                        SqlHelper.setConnection(ConfigData.CZMS_Connection);
                        DataTable table = SqlHelper.ExecuteDataTable("select  [Coding],[Name],[StartTime],[EndTime],[Channel],[Orders],[RegPort],[DetialInfo],[Remark],[NumCount],[PlanType] ,[TargetUsers],[PaysRlues],[Templet1] ,[Templet2],[Templet3],[Templet4] ,[IsMarketing] from " + tableName);
                        string filePath = sFileName;
                        //因为文件比较大，所有使用StreamReader的效率要比使用File.ReadLines高
                        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);


                        using (StreamReader sr = new StreamReader(fileStream, Encoding.GetEncoding("gb2312")))
                        {
                            int lineIndex = 0;
                            while (!sr.EndOfStream)
                            {
                                lineIndex++;
                                DataRow dr = table.NewRow();//创建数据行
                                string readStr = sr.ReadLine();//读取一行数据

                                //string[] strs = readStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);//将读取的字符串按"制表符/t“和””“分割成数组
                                string[] strs = readStr.Split('|');//将读取的字符串按"制表符/t“和””“分割成数组
                                try
                                {
                                    for (int i = 0; i < strs.Length && i < table.Columns.Count; i++)
                                    {
                                        // table.Columns[i].DataType = typeof(string);
                                        if (i == 3 || i == 4) /// StartTime，EndTime对应的文本格式为20161021
                                            dr[i] = string.IsNullOrEmpty(strs[i]) ? DBNull.Value:(object) new DateTime(Convert.ToInt32(strs[i].Substring(0, 4)), Convert.ToInt32(strs[i].Substring(4, 2)), Convert.ToInt32(strs[i].Substring(6, 2)));
                                        else
                                            dr[i] = string.IsNullOrEmpty(strs[i]) ? DBNull.Value : (object)strs[i];
                                    }
                                    table.Rows.Add(dr);//将创建的数据行添加到table中
                                }
                                catch (Exception ex)
                                {
                                    string str = readStr;

                                    AddStrategyLog(string.Format("{0}：文件" + sFileName + "内容出错：行号：" + lineIndex + "，出错原因为：" + ex.Message, strCurStrategyInfo), true);
                                    //throw ex;

                                }
                            }

                        }
                        try
                        {

                            AddStrategyLog(string.Format("{0}：SqlBulkInsert" + tableName, strCurStrategyInfo), true);

                            string[] result = SqlHelper.SqlBulkInsert(table, tableName);
                            AddStrategyLog(string.Format("{0}：导入临时表[" + tableName + "]结果：" + result[0], strCurStrategyInfo), true);

                            _MarketPlanMonitorService.Insert(new Ucs_MarketPlanMonitor()
                            {
                                Id = new Guid(),
                                Count = Convert.ToInt32(result[1]),
                                Creattime = DateTime.Now,
                                ImportName = filePath,
                                Status = "成功",
                                Remark = result[0]

                            });
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            _MarketPlanMonitorService.Insert(new Ucs_MarketPlanMonitor()
                            {
                                Id = new Guid(),
                                Count = 0,
                                Creattime = DateTime.Now,
                                ImportName = filePath,
                                Status = "失败",
                                Remark = "文件[" + sFileName + "]导入临时表[" + tableName + "]失败:" + ex.Message,

                            });

                            LogRecord.WriteLog(string.Format("{0}：文件[" + sFileName + "]导入临时表[" + tableName + "]失败: " + ex.ToString(), strCurStrategyInfo), LogResult.fail);
                        }

                    }
                    try
                    {
                        // 把文件移到历史文件夹
                        string sDatetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        File.Move(sFile, sHistoryDir + "\\" + sDatetime + "_" + oldFileName);

                        //移动
                        // 删除FTP文件
                        ftp.Delete(oldFileName);
                    }
                    catch (Exception ex)
                    {
                        AddStrategyLog(ex.ToString(), false);
                    }
                }
            }
            catch (Exception ex)
            {
                AddStrategyLog(ex.ToString(), false);
            }
            finally
            {
                ftp.DisConnect();
            }
        }
        public bool ClearFloder(string Folder)
        {
            if (!Directory.Exists(Folder)) return true;
            try
            {
                string[] files = Directory.GetFiles(Folder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                //迁移目录
                string[] directorys = Directory.GetDirectories(Folder);
                foreach (string directory in directorys)
                {
                    ClearFloder(directory);
                    Directory.Delete(directory, true);

                }
            }
            catch (Exception ex)
            {
                LogRecord.WriteLog(string.Format("{0}：清空" + Folder + "文件夹失败：" + ex.Message, strCurStrategyInfo), LogResult.normal);
                AddStrategyLog(string.Format("{0}：清空" + Folder + "文件夹失败" + ex.Message, strCurStrategyInfo), true);
                return false;
            }
            return true;
        }

        public void RunProcedure()
        {
            try
            {
                SqlHelper.setConnection(ConfigData.CZMS_Connection);
                SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, "PRO_UCS_MRKTPLAN");
            }
            catch (Exception ex)
            {
                AddStrategyLog("运行存储过程PRO_UCS_MRKTPLAN失败："+ex.ToString(), false);
                LogRecord.WriteLog(string.Format("{0}：运行存储过程PRO_UCS_MRKTPLAN失败" + ex.ToString(), strCurStrategyInfo), LogResult.fail);
            }

        }
    }
}
