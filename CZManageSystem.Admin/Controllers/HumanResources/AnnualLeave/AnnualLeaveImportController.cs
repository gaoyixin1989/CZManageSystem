using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.AnnualLeave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.AnnualLeave
{
    public class AnnualLeaveImportController : BaseController
    {
        // GET: AnnualLeaveImport
        IHRAnnualleaveImportService _importservice = new HRAnnualleaveImportService();
        [SysOperation(OperationType.Browse, "访问年休假导入日志页面")]
        public ActionResult Index(string ImportType)
        {
            ViewData["ImportType"] = ImportType;
            return View();
        }


        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HRAnnualleaveImportQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _importservice.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRAnnualleaveImport)u).ToList();
            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.Id,
                    item.ImportTitle,
                    item.Importor,
                    item.ImportTime,
                    item.ImportType
                });
            }
            return Json(new { items = listResult, count = count });
        }
        [SysOperation(OperationType.Browse, "访问年休假导入明细页面")]
        public ActionResult Detail(int? id)
        {
            ViewData["id"] = id;
            return View();
        }
        public ActionResult GetDataByID(int id)
        {
            var item = _importservice.FindById(id);
            var BaseInfo = new
            {
                item.Importor,
                ImportTime = item.ImportTime.Value.ToString("yyyy-MM-dd HH:mm"),
                item.ImportTitle,
                item.ImportMsg,
                item.ImportType
            };
            List<object> tmpcourseinformation = new List<object>();
            if (item.ImportType == "AnnualLeave")
            {
                if (!string.IsNullOrEmpty(item.ImportInformation))
                {
                    string[] temp1 = item.ImportInformation.ToString().Split('$');
                    for (int i = 0; i < temp1.Length; i++)
                    {
                        tmpcourseinformation.Add(new
                        {
                            UserName = temp1[i].Split('*')[0],
                            Years = temp1[i].Split('*')[1],
                            FdLastYearVDays = temp1[i].Split('*')[2],
                            FdYearVDays = temp1[i].Split('*')[3],
                            BcYearVDays = temp1[i].Split('*')[4]
                        });
                    }
                }
                else
                {
                    tmpcourseinformation.Add(new
                    {
                        UserName = "无异常记录",
                        Years = "",
                        Vdays = ""
                    });
                }
            }

            return Json(new
            {
                BaseInformation = BaseInfo,
                ImportCourseInformation = tmpcourseinformation
            });
        }
    }
}