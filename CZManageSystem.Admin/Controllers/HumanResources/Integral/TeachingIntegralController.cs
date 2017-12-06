using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.Integral;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.TeachingIntegral
{
    public class TeachingIntegralController : BaseController
    {
        IHRVacationTeachingService _teachingintegralservice = new HRVacationTeachingService();
        ISysUserService _userService = new SysUserService();
        ITIntegralConfigService _hrcintegralconfigservice = new TIntegralConfigService();
        IHRIntegralService _hrintegralservice = new HRIntegralService();
        // GET: TeachingIntegral
        [SysOperation(OperationType.Browse, "访问授课积分管理页面")]
        public ActionResult Index()
        {
            return View();
        }
        [SysOperation(OperationType.Browse, "访问授课积分明细页面")]
        public ActionResult Detail()
        {
            return View();
        }
        [SysOperation(OperationType.Browse, "访问个人授课积分明细页面")]
        public ActionResult PersonDetail(string RealName,string Year)
        {
            ViewData["RealName"] = RealName;
            ViewData["Year"] = Year;
            return View();
        }

        public ActionResult Edit(Guid? id)
        {
            ViewData["id"] = id;
            return View();
        }
        public ActionResult GetDataByID(Guid id)
        {
            var item = _teachingintegralservice.FindById(id);

            return Json(new
            {
                item.ID,
                item.AgreeFlag,
                item.ReVacationID,
                item.UserName,
                item.UserId,
                item.TeachingPlan,
                item.TeacherType,
                //DeptName_text = CommonFunction.getDeptNamesByIDs(item.DeptName),
                item.PeriodTime,
                //Name_text = CommonFunction.getUserRealNamesByIDs(item.Name),
                StartTime = item.StartTime.HasValue ? item.StartTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                EndTime = item.EndTime.HasValue ? item.EndTime.Value.ToString("yyyy-MM-dd HH:mm") : ""
            });
        }
        [SysOperation(OperationType.Save, "保存授课积分数据")]
        public ActionResult Save(HRVacationTeaching dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            HRIntegral objhrintegral = new HRIntegral();
            DateTime OldST, OldET;
            string buseformula = "";
            double integral = 0.00, times = 0.00, c_integral = 0.00;
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过    
            var configlist = _hrcintegralconfigservice.JudgeConfig(Convert.ToInt16(dataObj.PeriodTime.Value));
            if (configlist.Count == 0)
                tip = "没有该天数的基础配置！";
            else
            {
                buseformula = configlist[0].BuseFormula;
                times = configlist[0].Times.HasValue ? configlist[0].Times.Value : 0;
                integral = configlist[0].Integral.HasValue ? configlist[0].Integral.Value : 0;
            }
            if (buseformula == "公式")
                c_integral = Convert.ToDouble(dataObj.PeriodTime) * times;
            else if (buseformula == "常量")
                c_integral = integral;

            var temp = _hrintegralservice.FindByFeldName(u => u.Daoid == dataObj.ID);
            if (temp != null)
                objhrintegral = temp;
            objhrintegral.UserId = dataObj.UserId;
            objhrintegral.CIntegral = Convert.ToDecimal(c_integral);
            objhrintegral.TPeriodTime = dataObj.PeriodTime;
            objhrintegral.FinishTime = dataObj.EndTime.Value.ToString("yyyy-MM-dd HH:mm");
            objhrintegral.Daoid = dataObj.ID;
            objhrintegral.YearDate = dataObj.StartTime.Value.Year;

            dataObj.AgreeFlag = 2;
            dataObj.Integral = Convert.ToDecimal(c_integral);

            if (tip == "")
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
            if (dataObj.ID == Guid.Empty)//新增
            {
                dataObj.ID = Guid.NewGuid();
                objhrintegral.Daoid = dataObj.ID;
                //插入记录到明细表中
                result.IsSuccess = _teachingintegralservice.Insert(dataObj);
                //插入到积分明细表中
                objhrintegral.IntegralId = Guid.NewGuid();
                _hrintegralservice.Insert(objhrintegral);
                //判断得到班次，把休假状态更新到相应的班次上   
                UpdateHRCheckAttendance(dataObj.StartTime.Value, dataObj.EndTime.Value, "2", dataObj.UserId.Value);
            }
            else
            {//编辑                
                //判断去除原来的班次的休假状态
                OldST = Convert.ToDateTime(new EfRepository<string>().Execute<DateTime>(string.Format("select StartTime from HRVacationTeaching where   Id='{0}'", dataObj.ID)).ToList()[0]);
                OldET = Convert.ToDateTime(new EfRepository<string>().Execute<DateTime>(string.Format("select EndTime from HRVacationTeaching where   Id='{0}'", dataObj.ID)).ToList()[0]);
                UpdateHRCheckAttendance(OldST, OldET, "null", dataObj.UserId.Value);
                _hrintegralservice.Update(objhrintegral);
                result.IsSuccess = _teachingintegralservice.Update(dataObj);
                UpdateHRCheckAttendance(dataObj.StartTime.Value, dataObj.EndTime.Value, "2", dataObj.UserId.Value);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
        [SysOperation(OperationType.Delete, "删除授课积分数据")]
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            DateTime OldST, OldET;
            Guid UserId;
            var objs = _teachingintegralservice.List().Where(u => Ids.Contains(u.ID)).ToList();
            var hrintegralobjs = _hrintegralservice.List().Where(u => Ids.Contains(u.Daoid.Value)).ToList();
            if (objs.Count > 0)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    OldST = objs[i].StartTime.Value;
                    OldET = objs[i].EndTime.Value;
                    UserId = objs[i].UserId.Value;
                    UpdateHRCheckAttendance(OldST, OldET, "null", UserId);
                }
                isSuccess = _teachingintegralservice.DeleteByList(objs);
                _hrintegralservice.DeleteByList(hrintegralobjs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                SuccessCount = successCount
            });
        }
        public ActionResult GetListPersonalData(int pageIndex = 1, int pageSize = 5, HRVacationTeachingQueryBuilder queryBuilder = null)
        {
            int count = 0;

            //当有部门的条件时，应该先查询出该条件下所包含的所有部门(自身和子级部门)
            queryBuilder.TeacherType = null;
            queryBuilder.EmployeeID = null;
            queryBuilder.DpId = null;
            var modelList = _teachingintegralservice.GetForPersonalPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _userobj = _userService.FindByFeldName(u => u.UserId == item.UserId);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                listResult.Add(new
                {
                    Id = item.ID,
                    item.TeacherType,
                    item.TeachingPlan,
                    item.StartTime,
                    item.EndTime,
                    EmployeeId = _userobj.EmployeeId,
                    DpName = _tmpdplist[0],
                    DpmName = _tmpdplist[1],
                    //DpName = CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                    RealName = _userobj.RealName,
                    item.PeriodTime,
                    item.Integral
                });
            }
            return Json(new { items = listResult, count = count });
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HRVacationTeachingQueryBuilder queryBuilder = null)
        {
            int count = 0;

            //当有部门的条件时，应该先查询出该条件下所包含的所有部门(自身和子级部门)
            //if (DpId != null && DpId != "")
            //    queryBuilder.DpId = Get_Subdept_ByDept(DpId);
            //else
            //    queryBuilder.DpId = null;
            if (queryBuilder.DpId != null)
                queryBuilder.DpId = Get_Subdept_ByDept(queryBuilder.DpId);

            var modelList = _teachingintegralservice.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _userobj = _userService.FindByFeldName(u => u.UserId == item.UserId);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                listResult.Add(new
                {
                    Id = item.ID,
                    item.TeacherType,
                    item.TeachingPlan,
                    item.StartTime,
                    item.EndTime,
                    EmployeeId = _userobj.EmployeeId,
                    DpName = _tmpdplist[0],
                    DpmName = _tmpdplist[1],
                    //DpName = CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                    RealName = _userobj.RealName,
                    item.PeriodTime,
                    item.Integral
                });
            }
            return Json(new { items = listResult, count = count });
        }

        [SysOperation(OperationType.Export, "导出授课积分数据")]
        public ActionResult Export(string queryBuilder = null, string DpId = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<HRVacationTeachingQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                //if (DpId != null && DpId != "")
                //    QueryBuilder.DpId = Get_Subdept_ByDept(DpId);
                //else
                //    QueryBuilder.DpId = null;
                if (QueryBuilder.DpId != null)
                    QueryBuilder.DpId = Get_Subdept_ByDept(QueryBuilder.DpId);

                var modelList = _teachingintegralservice.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<HRVacationTeaching>;

                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    var _userobj = _userService.FindByFeldName(u => u.UserId == item.UserId);
                    List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                    listResult.Add(new
                    {
                        item.TeachingPlan,
                        item.TeacherType,
                        item.StartTime,
                        item.EndTime,
                        EmployeeId = _userobj.EmployeeId,
                        DpName = _tmpdplist[0],
                        DpmName = _tmpdplist[1],
                        //DpName = CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                        RealName = _userobj.RealName,
                        item.PeriodTime,
                        item.Integral
                    });
                }
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.TeachingIntegral + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.TeachingIntegral);
                //设置集合变量
                designer.SetDataSource(ImportFileType.TeachingIntegral, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();
                #endregion
            }
            catch (Exception ex)
            {

                throw;
            }
            return null;
        }


        #region 其他的方法

        private void UpdateHRCheckAttendance(DateTime StartTime, DateTime EndTime, string Flag, Guid UserId)
        {
            string sql = "";
            sql = string.Format(@"update HRCheckAttendance set doflag={0} where AttendanceId in  
                                ( 
                                    select AttendanceId from HRCheckAttendance where userid='{1}'
                                    and(('{2}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime))
                                    or ('{3}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime))
                                    or ('{2}'<=convert(varchar,atdate)+' '+convert(varchar,dotime) and '{3}'>=convert(varchar,atdate)+' '+convert(varchar,offtime)) and dotime is not null) 
                                ) ", Flag, UserId, CommonConvert.DateTimeToString(StartTime, "yyyy-MM-dd HH:mm:ss"), CommonConvert.DateTimeToString(EndTime, "yyyy-MM-dd HH:mm:ss"));
            SqlHelper.ExecuteNonQuery(sql);
            sql = string.Format(@"update HRCheckAttendanceHistoryNo1 set doflag={0} where HistoryId in  
                                ( 
                                    select HistoryId from HRCheckAttendanceHistoryNo1 where userid='{1}'
                                    and(('{2}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime))
                                    or ('{3}' between convert(varchar,atdate)+' '+convert(varchar,dotime) and convert(varchar,atdate)+' '+convert(varchar,offtime)) 
                                    or ('{2}'<=convert(varchar,atdate)+' '+convert(varchar,dotime) and '{3}'>=convert(varchar,atdate)+' '+convert(varchar,offtime)) and dotime is not null) 
                                ) ", Flag, UserId, CommonConvert.DateTimeToString(StartTime, "yyyy-MM-dd HH:mm:ss"), CommonConvert.DateTimeToString(EndTime, "yyyy-MM-dd HH:mm:ss"));
            SqlHelper.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 根据部门组织Ids获取包括自身的所有子节点的id
        /// </summary>
        /// <param name="ids">组织Ids</param>
        /// <returns></returns>
        //private List<string> Get_Subdept_ByDept(string ids)
        //{
        //    List<string> listResult = new List<string>();
        //    if (ids == null)
        //        return listResult;
        //    string[] temp = ids.Split(',');
        //    for (int i = 0; i < temp.Length; i++)
        //    {
        //        var mm = new EfRepository<string>().Execute<string>(string.Format("select * from  dbo.Get_Subdept_ByDept ('{0}')", temp[i].ToString())).ToList();
        //        listResult.AddRange(mm);
        //    }
        //    return listResult;
        //}
        private string[] Get_Subdept_ByDept(string[] ids)
        {
            if (ids == null)
                return null;
            string tmp2 = "";
            for (int i = 0; i < ids.Length; i++)
            {
                var mm = new EfRepository<string>().Execute<string>(string.Format("select * from  dbo.Get_Subdept_ByDept ('{0}')", ids[i].ToString())).ToList();
                tmp2 = tmp2 + "," + string.Join(",", mm);
            }
            string[] tmp = tmp2.Remove(0, 1).Split(',');
            return tmp;
        }
        #endregion
    }
}