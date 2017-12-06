using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.WelfareManage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.WelfareManage
{
    public class WelfareManageController : BaseController
    {
        IPersonalWelfareManageMonthInfoService _personalWelfareManageMonthInfoService = new PersonalWelfareManageMonthInfoService();
        IPersonalWelfareManageYearInfoService _personalWelfareManageYearInfoService = new PersonalWelfareManageYearInfoService();
        // GET: WelfareManage
        public ActionResult MonthInfo()
        {
            return View();
        }
        public ActionResult MonthEdit(Guid? MID, string type)
        {
            ViewData["MID"] = MID;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult YearInfo()
        {
            return View();
        }
        public ActionResult YearEdit(Guid? YID, string type)
        {
            ViewData["YID"] = YID;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult PersonalWelfareInfo()
        {
            return View();
        }
        #region 月福利
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, PersonalWelfareManageMonthInfoQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _personalWelfareManageMonthInfoService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).Select(u =>new
                {
                    u.MID,
                    u.Employee,
                    u.EmployeeName,
                    u.CYearAndMonth,
                    u.WelfarePackage,
                    u.WelfareMonthTotalAmount,
                    u.WelfareMonthAmountUsed,
                    WelfareMonthRemain= u.WelfareMonthTotalAmount - u.WelfareMonthAmountUsed
                }).ToList();
            return Json(new { items = modelList, count = count });

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _personalWelfareManageMonthInfoService.List().Where(u => Ids.Contains(u.MID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _personalWelfareManageMonthInfoService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }
        public ActionResult MonthInfoGetByID(Guid MID)
        {
            var monthInfo = _personalWelfareManageMonthInfoService.FindById(MID);

            return Json(new
            {
                monthInfo.MID,
                monthInfo.Employee,
                monthInfo.EmployeeName,
                CYearAndMonth=!String.IsNullOrEmpty(monthInfo.CYearAndMonth) ? Convert.ToDateTime(monthInfo.CYearAndMonth).ToString("yyyy-MM"):"",
                monthInfo.WelfarePackage,
                monthInfo.WelfareMonthTotalAmount,
                monthInfo.WelfareMonthAmountUsed

            });
        }

        public ActionResult Save_monthInfo(PersonalWelfareManageMonthInfo monthInfo)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            if (monthInfo.MID == null || monthInfo.MID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
            monthInfo.MID = Guid.NewGuid();
            result.IsSuccess = _personalWelfareManageMonthInfoService.Insert(monthInfo);
            }
            else
            {//编辑
                result.IsSuccess = _personalWelfareManageMonthInfoService.Update(monthInfo);
            }
            result.Message = tip;
            return Json(result);
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<PersonalWelfareManageMonthInfoQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _personalWelfareManageMonthInfoService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<PersonalWelfareManageMonthInfo>;
                var list = modelList.Select(u => new
                {
                    u.MID,
                    u.Employee,
                    u.EmployeeName,
                    u.CYearAndMonth,
                    u.WelfarePackage,
                    u.WelfareMonthTotalAmount,
                    u.WelfareMonthAmountUsed,
                    WelfareMonthRemain = u.WelfareMonthTotalAmount - u.WelfareMonthAmountUsed

                }).ToList<object>();

                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.MonthInfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.MonthInfo);
                //设置集合变量
                designer.SetDataSource(ImportFileType.MonthInfo, list);
                //根据数据源处理生成报表内容
                designer.Process();
                //designer.Save(path, FileFormatType.Excel2003);
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
        #endregion

        #region 年福利
        public ActionResult GetYearListData(int pageIndex = 1, int pageSize = 5, PersonalWelfareManageYearInfoQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _personalWelfareManageYearInfoService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).Select(u => new
            {               
                u.YID,
                u.Employee,
                u.EmployeeName,
                u.CYear,
                u.CreateTime,
                u.WelfareYearTotalAmount
            }).ToList();
            return Json(new { items = modelList, count = count });

        }
        public ActionResult DeleteYear(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _personalWelfareManageYearInfoService.List().Where(u => Ids.Contains(u.YID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _personalWelfareManageYearInfoService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }
        public ActionResult YearInfoGetByID(Guid YID)
        {
            var yearInfo = _personalWelfareManageYearInfoService.FindById(YID);

            return Json(new
            {
                yearInfo.YID,
                yearInfo.Employee,
                yearInfo.EmployeeName,
                yearInfo.CYear,
                yearInfo.CreateTime,
                yearInfo.WelfareYearTotalAmount

            });
        }

        public ActionResult Save_yearInfo(PersonalWelfareManageYearInfo yearInfo)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (yearInfo.YID == null || yearInfo.YID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                yearInfo.EditTime = DateTime.Now;
                yearInfo.CreateTime = DateTime.Now;
                yearInfo.YID = Guid.NewGuid();
                result.IsSuccess = _personalWelfareManageYearInfoService.Insert(yearInfo);
            }
            else
            {//编辑
                yearInfo.EditTime = DateTime.Now;
                result.IsSuccess = _personalWelfareManageYearInfoService.Update(yearInfo);
            }
            return Json(result);
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult DownloadYearInfo(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<PersonalWelfareManageYearInfoQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _personalWelfareManageYearInfoService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<PersonalWelfareManageYearInfo>;
                var list = modelList.Select(u => new
                {
                    u.YID,
                    u.Employee,
                    u.EmployeeName,
                    u.CYear,
                    u.CreateTime,
                    u.WelfareYearTotalAmount

                }).ToList<object>();

                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.YearInfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.YearInfo);
                //设置集合变量
                designer.SetDataSource(ImportFileType.YearInfo, list);
                //根据数据源处理生成报表内容
                designer.Process();
                //designer.Save(path, FileFormatType.Excel2003);
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

        #endregion

        public ActionResult GetPersonalWelfareInfoListData(string Year = null)
        {         
            var monthlist = _personalWelfareManageMonthInfoService.List().Where(s => s.Employee == this.WorkContext.CurrentUser.EmployeeId).Select(u => new
            {
                u.MID,
                u.Employee,
                u.EmployeeName,
                u.CYearAndMonth,
                u.WelfarePackage,
                u.WelfareMonthTotalAmount,
                u.WelfareMonthAmountUsed,
                WelfareMonthRemain = u.WelfareMonthTotalAmount - u.WelfareMonthAmountUsed
            }).ToList();
            var yearlist = _personalWelfareManageYearInfoService.List().Where(s => s.Employee == this.WorkContext.CurrentUser.EmployeeId).ToList();
            if (!String.IsNullOrEmpty(Year))
            {
                yearlist = yearlist.Where(s => s.CYear == Year).ToList();
                monthlist = monthlist.Where(m => m.CYearAndMonth.Contains(Year)).ToList();
            }
            var modelList = new {
                DpFullName=this.WorkContext.CurrentUser.Dept.DpFullName,
                monthlist,
                yearlist
            };
            return Json(new { items = modelList});

        }
    }
}