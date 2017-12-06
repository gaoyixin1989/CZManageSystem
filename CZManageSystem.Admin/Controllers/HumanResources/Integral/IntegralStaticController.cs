using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Data.Domain.HumanResources.Integral;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.Integral;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.IntegralStatic
{
    public class IntegralStaticController : BaseController
    {
        // GET: IntegralStatic
        IHRIntegralStaticService _staticservice = new HRIntegralStaticService();
        IUum_UserinfoService _uum_userinfoservice = new Uum_UserinfoService();
        [SysOperation(OperationType.Browse, "访问积分统计页面")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, IntegralStaticQueryBuilder QueryBuilder = null)
        {
            int count = 0;
            if (string.IsNullOrEmpty(QueryBuilder.Year))
                QueryBuilder.Year = DateTime.Now.Year.ToString();
            var modelList = _staticservice.GetForPagingByCondition(out count, pageIndex <= 0 ? 0 : pageIndex, pageSize, this.WorkContext.CurrentUser, QueryBuilder) as List<HRIntegralStatic>;

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _userobj = _sysUserService.FindByFeldName(u => u.EmployeeId == item.EmployeeId);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                var _uumobj = _uum_userinfoservice.FindByFeldName(u => u.employee == _userobj.EmployeeId);
                listResult.Add(new
                {
                    item.EmployeeId,
                    item.RealName,
                    YearDate= string.IsNullOrEmpty(item.YearDate) ? QueryBuilder.Year : item.YearDate,
                    item.NeedIntegral,
                    item.C_Integral,
                    item.T_Integral,
                    item.Integral,
                    item.FinishPer,
                    PosiLevel = _uumobj != null ? _uumobj.userPosiLevel : "",
                    DpName = _tmpdplist[0],
                    DpmName = _tmpdplist[1],
                    item.Alldays,
                    item.Gap
                });
            }
            return Json(new { items = listResult, count = count });
        }


        [SysOperation(OperationType.Export, "导出积分统计数据")]
        public ActionResult Export(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<IntegralStaticQueryBuilder>(queryBuilder);
                if (string.IsNullOrEmpty(QueryBuilder.Year)|| QueryBuilder.Year=="")
                    QueryBuilder.Year = DateTime.Now.Year.ToString();
                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                var modelList = _staticservice.GetForPagingByCondition( out count, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, this.WorkContext.CurrentUser, QueryBuilder) as List<HRIntegralStatic>;

                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    var _userobj = _sysUserService.FindByFeldName(u => u.EmployeeId == item.EmployeeId);
                    List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                    var _uumobj = _uum_userinfoservice.FindByFeldName(u => u.employee == _userobj.EmployeeId);
                    listResult.Add(new
                    {
                        item.EmployeeId,
                        item.RealName,
                        YearDate = string.IsNullOrEmpty(item.YearDate) ? QueryBuilder.Year : item.YearDate,
                        item.NeedIntegral,
                        item.C_Integral,
                        item.T_Integral,
                        item.Integral,
                        PosiLevel = _uumobj != null ? _uumobj.userPosiLevel : "",
                        FinishPer = Math.Round(Convert.ToDouble(item.FinishPer),2).ToString() +'%',
                        DpName = _tmpdplist[0],
                        DpmName = _tmpdplist[1],
                        //item.DpName,
                        item.Alldays,
                        item.Gap
                    });
                }
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.IntegralStatic + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.IntegralStatic);
                //设置集合变量
                designer.SetDataSource(ImportFileType.IntegralStatic, listResult);
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