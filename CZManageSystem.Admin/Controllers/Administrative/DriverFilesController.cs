using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.Administrative;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.Administrative
{
    public class DriverFilesController : BaseController
    {
        IDriverFilesService _driverFilesService = new DriverFilesService();

     
        // GET: DriverFiles
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(Guid? Id)
        {
            ViewData["Id"] = Id;
            
            return View(); 
        }
        public ActionResult DriverFilesInfo(CarDriverInfo Card)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (Card.DriverId != Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = _driverFilesService.Update(Card);
            }
            else
            {
                Card.DriverId= Guid.NewGuid();
                Card.EditorId= this.WorkContext.CurrentUser.UserId;
                Card.EditTime = DateTime.Now;
                result.IsSuccess = _driverFilesService.Insert(Card);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }

        public ActionResult DriverFilesGetListData(int pageIndex = 1, int pageSize = 5, DriverFilesQueryBuilder queryBuilder = null)
        {
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            int count = 0;

            var modelList = _driverFilesService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.DriverId,
                    item.EditorId,
                    item.EditTime,
                    item.CorpId,
                    CorpId_text = getDictText(CorpList, (item.CorpId ?? -1).ToString()),
                    item.SN,
                    item.DeptName,
                    item.Mobile,
                    item.Name,
                    item.CarAge,
                    item.Birthday,
                    item.Remark
                });
            }

            return Json(new { items = listResult, count = count });
        }

        private string getDictText(List<DataDictionary> CorpList, string DDValue)
        {
            string strResult = "";
            var temp = CorpList.Where(u => u.DDValue == DDValue).FirstOrDefault();
            if (temp != null)
                strResult = temp.DDText;
            return strResult;
        }

        public ActionResult DriverFilesDelete(Guid[] DriverId)
        {
            foreach (Guid id in DriverId)
            {
                var obj = _driverFilesService.FindById(id);
                if (obj != null)
                {
                    _driverFilesService.Delete(obj);
                }
            }
            return Json(new { status = 0, message = "成功" });
        }


        public ActionResult DriverFilesDataByID(Guid id)
        {
            List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);
            var item = _driverFilesService.FindById(id);

            return Json(new
            {
                item.DriverId,
                item.EditorId,
                item.EditTime,
                item.CorpId,
                CorpId_text= getDictText(CorpList, (item.CorpId ?? -1).ToString()),
                item.SN,
                //DeptName_text = CommonFunction.getDeptNamesByIDs(item.DeptName),
                item.DeptName,
                item.Mobile,
                //Name_text = CommonFunction.getUserRealNamesByIDs(item.Name),
                item.Name,
                CarAge = item.CarAge.HasValue ? item.CarAge.Value.ToString("yyyy-MM-dd") : "",
                Birthday = item.Birthday.HasValue ? item.Birthday.Value.ToString("yyyy-MM-dd") : "",
                item.Remark

            });

          
        }
        /// <summary>
        /// 获取司机编号
        /// </summary>
        /// <returns></returns>
       public ActionResult getAutoSn()
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            try
            {
                string MaxSn = _driverFilesService.List().Where(d => d.SN.StartsWith("SJ")).Max(d => d.SN);
                if (!String.IsNullOrEmpty(MaxSn))
                {
                    MaxSn = MaxSn.Substring(3);
                    MaxSn = "SJ-" + (Convert.ToInt32(MaxSn) + 1).ToString().PadLeft(3, '0');
                    result.IsSuccess = true;
                    result.data = MaxSn;
                }
                else
                {
                    result.data= "SJ-001";
                    result.IsSuccess = true;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.data = "SJ-001";
            }
            return Json(result);
        }

        /// <summary>
        /// 导出司机档案数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<CarDriverInfo>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;
                List<DataDictionary> CorpList = GetDictListByDDName(DataDic.CorpName);

                int count = 0;
                var modelList = _driverFilesService.GetForPaging(out count, QueryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize) as List<CarDriverInfo>;
                var listResult = modelList.Select(item => new
                {
                    item.DriverId,
                    item.EditorId,
                    item.EditTime,
                    item.CorpId,
                    CorpId_text = getDictText(CorpList, (item.CorpId ?? -1).ToString()),
                    item.SN,
                    //DeptName_text = CommonFunction.getDeptNamesByIDs(item.DeptName),
                    item.DeptName,
                    item.Mobile,
                    //Name_text = CommonFunction.getUserRealNamesByIDs(item.Name),
                    item.Name,
                    //CarAge = item.CarAge.HasValue ? item.CarAge.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    //Birthday = item.Birthday.HasValue ? item.Birthday.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    CarAge=Convert.ToDateTime(item.CarAge).ToString("yyyy-MM-dd HH:mm:ss"),
                    Birthday= Convert.ToDateTime(item.Birthday).ToString("yyyy-MM-dd HH:mm:ss"),
                    item.Remark

                }).ToList<object>();


                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.DriverFiles + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.DriverFiles);
                //设置集合变量
                designer.SetDataSource(ImportFileType.DriverFiles, listResult);
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