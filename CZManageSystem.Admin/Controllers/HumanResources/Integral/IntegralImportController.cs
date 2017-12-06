using CZManageSystem.Data.Domain.HumanResources.Integral;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.HumanResources.Integral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.HumanResources.IntegralImport
{
    public class IntegralImportController : BaseController
    {
        // GET: IntegralImport
        IHRVCoursesImportService _importservice = new HRVCoursesImportService();
        [SysOperation(OperationType.Browse, "访问积分导入日志页面")]
        public ActionResult Index(string ImportType)
        {
            ViewData["ImportType"] = ImportType;
            return View();
        }


        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, HRVCoursesImportQueryBuilder queryBuilder = null)
        {
            int count = 0;            
            var modelList = _importservice.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (HRVCoursesImport)u).ToList();
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
        [SysOperation(OperationType.Browse, "访问积分导入明细页面")]
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
            List<object> tmpteachinginformation = new List<object>();
            if(item.ImportType== "Course")
            {
                if (!string.IsNullOrEmpty(item.ImportInformation))
                {
                    string[] temp1 = item.ImportInformation.ToString().Split('$');
                    for (int i = 0; i < temp1.Length; i++)
                    {
                        tmpcourseinformation.Add(new
                        {
                            UserName = temp1[i].Split('*')[0],
                            CoursesName = temp1[i].Split('*')[1],
                            CoursesType = temp1[i].Split('*')[2],
                            ProvinceCity = temp1[i].Split('*')[3],
                            StartTime = Convert.ToDateTime(temp1[i].Split('*')[4]).ToString("yyyy-MM-dd HH:mm"),
                            EndTime = Convert.ToDateTime(temp1[i].Split('*')[5]).ToString("yyyy-MM-dd HH:mm"),
                            PeriodTime = temp1[i].Split('*')[6]
                        });
                    }
                }
                else
                {
                    tmpcourseinformation.Add(new
                    {
                        UserName = "无异常记录",
                        CoursesName = "",
                        CoursesType = "",
                        ProvinceCity = "",
                        StartTime = "",
                        EndTime = "",
                        PeriodTime = ""
                    });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(item.ImportInformation))
                {
                    string[] temp1 = item.ImportInformation.ToString().Split('$');
                    for (int i = 0; i < temp1.Length; i++)
                    {
                        tmpteachinginformation.Add(new
                        {
                            UserName = temp1[i].Split('*')[0],
                            TeachingPlan = temp1[i].Split('*')[1],
                            TeacherType = temp1[i].Split('*')[2],
                            StartTime = Convert.ToDateTime(temp1[i].Split('*')[3]).ToString("yyyy-MM-dd HH:mm"),
                            EndTime = Convert.ToDateTime(temp1[i].Split('*')[4]).ToString("yyyy-MM-dd HH:mm"),
                            PeriodTime = temp1[i].Split('*')[5]
                        });
                    }
                }
                else
                {
                    tmpteachinginformation.Add(new
                    {
                        UserName = "无异常记录",
                        TeachingPlan = "",
                        TeacherType = "",
                        StartTime = "",
                        EndTime = "",
                        PeriodTime = ""
                    });
                }
            }
            
            return Json(new
            {
                BaseInformation = BaseInfo,
                ImportCourseInformation = tmpcourseinformation,
                ImportTeachingInformation= tmpteachinginformation
            });
        }
    }
}