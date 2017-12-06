using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Service.HumanResources.Vacation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 休假配置控制器
/// </summary>
namespace CZManageSystem.Admin.Controllers.HumanResources.Vacation
{
    public class VacationConfigController : BaseController
    {

        IHRVacationConfigService _hRVacationConfigService = new HRVacationConfigService();
        // GET: VacationConfig
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid? ID, string type)
        {
            ViewData["ID"] = ID;
            ViewData["type"] = type;
            return View();
        }

        #region 休假配置
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, VacationConfigQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var queryDatas = _hRVacationConfigService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();

           
            return Json(new { items = queryDatas, count = count});

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _hRVacationConfigService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _hRVacationConfigService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult GetByID(Guid ID)
        {
            var list = _hRVacationConfigService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.VacationName,
                list.Limit,
                list.Scope,
                list.SpanTime,
                list.Daycalmethod

            });
        }
        public ActionResult Save(HRVacationConfig vacationconfig)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            if (vacationconfig.ID == null || vacationconfig.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                vacationconfig.ID = Guid.NewGuid();
                result.IsSuccess = _hRVacationConfigService.Insert(vacationconfig);
            }
            else
            {//编辑
                result.IsSuccess = _hRVacationConfigService.Update(vacationconfig);
            }
            result.Message = tip;
            return Json(result);
        }
        /// <summary>
        /// 获取下拉框信息
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetDropList()
        {
            var VacationList =this.GetDictListByDDName("休假类型");
            return Json(new
            {
                VacationList = VacationList
            });
        }

        #endregion
    }
}