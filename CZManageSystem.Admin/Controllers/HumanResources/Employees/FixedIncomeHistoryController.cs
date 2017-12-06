using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using CZManageSystem.Service.HumanResources.Employees;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.Employees
{
    public class FixedIncomeHistoryController : BaseController
    {
        // GET: FixedIncomeHistory
        IPayVService payVService = new PayVService();
        IPayService payService = new PayService();
        ISysUserService sysUserService = new SysUserService();
        ISysDeptmentService sysDeptmentService = new SysDeptmentService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string keys = null)
        {
            ViewData["keys"] = keys;
            return View();
        }
        public ActionResult GetDataByID(string keys)
        {
            var model = payService.FindByFeldName(f => (f.employerid + f.billcyc) == keys);
            var user = sysUserService.FindByFeldName(f => f.EmployeeId == model.employerid.ToString());
            if (user == null)
            {
                SystemResult result = new SystemResult() { IsSuccess = false };
                result.Message = "没有查询到[员工编码]为[" + model.employerid + "]的用户。";
                return Json(result);
            }
            user.Dept = sysDeptmentService.FindById(user.DpId);
            DateTime dtBillcyc = DateTime.ParseExact(model.billcyc.ToString(), "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
            return Json(new
            {
                User = new
                {
                    UserName = user.RealName,
                    DeptName = user.Dept?.DpName
                },
                Model = new
                {
                    EmployerId = model.employerid,
                    Billcyc = dtBillcyc.ToString("yyyy年M月"),
                    UpdateTime = model.更新时间,
                    FixedIncome = model.固定收入,
                    SeniorityPay = model.工龄工资,
                    ReviewMonthlyAward = model.月度考核奖,
                    PhoneSubsidies = model.话费补助,
                    TravelAllowance = model.交通补贴,
                    NightShiftAllowance = model.值夜夜班津贴,
                    HolidayOvertimePay = model.节假日加班工资,
                    Other = model.其它,
                    MotorCombinedPrize = model.机动奖合计,
                    TotalIncome = model.总收入,
                    SocialSecurityDeductions = model.社保扣款,
                    MedicalInsuranceDeductions = model.医保扣款,
                    HousingFundDeductions = model.住房公积金,
                    DormitoryUtilitiesRent = model.宿舍房租及水电费,
                    OtherDeductions = model.其它扣款,
                    SocialSecurityEnterprise = model.社保企,
                    MedicalInsuranceEnterprise = model.医保企,
                    HousingFundEnterprise = model.住房公积金企,
                    TaxableIncome = model.应纳税所得额,
                    IncomeTax = model.个人所得税,
                    SalaryPaid = model.实发,
                    Remark = model.备注

                }
            });
        }
        /// <summary>
        /// 下载数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string  queryBuilder = null)
        {
            try
            {
                var QueryBuilder = JsonConvert.DeserializeObject<FixedIncomeHistoryQueryBuilder>(queryBuilder);
                var userList = sysUserService.List().Select(s => new { s.EmployeeId, s.RealName }).ToList();
                
                var source = payVService.List().OrderByDescending(c => c.billcyc).Where(w => 1 == 1);
                if (!string .IsNullOrEmpty(QueryBuilder?.EmployerName))
                {
                    source=source.Where(w=>w.姓名== QueryBuilder.EmployerName);
                }
                if (!string.IsNullOrEmpty(QueryBuilder?.EmployerId))
                {
                    source = source.Where(w => w.employerid  == QueryBuilder.EmployerId);
                } 
                var modelList = source.ToList().Select(s => new 
                {
                    EmployerId = s.employerid,
                    EmployerName = userList.Find(f => f.EmployeeId == s.employerid)?.RealName,
                    Billcyc = CommonConvert.DateConveryyyyMM(s.billcyc.ToString()),
                    UpdateTime = s.更新时间,
                    FixedIncome = s.固定收入,
                    SeniorityPay = s.工龄工资,
                    ReviewMonthlyAward = s.月度考核奖,
                    PhoneSubsidies = s.话费补助,
                    TravelAllowance = s.交通补贴,
                    NightShiftAllowance = s.值夜夜班津贴,
                    HolidayOvertimePay = s.节假日加班工资,
                    Other = s.其它,
                    MotorCombinedPrize = s.机动奖合计,
                    TotalIncome = s.总收入,
                    SocialSecurityDeductions = s.社保扣款,
                    MedicalInsuranceDeductions = s.医保扣款,
                    HousingFundDeductions = s.住房公积金,
                    DormitoryUtilitiesRent = s.宿舍房租及水电费,
                    OtherDeductions = s.其它扣款,
                    SocialSecurityEnterprise = s.社保企,
                    MedicalInsuranceEnterprise = s.医保企,
                    HousingFundEnterprise = s.住房公积金企,
                    TaxableIncome = s.应纳税所得额,
                    IncomeTax = s.个人所得税,
                    SalaryPaid = s.实发,
                    Remark = s.备注

                }).ToList<object>();

                if(modelList .Count<1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.FixedIncomeHistory + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();

                //打开模板
                designer.Open(ExportTempPath.FixedIncomeHistory);
                designer.SetDataSource(ImportFileType.FixedIncomeHistory, modelList);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion
                return null;
            }
            catch (Exception ex)
            {
                SystemResult result = new SystemResult() { IsSuccess = false ,Message = ex.Message + ";" + ex.InnerException?.Message}; 
                return Json(result);
            }
        }



        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, FixedIncomeHistoryQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = payVService.GetForPaging(out count, new { 姓名 = queryBuilder.EmployerName, employerid = queryBuilder.EmployerId }, pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }
        public ActionResult Delete(string[] ids)
        {
            var list = payService.List().Where(f => ids.Contains<string>(f.employerid + f.billcyc));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (payService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult Save(PayViewModel payViewModel)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                bool isInsert = false;
                if (payViewModel == null)
                {
                    result.Message = "保存对象对Null！";
                    return Json(result);
                }
                var user = sysUserService.FindByFeldName(f => f.EmployeeId == payViewModel.EmployerId);
                if (user == null)
                {
                    result.Message = "没有查询到[员工编码]为[" + payViewModel.EmployerId + "]的用户。";
                    return Json(result);
                }
                int intBillcyc = 200010;
                if (!int.TryParse(payViewModel.Billcyc.ToString("yyyyMM"), out intBillcyc))
                {
                    result.Message = "账务周期格式出错！";
                    return Json(result);
                }
                Pay model = payService.FindByFeldName(f => f.employerid == payViewModel.EmployerId && f.billcyc == intBillcyc);
                if (model == null)
                {
                    isInsert = true;
                    model = new Pay();
                    model.employerid = payViewModel.EmployerId;
                }
                model.billcyc = intBillcyc;
                model.更新时间 = DateTime.Now;
                model.固定收入 = payViewModel.FixedIncome;
                model.工龄工资 = payViewModel.SeniorityPay;
                model.月度考核奖 = payViewModel.ReviewMonthlyAward;
                model.话费补助 = payViewModel.PhoneSubsidies;
                model.交通补贴 = payViewModel.TravelAllowance;
                model.值夜夜班津贴 = payViewModel.NightShiftAllowance;
                model.节假日加班工资 = payViewModel.HolidayOvertimePay;
                model.其它 = payViewModel.Other;
                model.机动奖合计 = payViewModel.MotorCombinedPrize;
                model.总收入 = payViewModel.TotalIncome;
                model.社保扣款 = payViewModel.SocialSecurityDeductions;
                model.医保扣款 = payViewModel.MedicalInsuranceDeductions;
                model.住房公积金 = payViewModel.HousingFundDeductions;
                model.宿舍房租及水电费 = payViewModel.DormitoryUtilitiesRent;
                model.其它扣款 = payViewModel.OtherDeductions;
                model.社保企 = payViewModel.SocialSecurityEnterprise;
                model.医保企 = payViewModel.MedicalInsuranceEnterprise;
                model.住房公积金企 = payViewModel.HousingFundEnterprise;
                model.应纳税所得额 = payViewModel.TaxableIncome;
                model.个人所得税 = payViewModel.IncomeTax;
                model.实发 = payViewModel.SalaryPaid;
                model.备注 = payViewModel.Remark;
                if (isInsert)
                {
                    if (payService.Insert(model))
                        result.IsSuccess = true;
                }
                else if (payService.Update(model))
                    result.IsSuccess = true;

                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message + ";" + ex.InnerException?.Message;
                return Json(result);

            }
        }


        public ActionResult GetUserInfo(string employerid)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var user = sysUserService.FindByFeldName(f => f.EmployeeId == employerid);
                if (user == null)
                {
                    result.Message = "没有查询到[员工编码]为[" + employerid + "]的用户。";
                    return Json(result);
                }
                user.Dept = sysDeptmentService.FindById(user.DpId);
                result.IsSuccess = true;
                result.data = new
                {
                    UserName = user.RealName,
                    DeptName = user.Dept?.DpName
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message + ex.InnerException?.Message;
                return Json(result);
                throw;
            }
        }


    }
}