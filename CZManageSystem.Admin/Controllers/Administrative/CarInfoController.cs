using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Service.Administrative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using CZManageSystem.Admin.Base;
using Aspose.Cells;
using CZManageSystem.Service.SysManger;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarInfoController : BaseController
    {
        ICarInfoService _carInfoService = new CarInfoService();
        IDriverFilesService _driverFilesService = new DriverFilesService();
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        // GET: CarInfo
        public ActionResult CarInfoList()
        {
            return View();
        }
        public ActionResult CarInfoEdit(Guid? CarId,string type)
        {
            ViewData["CarId"] = CarId;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult CarStatus()
        {
            return View();
        }

        //前台页面去掉编号
        /// <summary>
        /// 获取汽车编号
        /// </summary>
        /// <returns></returns>
        public ActionResult getAutoSn()
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                string MaxSn = _carInfoService.List().Where(c => c.SN.StartsWith("CL")).Max(c => c.SN);
                if (!String.IsNullOrEmpty(MaxSn))
                {
                    MaxSn = MaxSn.Substring(3);
                    MaxSn = "CL-" + (Convert.ToInt32(MaxSn) + 1).ToString().PadLeft(3, '0');
                    result.IsSuccess = true;
                    result.data = MaxSn;
                }
                else
                {
                    result.data = "CL-001";
                    result.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.data = "CL-001";
            }
            return Json(result);
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, CarInfoQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _carInfoService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize)as List<CarInfo>;
            var list = modelList.Select(s => new
            {
                s.CarId,
                s.CorpId,
                CorpName = _dataDictionaryService.FindByFeldName(d=>d.DDName== DataDic.CorpName && d.DDValue==s.CorpId.ToString()).DDText,
                s.SN,
                s.LicensePlateNum,
                s.CarBrand,
                s.CarModel,
                s.CarType,
                s.CarTonnage,
                BuyDate = s.BuyDate.HasValue ? s.BuyDate.Value.ToString("yyyy-MM-dd") : "",
                s.CarPrice,
                PolicyTime1 = s.PolicyTime1.HasValue ? s.PolicyTime1.Value.ToString("yyyy-MM-dd") : "",
                PolicyTime2 = s.PolicyTime2.HasValue ? s.PolicyTime2.Value.ToString("yyyy-MM-dd") : "",
                //DriverName = _driverFilesService.FindById(s.DriverId).Name,
                DriverName = s.CarDriverInfo?.Name,
                StatusText = GetSta_Text(s.Status)

            }).ToList<object>();
            return Json(new { items = list, count = count });

        }

        /// <summary>
        /// 获取司机
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetDrive()
        {
          var CorpList=  _driverFilesService.List();
            List<object> listResult = new List<object>();
            foreach (var item in CorpList)
            {
                listResult.Add(new
                {
                    Name = item.Name,
                    DriverId = item.DriverId
                });
            }
            return Json(listResult);
        }
        /// <summary>
        /// 获取下拉框信息
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetDropList()
        {
            var CarBrandList = GetDictListByDDName("车辆品牌");
            var CarTypeList = GetDictListByDDName("车辆类型");
            var CorpIdList = GetDictListByDDName(DataDic.CorpName);
            var CarTonnageList = GetDictListByDDName("吨位/人数");
            return Json(new
            {
                CarBrandList = CarBrandList,
                CarTypeList = CarTypeList,
                CorpIdList = CorpIdList,
                CarTonnageList
            });
        }
        public ActionResult Delete(Guid[] CarIds)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _carInfoService.List().Where(u => CarIds.Contains(u.CarId)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _carInfoService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult CarInfoGetByID(Guid CarId)
        {
            var carInfo = _carInfoService.FindById(CarId);
           
            return Json(new
            {
                carInfo.CarId,
                carInfo.CorpId,
                Corp =new {
                    text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == carInfo.CorpId.ToString()).DDText,
                    id = carInfo.CorpId
                },
               // CorpName = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == carInfo.CorpId.ToString()).DDText,
                carInfo.SN,
                carInfo.LicensePlateNum,
                carInfo.CarBrand,
                carInfo.CarModel,
                carInfo.CarEngine,
                carInfo.CarNum,
                carInfo.CarType,
                carInfo.CarTonnage,
                carInfo.DeptName,
                BuyDate = carInfo.BuyDate.HasValue ? carInfo.BuyDate.Value.ToString("yyyy-MM-dd") : "",
                carInfo.CarPrice,
                carInfo.CarLimit,
                carInfo.Depre,
                RentTime1 = carInfo.RentTime1.HasValue ? carInfo.RentTime1.Value.ToString("yyyy-MM-dd") : "",
                RentTime2 = carInfo.RentTime2.HasValue ? carInfo.RentTime2.Value.ToString("yyyy-MM-dd") : "",
                PolicyTime1 = carInfo.PolicyTime1.HasValue ? carInfo.PolicyTime1.Value.ToString("yyyy-MM-dd") : "",
                PolicyTime2 = carInfo.PolicyTime2.HasValue ? carInfo.PolicyTime2.Value.ToString("yyyy-MM-dd") : "",
                AnnualTime1 = carInfo.AnnualTime1.HasValue ? carInfo.AnnualTime1.Value.ToString("yyyy-MM-dd") : "",
                AnnualTime2 = carInfo.AnnualTime2.HasValue ? carInfo.AnnualTime2.Value.ToString("yyyy-MM-dd") : "",
                carInfo.DriverId,
                Driver = new {
                    DriverId=carInfo.DriverId,
                    Name = carInfo.CarDriverInfo?.Name
                },
                //carInfo.DriverId,
                //DriverName = carInfo.carDriverInfo?.Name,
                carInfo.Status,
                carInfo.Remark
            });
        }


        public string  getSn()
        {
            string result = "";
            try
            {
                string MaxSn = _carInfoService.List().Where(c => c.SN.StartsWith("CL")).Max(c => c.SN);
                if (!String.IsNullOrEmpty(MaxSn))
                {
                    MaxSn = MaxSn.Substring(3);
                    MaxSn = "CL-" + (Convert.ToInt32(MaxSn) + 1).ToString().PadLeft(3, '0');
                    result = MaxSn;
                }
                else
                {
                    result = "CL-001";
                }

            }
            catch (Exception ex)
            {
                result = "CL-001";
            }
            return result;
        }
        public ActionResult Save_CarInfo(CarInfo carInfo)
        {
            SystemResult result = new SystemResult() { IsSuccess = false};
            string tip = "";
            carInfo.EditTime = DateTime.Now;
            carInfo.EditorId = this.WorkContext.CurrentUser.UserId;
            carInfo.SN = getSn();
           
            if (carInfo.CarId == null || carInfo.CarId.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                bool isValid = false;//是否验证通过
                #region 验证数据是否合法
                var sn = _carInfoService.List().Where(c => c.SN == carInfo.SN).ToList();
                var LicensePlateNum = _carInfoService.List().Where(c => c.LicensePlateNum == carInfo.LicensePlateNum).ToList();
                if (sn.Count() > 0)
                {
                    tip = "车辆编号不能重复";
                }
                else if (LicensePlateNum.Count() > 0)
                {
                    tip = "车牌号不能重复";
                }
                else
                {
                    isValid = true;
                }
                #endregion

                if (isValid)
                {                  
                carInfo.CarId = Guid.NewGuid();
                result.IsSuccess = _carInfoService.Insert(carInfo);
                  
                }
            }
            else
            {//编辑
                result.IsSuccess = _carInfoService.Update(carInfo);
            }
            result.Message = tip;
            return Json(result);
        }
        public string GetSta_Text(int? sta)
        {
            string text = "";
            switch (sta)
            {
                case 0:
                    text= "空闲";
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
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<CarInfoQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _carInfoService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<CarInfo>;
                var list = modelList.Select(s => new
                {
                    s.CarId,
                    s.CorpId,
                    CorpName = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString()).DDText,
                    s.SN,
                    s.LicensePlateNum,
                    s.CarBrand,
                    s.CarModel,
                    s.CarType,
                    s.CarTonnage,
                    BuyDate = s.BuyDate.HasValue ? s.BuyDate.Value.ToString("yyyy-MM-dd") : "",
                    s.CarPrice,
                    DriverName = s.CarDriverInfo?.Name,
                    StatusText = GetSta_Text(s.Status)

                }).ToList<object>();
                  if (list.Count <= 0)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.CarInfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.CarInfo);
                //设置集合变量
                designer.SetDataSource(ImportFileType.CarInfo, list);
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
    }
}