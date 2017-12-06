using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative;
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
    public class CarFeeYearController : BaseController
    {
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        ICarFeeYearService _carFeeYearService = new CarFeeYearService();
        ICarInfoService _carInfoService = new CarInfoService();
        // GET: CarFeeYear
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid? CarFeeYearId, string type)
        {
            ViewData["CarFeeYearId"] = CarFeeYearId;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, CarFeeYearQueryBuilder queryBuilder = null)
        {
            try
            {
                int count = 0;
                var modelList = _carFeeYearService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<CarFeeYear>;
                var list = modelList.Select(s => new
                {
                    s.CarFeeYearId,
                    s.CarId,
                    LicensePlateNum = s.CarInfo?.LicensePlateNum,
                    s.CorpId,
                    CorpText = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString())?.DDText,
                    s.CorpName,
                    s.TotalFee,
                    PayTime = s.PayTime.HasValue ? s.PayTime.Value.ToString("yyyy-MM-dd") : "",
                    s.Person

                }).ToList<object>();
                return Json(new { items = list, count = count });

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 导出汇总
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<CarFeeYearQueryBuilder>(queryBuilder);
                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _carFeeYearService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<CarFeeYear>;
                var listResult = modelList.Select(s => new
                {
                    s.CarFeeYearId,
                    s.CarId,
                    LicensePlateNum = s.CarInfo?.LicensePlateNum,
                    s.CorpId,
                    CorpText = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString()).DDText,
                    s.CorpName,
                    s.TotalFee,
                    PayTime = s.PayTime.HasValue ? s.PayTime.Value.ToString("yyyy-MM-dd") : "",
                    s.Person 
                }).ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message"); 
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.CarFeeYear + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.CarFeeYear);
                //设置集合变量 
                designer.SetDataSource(ImportFileType.CarFeeYear, listResult);
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

        /// <summary>
        /// 获取字典名称
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetDictListByName(string DDName)
        {
            List<DataDictionary> CorpList = new List<DataDictionary>();
            CorpList = this.GetDictListByDDName(DDName);
            List<object> listResult = new List<object>();
            foreach (var item in CorpList)
            {
                listResult.Add(new
                {
                    text = item.DDText,
                    id = item.DDValue
                });
            }
            return Json(listResult);
        }

        public string GetSta_Text(int? sta)
        {
            string text = "";
            switch (sta)
            {
                case 0:
                    text = "空闲";
                    break;
                case 1:
                    text = "出车中";
                    break;
                case 2:
                    text = "送修";
                    break;
                case 3:
                    text = "保养";
                    break;
                case 4:
                    text = "停用";
                    break;
                default:
                    break;
            }
            return text;

        }

        /// <summary>
        /// 获取车辆
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetCarTemp(int CorpId)
        {
            var list = _carInfoService.List().Where(c => c.CorpId == CorpId).ToList().Select(s => new
            {
                s.CarId,
                s.LicensePlateNum,
                s.DriverId,
                DriverName = s.CarDriverInfo.Name,
                StatusText = GetSta_Text(s.Status)
            }).ToList<object>();

            return Json(list);
        }
        public ActionResult Delete(Guid[] CarFeeYearIds)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _carFeeYearService.List().Where(u => CarFeeYearIds.Contains(u.CarFeeYearId)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _carFeeYearService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });

        }

        public ActionResult GetByID(Guid CarFeeYearId)
        {
            var temp = _carFeeYearService.FindById(CarFeeYearId);
            return Json(new
            {
                temp.CarId,
                temp.CorpId,
                Corp = new
                {
                    text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == temp.CorpId.ToString()).DDText,
                    id = temp.CorpId
                },
                CarTemp = new
                {
                    temp.CarId,
                    temp.CarInfo?.DriverId,
                    temp.CarInfo?.LicensePlateNum,
                    DriverName = temp.CarInfo?.CarDriverInfo.Name,
                    StatusText = GetSta_Text(temp.CarInfo?.Status)
                },
                PayTime = temp.PayTime.HasValue ? temp.PayTime.Value.ToString("yyyy-MM-dd") : "",
                temp.CarInfo?.DriverId,
                temp.CorpName,
                temp.TotalFee,
                temp.Person,
                temp.Remark
            });
        }

        public ActionResult Save_CarFeeYear(CarFeeYear CarFeeYear)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            CarFeeYear.EditTime = DateTime.Now;
            CarFeeYear.EditorId = this.WorkContext.CurrentUser.UserId;
            if (CarFeeYear.CarFeeYearId == null || CarFeeYear.CarFeeYearId.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                #region 验证数据是否合法

                #endregion

                CarFeeYear.CarFeeYearId = Guid.NewGuid();
                result.IsSuccess = _carFeeYearService.Insert(CarFeeYear);
            }
            else
            {//编辑
                result.IsSuccess = _carFeeYearService.Update(CarFeeYear);
            }
            return Json(result);
        }
    }
}