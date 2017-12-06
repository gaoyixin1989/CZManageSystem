using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data;
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
    public class InvestMaterialsController : BaseController
    {
        // GET: InvestMaterials

        IInvestMaterialsService investMaterialsService = new InvestMaterialsService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexMy()
        {
            return View();
        }
        public ActionResult Edit(string key = null)
        {
            ViewData["key"] = string.IsNullOrEmpty(key) ? new Guid() : new Guid(key);
            return View();
        }
        public ActionResult GetDataByID(string key)
        {
            Guid id;
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "当前记录不存在！" };
            if (!Guid.TryParse(key, out id))
            {
                result.Message = "参数错误。";
                return Json(result);
            }
            var model = investMaterialsService.FindById(id);
            if (model != null)
            {
                result.IsSuccess = true;
                result.data = model;
            }
            return Json(result);
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
                var QueryBuilder = JsonConvert.DeserializeObject<InvestMaterialsQueryBuilder>(queryBuilder);
                int count = 0;
                var modelList = investMaterialsService.GetForPaging(out count, QueryBuilder);
                if(modelList .Count() <1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestMaterials + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();

                //打开模板
                designer.Open(ExportTempPath.InvestMaterials);
                designer.SetDataSource(ImportFileType.InvestMaterials, modelList);
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
                SystemResult result = new SystemResult() { IsSuccess = false, Message = ex.Message + ";" + ex.InnerException?.Message };
                return Json(result);
            }
        }



        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, InvestMaterialsQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = investMaterialsService.GetForPaging(out count, queryBuilder, pageIndex, pageSize);
            return Json(new { items = modelList, count = count });
        }
        public ActionResult Delete(Guid[] ids)
        {
            var list = investMaterialsService.List().Where(f => ids.Contains(f.ID));
            var models = list.ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (investMaterialsService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult Save(InvestMaterials investMaterialsViewModel)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            { 
                var isValid = new ViewModelValidator(investMaterialsViewModel);
                if (!isValid.IsValid())
                { 
                    result.Message = isValid.ValidationErrorsToString; 
                    return Json(result);
                }
                if (investMaterialsViewModel.ID == new Guid())
                {
                    investMaterialsViewModel.ID = Guid.NewGuid();
                    if (investMaterialsService.Insert(investMaterialsViewModel))
                    {
                        result.IsSuccess = true;
                        return Json(result);
                    }
                }
                var model = investMaterialsService.FindById (investMaterialsViewModel.ID);
                if (model==null)
                {
                    result.Message ="此记录不存在！";
                    return Json(result);
                }
                model.Apply = investMaterialsViewModel.Apply;
                model.AuditStatus  = investMaterialsViewModel.AuditStatus;
                model.ContractID  = investMaterialsViewModel.ContractID;
                model.ContractName  = investMaterialsViewModel.ContractName;
                model.OrderCreateTime   = investMaterialsViewModel.OrderCreateTime;
                model.OrderDesc  = investMaterialsViewModel.OrderDesc;
                model.OrderID  = investMaterialsViewModel.OrderID;
                model.OrderInCompany  = investMaterialsViewModel.OrderInCompany;
                model.OrderInPay  = investMaterialsViewModel.OrderInPay;
                model.OrderNote  = investMaterialsViewModel.OrderNote;
                model.OrderOutCompany  = investMaterialsViewModel.OrderOutCompany;
                model.OrderOutRate  = investMaterialsViewModel.OrderOutRate;
                model.OrderOutSum  = investMaterialsViewModel.OrderOutSum;
                model.OrderTitle  = investMaterialsViewModel.OrderTitle;
                model.OrderUnReceived  = investMaterialsViewModel.OrderUnReceived;
                model.OutContractID  = investMaterialsViewModel.OutContractID;
                model.ProjectID  = investMaterialsViewModel.ProjectID;
                model.ProjectName  = investMaterialsViewModel.ProjectName;
                if (investMaterialsService.Update(model))
                    result.IsSuccess = true; 
                return Json(result);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message + ";" + ex.InnerException?.Message;
                return Json(result);

            }
        }
         
    }
}