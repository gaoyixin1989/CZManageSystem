using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative.VehicleManages;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarFeeRentController : BaseController
    {

        ICarFeeRentService _carFeeRentService = new CarFeeRentService();
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        // GET: CarFeeRent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }

        public ActionResult CarFeeRentInfo(CarFeeRent carf)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (carf.CarFeeRentId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = _carFeeRentService.Update(carf);
            }
            else
            {
                carf.CarFeeRentId = Guid.NewGuid();
                carf.EditorId = this.WorkContext.CurrentUser.UserId;
                carf.EditTime = DateTime.Now;
                result.IsSuccess = _carFeeRentService.Insert(carf);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        public ActionResult CarFeeRentGetListData(int pageIndex = 1, int pageSize = 5, DriverFilesQueryBuilder queryBuilder = null)
        {
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            int count = 0;

            var modelList = _carFeeRentService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<CarFeeRent>;
            var list = modelList.Select(s => new
            {
                s.CarFeeRentId,
                s.EditorId,
                s.EditTime,
                s.CorpId,
                CorpId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString()).DDText,
                s.CarId,
                LicensePlateNum = s.CarInfo?.LicensePlateNum,
                s.CorpName,
                s.DriverFee,
                s.GasFee,
                s.RoadFee,
                s.MoreFee,
                s.MoreRoad,
                s.RentCount,
                s.RentFee,
                s.SortId,
                SortId_text= _dataDictionaryService.FindByFeldName(d => d.DDName == "缴费类型" && d.DDValue == s.SortId.ToString()).DDText,
                s.TotalFee,
                s.StartTime,
                s.EndTime,
                s.Person,
                s.Remark
            }).ToList<object>();
            return Json(new { items = list, count = count });
        }

        public ActionResult CarFeeRentDelete(Guid[] DriverId)
        {
            foreach (Guid id in DriverId)
            {
                var obj = _carFeeRentService.FindById(id);
                if (obj != null)
                {
                    _carFeeRentService.Delete(obj);
                }
            }
            return Json(new { status = 0, message = "成功" });
        }

        public ActionResult CarFeeRentDataByID(Guid id)
        {
            var temp = _carFeeRentService.FindById(id);
            return Json(new
            {
                temp.CarFeeRentId,
                temp.EditorId,
                temp.EditTime,
                temp.CorpId,
                CorpId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == temp.CorpId.ToString()).DDText,
                temp.CarId,
                LicensePlateNum = temp.CarInfo?.LicensePlateNum,
                temp.CorpName,
                temp.DriverFee,
                temp.GasFee,
                temp.RoadFee,
                temp.MoreFee,
                temp.MoreRoad,
                temp.RentCount,
                temp.RentFee,
                temp.SortId,
                temp.RoadCount,
                SortId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == "缴费类型" && d.DDValue == temp.SortId.ToString()).DDText,
                temp.TotalFee,
                StartTime=temp.StartTime.HasValue ? temp.StartTime.Value.ToString("yyyy-MM-dd") : "",
                EndTime = temp.EndTime.HasValue ? temp.EndTime.Value.ToString("yyyy-MM-dd") : "",
                temp.Person,
                temp.Remark
                
            });
        }

        /// <summary>
        /// 导出固定费用
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<DriverFilesQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);

                int count = 0;
                var modelList = _carFeeRentService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<CarFeeRent>;
                var listResult = modelList.Select(s => new
                {
                    s.CarFeeRentId,
                    s.EditorId,
                    EditTime = Convert.ToDateTime(s.EditTime).ToString("yyyy-MM-dd"),
                    s.CorpId,
                    CorpId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString()).DDText,
                    s.CarId,
                    LicensePlateNum = s.CarInfo?.LicensePlateNum,
                    s.CorpName,
                    s.DriverFee,
                    s.GasFee,
                    s.RoadFee,
                    s.MoreFee,
                    s.MoreRoad,
                    s.RentCount,
                    s.RentFee,
                    s.SortId,
                    SortId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == "缴费类型" && d.DDValue == s.SortId.ToString()).DDText,
                    s.TotalFee,
                    StartTime = Convert.ToDateTime(s.StartTime).ToString("yyyy-MM-dd"),
                    EndTime = Convert.ToDateTime(s.EndTime).ToString("yyyy-MM-dd"),
                    s.Person,
                    s.Remark

                }).ToList<object>();


                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.CarFeeRent + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.CarFeeRent);
                //设置集合变量
                designer.SetDataSource(ImportFileType.CarFeeRent, listResult);
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

    }
}