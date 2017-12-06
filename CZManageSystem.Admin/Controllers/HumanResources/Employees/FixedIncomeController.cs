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
    public class FixedIncomeController : BaseController
    {
        // GET: FixedIncome
        IUum_UserinfoService uum_UserinfoService = new Uum_UserinfoService();
        IGdPayVService gdPayVService = new GdPayVService();
        IGdPayIdService gdPayIdService = new GdPayIdService();
        IGdPayService gdPayService = new GdPayService();
        IHRLzUserInfoService hrLzUserInfoService = new HRLzUserInfoService();
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
            var model = gdPayService.FindByFeldName(f => (f.employerid + f.billcyc + f.payid) == keys);
            DateTime dtBillcyc = DateTime.ParseExact(model.billcyc.ToString(), "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);
            return Json(new
            {
                EmployerId = model.employerid,
                Billcyc = dtBillcyc.ToString("yyyy年M月"),
                PayId = model.payid,
                Value = model.je == null ? model.value_str : model.je
            });
        }
        /// <summary>
        /// 下载数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            try
            {
                var QueryBuilder = JsonConvert.DeserializeObject<FixedIncomeQueryBuilder>(queryBuilder);
                #region 
                var listAll = gdPayIdService.List().Where(w => w.pid > 0).ToList();

                var GdPayIdList = listAll.Select(
                    s => new
                    {
                        PayId = s.payid,
                        Pid = s.pid,
                        PayName = s.payname,
                        Title = listAll.Find(f => f.payid == s.pid)?.payname
                    }
                    ).ToList();



                IDictionary<string, dynamic> setDict = new Dictionary<string, dynamic>();
                var dictionary = new Dictionary<string, string>();
                var tbdictionary = new Dictionary<string, DataRow>();


                List<string> strT = new List<string>() { "员工编号", "姓名", "账务周期", "部门" };
                List<string> strD = new List<string>() { "&=" + ImportFileType.FixedIncome + ".员工编号", "&=" + ImportFileType.FixedIncome + ".姓名", "&=" + ImportFileType.FixedIncome + ".账务周期", "&=" + ImportFileType.FixedIncome + ".部门" };
                DataTable newDt = new DataTable();
                newDt.Columns.Add("员工编号");
                newDt.Columns.Add("姓名");
                newDt.Columns.Add("账务周期");
                newDt.Columns.Add("部门");

                //表头
                foreach (var model in GdPayIdList)
                {
                    if (!setDict.ContainsKey(model.PayName))
                    {
                        setDict.Add(model.PayName, model);
                        newDt.Columns.Add(model.PayName);
                        strT.Add(model.PayName);
                        strD.Add("&=" + ImportFileType.FixedIncome + "." + model.PayName);
                    }
                }
                #endregion



                var source = GetSource(QueryBuilder);


                #region Select
                var modelList = source.Select(s => new FixedIncomeToList
                {
                    EmployerId = s.员工编号,
                    AccountingCycle = s.账务周期,
                    EmployerName = s.姓名,
                    DeptName = s.部门,
                    FixedIncomeProject = s.固定收入项目,
                    Value = s.DataType == "num" ? s.收入 : s.value_str

                    #region MyRegion
                    //Billcyc = s.billcyc,
                    //DataType = s.DataType,
                    //DeptId = s.deptid,
                    //Pid = s.pid,
                    //ValueStr = s.value_str,
                    //TypeOf = s.所属类型,
                    //Income = s.收入,
                    //PayId = s.收入编号,
                    //UpdateTime = s.更新时间 
                    #endregion

                }).ToList();

                #endregion
                if (modelList.Count < 1)
                    return View("../Export/Message");
                var data = from x in modelList
                           group x by new { x.EmployerId, x.AccountingCycle, x.EmployerName } into g
                           select new { Key = g.Key.ToString(), Items = g };
                data.ToList().ForEach(x =>
               {
                   //这里用的是一个string 数组 也可以用DataRow根据个人需要用
                   string[] array = new string[newDt.Columns.Count];
                   var items = x.Items.ToList();

                   array[0] = items[0].EmployerId;
                   array[1] = items[0].EmployerName;
                   array[2] = items[0].AccountingCycle;
                   array[3] = items[0].DeptName;
                   //从第二列开始遍历
                   for (int i = 4; i < newDt.Columns.Count; i++)
                   {
                       // array[0]  就是 ID
                       if (array[0] == null || array[0] == "")
                           break;
                       //array[i]就是 各种列
                       array[i] = (from y in items
                                   where y.FixedIncomeProject == newDt.Columns[i].ToString()//   y[2]  名字等于table中列的名字
                                   select y.Value                            //   
                                  ).SingleOrDefault();
                   }
                   if (items[0].EmployerId != null || items[0].EmployerId != "")
                       newDt.Rows.Add(array);   //添加到table中
               });


                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.FixedIncome + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                designer.Open(ExportTempPath.FixedIncome);
                //设置集合变量 
                designer.SetDataSource("strT", strT.ToArray());
                designer.SetDataSource("strD", strD.ToArray());
                designer.Process();
                designer.Save(ExportTempPath.FixedIncome_, FileFormatType.Excel2003XML);//保存格式


                designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.FixedIncome_);
                newDt.TableName = ImportFileType.FixedIncome;
                designer.SetDataSource(newDt);
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
                throw;
            }
        }



        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, FixedIncomeQueryBuilder queryBuilder = null)
        {
            var source = GetSource(queryBuilder);
            int count = 0;
            count = source.Count();
            pageIndex = pageIndex <= 0 ? 0 : pageIndex - 1;
            var pageList = source.Skip(pageIndex * pageSize).Take(pageSize);

            #region Select
            var modelList = pageList.Select(s => new
            {
                EmployerId = s.员工编号,
                DeptName = s.部门,
                Billcyc = s.billcyc,
                DataType = s.DataType,
                DeptId = s.deptid,
                Pid = s.pid,
                ValueStr = s.value_str,
                FixedIncomeProject = s.固定收入项目,
                TypeOf = s.所属类型,
                Income = s.收入,
                PayId = s.收入编号,
                UpdateTime = s.更新时间,
                AccountingCycle = s.账务周期,
                EmployerName = s.姓名,

            }).ToList();
            #endregion
            return Json(new { items = modelList, count = count });
        }
        public ActionResult Delete(string[] ids)
        {
            var list = gdPayService.List().Where(f => ids.Contains<string>(f.employerid + f.billcyc + f.payid));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (gdPayService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult Save(GdPayViewModel gdPayViewModel)
        {
            try
            {
                SystemResult result = new SystemResult() { IsSuccess = false };
                if (gdPayViewModel == null)
                {
                    result.Message = "保存对象对Null！";
                    return Json(result);
                }
                GdPay gdPay = new GdPay();
                gdPay.billcyc = Convert.ToInt32(gdPayViewModel.Billcyc.ToString("yyyyMM"));
                gdPay.employerid = gdPayViewModel.EmployerId;
                gdPay.payid = gdPayViewModel.PayId;

                var user = uum_UserinfoService.FindByFeldName(f => f.employee == gdPay.employerid.ToString());
                if (user == null)
                {
                    result.Message = "没有查询到[员工编码]为[" + gdPay.employerid + "]的用户。";
                    return Json(result);
                }
                var gdPayId = gdPayIdService.FindByFeldName(f => f.payid == gdPay.payid);
                if (gdPayId == null)
                {
                    result.Message = "没有查询到[收入类型]为[" + gdPay.payid + "]的类型。";
                    return Json(result);
                }
                if (gdPayId.DataType == "num")
                    gdPay.je = gdPayViewModel.Value;
                else
                    gdPay.value_str = gdPayViewModel.Value;
                var model = gdPayService.FindByFeldName(f => f.payid == gdPay.payid && f.billcyc == gdPay.billcyc && f.employerid == gdPay.employerid);
                gdPay.updatetime = DateTime.Now;
                if (model == null)
                    if (gdPayService.Insert(gdPay))
                    {
                        result.IsSuccess = true;
                        return Json(result);
                    }
                model.billcyc = gdPay.billcyc;
                model.employerid = gdPay.employerid;
                model.je = gdPay.je;
                model.payid = gdPay.payid;
                model.updatetime = gdPay.updatetime;
                model.value_str = gdPay.value_str;
                if (gdPayService.Update(model))
                    result.IsSuccess = true;

                return Json(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public ActionResult GetIncomeType()
        {

            var modelList = gdPayIdService.List().Select(
                s => new
                {
                    PayId = s.payid,
                    Pid = s.pid,
                    PayName = s.payname
                }
                ).ToList();
            var payNames = modelList.FindAll(f => f.Pid > 0);
            var parents = modelList.FindAll(f => f.Pid == 0);
            return Json(new { Parents = parents, PayNames = payNames });
        }
        public ActionResult GetIncomeTypeS()
        {

            var modelList = gdPayIdService.List().Where(w => w.pid > 0).Select(
                s => new
                {
                    PayId = s.payid,
                    Pid = s.pid,
                    PayName = s.payname
                }
                ).ToList();

            return Json(modelList);
        }

        public ActionResult GetUserInfo(string id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                var model = gdPayVService.FindByFeldName(f => f.员工编号 == id);
                if (model == null)
                {
                    result.Message = "不存在该员工编号！";
                    return Json(result);
                }
                if (string.IsNullOrEmpty(model.姓名))
                {
                    result.Message = "不存在该员工信息！";
                    return Json(result);
                }
                result.IsSuccess = true;
                result.data = new
                {
                    UserName = model.姓名,
                    DeptName = model.部门,
                    Billcyc = model.账务周期,
                    EmployeeId = model.员工编号,
                    PayId = model.收入编号,
                    Value = model.DataType == "num" ? model.收入 : model.value_str
                };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(result);
                throw;
            }
        }
        public ActionResult GetEmployeeIds()
        {
            var modelList = uum_UserinfoService.List().Select(
                s => s.employee
                ).ToList();
            return Json(modelList);
        }

        IQueryable<GdPayV> GetSource(FixedIncomeQueryBuilder queryBuilder = null)
        {
            var source = gdPayVService.List().OrderByDescending(c => c.billcyc).Where(w => 1 == 1);

            #region EmployeeList
            int count = 0;
            var hrLzUserInfoList = hrLzUserInfoService.List().OrderByDescending(c => c.EmployeeId).Where(w => 1 == 1);

            if (queryBuilder?.Tantile1 >= 0)
            {
                count++;
                hrLzUserInfoList = hrLzUserInfoList.Where(w => w.Tantile >= queryBuilder.Tantile1);
            }
            if (queryBuilder?.Tantile2 >= 0)
            {
                count++;
                hrLzUserInfoList = hrLzUserInfoList.Where(w => w.Tantile <= queryBuilder.Tantile2);
            }
            if (queryBuilder?.Gears != null && queryBuilder?.Gears.Count > 0)
            {
                count++;
                hrLzUserInfoList = hrLzUserInfoList.Where(w => queryBuilder.Gears.Contains(w.Gears));
            }
            if (queryBuilder?.PositionRank != null && queryBuilder?.PositionRank.Count > 0)
            {
                count++;
                hrLzUserInfoList = hrLzUserInfoList.Where(w => queryBuilder.PositionRank.Contains(w.PositionRank));
            }
            if (count > 0)
            {
                var EmployeeList = hrLzUserInfoList.Where(w => w.Users != null).Select(s => s.EmployeeId).ToList();


                //if (EmployeeList.Count > 0)
                //{
                source = source.Where(w => EmployeeList.Contains(w.员工编号));
                //}
            }
            #endregion
            #region 条件
            if (!string.IsNullOrEmpty(queryBuilder?.TypeOf))
                source = source.Where(w => w.所属类型 == queryBuilder.TypeOf);
            if (!string.IsNullOrEmpty(queryBuilder?.DeptName))
                source = source.Where(w => queryBuilder.DeptName.Contains(w.部门));
            if (queryBuilder?.AppTime_start != null)
            {
                var intValue = Convert.ToInt32(Convert.ToDateTime(queryBuilder.AppTime_start).ToString("yyyyMM"));
                source = source.Where(w => w.billcyc >= intValue);
            }
            if (queryBuilder?.AppTime_end != null)
            {
                var intValue = Convert.ToInt32(Convert.ToDateTime(queryBuilder.AppTime_end).ToString("yyyyMM"));
                source = source.Where(w => w.billcyc <= intValue);
            }
            if (!string.IsNullOrEmpty(queryBuilder?.FixedIncomeProject))
                source = source.Where(w => w.固定收入项目 == queryBuilder.FixedIncomeProject);
            if (!string.IsNullOrEmpty(queryBuilder?.CodeOrName))
                source = source.Where(w => w.员工编号.Contains(queryBuilder.CodeOrName) || w.姓名.Contains(queryBuilder.CodeOrName));
            #endregion
            return source;
        }



    }
}