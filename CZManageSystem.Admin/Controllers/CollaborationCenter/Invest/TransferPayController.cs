using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Service.CollaborationCenter.Invest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Invest
{
    public class TransferPayController : BaseController
    {
        IInvestTransferPayService _investTransferPayService = new InvestTransferPayService();
        IInvestProjectService _projectService = new InvestProjectService();//投资项目信息
        IInvestContractService _contractService = new InvestContractService();//合同信息
        // GET: TransferPay
        public ActionResult TransferPayIndex()
        {
            return View();
        }
        public ActionResult TransferPayEdit(Guid? ID, string type)
        {
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }
        /// <summary>
        /// 已转资查询
        /// </summary>
        /// <returns></returns>
        public ActionResult TransferPayQueryPage()
        {
            return View();
        }
        #region 已转资查询


        /// <summary>
        /// 已转资查询页面查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetData_TransferPayQueryPage(int pageIndex = 1, int pageSize = 5, string ProjectID = null, string ProjectName = null, decimal? TransferPayAll_start = null, decimal? TransferPayAll_end = null)
        {
            #region 查询数据
            List<object> modelList = new List<object>();
            var query = _investTransferPayService.List().Where(u => u.IsTransfer == "是");
            if (!string.IsNullOrEmpty(ProjectID))
                query = query.Where(u => u.ProjectID.Contains(ProjectID));
            if (!string.IsNullOrEmpty(ProjectName))
            {
                var queryProjectIDs = _projectService.List().Where(u => u.ProjectName.Contains(ProjectName)).Select(u => u.ProjectID).ToList();
                if (queryProjectIDs.Count > 0)
                    query = query.Where(u => queryProjectIDs.Contains(u.ProjectID));
            }

            var allDataQuery = query.GroupBy(u => u.ProjectID).ToList();
            if (TransferPayAll_start.HasValue)
                allDataQuery = allDataQuery.Where(u => u.Sum(a => (a.TransferPay ?? 0)) >= TransferPayAll_start.Value).ToList();
            if (TransferPayAll_end.HasValue)
                allDataQuery = allDataQuery.Where(u => u.Sum(a => (a.TransferPay ?? 0)) <= TransferPayAll_end.Value).ToList();
            #endregion

            var list = allDataQuery.OrderBy(u => u.Key).Skip((pageIndex > 0 ? (pageIndex - 1) : 0) * pageSize).Take(pageSize);

            var listProjectID = allDataQuery.Select(u => (string)u.Key).ToList();
            var listProject = _projectService.List().Where(u => listProjectID.Contains(u.ProjectID)).ToList();

            foreach (var item in list)
            {
                string sProjectID = item.First().ProjectID;
                var projectObj = _projectService.FindByFeldName(u => u.ProjectID == sProjectID);
                string sProjectName = projectObj == null ? "" : projectObj.ProjectName;
                decimal ProjectTotal = projectObj == null ? 0 : projectObj.Total ?? 0;
                decimal TransferPay = item.Sum(u => (decimal)(u.TransferPay ?? 0));
                modelList.Add(new
                {
                    ProjectID = sProjectID,
                    ProjectName = sProjectName,
                    ProjectTotal = ProjectTotal,
                    TransferPay = TransferPay,
                    Percent = Math.Round((ProjectTotal > 0 ? (TransferPay * 100 / ProjectTotal) : 0), 2) + "%"
                });
            }


            var total = new
            {
                ProjectTotal = listProject.Sum(u => u.Total ?? 0),
                TransferPay = allDataQuery.Sum(u => u.Sum(a => a.TransferPay ?? 0))
            };
            return Json(new { items = modelList, count = allDataQuery.Count(), total = total });
        }
        class TransferPayQuetyBuilder
        {
            public string  ProjectID { get; set; }
            public string  ProjectName { get; set; }
            public decimal? TransferPayAll_start { get; set; }
            public decimal? TransferPayAll_end { get; set; }
        }
        /// <summary>
        /// 导出
        /// </summary> 
        public ActionResult Download_TransferPayQueryPage(string queryBuilder = null)
        {
            try
            {
                var QueryBuilder = JsonConvert.DeserializeObject<TransferPayQuetyBuilder>(queryBuilder);
                #region 查询数据
                List<object> modelList = new List<object>();
                var query = _investTransferPayService.List().Where(u => u.IsTransfer == "是");
                if (!string.IsNullOrEmpty(QueryBuilder.ProjectID))
                    query = query.Where(u => u.ProjectID.Contains(QueryBuilder.ProjectID));
                if (!string.IsNullOrEmpty(QueryBuilder.ProjectName))
                {
                    var queryProjectIDs = _projectService.List().Where(u => u.ProjectName.Contains(QueryBuilder.ProjectName)).Select(u => u.ProjectID).ToList();
                    if (queryProjectIDs.Count > 0)
                        query = query.Where(u => queryProjectIDs.Contains(u.ProjectID));
                }

                var allDataQuery = query.GroupBy(u => u.ProjectID).ToList();
                if (QueryBuilder.TransferPayAll_start.HasValue)
                    allDataQuery = allDataQuery.Where(u => u.Sum(a => (a.TransferPay ?? 0)) >= QueryBuilder.TransferPayAll_start.Value).ToList();
                if (QueryBuilder.TransferPayAll_end.HasValue)
                    allDataQuery = allDataQuery.Where(u => u.Sum(a => (a.TransferPay ?? 0)) <= QueryBuilder.TransferPayAll_end.Value).ToList();
                #endregion

                foreach (var item in allDataQuery)
                {
                    string sProjectID = item.First().ProjectID;
                    var projectObj = _projectService.FindByFeldName(u => u.ProjectID == sProjectID);
                    string sProjectName = projectObj == null ? "" : projectObj.ProjectName;
                    decimal ProjectTotal = projectObj == null ? 0 : projectObj.Total ?? 0;
                    decimal TransferPay = item.Sum(u => (decimal)(u.TransferPay ?? 0));
                    modelList.Add(new
                    {
                        ProjectID = sProjectID,
                        ProjectName = sProjectName,
                        ProjectTotal = ProjectTotal,
                        TransferPay = TransferPay,
                        Percent = Math.Round((ProjectTotal > 0 ? (TransferPay * 100 / ProjectTotal) : 0), 2) + "%"
                    });
                }


                if (modelList.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestTransferPayQueryPage + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Download_InvestTransferPayQueryPage);
                //设置集合变量
                designer.SetDataSource("emp", modelList);
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
        #endregion

        #region 已转资合同导入
        public ActionResult GetTransferPayListData(int pageIndex = 1, int pageSize = 5, InvestTransferPayQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var queryDatas = _investTransferPayService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).Select(u => (InvestTransferPay)u).ToList();
            var modelList = queryDatas.Select(u => new
            {
                u.ID,
                u.ContractID,
                ContractName = _contractService.FindByFeldName(a => a.ProjectID == u.ProjectID && a.ContractID == u.ContractID)?.ContractName,
                u.ProjectID,
                ProjectName = _projectService.FindByFeldName(a => a.ProjectID == u.ProjectID)?.ProjectName,
                u.IsTransfer,
                u.TransferPay
            }).ToList();

            var total = new
            {
                TransferPay = _investTransferPayService.GetForPaging(out count, queryBuilder).Sum(u => (decimal)(u.TransferPay ?? 0))
            };
            return Json(new { items = modelList, count = count, total = total });

        }
        public ActionResult DeleteTransferPay(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _investTransferPayService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _investTransferPayService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult TransferPayInfoGetByID(Guid ID)
        {
            var list = _investTransferPayService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.ContractID,
                ContractName = _contractService.FindByFeldName(a => a.ProjectID == list.ProjectID && a.ContractID == list.ContractID)?.ContractName,
                list.ProjectID,
                ProjectName = _projectService.FindByFeldName(a => a.ProjectID == list.ProjectID)?.ProjectName,
                list.IsTransfer,
                list.TransferPay

            });
        }
        public ActionResult Save_TransferPay(InvestTransferPay transferpay)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";

            if (!_investTransferPayService.CheckRepeat(transferpay.ID, transferpay.ProjectID, transferpay.ContractID))
            {
                tip = "改项目编号和合同编号的组合已经被占用";
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }

            if (transferpay.ID == null || transferpay.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                transferpay.ID = Guid.NewGuid();
                result.IsSuccess = _investTransferPayService.Insert(transferpay);
            }
            else
            {//编辑
                result.IsSuccess = _investTransferPayService.Update(transferpay);
            }
            result.Message = tip;
            return Json(result);
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult TransferPayDownload(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestTransferPayQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _investTransferPayService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<InvestTransferPay>;
                var list = modelList.Select(s => new
                {
                    s.ID,
                    s.ContractID,
                    ContractName = _contractService.FindByFeldName(a => a.ProjectID == s.ProjectID && a.ContractID == s.ContractID)?.ContractName,
                    s.ProjectID,
                    ProjectName = _projectService.FindByFeldName(a => a.ProjectID == s.ProjectID)?.ProjectName,
                    s.IsTransfer,
                    s.TransferPay


                }).ToList<object>();
                if (list.Count <1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.TransferPay + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.TransferPay);
                //设置集合变量
                designer.SetDataSource(ImportFileType.TransferPay, list);
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
    }
}