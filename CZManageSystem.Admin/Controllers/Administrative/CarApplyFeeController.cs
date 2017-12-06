using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative.VehicleManages;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class CarApplyFeeController : BaseController
    {
        IDataDictionaryService _dataDictionaryService = new DataDictionaryService();
        ICarApplyFeeService _carApplyFeeService = new CarApplyFeeService();
        // GET: CarApplyFee
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid? Id)
        {
            ViewData["Id"] = Id;
            return View();
        }
        public ActionResult CarApplyFeeGetListData(int pageIndex = 1, int pageSize = 5, DriverFilesQueryBuilder queryBuilder = null)
        {
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            int count = 0;

            //var modelList = _carApplyFeeService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<CarApplyFee>;
            var source = _carApplyFeeService.List().Where(w => 1 == 1);
           
            if (queryBuilder.CorpId!=null)
            {
                source = source.Where(w => w.CarApply.CorpId == queryBuilder.CorpId);
            }
            var pageList = new PagedList<CarApplyFee>().QueryPagedList(source.OrderByDescending(o => o.ApplyFeeId), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count).ToList();
            var list = pageList.Select(s => new
            {
                s.ApplyFeeId,
                s.CarApplyId,
                s.ApplyTitle,
                CorpId_text = GetCorpName( s.CarApply?.CorpId.ToString()),
                s.ApplySn,
                s.CarId,
                s.BalUser,
                s.BalTime,
                s.KmNum1,
                s.KmNum2,
                s.KmCount,
                s.BalCount,
                s.BalTotal,
                s.BalRemark
            }).ToList<object>();
            return Json(new { items = list, count = count });
        }
        string  GetCorpName(string  id)
        {
            return _dataDictionaryService.FindByFeldName(d => d.DDName == DataDic.CorpName && d.DDValue == id).DDText;

        }

        public ActionResult CarApplyFeeDataByID(Guid id)
        {
            var temp = _carApplyFeeService.FindById(id);
            return Json(new
            {
                temp.ApplyFeeId,
                temp.CarApplyId,
                temp.ApplyTitle,
                temp.ApplySn,
                BalTime = temp.BalTime.HasValue ? temp.BalTime.Value.ToString("yyyy-MM-dd") : "",
                temp.CarId,
                temp.BalUser,
                temp.KmNum1,
                temp.KmNum2,
                temp.KmCount,
                temp.BalCount,
                temp.BalTotal,
                temp.BalRemark
                
            });
        }

        public ActionResult CarApplyFeeInfo(CarApplyFee caf)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (caf.ApplyFeeId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                caf.BalUser= this.WorkContext.CurrentUser.UserName;
                caf.BalTime= DateTime.Now;
                result.IsSuccess = _carApplyFeeService.Update(caf);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
    }
    
}