using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core.Helpers;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.Invest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Invest
{
    public class ProjectPayController : BaseController
    {
        IInvestTempEstimateService _investTempEstimateService = new InvestTempEstimateService();
        IInvestProjectService _projectService = new InvestProjectService();//投资项目信息
        IInvestContractService _contractService = new InvestContractService();//合同信息
        IInvestEstimateService _investEstimateService = new InvestEstimateService();
        IInvestMonthEstimateApplyService _investMonthEstimateApplyService = new InvestMonthEstimateApplyService();
        IInvestMonthEstimateApplySubListService _investMonthEstimateApplySubListService = new InvestMonthEstimateApplySubListService();
        // GET: ProjectPay
        public ActionResult TempEstimateIndex()
        {
            return View();
        }
        public ActionResult TempEstimateEdit(Guid? ID, string type, string ProjectID, string ContractID)
        {
            if (ID ==null)
            {
                if (!string.IsNullOrEmpty(ProjectID) && !string.IsNullOrEmpty(ContractID))
                {
                    var list = _investTempEstimateService.FindByFeldName(u => u.ProjectID == ProjectID && u.ContractID == ContractID);
                    if (list != null && list.ID != Guid.Empty)
                        ID = list.ID;
                }
            }
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }

        public ActionResult TempEstimateQuery()
        {
            return View();
        }
        public ActionResult EstimateSelect(string selected)
        {
            ViewData["selected"] = selected;
            return View();
        }
        public ActionResult EstimateDetail(Guid? ID, string type, string ProjectID, string ContractID)
        {
            if (ID == null)
            {
                if (!string.IsNullOrEmpty(ProjectID) && !string.IsNullOrEmpty(ContractID))
                {
                    var list = _investEstimateService.FindByFeldName(u => u.ProjectID == ProjectID && u.ContractID == ContractID);
                    if (list != null && list.ID != Guid.Empty)
                        ID = list.ID;
                }
            }
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }

        public ActionResult TempEstimateStop()
        {
            return View();
        }
        public ActionResult Follow()
        {
            return View();
        }
        public ActionResult FollowDetail(Guid? ID, string type)
        {
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }

        #region 暂估管理(业务部门)
        public ActionResult GetTempEstimateListData(int pageIndex = 1, int pageSize = 5, InvestTempEstimateQueryBuilder queryBuilder = null)
        {
            int count = 0;

            var pageList = _investTempEstimateService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();

            var modelList = pageList.Select(u => new
            {
                u.ID,
                u.ProjectID,
                u.ContractID,
                ProjectName = GetProjectName(u.ProjectID),
                ContractName = GetContractName(u.ProjectID, u.ContractID),
                u.SignTotal,
                u.PayTotal,
                u.Rate,
                u.Pay,
                u.NotPay,
                EstimateUserName = u.ManagerID == null ? "" : u.ManagerObj.RealName

            });
            var total = new
            {
                SignTotalCount = pageList.Sum(u => (decimal)(u.SignTotal ?? 0)),//合同金额
                PayTotalCount = pageList.Sum(u => (decimal)(u.PayTotal ?? 0)),//实际合同金额
                NotPayCount = pageList.Sum(u => (decimal)(u.NotPay ?? 0)),//暂估金额
                PayCount = pageList.Sum(u => (decimal)(u.Pay ?? 0))//已付款金额

            };
            return Json(new { items = modelList, total= total, count = count });

        }
        /// <summary>
        /// 优先使用ID，如果ID值为空，或根据ID值查询不到对应数据，则使用ProjectID+ContractID查询数据获取ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ProjectID"></param>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public ActionResult InvestTempEstimateInfoGetByID(Guid ID)
        {
            var list = _investTempEstimateService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.ProjectID,
                list.ContractID,
                ProjectName = GetProjectName(list.ProjectID),
                ContractName = GetContractName(list.ProjectID, list.ContractID),
                list.SignTotal,
                list.PayTotal,
                list.Rate,
                list.Pay,
                list.NotPay,
                list.Course,
                list.Study,
                list.Supply,
                list.BackRate,
                EstimateUserName = list.ManagerID == null ? "" : list.ManagerObj.RealName

            });
        }
       

        public ActionResult Save_InvestTempEstimate(InvestTempEstimate tempEstimate)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (tempEstimate.ID == null || tempEstimate.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {

                tempEstimate.ID = Guid.NewGuid();
                result.IsSuccess = _investTempEstimateService.Insert(tempEstimate);

            }
            else
            {//编辑
                result.IsSuccess = _investTempEstimateService.Update(tempEstimate);
            }
            return Json(result);
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult InvestTempEstimateDownload(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestTempEstimateQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var pageList = _investTempEstimateService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<InvestTempEstimate>;

                var modelList = pageList.Select(u => new
                {
                    u.ID,
                    u.ProjectID,
                    u.ContractID,
                    ProjectName = GetProjectName(u.ProjectID),
                    ContractName = GetContractName(u.ProjectID, u.ContractID),
                    u.SignTotal,
                    u.PayTotal,
                    u.Rate,
                    u.Pay,
                    u.NotPay,
                    EstimateUserName = u.ManagerID == null ? "" : u.ManagerObj.RealName

                }).ToList<object>();

                if (modelList.Count <1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.TempEstimate + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.TempEstimate);
                //设置集合变量
                designer.SetDataSource(ImportFileType.TempEstimate, modelList);
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

        #region 暂估查询
        public ActionResult GetEstimateListData(int pageIndex = 1, int pageSize = 5, InvestEstimateQueryBuilder queryBuilder = null)
        {
            int count = 0;
            dynamic Total = new { };
            var ModelList = _investEstimateService.GetForPaging_(out count, ref Total, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = new { ModelList = ModelList, Total = Total }, count = count });

        }
        public ActionResult InvestEstimateInfoGetByID(Guid ID)
        {
            string message = "";
            var list = _investEstimateService.FindById(ID);
            if (list==null)
            {
                message = "不存在该项目明细";
                return Json(new { message = message });
            }
            return Json(new { items = new
            {
                list.ID,
                list.ProjectID,
                list.ContractID,
                list.ProjectName,
                list.ContractName,
                //ProjectName = GetProjectName(list.ProjectID),
                //ContractName = GetContractName(list.ProjectID, list.ContractID),
                list.SignTotal,
                list.PayTotal,
                list.Rate,
                list.Pay,
                list.NotPay,
                list.Course,
                list.Study,
                list.Supply,
                list.BackRate,
                EstimateUserName = list.ManagerID == null ? "" : list.ManagerObj.RealName
            },
                message = message
            });
           
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult InvestEstimateDownload(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestEstimateQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;

                object Total = new { };
                var modelList = _investEstimateService.GetForPaging_(out count, ref Total, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList<object>();


                if (modelList.Count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Estimate + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Estimate);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Estimate, modelList);
                var TotalList = new List<object>();
                TotalList.Add(Total);
                designer.SetDataSource("Total", TotalList);
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

        #region 已终止的暂估条目

        public ActionResult GetStopTempEstimateList(int pageIndex = 1, int pageSize = 5, StopInvestTempEstimateQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var pageList = _investTempEstimateService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).Select(u => (InvestTempEstimate)u).Select(u => new
            {
                u.ID,
                u.ProjectID,
                u.ContractID,
                ProjectName = GetProjectName(u.ProjectID),
                ContractName = GetContractName(u.ProjectID, u.ContractID),
                u.SignTotal,
                u.PayTotal,
                u.Rate,
                u.Pay,
                u.NotPay,
                u.Status,
                u.StatusTime,
                EstimateUserName = u.ManagerObj?.RealName
            }).ToList();
           var  total = new
            {
                SignTotalCount = pageList.Sum(u => u.SignTotal ?? 0),//合同金额
                PayTotalCount = pageList.Sum(u => u.PayTotal ?? 0),//实际合同金额
                NotPayCount = pageList.Sum(u => u.NotPay ?? 0),//暂估金额
                PayCount = pageList.Sum(u => u.Pay ?? 0)//已付款金额

            };
            return Json(new { items = pageList, total= total, count = count });

        }

        public ActionResult RecoverData(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _investTempEstimateService.List().Where(u => Ids.Contains(u.ID)).ToList();          
            if (objs.Count > 0)
            {
                List<InvestTempEstimate> list = new List<InvestTempEstimate>();
                foreach (var item in objs)
                {
                    item.Status = "正常";
                    item.StatusTime = DateTime.Now;
                    item.IsLock = "否";
                    list.Add(item);
                }
                isSuccess = _investTempEstimateService.UpdateByList(list);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        #endregion

        #region 流程跟踪
        public ActionResult GetFollow(int pageIndex = 1, int pageSize = 5,  string Series = null)
        {

            int count = 0;
            var modelList = _investMonthEstimateApplyService.GetForPaging(out count, Series, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            var items = modelList;
            return Json(new { items = items, count = count });
        }

        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult FollowDetailByApplyID(Guid ID)
        {
            var model = _investMonthEstimateApplySubListService.List().Where(u => u.ApplyID == ID).ToList();
            decimal? TaxCount = 0;
            var modelList = model.ToList().Select(u => new
            {
                u.Month,
                u.Year,
                u.ID,
                u.ProjectID,
                u.ContractID,
                u.ProjectName,
                u.ContractName,
                u.Supply,
                u.SignTotal,
                u.PayTotal,
                u.Study,
                u.Course,
                u.BackRate,
                u.Rate,
                u.Pay,
                u.NotPay,
                Tax = GetTax(u.ProjectID, u.ContractID)
            });
            var total = new
            {
                SignTotalCount = modelList.Sum(u => (decimal)(u.SignTotal ?? 0)),//合同金额
                PayTotalCount = modelList.Sum(u => (decimal)(u.PayTotal ?? 0)),//实际合同金额
                TaxCount = modelList.Sum(u => (decimal)(u.Tax ?? 0)),//合同税金额
                NotPayCount = modelList.Sum(u => (decimal)(u.NotPay ?? 0)),//暂估金额
                PayCount = modelList.Sum(u => (decimal)(u.Pay ?? 0))//已付款金额

            };
            return Json(new { items = modelList, Total = total });
        }

        //public ActionResult Send(Guid[] Ids,string Message )
        //{
        //    bool isSuccess = false;
        //    int successCount = 0;
        //    var objs = _investMonthEstimateApplyService.List().Where(u => Ids.Contains(u.ApplyID)).ToList();
        //    string SMS_Port = ConfigurationManager.AppSettings["SMS_Port"];
        //    string SMS_Type = ConfigurationManager.AppSettings["SMS_Type"];
        //    string SMS_State = ConfigurationManager.AppSettings["SMS_State"];
        //    string SMS_ProName = ConfigurationManager.AppSettings["SMS_ProName"];
        //    SqlHelper.setConnection(ConfigurationManager.AppSettings["SMS_Contention"]);

        //    foreach(var item in objs)
        //    {
        //        SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, SMS_ProName,new { item.Mobile }

        //    }

        //    return Json(new
        //    {
        //        isSuccess = successCount > 0 ? true : false,
        //        successCount = successCount
        //    });
        //}
        #endregion

        #region 其他
        string GetProjectName(string ProjectID)
        {
            var model = _projectService.FindByFeldName(t => t.ProjectID == ProjectID);
            if (model == null)
                return "";
            return model.ProjectName;
        }
        string GetContractName(string ProjectID, string ContractID)
        {
            var model = _contractService.FindByFeldName(t => t.ProjectID == ProjectID && t.ContractID == ContractID);
            if (model == null)
                return "";
            return model.ContractName;
        }

        decimal? GetTax(string ProjectID, string ContractID)
        {
            var model = _contractService.FindByFeldName(t => t.ProjectID == ProjectID && t.ContractID == ContractID);
            if (model == null)
                return 0;
            return model.Tax;
        }

        public ActionResult GetDropList()
        {
            var YearList = GetDictListByDDName("年");
            var MonthList = GetDictListByDDName("月份");
            return Json(new
            {
                YearList = YearList,
                MonthList = MonthList
            });
        }
        #endregion
    }
}