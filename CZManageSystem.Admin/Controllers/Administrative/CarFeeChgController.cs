using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarFeeChgController : BaseController
    {
        // GET: CarFeeChg
        #region 成员
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        ICarFeeChgService carFeeChgService = new CarFeeChgService();
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="Name">会议室名称</param>
        /// <param name="Address">会议室地点</param>
        /// <param name="Code">会议室编号</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, string Person = null)
        {
            // List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);

            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            int count = 0;

            var modelList = carFeeChgService.GetForPaging(out count, new { Person }, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<CarFeeChg>;
            var list = modelList.Select(s => new
            {
               
                s.CarFeeChgId,
                s.EditorId,
                s.EditTime,
                s.CorpId,
                CorpId_text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == s.CorpId.ToString()).DDText,
                s.CorpName,
                s.CarId,
                LicensePlateNum = s.CarInfo?.LicensePlateNum,
                s.PayTime,
                s.RoadLast,
                s.RoadThis,
                s.RoadCount,
                s.OilCount,
                s.OilFee,
                s.OilPrice,
                s.FixFee,
                s.RoadFee,
                s.LiveFee,
                s.EatFee,
                s.OtherFee,
                s.TotalFee,
                s.Person,
                s.Remark
            }).ToList<object>();
            return Json(new { items = list, count = count });
        }
        public ActionResult Edit(Guid? Id)
        {
            //有值=》编辑状态
            ViewData["Id"] = Id;
            return View();
        }
        public ActionResult GetDataByID(string id, string type = null)
        {
            return View();
        }
        public ActionResult Delete(Guid [] ids)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var models = carFeeChgService.List().Where(f => ids.Contains(f.CarFeeChgId)).ToList();
            if (models.Count <= 0)
            {
                result.Message = "该问题不存在！";
                return Json(result);
            }
            if (carFeeChgService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }

        public ActionResult CarFeeChgInfo(CarFeeChg cfc)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (cfc.CarFeeChgId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = carFeeChgService.Update(cfc);
            }
            else
            {
                cfc.CarFeeChgId = Guid.NewGuid();
                cfc.EditorId = this.WorkContext.CurrentUser.UserId;
                cfc.EditTime = DateTime.Now;
                result.IsSuccess = carFeeChgService.Insert(cfc);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        public ActionResult CarFeeChgDataByID(Guid id)
        {
            var temp = carFeeChgService.FindById(id);
            return Json(new
            {
                temp.CarFeeChgId,
                temp.EditorId,
                temp.EditTime,
                temp.CorpId,
                CorpId_text = new
                {
                    text = _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == temp.CorpId.ToString()).DDText,
                    id = temp.CorpId
                },
                temp.CorpName,
                temp.CarId,

                CarTemp = new
                {
                    temp.CarId,
                    temp.CarInfo?.LicensePlateNum,
                    DriverName = temp.CarInfo?.CarDriverInfo.Name,
                    StatusText = GetSta_Text(temp.CarInfo?.Status)
                },
                PayTime = temp.PayTime.HasValue ? temp.PayTime.Value.ToString("yyyy-MM-dd") : "",
                temp.RoadLast,
                temp.RoadThis,
                temp.RoadCount,
                temp.OilCount,
                temp.OilFee,
                temp.OilPrice,
                temp.FixFee,
                temp.RoadFee,
                temp.LiveFee,
                temp.EatFee,
                temp.OtherFee,
                temp.TotalFee,
                temp.Person,
                temp.Remark
            });
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
    }
}