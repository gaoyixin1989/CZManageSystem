using CZManageSystem.Admin.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CZManageSystem.Service.ITSupport;
using CZManageSystem.Data.Domain.ITSupport;
using Newtonsoft.Json;
using CZManageSystem.Admin.Base;
using Aspose.Cells;

namespace CZManageSystem.Admin.Controllers.ITSupport
{
   
    public class StockController : BaseController
    {
        IStockService _sysStockService = new StockService();
        IStockAssetService _sysStockAssetService = new StockAssetService();

        // GET: Stock
        #region 设备入库
        public ActionResult StockIndex()
        {
            return View();
        }
       
        public ActionResult StockEdit(int? Id)
        {
            ViewData["Id"] = Id;
            if (Id == null)
                ViewBag.Title = "设备入库新增";
            else
                ViewBag.Title = "设备入库编辑";
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, StockQueryBuilder queryBuilder = null)
        {
         
            int count = 0;
            var modelList = _sysStockService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }

        public ActionResult GetDataByID(int Id)
        {
            var stock = _sysStockService.FindById(Id);
            return Json(new
            {
                stock.LableNo,
                stock.ProjSn,
                stock.StockTime,
                stock.StockType,
                stock.EquipNum,
                stock.EquipInfo,
                stock.EquipClass,
                stock.Content
            });
        }
        public ActionResult Save(Stock stock)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法

            #endregion
            stock.EditTime = DateTime.Now;
            if (stock.Id == 0)//新增
            {
                stock.StockTime = DateTime.Now;
               // stock.Totalnum = stock.EquipNum;
                result.IsSuccess = _sysStockService.Insert(stock);
            }
            else
            {//编辑
                result.IsSuccess = _sysStockService.Update(stock);
            }
            return Json(result);
        }
        public ActionResult Delete(int[] ids)
        {
            foreach (int Id in ids)
            {
                var obj = _sysStockService.FindById(Id);
                if (obj != null && obj.Id != 0)
                    _sysStockService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }

        /// <summary>
        /// 获取下拉框类型
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetEquipClassList()
        {
            var EquipClassList = this.GetDictListByDDName("设备信息");
            return Json(new
            {
                EquipClassList = EquipClassList
            });
        }
        #endregion

        #region 固定资产明细
        public ActionResult StockAsset(int? Id)
        {
            ViewData["Id"] = Id;
            ViewBag.Title = "固定资产明细";
            return View();
        }
        public ActionResult StockAssetEdit(int? Id,int? StockId)
        {
            ViewData["Id"] = Id;
            ViewData["StockId"] = StockId;
            if (Id == null)
                ViewBag.Title = "固定资产明细新增";
            else
                ViewBag.Title = "设备入库编辑修改";
            return View();
        }
        public ActionResult GetStockAssetbyid(int pageIndex, int pageSize,int Id)
        {
            int count = 0;
            var modelList = _sysStockAssetService.GetStockAssetbyid(out count, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize,Id);
            return Json(new { items=modelList, count = count });

        }
        public ActionResult DeleteAsset(int[] ids)
        {
            foreach (int Id in ids)
            {
                var obj = _sysStockAssetService.FindById(Id);
                if (obj != null && obj.Id != 0)
                    _sysStockAssetService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }

        public ActionResult SaveAsset(StockAsset stockasset)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (stockasset.Id == 0)//新增
            {
                result.IsSuccess = _sysStockAssetService.Insert(stockasset);
            }
            else
            {//编辑
                result.IsSuccess = _sysStockAssetService.Update(stockasset);
            }
            return Json(result);
        }

        public ActionResult GetAssetID(int Id)
        {
            var asset = _sysStockAssetService.FindById(Id);
            return Json(new
            {
                AssetSn= asset.AssetSn,
                Id= asset.Id
            });
        }

        #endregion

        #region 设备库存管理
        public ActionResult EquipStock()
        {
            return View();
        }
        public ActionResult EquipStockNum(int pageIndex = 1, int pageSize = 5, EquipAppQueryBuilder queryBuilder = null)
        {

            int count = 0;
            var modelList = _sysStockService.EquipStockNum(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }

        #endregion

        #region 设备出库信息
        public ActionResult OutStock(string stock)
        {
             ViewData["stock"] = stock;
             Session["stock"] = stock;
            return View();
        }
        public ActionResult InStock(string stock)
        {
            ViewData["stock"] = stock;
            Session["stock"] = stock;
            return View();
        }
        public ActionResult Outmatinfo(int pageIndex = 1, int pageSize = 5, OutstockQueryBuilder queryBuilder = null, string stock="")
        {
            stock = Session["stock"].ToString();
            List<Stock> sto = new List<Stock>();
            sto = new JavaScriptSerializer().Deserialize<List<Stock>>(stock);
            int count = 0;
            var modelList = _sysStockService.Outmatinfo(out count, sto, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }
        public ActionResult InStockinfo(int pageIndex = 1, int pageSize = 5, StockQueryBuilder queryBuilder = null)
        {
           var stock =Session["stock"].ToString();
            List<Stock> sto = new List<Stock>();
            sto = new JavaScriptSerializer().Deserialize<List<Stock>>(stock);
            int count = 0;
            var modelList = _sysStockService.InStockinfo(out count, sto, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }
        #endregion

        #region 设备资产管理
        public ActionResult EquipAsset()
        {
            return View();
        }
        public ActionResult GetEquipAsset(int pageIndex = 1, int pageSize = 5, EquipAssetQueryBuilder queryBuilder = null)
        {

            int count = 0;
            var modelList = _sysStockService.EquipAsset(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }
        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult DownloadAsset(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<EquipAssetQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _sysStockService.EquipAsset(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Asset + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Asset);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Asset, modelList);
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