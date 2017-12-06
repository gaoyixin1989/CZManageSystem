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
    public class ContractPayController : BaseController
    {
        IInvestContractPayService _investContractPayService = new InvestContractPayService();
        IInvestProjectService _projectService = new InvestProjectService();//投资项目信息
        IInvestContractService _contractService = new InvestContractService();//合同信息
        // GET: ContractPay
        public ActionResult ContractPayIndex()
        {
            return View();
        }
        public ActionResult ContractPayEdit(Guid? ID, string type)
        {
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }
        #region 合同已付款导入
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, InvestContractPayQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _investContractPayService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).Select(u => new
            {
                u.ID,
                u.Batch,
                u.DateAccount,
                u.RowNote,
                u.ProjectID,
                ProjectName = _projectService.FindByFeldName(a => a.ProjectID == u.ProjectID)?.ProjectName,
                u.ContractID,
                ContractName = _contractService.FindByFeldName(a => a.ProjectID == u.ProjectID && a.ContractID == u.ContractID)?.ContractName,
                u.Supply,
                u.Pay,
                u.Time,
                u.UserID
            }).ToList();
            var total = new
            {
                Pay = _investContractPayService.GetForPaging(out count, queryBuilder).Sum(u => u.Pay ?? 0)
            };

            return Json(new { items = modelList, count = count, total = total });

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _investContractPayService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _investContractPayService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult InvestContractPayInfoGetByID(Guid ID)
        {
            var list = _investContractPayService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.Batch,
                list.DateAccount,
                list.RowNote,
                list.ProjectID,
                list.ContractID,
                list.Supply,
                list.Pay

            });
        }
        public ActionResult Save_InvestContractPay(InvestContractPay contractpay)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            if (contractpay.ID == null || contractpay.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                bool isValid = false;//是否验证通过
                #region 验证数据是否合法
                var Coding = _investContractPayService.List().Where(c => c.RowNote == contractpay.RowNote).ToList();
                if (Coding.Count() > 0)
                {
                    tip = "行说明不能重复";
                }
                else
                {
                    isValid = true;
                }
                #endregion

                if (isValid)
                {
                    contractpay.ID = Guid.NewGuid();
                    result.IsSuccess = _investContractPayService.Insert(contractpay);

                }
            }
            else
            {//编辑
                result.IsSuccess = _investContractPayService.Update(contractpay);
            }
            result.Message = tip;
            return Json(result);
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult InvestContractPayDownload(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestContractPayQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _investContractPayService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<InvestContractPay>;
                var list = modelList.Select(s => new
                {
                    s.ID,
                    s.Batch,
                    s.DateAccount,
                    s.RowNote,
                    s.ProjectID,
                    s.ContractID,
                    s.Supply,
                    s.Pay

                }).ToList<object>();
                if (list.Count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestContractPay + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InvestContractPay);
                //设置集合变量
                designer.SetDataSource(ImportFileType.InvestContractPay, list);
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

        #region 已付款查询
        /// <summary>
        /// 已付款查询
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractPayQueryPage()
        {
            return View();
        }

        /// <summary>
        /// 根据项目编号和合同编号显示数据列表
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ContractID"></param>
        /// <returns></returns>
        public ActionResult ContractPayListShow(string ProjectID = null, string ContractID = null)
        {
            ViewData["ProjectID"] = ProjectID;
            ViewData["ContractID"] = ContractID;
            return View();
        }

        /// <summary>
        /// 已付款查询页面查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetData_ContractPayQueryPage(int pageIndex = 1, int pageSize = 5, InvestContractPayQueryBuilder queryBuilder = null)
        {
            List<object> modelList = new List<object>();
            int count = 0;
            var query = _investContractPayService.GetForPaging(out count, queryBuilder);

            var allDataQuery = query.GroupBy(u => u.ProjectID + "," + u.ContractID).ToList();
            var list = allDataQuery.OrderBy(u => u.Key).Skip((pageIndex > 0 ? (pageIndex - 1) : 0) * pageSize).Take(pageSize);

            foreach (var item in list)
            {
                string ProjectID = item.First().ProjectID;
                string ContractID = item.First().ContractID;
                modelList.Add(new
                {
                    ProjectID = ProjectID,
                    ProjectName = _projectService.FindByFeldName(a => a.ProjectID == ProjectID)?.ProjectName,
                    ContractID = ContractID,
                    ContractName = _contractService.FindByFeldName(a => a.ProjectID == ProjectID && a.ContractID == ContractID)?.ContractName,
                    AllPay = item.Sum(u => u.Pay ?? 0)
                });
            }


            var total = new
            {
                Pay = query.Sum(u => u.Pay ?? 0)
            };
            return Json(new { items = modelList, count = allDataQuery.Count(), total = total });
        }

        /// <summary>
        /// 导出
        /// </summary> 
        public ActionResult Download_ContractPayQueryPage(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestContractPayQueryBuilder>(queryBuilder);


                List<object> modelList = new List<object>();
                int count = 0;
                var query = _investContractPayService.GetForPaging(out count, QueryBuilder);

                var allDataQuery = query.GroupBy(u => u.ProjectID + "," + u.ContractID).ToList();

                foreach (var item in allDataQuery)
                {
                    string ProjectID = item.First().ProjectID;
                    string ContractID = item.First().ContractID;
                    modelList.Add(new
                    {
                        ProjectID = ProjectID,
                        ProjectName = _projectService.FindByFeldName(a => a.ProjectID == ProjectID)?.ProjectName,
                        ContractID = ContractID,
                        ContractName = _contractService.FindByFeldName(a => a.ProjectID == ProjectID && a.ContractID == ContractID)?.ContractName,
                        AllPay = item.Sum(u => u.Pay ?? 0)
                    });
                }


                if (modelList.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestContractPayQueryPage + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InvestContractPayQueryPage);
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
    }
}