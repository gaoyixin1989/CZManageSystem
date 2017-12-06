﻿using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.Administrative.BirthControl;
using CZManageSystem.Service.Administrative.BirthControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class BirthControlChildController : BaseController
    {
        // GET: BirthControlChild
        IVW_Birthcontrol_Children_DataService _birthcontrolService = new VW_Birthcontrol_Children_DataService();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, BirthControlGEBuilder objs = null)
        {
            int count = 0;
            var modelList = _birthcontrolService.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, objs, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize)as List<VW_Birthcontrol_Children_Data>;
            var list = modelList.Select(
                s => new
                {
                    s.Name,
                    s.SpouseName,
                    s.DpName,
                    s.RealName,
                    Birthday = s.Birthday.HasValue ? s.Birthday.Value.ToString("yyyy-MM-dd") : "",
                    s.PolicyPostiton
                }
                ).ToList<object>();
            return Json(new { items = list, count = count });

        }
        public ActionResult Export()
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                //var QueryBuilder = JsonConvert.DeserializeObject<BirthControlGEBuilder>(queryBuilder); ;

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _birthcontrolService.GetForPagingByCondition(this.WorkContext.CurrentUser, out count, null, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<VW_Birthcontrol_Children_Data>;
                var listResult = modelList.Select(s => new
                {
                    s.Name,
                    s.SpouseName,
                    s.DpName,
                    s.RealName,
                    Birthday = s.Birthday.HasValue ? s.Birthday.Value.ToString("yyyy-MM-dd") : "",
                    s.PolicyPostiton
                }).ToList<object>();
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.BirthControlChildren + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.BirthControlChildren);
                //设置集合变量
                designer.SetDataSource(ImportFileType.BirthControlChildren, listResult);
                //根据数据源处理生成报表内容
                designer.Process();
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