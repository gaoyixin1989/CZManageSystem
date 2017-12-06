using CZManageSystem.Core;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.OverTime;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.HumanResources.OverTime
{
    public class OverTimeStaticService : BaseService<OverTimeStatic>, IOverTimeStaticService
    {
        static Users _user;
        ISysUserService _userService = new SysUserService();
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        public IList<OverTimeStatic> GetForPagingByCondition(out int count, int pageIndex, int pageSize, Users user, OverTimeStaticQueryBuilder objs = null)
        {
            _user = user;
            string sdate = Convert.ToDateTime(objs.Year).AddDays(1 - Convert.ToDateTime(objs.Year).Day).ToString("yyyy-MM-dd");
            string edate = Convert.ToDateTime(objs.Year).AddDays(1 - Convert.ToDateTime(objs.Year).Day).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");


            string tempsql = String.Format(" select * from HRHolidays where holidayclass='法定假日' and StartTime >='{0}' and EndTime <= '{1}'", sdate, edate);
            DataTable dtholiday = SqlHelper.ExecuteDataTable(tempsql);

            tempsql = String.Format(@"select A.* FROM HROverTimeApply A LEFT JOIN bwwf_Tracking_Workflows  B ON A.WorkflowInstanceId=B.WorkflowInstanceId 
WHERE B.State = 2 AND  a.StartTime>='{0}' and a.StartTime<='{1}'", sdate+ " 00:00:00", edate+ " 23:59:59");
            DataTable dtjiaban = SqlHelper.ExecuteDataTable(tempsql);

            DataTable dtcheck ;

            if (DateTime.Now.ToString("yyyy-MM-01").CompareTo(sdate) <= 0)
            {
                string wensql = "select * from HRCheckAttendance where DoReallyTime is not null and AtDate in(" + GetPeriodTimeHoliday(sdate, edate) + ")";
                dtcheck= SqlHelper.ExecuteDataTable(wensql);
            }
            else
            {
               string  wensql = "SELECT *  FROM (SELECT AttendanceId, UserId, AtDate, DoTime, OffTime, DoReallyTime, OffReallyTime FROM HRCheckAttendance union all SELECT HistoryId  As AttendanceId, UserId, AtDate, DoTime, OffTime, DoReallyTime, OffReallyTime FROM HRCheckAttendanceHistoryNo1 ) R where DoReallyTime is not null and AtDate in(" + GetPeriodTimeHoliday(sdate, edate) + ")";
                dtcheck = SqlHelper.ExecuteDataTable(wensql);
            }

            var mm3 = new EfRepository<string>().ExecuteResT<string>(string.Format("exec HROverTime_GetUser '{0}','{1}','{2}','{3}','{4}','{5}'", _user.UserName, objs.Year, objs.EmployeeId, objs.RealName, objs.DpId,objs.UserType));
            List<OverTimeStatic> resultList = new List<OverTimeStatic>();
            foreach (var x in mm3)
            {
                decimal jbtime = 0;
                decimal fdjbtime = 0;
                decimal gxjbtime = 0;
                string fdstr = "";
                string gxstr = "";
                string tempdetaildate = "";
                Guid _tempuserid = new Guid(x);
                GetHolday(_tempuserid, dtholiday, dtjiaban, out jbtime, out fdjbtime, out gxjbtime, out fdstr, out gxstr, dtcheck);
                var _userobj = _userService.FindByFeldName(u => u.UserId == _tempuserid);
                List<string> _tmpdplist = GetDepartMent(_userobj.DpId);
                if (fdstr!="")
                {
                    tempdetaildate = fdstr + "法定节假日加班。";
                }
                if (gxstr != "")
                {
                    tempdetaildate += gxstr + "公假日加班。";
                }
                resultList.Add(new OverTimeStatic {
                    UserId = _tempuserid,
                    RealName = _userobj.RealName,
                    DpName = _tmpdplist[0],
                    //DpName = _sysDeptmentService.FindById(_userobj.DpId).DpName,
                    GJOTTime = gxjbtime,
                    FJOTTime = fdjbtime,
                    AllOTTime = jbtime,
                    DetailDate = tempdetaildate,
                    UserType =_userobj.UserType == 1? "合同制员工" : "社会化员工",
                    EmployeeId=_userobj.EmployeeId
                });
            }

            PagedList<OverTimeStatic> pageList = new PagedList<OverTimeStatic>(resultList, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            count = resultList.Count();
            return pageList;
        }

        public  List<string> GetDepartMent(string dpid)
        {
            List<string> listResult = new List<string>();
            if (!string.IsNullOrEmpty(dpid))
            {
                IUum_OrganizationinfoService _sysDeptmentService = new Uum_OrganizationinfoService();
                var _obj = _sysDeptmentService.FindById(dpid);
                if (_obj.department.Contains("营销中心") || _obj.department.Contains("服营厅"))
                {
                    listResult.Add(_obj.branch);
                    listResult.Add("");
                }
                else if (string.IsNullOrEmpty(_obj.department) || _obj.department == "")
                {
                    listResult.Add(_obj.branch);
                    listResult.Add("");
                }
                else
                {
                    listResult.Add(_obj.department);
                    listResult.Add(_obj.userGroup);
                }
            }
            return listResult;
        }
        public void GetHolday(Guid userid, DataTable tbholday, DataTable tbjiaban, out decimal jbtime, out decimal fdjbtime, out decimal gxjbtime, out string fdstr, out string gxstr, DataTable dtcheck)
        {
            jbtime = 0;
            fdjbtime = 0;
            gxjbtime = 0;
            fdstr = "";
            gxstr = "";
            //本月存在法定节假日
            if (tbholday != null)
            {
                foreach (DataRowView drv in tbholday.DefaultView)
                {
                    DataRow[] lsdr = tbjiaban.Select("Editor='" + userid + "' and StartTime>= '" + Convert.ToDateTime(drv["StartTime"]).ToString("yyyy-MM-dd") + " 00:00:00' and StartTime<= '" + Convert.ToDateTime(drv["EndTime"]).ToString("yyyy-MM-dd") + " 23:59:59'");
                    if (lsdr.Length > 0)
                    {
                        for (int i = 0; i < lsdr.Length; i++)
                        {
                            jbtime += Convert.ToDecimal(lsdr[i]["PeriodTime"].ToString());
                            fdjbtime += Convert.ToDecimal(lsdr[i]["PeriodTime"].ToString());
                            fdstr += " " + Convert.ToDateTime(lsdr[i]["StartTime"].ToString()).ToString("yyyy-MM-dd");
                            tbjiaban.Rows.Remove(lsdr[i]);
                        }
                    }
                    else
                    {
                        string nowdate = "";
                        lsdr = dtcheck.Select("UserId='" + userid + "' and AtDate>= '" + Convert.ToDateTime(drv["StartTime"]).ToString("yyyy-MM-dd") + "' and AtDate<= '" + Convert.ToDateTime(drv["EndTime"]).ToString("yyyy-MM-dd") + "'");
                        if (lsdr.Length > 0)
                        {
                            for (int i = 0; i < lsdr.Length; i++)
                            {
                                if (nowdate != lsdr[i]["AtDate"].ToString())
                                {
                                    jbtime += 8;
                                    fdjbtime += 8;
                                    fdstr += " " + Convert.ToDateTime(lsdr[i]["AtDate"].ToString()).ToString("yyyy-MM-dd");
                                    //tbjiaban.Rows.Remove(lsdr[i]);
                                    nowdate = lsdr[i]["AtDate"].ToString();
                                }
                            }
                        }
                    }
                }
                DataRow[] lsdrt = tbjiaban.Select("Editor='" + userid + "'");
                if (lsdrt.Length > 0)
                {
                    for (int i = 0; i < lsdrt.Length; i++)
                    {
                        jbtime += Convert.ToDecimal(lsdrt[i]["PeriodTime"].ToString());
                        gxjbtime += Convert.ToDecimal(lsdrt[i]["PeriodTime"].ToString());
                        gxstr += " " + Convert.ToDateTime(lsdrt[i]["StartTime"].ToString()).ToString("yyyy-MM-dd");
                    }
                }
            }
            //本月不存在法定节假日
            else
            {
                DataRow[] tempdr = tbjiaban.Select("Editor='" + userid + "'");
                if (tempdr.Length > 0)
                {
                    for (int i = 0; i < tempdr.Length; i++)
                    {
                        jbtime += Convert.ToDecimal(tempdr[i]["PeriodTime"].ToString());
                        gxjbtime += Convert.ToDecimal(tempdr[i]["PeriodTime"].ToString());
                        gxstr += " " + Convert.ToDateTime(tempdr[i]["DtartTime"].ToString()).ToString("yyyy-MM-dd");
                    }
                }
            }
        }


        public  string GetPeriodTimeHoliday(string sdate, string edate)
        {
            string vadate = "";
            string sql = "";
            string rdate = "";
            //查询日期不超过当前日期
            DateTime nowdate = DateTime.Now;
            for (int i = 0; Convert.ToDateTime(sdate).AddDays(i) <= Convert.ToDateTime(edate) && Convert.ToDateTime(sdate).AddDays(i) <= nowdate; i++)
            {
                rdate = Convert.ToDateTime(sdate).AddDays(i).ToString("yyyy-MM-dd");

                //是否节假日
                sql = " select Top 1 * from HRHolidays where StartTime <= '" + rdate + "' and EndTime >= '" + rdate + "'and holidayclass='法定假日'";
                DataTable tb = SqlHelper.ExecuteDataTable(sql);
                if (tb.Rows.Count > 0)
                {
                    vadate += "'" + rdate + "',";
                }
            }
            if (vadate == "")
                vadate += "''";
            else
                vadate = vadate.Substring(0, vadate.Length - 1);
            return vadate;
        }
    }
}
