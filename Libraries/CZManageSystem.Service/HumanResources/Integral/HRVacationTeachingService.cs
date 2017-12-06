using Aspose.Cells;
using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Integral;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.Integral
{
    public class HRVacationTeachingService : BaseService<HRVacationTeaching>, IHRVacationTeachingService
    {
        static Users _user;
        private readonly IRepository<Users> _bw_Users = new EfRepository<Users>();
        ITIntegralConfigService _hrcintegralconfigservice = new TIntegralConfigService();
        IHRIntegralService _hrintegralservice = new HRIntegralService();
        IHRVCoursesImportService _hrvcourseimporservice = new HRVCoursesImportService();
        public IList<HRVacationTeaching> GetForPagingByCondition(Users user, out int count, HRVacationTeachingQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curData = GetQueryTable(objs); ;
            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._bw_Users.Table
                          on hrvc.UserId equals bwu.UserId
                          select new
                          {
                              hrvc.ID,
                              DpId = bwu.DpId,
                              UserName = bwu.UserName,
                              RealName = bwu.RealName,
                              hrvc.TeacherType,
                              hrvc.TeachingPlan,
                              hrvc.UserId,
                              hrvc.PeriodTime,
                              hrvc.StartTime,
                              hrvc.EndTime,
                              bwu.EmployeeId,
                              hrvc.Integral,
                              bwu.Status,
                              bwu.UserType,
                              Year = hrvc.StartTime.Value.Year.ToString()
                          };

            curData = curData.Where(u => u.UserType == 1 && u.Status == 0);
            if (objs.DpId != null)
                curData = curData.Where(u => objs.DpId.Contains(u.DpId));
            if (objs.Year != null && objs.Year != "")
                curData = curData.Where(u => objs.Year.Contains(u.Year));
            if (objs.TeacherType != null && objs.TeacherType != "")
                curData = curData.Where(u => objs.TeacherType.Contains(u.TeacherType));
            if (objs.EmployeeID != null && objs.EmployeeID != "")
                curData = curData.Where(u => u.EmployeeId.Contains(objs.EmployeeID));
            if (objs.RealName != null && objs.RealName != "")
                curData = curData.Where(u => u.RealName.Contains(objs.RealName));
            _user = user;
            var mm = new EfRepository<string>().Execute<string>(string.Format("exec HR_GetUser @UserName='{0}'", _user.UserName)).ToList();
            curData = curData.Where(u => mm.Contains(u.UserName.ToString()));
            count = curData.Count();
            var list = curData.OrderByDescending(p => p.StartTime).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new HRVacationTeaching()
            {
                ID = x.ID,
                TeachingPlan = x.TeachingPlan,
                TeacherType = x.TeacherType,
                UserId = x.UserId,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                PeriodTime = x.PeriodTime,
                Integral = x.Integral
            });
            return list.ToList();
        }

        public IList<HRVacationTeaching> GetForPersonalPagingByCondition(out int count, HRVacationTeachingQueryBuilder objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var curData = GetQueryTable(objs); ;
            var curData = from hrvc in this._entityStore.Table
                          join bwu in this._bw_Users.Table
                          on hrvc.UserId equals bwu.UserId
                          select new
                          {
                              hrvc.ID,
                              DpId = bwu.DpId,
                              UserName = bwu.UserName,
                              RealName = bwu.RealName,
                              hrvc.TeacherType,
                              hrvc.TeachingPlan,
                              hrvc.UserId,
                              hrvc.PeriodTime,
                              hrvc.StartTime,
                              hrvc.EndTime,
                              bwu.EmployeeId,
                              hrvc.Integral,
                              Year = hrvc.StartTime.Value.Year.ToString()
                          };
            if (objs.Year != null && objs.Year != "")
            {
                DateTime SD = Convert.ToDateTime(objs.Year + "-01-01");
                DateTime ED = Convert.ToDateTime(objs.Year + "-12-31");
                curData = curData.Where(u => u.StartTime.Value >= SD && u.EndTime.Value <= u.EndTime.Value);
            }                

            if (objs.RealName != null && objs.RealName != "")
                curData = curData.Where(u => objs.RealName.Contains(u.RealName));

            count = curData.Count();
            var list = curData.OrderByDescending(p => p.StartTime).Skip(pageIndex * pageSize).Take(pageSize).AsEnumerable().Select(x => new HRVacationTeaching()
            {
                ID = x.ID,
                TeachingPlan = x.TeachingPlan,
                TeacherType = x.TeacherType,
                UserId = x.UserId,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                PeriodTime = x.PeriodTime,
                Integral = x.Integral
            });
            return list.ToList();
        }


        public dynamic ImportVacationCourses(string filename, Stream fileStream, Users user)
        {
            try
            {
                bool IsSuccess = false;
                int count = 0;
                _user = user;
                int row = 1;
                string error = "", niimport = "", dbniimport = "";
                string repeaterror = "", usererror = "", enougherror = "", userrepeaterror = "",configerror="";
                var dataTable = ExcelToDatatable(fileStream);
                if (dataTable.Rows.Count < 1)
                    return false;
                List<HRVacationTeaching> list = new List<HRVacationTeaching>();
                List<HRIntegral> listHRIntegral = new List<HRIntegral>();
                foreach (DataRow item in dataTable.Rows)
                {
                    row++;
                    List<string> tip = new List<string>();
                    HRIntegral HRIntegralModel = new HRIntegral();
                    var model = GetModel(item, out tip, out HRIntegralModel, out niimport);
                    dbniimport += niimport;
                    if (model == null)
                    {
                        switch (tip[0])
                        {
                            case "重复记录":
                                repeaterror += "," + row;
                                break;
                            case "信息不完整":
                                enougherror += "," + row;
                                break;
                            case "用户不存在或数据出错":
                                usererror += "," + row;
                                break;
                            case "名字重复":
                                userrepeaterror += "," + row;
                                break;
                            case "基础配置":
                                configerror += "," + row;
                                break;
                        }
                        continue;
                    }
                    list.Add(model);
                    listHRIntegral.Add(HRIntegralModel);
                    if (list.Count == 100) //足够100的
                    {
                        //休假状态更新到相应的班次上
                        for (int i = 0; i < list.Count; i++)
                        {
                            UpdateHRCheckAttendance(list[i].StartTime.Value, list[i].EndTime.Value, "2", list[i].UserId.Value);
                        }
                        if (this.InsertByList(list))
                        {
                            count += list.Count;
                            list.Clear();
                            IsSuccess = true;
                        };

                    }
                    if (listHRIntegral.Count == 100) //足够100的
                    {
                        if (_hrintegralservice.InsertByList(listHRIntegral))
                        {
                            listHRIntegral.Clear();
                        };
                    }

                }
                if (list.Count > 0)//不足100的
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        UpdateHRCheckAttendance(list[i].StartTime.Value, list[i].EndTime.Value, "2", list[i].UserId.Value);
                    }
                    if (this.InsertByList(list))
                    {
                        count += list.Count;
                        list.Clear();
                        IsSuccess = true;
                    };
                }
                if (listHRIntegral.Count > 0)//不足100的
                {
                    if (_hrintegralservice.InsertByList(listHRIntegral))
                    {
                        listHRIntegral.Clear();
                    };
                }
                if (repeaterror.Length > 0)
                    error += "第" + repeaterror.Remove(0, 1) + "行:有重复的记录;";
                if (enougherror.Length > 0)
                    error += "第" + enougherror.Remove(0, 1) + "行:信息不完整;";
                if (usererror.Length > 0)
                    error += "第" + usererror.Remove(0, 1) + "行:用户不存在或数据出错;";
                if (userrepeaterror.Length > 0)
                    error += "第" + userrepeaterror.Remove(0, 1) + "行:名字重复;";
                if (configerror.Length > 0)
                    error += "第" + configerror.Remove(0, 1) + "行:没有该天数的基础配置;";
                int falCount = dataTable.Rows.Count - count;
                var result = new { IsSuccess = IsSuccess, Message = "成功导入" + count + "条，失败" + falCount + "条。其他提示：" + error };
                HRVCoursesImport impobj = new HRVCoursesImport();
                impobj.Importor = user.RealName;
                impobj.ImportTime = DateTime.Now;
                impobj.ImportMsg = error;
                if (dbniimport.Length > 0)
                    impobj.ImportInformation = dbniimport.Remove(dbniimport.Length - 1, 1);
                impobj.ImportTitle = filename;
                impobj.ImportType = "Teaching";
                _hrvcourseimporservice.Insert(impobj);
                return result;
            }
            catch (Exception ex)
            {
                var result = new { IsSuccess = false, Message = "文件内容错误！" };
                return result;
            }
        }

        DataTable ExcelToDatatable(Stream fileStream)
        {
            try
            {
                Workbook book = new Workbook(fileStream);
                Worksheet sheet = book.Worksheets[0];
                Cells cells = sheet.Cells;

                DataTable dt = new DataTable("Workbook");
                DataColumnCollection columns = dt.Columns;
                for (int i = 0; i < cells.MaxDataColumn + 1; i++)
                    columns.Add(i.ToString(), typeof(System.String));
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        HRVacationTeaching GetModel(DataRow dataRow, out List<string> tip, out HRIntegral HRIntegralModel, out string shtmw)
        {
            //验证数据是否合法 
            tip = new List<string>();
            shtmw = "";
            HRIntegralModel = new HRIntegral();
            Users dicUsers = new Users();
            DateTime? TimeNull = null;
            decimal? decimalnull = null;
            double integral = 0.00, times = 0.00, c_integral = 0.00;
            string buseformula = "";
            var _temp = dataRow.ItemArray;
            HRVacationTeaching temp = new HRVacationTeaching();
            IHRCintegralConfigService _hrcintegralconfigservice = new HRCintegralConfigService();
            ISysUserService _sysuserservice = new SysUserService();

            var CurrDataBase = this._entityStore.Table;


            if (string.IsNullOrEmpty(_temp[0].ToString().Trim()) || string.IsNullOrEmpty(_temp[1].ToString().Trim()) || string.IsNullOrEmpty(_temp[2].ToString().Trim()) || string.IsNullOrEmpty(_temp[3].ToString().Trim()) || string.IsNullOrEmpty(_temp[4].ToString().Trim()) || string.IsNullOrEmpty(_temp[5].ToString().Trim()) || string.IsNullOrEmpty(_temp[6].ToString().Trim()))
                tip.Add("信息不完整");
            else
            {
                //检测是否有重复的记录
                var tmp0 = _temp[0].ToString();
                var tmp1 = _temp[1].ToString();
                var tmp2 = _temp[2].ToString();
                var tmp3 = Convert.ToDateTime(_temp[4]);
                var checkexsitlist = CurrDataBase.Where(u => u.TeachingPlan == tmp1).Where(u => u.UserName == tmp0).Where(u => u.StartTime.Value == tmp3).ToList();
                if (checkexsitlist.Count > 0)
                {
                    tip.Add("重复记录");
                    shtmw += _temp[0].ToString() + "*" +
                             _temp[1].ToString() + "*" +
                             _temp[2].ToString() + "*" +
                             _temp[3].ToString() + "*" +
                             _temp[4].ToString() + "*" +
                             _temp[5].ToString() + "*" +
                             _temp[6].ToString() + "*" + "$";
                }
                else
                {
                    //导入时检查用户表中是否有同名的人，有则不导入此用户的信息。
                    var UsersDataList = _bw_Users.Table.Where(u => u.EmployeeId == tmp0).ToList();
                    if (UsersDataList.Count > 1)
                        tip.Add("名字重复");
                    else if (UsersDataList.Count == 0)
                        tip.Add("用户不存在或数据出错");
                    else
                    {
                        temp.UserId = UsersDataList[0].UserId;
                        temp.UserName = _temp[1].ToString();
                        //检测天数的基础配置
                        var configlist = _hrcintegralconfigservice.JudgeConfig(Convert.ToDouble(_temp[6].ToString()));
                        if (configlist==null)
                            tip.Add("基础配置");
                        else
                        {
                            buseformula = configlist[0].BuseFormula;
                            times = configlist[0].Times.HasValue ? configlist[0].Times.Value : 0;
                            integral = configlist[0].Integral.HasValue ? configlist[0].Integral.Value : 0;
                        }
                    }
                }
            }
            if (tip.Count > 0)
            {
                return null;
            }
            if (buseformula == "公式")
                c_integral = Convert.ToDouble(_temp[6].ToString()) * times;
            else if (buseformula == "常量")
                c_integral = integral;

            temp.ID = Guid.NewGuid();
            temp.Integral = Convert.ToDecimal(c_integral);
            temp.TeachingPlan = _temp[2].ToString();
            temp.TeacherType = _temp[3].ToString();
            temp.StartTime = string.IsNullOrEmpty(_temp[4].ToString()) ? TimeNull : Convert.ToDateTime(_temp[4].ToString());
            temp.EndTime = string.IsNullOrEmpty(_temp[5].ToString()) ? TimeNull : Convert.ToDateTime(_temp[5].ToString());
            temp.PeriodTime = string.IsNullOrEmpty(_temp[6]?.ToString()) ? decimalnull : Convert.ToDecimal(_temp[6].ToString());
            temp.AgreeFlag = 2;

            HRIntegralModel.YearDate = temp.StartTime.Value.Year;
            HRIntegralModel.CIntegral = temp.Integral;
            HRIntegralModel.TPeriodTime = temp.PeriodTime;
            HRIntegralModel.FinishTime = temp.EndTime.Value.ToString("yyyy-MM-dd HH:mm");
            HRIntegralModel.Daoid = temp.ID;
            HRIntegralModel.IntegralId = Guid.NewGuid();
            HRIntegralModel.UserId = temp.UserId;

            return temp;
        }


        /// <summary>
        /// 更新对应的考勤的休假状态
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Flag"></param>
        /// <param name="UserId"></param>
        private void UpdateHRCheckAttendance(DateTime StartTime, DateTime EndTime, string Flag, Guid UserId)
        {
            string sql = "";
            sql = string.Format(@"update HRCheckAttendance set doflag={0} where AttendanceId in  
                                ( 
                                    select AttendanceId from HRCheckAttendance where userid='{1}'
                                    and(('{2}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime))
                                    or ('{3}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime))
                                    or ('{2}'<=convert(varchar,atdate)+' '+convert(varchar,dotime) and '{3}'>=convert(varchar,atdate)+' '+convert(varchar,offtime)) and dotime is not null) 
                                ) ", Flag, UserId, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            SqlHelper.ExecuteNonQuery(sql);
            sql = string.Format(@"update HRCheckAttendanceHistoryNo1 set doflag={0} where HistoryId in  
                                ( 
                                    select HistoryId from HRCheckAttendanceHistoryNo1 where userid='{1}'
                                    and(('{2}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime))
                                    or ('{3}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime)) 
                                    or ('{2}'<=convert(varchar,atdate)+' '+convert(varchar,dotime) and '{3}'>=convert(varchar,atdate)+' '+convert(varchar,offtime)) and dotime is not null) 
                                ) ", Flag, UserId, StartTime.ToString("yyyy-MM-dd HH:mm:ss"), EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            SqlHelper.ExecuteNonQuery(sql);
        }
    }
}
