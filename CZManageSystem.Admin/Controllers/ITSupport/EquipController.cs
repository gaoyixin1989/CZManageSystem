using CZManageSystem.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CZManageSystem.Service.ITSupport;
using CZManageSystem.Data.Domain.ITSupport;
using Newtonsoft.Json;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Admin.Base;
using Aspose.Cells;

namespace CZManageSystem.Admin.Controllers.ITSupport
{
    public class EquipController : BaseController
    {
        IEquipService _sysEquipService = new EquipService();
        // GET: Equip
        #region 设备信息管理
        public ActionResult EquipIndex()
        {
            return View();
        }
        public ActionResult EquipWindow(string selected)
        {
            ViewData["selected"] = selected; 
            return View();
        }
        public ActionResult EquipEdit(int? Id)
        {
            ViewData["Id"] = Id;
            if (Id == null)
                ViewBag.Title = "设备信息新增";
            else
                ViewBag.Title = "设备信息编辑";
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, EquipQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _sysEquipService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize);
            return Json(new { items = modelList, count = count });

        }

        public ActionResult GetDataByID(int Id)
        {
            var equip = _sysEquipService.FindById(Id);
            return Json(new
            {
                equip.EquipClass
            });
        }
        public ActionResult Save(Equip equip)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法

            #endregion
            equip.Edittime = DateTime.Now.ToShortDateString();
            equip.Editor = this.WorkContext.CurrentUser.UserName;
            if (equip.Id == 0)//新增
            {
                result.IsSuccess = _sysEquipService.Insert(equip);
            }
            else
            {//编辑
                result.IsSuccess = _sysEquipService.Update(equip);
            }
            return Json(result);
        }
        public ActionResult Delete(int[] ids)
        {
            foreach (int Id in ids)
            {
                var obj = _sysEquipService.FindById(Id);
                if (obj != null && obj.Id != 0)
                    _sysEquipService.Delete(obj);
            }

            return Json(new { status = 0, message = "成功" });
        }

        /// <summary>
        /// 下载会议室资料信息数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<EquipQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _sysEquipService.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize).ToList();
                if (modelList.Count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Equip + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Equip);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Equip, modelList);
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
        #endregion
    }
}