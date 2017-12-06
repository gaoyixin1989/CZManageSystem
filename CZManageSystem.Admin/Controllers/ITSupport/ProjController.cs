using CZManageSystem.Admin.Models;
using System;
using System.Web.Mvc;
using CZManageSystem.Service.ITSupport;
using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Admin.Base;
using Aspose.Cells;
using Newtonsoft.Json;

namespace CZManageSystem.Admin.Controllers.ITSupport
{
    public class ProjController : BaseController
    {
        IProjService _sysProjService = new ProjService();
        // GET: Proj
        #region 投资项目管理
        public ActionResult ProjIndex()
        {
            return View();
        }
        public ActionResult ProjWindow(string selected)
        {
            ViewData["selected"] = selected;
            return View();
        }
        public ActionResult ProjEdit(int? Id)
        {
            ViewData["Id"] = Id;
            if (Id == null)
                ViewBag.Title = "投资项目新增";
            else
                ViewBag.Title = "投资项目编辑";
            return View();
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
                var QueryBuilder = JsonConvert.DeserializeObject<ProjQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _sysProjService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
                if (count<1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Proj + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Proj);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Proj, modelList);
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

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, ProjQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _sysProjService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }

        public ActionResult GetDataByID(int Id)
        {
            var proj = _sysProjService.FindById(Id);
            return Json(new
            {
                proj.ProjName,
                proj.ProjSn
            });
        }
        public ActionResult Save(Proj proj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法

            #endregion
            proj.EditTime = DateTime.Now;
            proj.Editor = this.WorkContext.CurrentUser.UserName;
            if (proj.Id == 0)//新增
            {
                result.IsSuccess = _sysProjService.Insert(proj);
            }
            else
            {//编辑
                result.IsSuccess = _sysProjService.Update(proj);
            }
            return Json(result);
        }
        public ActionResult Delete(int[] ids)
        {
            foreach (int Id in ids)
            {
                var obj = _sysProjService.FindById(Id);
                if (obj != null && obj.Id != 0)
                    _sysProjService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }
        #endregion
    }
}