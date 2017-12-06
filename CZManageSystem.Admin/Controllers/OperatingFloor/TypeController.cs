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
    public class TypeController : BaseController
    {
        IComebackTypeService _comebackTypeService = new ComebackTypeService();
        IComebackSourceService _comebackSourceService = new ComebackSourceService();
        IComebackChildService _comebackChildService = new ComebackChildService();
        // GET: Type
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
        public ActionResult ChildIndex(Guid? TypeId, string BudgetDept)
        {
            ViewData["TypeId"] = TypeId;
            ViewData["BudgetDept"] = BudgetDept;
            return View();
        }
        public ActionResult ChildEdit(Guid? ID, Guid? TypeId, string BudgetDept, string type)
        {
            ViewData["ID"] = ID;
            ViewData["TypeId"] = TypeId;
            ViewData["BudgetDept"] = BudgetDept;
            ViewData["type"] = type;
            return View();
        }

        public ActionResult GetTypeListData(int pageIndex = 1, int pageSize = 5, ComebackTypeQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var queryDatas = _comebackTypeService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();


            return Json(new { items = queryDatas, count = count });

        }
        public ActionResult DeleteType(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _comebackTypeService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _comebackTypeService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult GetTypeByID(Guid ID)
        {
            var list = _comebackTypeService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.Amount,
                list.BudgetDept,
                list.Remark,
                list.PID

            });
        }
        public ActionResult SaveType(ComebackType curObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";

            if (curObj.ID == null || curObj.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                curObj.ID = Guid.NewGuid();
                result.IsSuccess = _comebackTypeService.Insert(curObj);
            }
            else
            {//编辑
                result.IsSuccess = _comebackTypeService.Update(curObj);
            }

            result.Message = tip;
            return Json(result);
        }

        #region 归口小项
        public ActionResult GetChildListData(int pageIndex = 1, int pageSize = 5, ComebackChildQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var queryDatas = _comebackChildService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();


            return Json(new { items = queryDatas, count = count });

        }
        public ActionResult DeleteChild(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _comebackChildService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _comebackChildService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        public ActionResult GetChildByID(Guid ID)
        {
            var list = _comebackChildService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.Name,
                list.Year,
                BudgetDept = list.ComebackType.BudgetDept,
                list.Amount,
                list.Remark,
                list.PID

            });
        }
        public ActionResult SaveChild(ComebackChild curObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";

            if (curObj.ID == null || curObj.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                curObj.ID = Guid.NewGuid();
                result.IsSuccess = _comebackChildService.Insert(curObj);
            }
            else
            {//编辑
                result.IsSuccess = _comebackChildService.Update(curObj);
            }

            result.Message = tip;
            return Json(result);
        }



        #endregion
        /// <summary>
        /// 获取归口项目
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetNameList()
        {
            var NameList = _comebackSourceService.List().Select(s => new {
                s.Name,
                PID = s.ID
            });
            return Json(new
            {
                NameList = NameList
            });
        }
        /// <summary>
        /// 获取归口小项
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetChildList(string BudgetDept)
        {
            var NameList = _comebackSourceService.List().Where(s => s.BudgetDept == BudgetDept).Select(s => new {
                s.Name,
                PID = s.ID
            });
            return Json(new
            {
                NameList = NameList
            });
        }
        /// <summary>
        /// 校验归口小项额度
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        public ActionResult CheckChildRemain(Guid PID)
        {

            var typeremain = _comebackTypeService.CheckChildRemain(PID);
            return Json(new
            {
                typeremain = typeremain
            });
        }
    }
}