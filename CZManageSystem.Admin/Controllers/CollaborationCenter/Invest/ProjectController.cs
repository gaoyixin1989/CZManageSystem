using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Service.CollaborationCenter.Invest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Invest
{
    public class ProjectController : BaseController
    {
        IInvestProjectService _projectService = new InvestProjectService();//投资项目信息
        IInvestProjectYearTotalService _projectYearTotalService = new InvestProjectYearTotalService();//投资项目信息
        IV_InvestProjectQueryService _projectQueryService = new V_InvestProjectQueryService();//项目查询视图
        IV_InvestProjectYearTotalService _v_projectYaerTotalQueryService = new V_InvestProjectYearTotalService();//年度投资金额视图
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ProjectSelect(string selected)
        {
            ViewData["selected"] = selected;
            return View();
        }

        public ActionResult ProjectQuery()
        {
            return View();
        }

        public ActionResult ProjectEdit(Guid? ID)
        {
            ViewData["ID"] = ID;
            if (ID.HasValue)
                ViewBag.Title = "项目编辑";
            else
                ViewBag.Title = "项目新增";
            ViewData["DeptID"] = this.WorkContext.CurrentUser.DpId;
            return View();
        }

        //编辑年度投资金额
        public ActionResult ProjectYearTotalEdit(string ProjectID, Guid? ID)
        {
            ViewData["ProjectID"] = ProjectID;

            ViewData["ID"] = ID;
            return View();
        }

        /// <summary>
        /// 项目信息展示,如果ID值存在且能查询到对应数据，则更新掉ProjectID
        /// </summary>
        /// <param name="ProjectID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult ProjectInfoShow(string ProjectID, Guid? ID)
        {
            if (ID.HasValue && ID != Guid.Empty)
            {
                var obj = _projectService.FindById(ID);
                if (obj != null && obj.ID != Guid.Empty)
                    ProjectID = obj.ProjectID;
            }
            ViewData["ProjectID"] = ProjectID;
            return View();
        }

        /// <summary>
        /// 查询资金项目数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetListData_Project(int pageIndex = 1, int pageSize = 5, InvestProjectQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _projectService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (InvestProject)u).Select(u => new
            {
                u.ID,
                u.ProjectID,
                u.Year,
                u.TaskID,
                u.ProjectName,
                u.BeginEnd,
                u.Total,
                u.YearTotal,
                u.Content,
                u.FinishDate,
                u.DpCode,
                DpCode_Text = CommonFunction.getDeptNamesByIDs(u.DpCode),
                u.UserID,
                User_Text = CommonFunction.getUserRealNamesByIDs(u.UserID?.ToString()),
                u.ManagerID,
                Manager_Text = CommonFunction.getUserRealNamesByIDs(u.ManagerID?.ToString())
            }).ToList();

            var total = new
            {
                Total = _projectService.GetForPaging(out count, queryBuilder).Sum(u => u.Total ?? 0),
                YearTotal = _projectService.GetForPaging(out count, queryBuilder).Sum(u => u.YearTotal ?? 0)
            };

            return Json(new { items = modelList, count = count, total = total });
        }

        public ActionResult GetListData_ProjectQuery(int pageIndex = 1, int pageSize = 5, VInvestProjectQueryQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _projectQueryService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (V_InvestProjectQuery)u).Select(u => new
            {
                u.Year,
                u.ProjectID,
                u.ProjectName,
                u.Total,
                u.YearTotal,
                u.DpCode,
                DpCode_Text = CommonFunction.getDeptNamesByIDs(u.DpCode),
                u.UserID,
                UserID_Text = CommonFunction.getUserRealNamesByIDs(u.UserID?.ToString()),
                u.ManagerID,
                ManagerID_Text = CommonFunction.getUserRealNamesByIDs(u.ManagerID?.ToString()),
                u.SignTotal,
                u.NotPay,
                u.Pay,
                u.MISMoney,
                u.MustPay,
                u.ProjectRate,
                u.YearMustPay,
                u.YearRate,
                u.TransferRate
            }).ToList();

            var query = _projectQueryService.GetForPaging(out count, queryBuilder);
            var total = new
            {
                Total = query.Sum(u => (decimal)(u.Total ?? 0)),
                YearTotal = query.Sum(u => (decimal)(u.YearTotal ?? 0)),
                NotPay = query.Sum(u => (decimal)(u.NotPay ?? 0)),
                Pay = query.Sum(u => (decimal)(u.Pay ?? 0)),
                MISMoney = query.Sum(u => (decimal)(u.MISMoney ?? 0)),
                MustPay = query.Sum(u => (decimal)(u.MustPay ?? 0)),
                YearMustPay = query.Sum(u => (decimal)(u.YearMustPay ?? 0))
            };

            return Json(new { items = modelList, count = count, total = total }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 下载
        /// </summary> 
        public ActionResult Download_InvestProjectQuery(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<VInvestProjectQueryQueryBuilder>(queryBuilder);

                int count = 0;

                var listResult = _projectQueryService.GetForPaging(out count, QueryBuilder).Select(u => (V_InvestProjectQuery)u).Select(u => new
                {
                    u.Year,
                    u.ProjectID,
                    u.ProjectName,
                    u.Total,
                    u.YearTotal,
                    u.DpCode,
                    DpCode_Text = CommonFunction.getDeptNamesByIDs(u.DpCode),
                    u.UserID,
                    UserID_Text = CommonFunction.getUserRealNamesByIDs(u.UserID?.ToString()),
                    u.ManagerID,
                    ManagerID_Text = CommonFunction.getUserRealNamesByIDs(u.ManagerID?.ToString()),
                    u.SignTotal,
                    u.NotPay,
                    u.Pay,
                    u.MISMoney,
                    u.MustPay,
                    u.ProjectRate,
                    u.YearMustPay,
                    u.YearRate,
                    u.TransferRate
                }).ToList();


                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestProjectQuery + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InvestProjectQuery);
                //设置集合变量
                designer.SetDataSource("emp", listResult);
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

        public ActionResult GetDataByProjectID(string ProjectID)
        {
            var obj = _projectService.FindByFeldName(u => u.ProjectID == ProjectID);
            if (obj != null && obj.ID != Guid.Empty)
                return Json(new
                {
                    obj.ID,
                    obj.ProjectID,
                    obj.Year,
                    obj.TaskID,
                    obj.ProjectName,
                    obj.BeginEnd,
                    obj.Total,
                    obj.YearTotal,
                    obj.Content,
                    obj.FinishDate,
                    obj.DpCode,
                    DpCode_Text = CommonFunction.getDeptNamesByIDs(obj.DpCode),
                    obj.UserID,
                    User_Text = CommonFunction.getUserRealNamesByIDs(obj.UserID?.ToString()),
                    obj.ManagerID,
                    Manager_Text = CommonFunction.getUserRealNamesByIDs(obj.ManagerID?.ToString())
                });
            else
                return Json(new { });
        }

        /// <summary>
        /// 根据id获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_Project(Guid id)
        {
            var obj = _projectService.FindById(id);
            return Json(new
            {
                obj.ID,
                obj.ProjectID,
                obj.Year,
                obj.TaskID,
                obj.ProjectName,
                obj.BeginEnd,
                obj.Total,
                obj.YearTotal,
                obj.Content,
                obj.FinishDate,
                obj.DpCode,
                DpCode_Text = CommonFunction.getDeptNamesByIDs(obj.DpCode),
                obj.UserID,
                User_Text = CommonFunction.getUserRealNamesByIDs(obj.UserID?.ToString()),
                obj.ManagerID,
                Manager_Text = CommonFunction.getUserRealNamesByIDs(obj.ManagerID?.ToString())
            });
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete_Project(Guid[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;

            var objs = _projectService.List().Where(u => ids.Contains(u.ID)).ToList();

            if (objs.Count > 0)
            {
                isSuccess = _projectService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            if (isSuccess)
            {
                List<string> listProjectID = objs.Select(a => a.ProjectID).ToList();
                var yearTotalList = _projectYearTotalService.List().Where(u => listProjectID.Contains(u.ProjectID)).ToList();
                if (yearTotalList.Count > 0)
                    _projectYearTotalService.DeleteByList(yearTotalList);
            }
            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 保存项目
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save_Project(InvestProject dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.ProjectID == null || string.IsNullOrEmpty(dataObj.ProjectID.Trim()))
                tip = "项目编号不能为空";
            else if (dataObj.ProjectName == null || string.IsNullOrEmpty(dataObj.ProjectName.Trim()))
                tip = "项目名称不能为空";
            else if (dataObj.TaskID == null || string.IsNullOrEmpty(dataObj.TaskID.Trim()))
                tip = "计划任务书文号不能为空";
            else if ((dataObj.Year ?? 0) == 0)
                tip = "下达年份不能为空";
            else if ((dataObj.Total ?? 0) == 0)
                tip = "项目总投资不能为空";
            else if ((dataObj.YearTotal ?? 0) == 0)
                tip = "年度项目投资不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "年度建设内容不能为空";
            else if (dataObj.DpCode == null || string.IsNullOrEmpty(dataObj.DpCode.Trim()))
                tip = "负责专业室不能为空";
            else if (dataObj.UserID == Guid.Empty)
                tip = "室负责人不能为空";
            else if (dataObj.ManagerID == Guid.Empty)
                tip = "项目经理不能为空";
            else if (!CheckProjectID(dataObj))
                tip = "该项目编号已经被占用";
            else
            {
                isValid = true;
                dataObj.ProjectID = dataObj.ProjectID.Trim();
                dataObj.ProjectName = dataObj.ProjectName.Trim();
                dataObj.TaskID = dataObj.TaskID.Trim();
                dataObj.Content = dataObj.Content.Trim();
                dataObj.DpCode = dataObj.DpCode.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == Guid.Empty)//新增
            {
                dataObj.ID = Guid.NewGuid();
                result.IsSuccess = _projectService.Insert(dataObj);
            }
            else
            {//编辑
                var oldProjectID = _projectService.List().Where(u => u.ID == dataObj.ID).Select(u => u.ProjectID).FirstOrDefault();
                result.IsSuccess = _projectService.Update(dataObj);
                if (oldProjectID != dataObj.ProjectID)
                {
                    var tempList = _projectYearTotalService.List().Where(u => u.ProjectID == oldProjectID).ToList();
                    foreach (var item in tempList)
                        item.ProjectID = dataObj.ProjectID;
                    if (tempList.Count > 0)
                        _projectYearTotalService.UpdateByList(tempList);

                }
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                result.Message = dataObj.ID.ToString();
            }
            return Json(result);

        }

        /// <summary>
        /// 根据项目id查询获取年度投资金额列表
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult GetListData_YearTotal(string projectId)
        {
            var datas = _projectYearTotalService.List().Where(u => u.ProjectID == projectId).OrderByDescending(u => u.Year ?? 0).ToList();
            decimal total = datas.Sum(u => u.YearTotal ?? 0);
            return Json(new
            {
                items = datas.Select(u => new
                {
                    u.ID,
                    u.ProjectID,
                    u.Year,
                    u.YearTotal
                }),
                total = total
            });
        }

        /// <summary>
        /// 根据id查询获取年度投资金额数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_YearTotal(Guid id)
        {
            var obj = _projectYearTotalService.FindById(id);
            return Json(new
            {
                obj.ID,
                obj.ProjectID,
                obj.Year,
                obj.YearTotal
            });
        }

        /// <summary>
        /// 删除年度投资金额数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete_YearTotal(Guid[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;

            var objs = _projectYearTotalService.List().Where(u => ids.Contains(u.ID)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _projectYearTotalService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 保存年度投资金额数据
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        public ActionResult Save_YearTotal(InvestProjectYearTotal dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过

            if (dataObj.ProjectID == null || string.IsNullOrEmpty(dataObj.ProjectID.Trim()))
                tip = "隶属项目编号不能为空";
            else if ((dataObj.Year ?? 0) == 0)
                tip = "年份不能为空";
            else if ((dataObj.YearTotal ?? 0) == 0)
                tip = "年度投资金额不能为空";
            else if (!CheckYearTotal(dataObj))
                tip = "该条记录已经存在，不能重复";
            else
            {
                isValid = true;
                dataObj.ProjectID = dataObj.ProjectID.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == Guid.Empty)//新增
            {
                dataObj.ID = Guid.NewGuid();
                result.IsSuccess = _projectYearTotalService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _projectYearTotalService.Update(dataObj);
            }
            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                result.Message = dataObj.ID.ToString();
            }
            return Json(result);
        }

        /// <summary>
        /// 检查年度投资数据，年份不能重复
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        private bool CheckYearTotal(InvestProjectYearTotal dataObj)
        {
            var dd = _projectYearTotalService.List().Where(u => u.ID != dataObj.ID && u.ProjectID == dataObj.ProjectID.Trim() && u.Year == dataObj.Year).ToList();
            if (dd.Count > 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 检查项目编号，不能重复,ID和ProjectID不能为空
        /// </summary>
        /// <param name="dataObj"></param>
        /// <returns></returns>
        private bool CheckProjectID(InvestProject dataObj)
        {
            var dd = _projectService.List().Where(u => u.ID != dataObj.ID && u.ProjectID == dataObj.ProjectID.Trim()).ToList();
            if (dd.Count > 0)
                return false;
            else
                return true;
        }


        /// <summary>
        /// 下载会投资项目信息数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download_InvestProject(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<InvestProjectQueryBuilder>(queryBuilder);

                int count = 0;

                var listResult = _projectService.GetForPaging(out count, QueryBuilder).Select(u => (InvestProject)u).Select(u => new
                {
                    u.ID,
                    u.ProjectID,
                    u.Year,
                    u.TaskID,
                    u.ProjectName,
                    u.BeginEnd,
                    u.Total,
                    u.YearTotal,
                    u.Content,
                    u.FinishDate,
                    u.DpCode,
                    DpCode_Text = CommonFunction.getDeptNamesByIDs(u.DpCode),
                    u.UserID,
                    User_Text = CommonFunction.getUserRealNamesByIDs(u.UserID?.ToString()),
                    u.ManagerID,
                    Manager_Text = CommonFunction.getUserRealNamesByIDs(u.ManagerID?.ToString())
                }).ToList();


                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestProject + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InvestProject);
                //设置集合变量
                designer.SetDataSource(ImportFileType.InvestProject, listResult);
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

        /// <summary>
        /// 年度投资金额查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ProjectYearTotalQuery()
        {
            return View();
        }

        /// <summary>
        /// 年度投资金额查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetListData_ProjectYearTotalQuery(int pageIndex = 1, int pageSize = 5, ProjectYearTotalQueryQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _v_projectYaerTotalQueryService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (V_InvestProjectYearTotal)u).Select(u => new
            {
                u.ID,
                u.ProjectID,
                u.ProjectName,
                u.Year,
                u.YearTotal,
                u.Total,
                u.ManagerID,
                Manager_Text = CommonFunction.getUserRealNamesByIDs(u.ManagerID?.ToString())
            }).ToList();

            var total = new
            {
                Total = _v_projectYaerTotalQueryService.GetForPaging(out count, queryBuilder).GroupBy(u=>u.ProjectID).Select(u=>u.First()).Sum(u => (decimal)(u.Total ?? 0)),
                YearTotal = _v_projectYaerTotalQueryService.GetForPaging(out count, queryBuilder).Sum(u => (decimal)(u.YearTotal ?? 0))
            };

            return Json(new { items = modelList, count = count, total = total },JsonRequestBehavior.AllowGet);
        }



    }
}