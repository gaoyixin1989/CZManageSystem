using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.OperatingFloor.ComeBack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.OperatingFloor
{
    public class ComebackDeptController : BaseController
    {
        IComebackDeptService _comebackDeptService = new ComebackDeptService();
        IComebackTypeService _comebackTypeService = new ComebackTypeService();
        IComebackSourceService _comebackSourceService = new ComebackSourceService();
        IComebackChildService _comebackChildService = new ComebackChildService();
        // GET: ComebackDept
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
        public ActionResult TypeIndex(string BudgetDept)
        {
            ViewData["BudgetDept"] = BudgetDept;
            return View();
        }
        public ActionResult TypeEdit(Guid? ID, string BudgetDept, string type)
        {
            ViewData["ID"] = ID;
            ViewData["BudgetDept"] = BudgetDept;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult SourceChildIndex(Guid? TypeId ,string BudgetDept)
        {
            ViewData["TypeId"] = TypeId;
            ViewData["BudgetDept"] = BudgetDept;
            return View();
        }
        public ActionResult SourceChildEdit(Guid? ID, Guid? TypeId, string BudgetDept, string type)
        {
            ViewData["ID"] = ID;
            ViewData["TypeId"] = TypeId;
            ViewData["BudgetDept"] = BudgetDept;
            ViewData["type"] = type;
            return View();
        }

        #region 部门年度预算
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, ComebackQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var queryDatas = _comebackDeptService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();


            return Json(new { items = queryDatas, count = count });

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _comebackDeptService.List().Where(u => Ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _comebackDeptService.DeleteByList(objs);
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
            var list = _comebackDeptService.FindById(ID);

            return Json(new
            {
                list.ID,
                list.Amount,
                list.BudgetDept,
                list.Remark,
                list.Year

            });
        }
        public ActionResult Save(ComebackDept curObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";

            if (curObj.ID == null || curObj.ID.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {               
                curObj.ID = Guid.NewGuid();
                result.IsSuccess = _comebackDeptService.Insert(curObj);
            }
            else
            {//编辑
                result.IsSuccess = _comebackDeptService.Update(curObj);
            }

            result.Message = tip;
            return Json(result);
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult ComebackDeptDownload(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<ComebackQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _comebackDeptService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();
                var list = modelList.Select(s => new
                {
                    s.ID,
                    s.BudgetDept,
                   s.Year,
                   s.Amount,
                   s.Remark
                }).ToList<object>();


                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.ComebackDept + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.ComebackDept);
                //设置集合变量
                designer.SetDataSource(ImportFileType.ComebackDept, list);
                //根据数据源处理生成报表内容
                designer.Process();
                //designer.Save(path, FileFormatType.Excel2003);
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), Aspose.Cells.SaveType.OpenInExcel, FileFormatType.Excel2003, response);
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

        #endregion

        #region 部门年度归口项目管理
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

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult ComebackTypeDownload(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<ComebackTypeQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _comebackTypeService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<ComebackType>;
                var list = modelList.Select(s => new
                {
                    s.ID,
                    Name= s.ComebackSource.Name,
                    s.BudgetDept,
                    Year=s.ComebackSource.Year,
                    s.Amount,
                    s.Remark
                }).ToList<object>();


                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.ComebackType + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.ComebackType);
                //设置集合变量
                designer.SetDataSource(ImportFileType.ComebackType, list);
                //根据数据源处理生成报表内容
                designer.Process();
                //designer.Save(path, FileFormatType.Excel2003);
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), Aspose.Cells.SaveType.OpenInExcel, FileFormatType.Excel2003, response);
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

        #endregion

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
                BudgetDept= list.ComebackType.BudgetDept,
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

        #region 其他
        /// <summary>
        /// 获取归口项目
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetNameList()
        {
            var NameList = _comebackSourceService.List().Select(s=>new {
                s.Name,
                PID=s.ID
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
            var NameList = _comebackSourceService.List().Where(s=>s.BudgetDept== BudgetDept).Select(s => new {
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
        #endregion
    }
}