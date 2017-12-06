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
    public class CarFeeFixController : BaseController
    {
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        ICarFeeFixService _carFeeFixService = new CarFeeFixService();
        ICarInfoService _carInfoService = new CarInfoService();
        // GET: CarFeeFix
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit(Guid? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }

        public ActionResult CarFeeFixInfo(CarFeeFix carf)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (carf.CarFeeFixId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = _carFeeFixService.Update(carf);
            }
            else
            {
                carf.CarFeeFixId = Guid.NewGuid();
                carf.EditorId = this.WorkContext.CurrentUser.UserId;
                carf.EditTime = DateTime.Now;
                result.IsSuccess = _carFeeFixService.Insert(carf);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
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
                DriverName = s.CarDriverInfo.Name,
                StatusText = GetSta_Text(s.Status)
            }).ToList<object>();

            return Json(list);
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

        public ActionResult CarFeeFixGetListData(int pageIndex = 1, int pageSize = 5, DriverFilesQueryBuilder queryBuilder = null)
        {
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            int count = 0;

            var modelList = _carFeeFixService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<CarFeeFix>;
            var list = modelList.Select(s => new
            {
                s.CarFeeFixId,
                s.EditorId,
                s.EditTime,
                s.CorpId,
                CorpId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString()).DDText,
                s.CarId,
                LicensePlateNum = s.CarInfo?.LicensePlateNum,
                s.PayTime,
                s.FolicyFee,
                s.TaxFee,
                s.RoadFee,
                s.OtherFee,
                s.TotalFee,
                s.StartTime,
                s.EndTime,
                s.Person,
                s.Remark
            }).ToList<object>();
            return Json(new { items = list, count = count });

        }

        public ActionResult CarFeeFixDelete(Guid[] DriverId)
        {
            foreach (Guid id in DriverId)
            {
                var obj = _carFeeFixService.FindById(id);
                if (obj != null)
                {
                    _carFeeFixService.Delete(obj);
                }
            }
            return Json(new { status = 0, message = "成功" });
        }


        public ActionResult CarFeeFixDataByID(Guid id)
        {
            var temp = _carFeeFixService.FindById(id);
            return Json(new
            {
                temp.CarFeeFixId,
                temp.EditorId,
                temp.EditTime,
                temp.CorpId,
                CorpId_text = new
                {
                    text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == temp.CorpId.ToString()).DDText,
                    id = temp.CorpId
                },
                temp.CarId,

                CarTemp = new
                {
                    temp.CarId,
                    temp.CarInfo?.LicensePlateNum,
                    DriverName = temp.CarInfo?.CarDriverInfo.Name,
                    StatusText = GetSta_Text(temp.CarInfo?.Status)
                },
                PayTime = temp.PayTime.HasValue ? temp.PayTime.Value.ToString("yyyy-MM-dd") : "",
                temp.FolicyFee,
                temp.TaxFee,
                temp.RoadFee,
                temp.OtherFee,
                temp.TotalFee,
                StartTime = temp.StartTime.HasValue ? temp.StartTime.Value.ToString("yyyy-MM-dd") : "",
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
                //var modelList = _driverFilesService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (CarDriverInfo)u).ToList();
                var modelList = _carFeeFixService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<CarFeeFix>;
                var listResult = modelList.Select(s => new
                {
                    s.CarFeeFixId,
                    s.EditorId,
                    EditTime = Convert.ToDateTime(s.EditTime).ToString("yyyy-MM-dd"),
                    s.CorpId,
                    CorpId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString()).DDText,
                    s.CarId,
                    LicensePlateNum = s.CarInfo?.LicensePlateNum,
                    PayTime = Convert.ToDateTime(s.PayTime).ToString("yyyy-MM-dd"),
                    s.FolicyFee,
                    s.TaxFee,
                    s.RoadFee,
                    s.OtherFee,
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
                var fileToSaveName = SaveName.CarFeeFix + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.CarFeeFix);
                //设置集合变量
                designer.SetDataSource(ImportFileType.CarFeeFix, listResult);
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