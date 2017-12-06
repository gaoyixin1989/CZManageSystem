using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.HumanResources.OverTime;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.OverTime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.OverTime
{
    public class OverTimeStaticController : BaseController
    {
        // GET: OverTimeStatic
        IOverTimeStaticService _ovettimeservice = new OverTimeStaticService();
        [SysOperation(OperationType.Browse, "访问加班统计页面")]
        public ActionResult Index()
        {
            ViewData["Year"] = DateTime.Now.ToString("yyyy-MM"); ;
            return View();
        }

        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OverTimeStaticQueryBuilder QueryBuilder = null)
        {
            int count = 0;
            if (string.IsNullOrEmpty(QueryBuilder.Year))
                QueryBuilder.Year = DateTime.Now.ToString("yyyy-MM-01");
            else
                QueryBuilder.Year = QueryBuilder.Year +"-01";
            var modelList = _ovettimeservice.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, this.WorkContext.CurrentUser, QueryBuilder).ToList();
            return Json(new { items = modelList, count = count });
        }
        [SysOperation(OperationType.Export, "导出加班统计数据")]
        public ActionResult Export(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<OverTimeStaticQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;

                if (string.IsNullOrEmpty(QueryBuilder.Year))
                    QueryBuilder.Year = DateTime.Now.ToString("yyyy-MM-01");
                else
                    QueryBuilder.Year = QueryBuilder.Year + "-01";

                var modelList = _ovettimeservice.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, this.WorkContext.CurrentUser, QueryBuilder).ToList();

                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    listResult.Add(new
                    {
                        item.EmployeeId,
                        item.RealName,
                        item.DetailDate,
                        item.FJOTTime,
                        item.GJOTTime,
                        item.AllOTTime
                    });
                }
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.OverTimeStatic + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();

                var _temptitle = "";
                var _tempusertype = "";
                if ( QueryBuilder.UserType!="" )
                {
                    if (QueryBuilder.UserType == "1")
                    {
                        _tempusertype = "合同制员工";                        
                    }
                    else if (QueryBuilder.UserType == "0")
                    {
                        _tempusertype = "社会化员工";
                    }                   
                }
                if(QueryBuilder.DpId != "")
                {
                    _temptitle = CommonFunction.getDeptNamesByIDs(QueryBuilder.DpId) + "(" + Convert.ToDateTime(QueryBuilder.Year).ToString("yyyy年MM月") + ")" + _tempusertype + "加班情况汇总表";
                }
                else
                {
                    _temptitle = Convert.ToDateTime(QueryBuilder.Year).ToString("yyyy年MM月") + _tempusertype + "加班情况汇总表";
                }
                designer.SetDataSource("Title", _temptitle);
                designer.SetDataSource("DpName", CommonFunction.getDeptNamesByIDs(this.WorkContext.CurrentUser.DpId));
                designer.SetDataSource("ReportName", this.WorkContext.CurrentUser.RealName);
                designer.SetDataSource("ReportTime", DateTime.Now.ToString("yyyy年MM月dd日"));
                //打开模板
                designer.Open(ExportTempPath.OverTimeStatic);
                //设置集合变量
                designer.SetDataSource(ImportFileType.OverTimeStatic, listResult);
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