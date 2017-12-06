using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Service.Composite;
using CZManageSystem.Service.ITSupport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Uploadify
{
    public class ExportController : BaseController
    {
        static string _saveName { get; set; }
        static string _tempPath { get; set; }
        static readonly string tempPath = "~/Template/";
        IVoteQuestionTempService voteQuestionTempService = new VoteQuestionTempService();
        IOGSMService OGSMTempService = new OGSMService();
        IOGSMMonthService OGSMMonthTempService = new OGSMMonthService();
        IConsumable_SporadicDetailService _consumable_SporadicDetailService = new Consumable_SporadicDetailService();
        // GET: Uploadify
        public ActionResult Index(string type = null, string data = null)
        {
            ViewData["type"] = type; //类型
            ViewData["data"] = data; //参数
            Set(type);
            ViewData["saveName"] = _saveName;
            return View();
        }
        public ActionResult Message()
        { 
            return View();
        }
        public ActionResult ExportToFiles(string type = null)
        {
            dynamic result = new SystemResult() { IsSuccess = false, Message = "导入失败" };

            if (Request.Files.Count <= 0)
                return Json(result);
            HttpPostedFileBase file = Request.Files[0];
            switch (type)//分支处理。
            {
                case ImportFileType.Question:
                    result = voteQuestionTempService.ImportQuestion(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.OGSMBase:
                    result = OGSMTempService.ImportOGSMBase(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.OGSMMonth:
                    result = OGSMMonthTempService.ImportOGSMMonth(file.InputStream, WorkContext.CurrentUser);
                    break;
                case ImportFileType.Sporadic:
                    var data = Request.Form["data"];
                    result = _consumable_SporadicDetailService.ImportSporadicDetail(file.InputStream, WorkContext.CurrentUser, Guid.Parse(data));
                    break;
                default:
                    break;
            }
            return Json(result);
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <returns></returns>
        public ActionResult Download(string type)
        {
            Set(type);
            var path = Server.MapPath(_tempPath);//服务器中的文件路径
            return File(path, "application/vnd.ms-excel	application/x-excel", Url.Encode(_saveName));
        }
        
        #region 方法
        void Set(string type)
        {
            switch (type)//分支处理。
            {
                case ImportFileType.Question:
                    _tempPath = tempPath + "Vote/QuestionTemp.xls";//路径
                    _saveName = "题目模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OGSMBase:
                    _tempPath = tempPath + "OGSM/OGSM.xls";//路径
                    _saveName = "基站基础数据模板.xls";//下载保存时默认名 
                    break;
                case ImportFileType.OGSMMonth:
                    _tempPath = tempPath + "OGSM/OGSMMonth.xls";//路径
                    _saveName = "基站月度数据模板.xls";//下载保存时默认名 
                    break;
                default:
                    break;
            }
        }
        private static void Export<T>(IEnumerable<T> data, HttpResponse response)
        {
            Workbook workbook = new Workbook();
            Worksheet sheet = (Worksheet)workbook.Worksheets[0];

            PropertyInfo[] ps = typeof(T).GetProperties();
            var colIndex = "A";

            foreach (var p in ps)
            {

                sheet.Cells[colIndex + 1].PutValue(p.Name);
                int i = 2;
                foreach (var d in data)
                {
                    sheet.Cells[colIndex + i].PutValue(p.GetValue(d, null));
                    i++;
                }

                colIndex = ((char)(colIndex[0] + 1)).ToString();
            }

            response.Clear();
            response.Buffer = true;
            response.Charset = "utf-8";
            response.AppendHeader("Content-Disposition", "attachment;filename=xxx.xls");
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "application/ms-excel";
            response.BinaryWrite(workbook.SaveToStream().ToArray());
            response.End();
        }
        public static bool ExportExcel(List<DataTable> dataTables, string fileName, System.Web.HttpResponse response)
        {
            try
            {
                WorkbookDesigner designer = new WorkbookDesigner();

                //打开模板
                designer.Open(_tempPath);

                //设置数据源 
                foreach (var item in dataTables)
                {
                    designer.SetDataSource(item);
                }

                //设置集合变量
                //designer.SetDataSource("MVP", MVP);
                 
                designer.Process();
                designer.Save(fileName, SaveType.OpenInExcel, FileFormatType.Excel2007Xlsx, response);//保存格式
                response.Flush();
                response.Close();
                response.End();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        #endregion
    }
}