using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.OperatingFloor.ComeBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.OperatingFloor
{
    public class ComebackSourceController : BaseController
    {
        IComebackSourceService _comebackSourceService = new ComebackSourceService();
        // GET: ComebackSource
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

        #region 归口项目管理
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, ComebackQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var queryDatas = _comebackSourceService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();


            return Json(new { items = queryDatas, count = count });

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _comebackSourceService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _comebackSourceService.DeleteByList(objs);
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
            var list = _comebackSourceService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.Amount,
                list.BudgetDept,
                list.Name,
                list.Remark,
                list.Year

            });
        }
        public ActionResult Save(ComebackSource curObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
           
            if (curObj.ID == null || curObj.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                if (_comebackSourceService.Contains (s => s.Name == curObj.Name))
                {
                    tip = "该年度已存在相同项目";
                }
                else
                {
                    curObj.ID = Guid.NewGuid();
                    result.IsSuccess = _comebackSourceService.Insert(curObj);
                }

            }
            else
            {//编辑
                result.IsSuccess = _comebackSourceService.Update(curObj);
            }

            result.Message = tip;
            return Json(result);
        }
        /// <summary>
        /// 获取下拉框信息
        /// </summary>
        /// <param name="CorpList"></param>
        //public ActionResult GetDropList()
        //{
        //    var VacationList = this.GetDictListByDDName("休假类型");
        //    return Json(new
        //    {
        //        VacationList = VacationList
        //    });
        //}

        #endregion
    }
}