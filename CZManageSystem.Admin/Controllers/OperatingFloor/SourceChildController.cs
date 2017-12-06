using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.OperatingFloor.ComeBack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.OperatingFloor
{
    public class SourceChildController : BaseController
    {
        IComebackChildService _comebackChildService = new ComebackChildService();
        IComebackTypeService _comebackTypeService = new ComebackTypeService();
        IComebackSourceService _comebackSourceService = new ComebackSourceService();
        // GET: SourceChild
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
        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult ComebackChildDownload(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<ComebackChildQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _comebackChildService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<ComebackChild>;
                var list = modelList.Select(s => new
                {
                    s.ID,
                    Name = s.Name,
                    BudgetDept = s.ComebackType.BudgetDept,
                    Year = s.Year,
                    s.Amount,
                    s.Remark
                }).ToList<object>();


                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.ComebackChild + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.ComebackChild);
                //设置集合变量
                designer.SetDataSource(ImportFileType.ComebackChild, list);
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
        /// <summary>
        /// 获取归口小项
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetChildList(string BudgetDept)
        {
            var NameList = _comebackSourceService.List().Where(s => s.BudgetDept == BudgetDept).Select(s => new
            {
                s.Name,
                s.ID,
                TypesID = s.ComebackTypes.Where(a => a.BudgetDept == s.BudgetDept && a.PID == s.ID).FirstOrDefault().ID
            });
            return Json(new
            {
                NameList = NameList
            });
        }
    }
}