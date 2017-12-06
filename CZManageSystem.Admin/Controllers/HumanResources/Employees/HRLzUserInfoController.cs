using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Core;
using CZManageSystem.Data.Domain.HumanResources.Employees;
using CZManageSystem.Service.HumanResources.Employees;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CZManageSystem.Service.HumanResources.Employees.HRLzUserInfoService;

namespace CZManageSystem.Admin.Controllers.HumanResources.Employees
{
    public class HRLzUserInfoController : BaseController
    {
        // GET: HRLzUserInfo
        IHRLzUserInfoService hrLzUserInfoService = new HRLzUserInfoService();
        ISysUserService sysUserService = new SysUserService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit(string id = null)
        {
            ViewData["id"] = id;
            ViewData["startId"] = WorkContext.CurrentUser.Dept.DpId;
            return View();
        }
        public ActionResult GetDataByID(string id)
        {
            var model = hrLzUserInfoService.FindByFeldName(f => f.EmployeeId == id && f.Users != null);

            return Json(new
            {
                model.EmployeeId,
                model.Gears,
                model.LastModFier,
                model.LastModTime,
                model.PositionRank,
                model.Remark,
                model.SetIntoTheRanks,
                model.Tantile,
                model.UserId,
                EmployeeName = model.Users?.RealName
            });
        }
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, QueryHRLzUserInfoBuilder queryBuilder = null)
        {
            int count = 0;
            try
            {
                var source = hrLzUserInfoService.List().OrderBy(c => c.EmployeeId).Where(w => 1 == 1);

                if (queryBuilder != null)
                {
                    if (!string.IsNullOrEmpty(queryBuilder.EmployeeId))
                        source = source.Where(w => w.EmployeeId == queryBuilder.EmployeeId);
                    if (!string.IsNullOrEmpty(queryBuilder.EmployeeName))
                        source = source.Where(w => w.Users.RealName == queryBuilder.EmployeeName);
                }
                PagedList<HRLzUserInfo> pageList = new PagedList<HRLzUserInfo>();
                var pageListResult = pageList.QueryPagedList(source.Where(u => u.Users != null), pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count).ToList();
                count = pageList.TotalCount;
                var modelList = pageListResult.Select(s => new
                {
                    s.EmployeeId,
                    s.Gears,
                    s.LastModFier,
                    LastModTime = s.LastModTime == null ? "" : Convert.ToDateTime(s.LastModTime).ToString("yyyy-MM-dd"),
                    s.PositionRank,
                    s.Remark,
                    s.SetIntoTheRanks,
                    s.Tantile,
                    s.UserId,
                    EmployeeName = s.Users?.RealName
                });
                return Json(new { items = modelList, count = count });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public ActionResult Delete(string[] ids)
        {
            var models = hrLzUserInfoService.List().Where(f => ids.Contains<string>(f.EmployeeId)).ToList();
            SystemResult result = new SystemResult() { IsSuccess = false, data = new { successCount = models.Count } };
            if (models.Count <= 0)
            {
                result.Message = "该记录不存在！";
                return Json(result);
            }
            if (hrLzUserInfoService.DeleteByList(models))
                result.IsSuccess = true;
            return Json(result);
        }

        public ActionResult Save(HRLzUserInfo hrLzUserInfo)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            if (hrLzUserInfo == null)
            {
                result.Message = "保存的对象不能为空！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(hrLzUserInfo.EmployeeId))
            {
                result.Message = "员工编号不能为空！";
                return Json(result);
            }
            #region 新员工保存 
            if (!hrLzUserInfoService.Contains(f => f.EmployeeId == hrLzUserInfo.EmployeeId))
            {
               var user= sysUserService.FindByFeldName (f => f.EmployeeId == hrLzUserInfo.EmployeeId);
                if (user!=null )
                {
                    hrLzUserInfo.LastModTime = DateTime.Now;
                    hrLzUserInfo.UserId = user.UserId;
                    if (hrLzUserInfoService.Insert(hrLzUserInfo))
                        result.IsSuccess = true;
                    return Json(result);
                }
                else
                {
                    result.Message = "员工编号[" + hrLzUserInfo.EmployeeId + "]不存在！";
                    return Json(result);
                }
            }
            #endregion



            var model = hrLzUserInfoService.FindById(hrLzUserInfo.EmployeeId);
            model.Gears = hrLzUserInfo.Gears;
            model.LastModFier = hrLzUserInfo.LastModFier;
            model.LastModTime = DateTime.Now;
            model.PositionRank = hrLzUserInfo.PositionRank;
            model.Remark = hrLzUserInfo.Remark;
            model.SetIntoTheRanks = hrLzUserInfo.SetIntoTheRanks;
            model.Tantile = hrLzUserInfo.Tantile;
            if (hrLzUserInfoService.Update(model))
                result.IsSuccess = true;
            return Json(result);
        }
        public ActionResult GetEmployeeId(Guid? id)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            var user = sysUserService.FindByFeldName(f => f.UserId == id);
            if (user != null)
            {
                result.data = user.EmployeeId;
                result.IsSuccess = true;
            }
            return Json(result);
        }

        public ActionResult Download(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<QueryHRLzUserInfoBuilder>(queryBuilder);
                var source = hrLzUserInfoService.List().OrderBy(c => c.EmployeeId).Where(w => w.Users != null);

                if (QueryBuilder != null)
                {
                    if (!string.IsNullOrEmpty(QueryBuilder.EmployeeId))
                        source = source.Where(w => w.EmployeeId == QueryBuilder.EmployeeId);
                    if (!string.IsNullOrEmpty(QueryBuilder.EmployeeName))
                        source = source.Where(w => w.Users.RealName == QueryBuilder.EmployeeName);
                }

                var modelList = source.Select(s => new
                {
                    s.EmployeeId,
                    s.Gears,
                    s.LastModFier,
                    s.LastModTime,
                    s.PositionRank,
                    s.Remark,
                    s.SetIntoTheRanks,
                    s.Tantile,
                    s.UserId,
                    EmployerName = s.Users.RealName
                }).ToList();
                if (modelList.Count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.HRLzUserInfo + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();

                //打开模板
                designer.Open(ExportTempPath.HRLzUserInfo);
                designer.SetDataSource(ImportFileType.HRLzUserInfo, modelList);
                //根据数据源处理生成报表内容
                designer.Process();
                var response = GetResponse(fileToSaveName);
                designer.Save(Url.Content(fileToSaveName), SaveType.OpenInExcel, FileFormatType.Excel2003, response);
                response.Flush();
                response.Close();
                designer = null;
                response.End();

                #endregion
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}