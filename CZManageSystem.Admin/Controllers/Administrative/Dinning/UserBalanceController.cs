using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.Administrative.Dinning;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative.Dinning;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative.Dinning
{
    public class UserBalanceController : BaseController
    {
        // GET: UserBalance
        IView_EXT_XF_AccountService _balanceservice = new View_EXT_XF_AccountService();
        IView_XF_AmountLogService _balancedetailservice = new View_XF_AmountLogService();
        [SysOperation(OperationType.Browse, "访问食堂用户余额信息页面")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, view_EXT_XF_AccountQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _balanceservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var department = "";
                var _userobj = _sysUserService.FindByFeldName(u => u.EmployeeId == item.JobNumber);
                if(_userobj!=null)
                {
                    List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                    department = _tmpdplist[0];
                }
                else
                    department = item.DepartmentName;

                              
                listResult.Add(new
                {
                    item.JobNumber,
                    item.Name,
                    DepartmentName = department,//item.DepartmentName,
                    item.BelAmount
                });
            }
            return Json(new { items = listResult, count = count });
        }
        [SysOperation(OperationType.Browse, "访问食堂用户余额明细信息页面")]
        public ActionResult Detail(string JobNumber="")
        {
            ViewData["JobNumber"] = JobNumber;
            var _tmp = _balanceservice.FindByFeldName(u => u.JobNumber == JobNumber);
            ViewData["Name"] = _tmp.Name;
            return View();
        }


        public ActionResult GetDetailListData(int pageIndex = 1, int pageSize = 5, view_XF_AmountLogQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _balancedetailservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.JobNumber,
                    item.Name,
                    item.AddAmount,
                    item.TypeContent,
                    CreateTime = item.CreateTime.HasValue ? Convert.ToDateTime(item.CreateTime).ToString("yyyy-MM-dd HH:mm:ss") : "",
                    //item.CreateTime,
                    item.DepartmentName,
                    item.BelAmount
                });
            }
            var query = _balancedetailservice.GetForPaging(out count, queryBuilder);
            var total = new
            {
                InTotal = query.Where(u=>u.TypeContent== "充值" || u.TypeContent== "退款").Sum(u => (decimal)(u.AddAmount ?? 0)),
                OutTotal = query.Where(u => u.TypeContent == "补扣款" || u.TypeContent == "订餐扣款" || u.TypeContent == "消费").Sum(u => (decimal)(u.AddAmount ?? 0))
            };
            return Json(new { items = listResult, total= total,count = count });
        }
        [SysOperation(OperationType.Export, "导出食堂用户余额明细")]

        public ActionResult Export(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<view_XF_AmountLogQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _balancedetailservice.GetForPagingByCondition(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();

                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    listResult.Add(new
                    {
                        item.JobNumber,
                        item.Name,
                        item.AddAmount,
                        item.TypeContent,
                        CreateTime = item.CreateTime.HasValue ? Convert.ToDateTime(item.CreateTime).ToString("yyyy-MM-dd HH:mm:ss") : "",
                        //item.CreateTime,
                        item.DepartmentName,
                        item.BelAmount
                    });
                }

                decimal InTotal = modelList.Where(u => u.TypeContent == "充值" || u.TypeContent == "退款").Sum(u => (decimal)(u.AddAmount ?? 0));
                decimal OutTotal = modelList.Where(u => u.TypeContent == "补扣款" || u.TypeContent == "订餐扣款" || u.TypeContent == "消费").Sum(u => (decimal)(u.AddAmount ?? 0));
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.UserBalance + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();

                
                designer.SetDataSource("InTotal", InTotal);
                designer.SetDataSource("OutTotal", OutTotal);
                //打开模板
                designer.Open(ExportTempPath.UserBalance);
                //设置集合变量
                designer.SetDataSource(ImportFileType.UserBalance, listResult);
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

        public ActionResult getTypeContentAsDict()
        {
            var objList = _balancedetailservice.List().Select(u => new { u.TypeContent }).Distinct().ToList();
            return Json(objList, JsonRequestBehavior.AllowGet);
        }
    }
}