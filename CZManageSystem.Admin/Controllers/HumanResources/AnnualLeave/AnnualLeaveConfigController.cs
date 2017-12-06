using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.AnnualLeave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.AnnualLeave
{
    public class AnnualLeaveConfigController : BaseController
    {
        // GET: AnnualLeaveConfig
        IHRVacationAnnualLeaveConfigService configservice = new HRVacationAnnualLeaveConfigService();
        [SysOperation(OperationType.Browse, "访问年休假配置页面")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid? id)
        {
            ViewData["Id"] = id;
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HRVacationAnnualLeaveConfigQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = configservice.GetForPagingByCondition(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).ToList();
            //List<object> listResult = new List<object>();
            //foreach (var item in modelList)
            //{
            //    listResult.Add(new
            //    {
            //        Id=item.ID,
            //        item.LimitMonth,
            //        item.Annual
            //    });
            //}
            return Json(new { items = modelList, count = count });
        }
        public ActionResult GetDataByID(Guid id)
        {
            var item = configservice.FindById(id);
            return Json(new
            {
                item.ID,
                item.LimitMonth,
                item.Annual,
                item.SpanTime
            });
        }
        [SysOperation(OperationType.Delete, "删除年休假设置数据")]
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = configservice.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = configservice.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                IsSuccess = successCount > 0 ? true : false,
                SuccessCount = successCount
            });
        }
        [SysOperation(OperationType.Save, "保存年休假配置数据")]
        public ActionResult Save(HRVacationAnnualLeaveConfig dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            HRVacationAnnualLeaveConfig obj = new HRVacationAnnualLeaveConfig();
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过            
            var list = configservice.List().Where(u => dataObj.Annual == u.Annual && dataObj.ID != u.ID).ToList();
            if (list.Count > 0)
                tip = "该年数据已建立！";
            if (tip == "")
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion
            if (dataObj.ID == Guid.Empty)//新增
            {
                dataObj.ID= Guid.NewGuid();
                result.IsSuccess = configservice.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = configservice.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }
    }
}