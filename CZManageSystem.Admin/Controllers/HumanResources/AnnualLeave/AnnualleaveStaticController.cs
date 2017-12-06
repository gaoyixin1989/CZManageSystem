using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.AnnualLeave;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.AnnualLeave
{
    public class AnnualleaveStaticController : BaseController
    {
        // GET: AnnualleaveStatic
        ISysUserService _userService = new SysUserService();
        IHRAnnualleaveStaticService _staticservice = new HRAnnualleaveStaticService();
        [SysOperation(OperationType.Browse, "访问年休假统计页面")]
        public ActionResult Index()
        {
            ViewData["Year"] = DateTime.Now.ToString("yyyy");
            return View();
        }


        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HRAnnualLeaveStaticQueryBuilder QueryBuilder = null)
        {
            int count = 0;
            if (string.IsNullOrEmpty(QueryBuilder.Year))
                QueryBuilder.Year = DateTime.Now.ToString("yyyy");
            else
                QueryBuilder.Year = QueryBuilder.Year;
            var modelList = _staticservice.GetForPagingByCondition(out count, this.WorkContext.CurrentUser, pageIndex , pageSize, QueryBuilder).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                var _userobj = _userService.FindByFeldName(u => u.EmployeeId == item.EmployeeId);
                List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                listResult.Add(new
                {
                    item.Years,
                    item.EmployeeId,
                    item.RealName,
                    item.SpendDays,
                    item.VDays,
                    item.ThisYearLeftdays,
                    item.ThisYearSpendDays,
                    item.FdYearVDays,
                    item.Leftdays,
                    DpName = _tmpdplist[0],//CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                    DpmName = _tmpdplist[1],
                    item.UseDate
                });
            }
            return Json(new { items = listResult, count = count });
        }

        [SysOperation(OperationType.Export, "导出年休假统计数据")]
        public ActionResult Export(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<HRAnnualLeaveStaticQueryBuilder>(queryBuilder);

                int pageIndex = 1;
                int pageSize = int.MaxValue;
                int count = 0;
                if (string.IsNullOrEmpty(QueryBuilder.Year))
                    QueryBuilder.Year = DateTime.Now.ToString("yyyy");
                else
                    QueryBuilder.Year = QueryBuilder.Year;


                var modelList = _staticservice.GetForPagingByCondition(out count, this.WorkContext.CurrentUser, pageIndex, pageSize, QueryBuilder).ToList();

                List<object> listResult = new List<object>();
                foreach (var item in modelList)
                {
                    var _userobj = _userService.FindByFeldName(u => u.EmployeeId == item.EmployeeId);
                    List<string> _tmpdplist = CommonFunction.GetDepartMent(_userobj.DpId);
                    listResult.Add(new
                    {
                        item.Years,
                        item.EmployeeId,
                        item.RealName,
                        item.SpendDays,
                        item.VDays,
                        item.ThisYearLeftdays,
                        item.ThisYearSpendDays,
                        item.FdYearVDays,
                        item.Leftdays,
                        DpName = _tmpdplist[0],//CommonFunction.getDeptNamesByIDs(_userobj.DpId),
                        DpmName = _tmpdplist[1],
                        item.UseDate
                    });
                }
                if (listResult.Count < 1)
                    return View("../Export/Message");
                #region Excel部分

                string fileToSaveName = SaveName.Annualleave + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();

                var _temptitle = "";
                _temptitle = QueryBuilder.Year+ "年年休假统计表";
                designer.SetDataSource("Title", _temptitle);
                //打开模板
                designer.Open(ExportTempPath.Annualleave);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Annualleave, listResult);
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


        #region
        /// <summary>
        /// 根据部门组织Ids获取包括自身的所有子节点的id
        /// </summary>
        /// <param name="ids">组织Ids</param>
        /// <returns></returns>
        private List<string> Get_Subdept_ByDept(List<string> ids)
        {
            List<string> listResult = new List<string>();
            if (ids == null || ids.Count == 0)
                return listResult;
            string[] temp = ids[0].Split(',');
            for (int i = 0; i < temp.Length; i++)
            {
                var mm = new EfRepository<string>().Execute<string>(string.Format("select * from  dbo.Get_Subdept_ByDept ('{0}')", temp[i].ToString())).ToList();
                listResult.AddRange(mm);
            }
            return listResult;
        }

        
        #endregion
    }
}