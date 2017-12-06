using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Service.MarketPlan;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CZManageSystem.Data.Domain.MarketPlan;

namespace CZManageSystem.Admin.Controllers.MarketPlan
{
    public class MarketPlanController : BaseController
    {
        IUcs_MarketPlan2Service _ucs_MarketPlan2Service = new Ucs_MarketPlan2Service();
        IUcs_MarketPlanLogService _ucs_MarketPlanLogService = new Ucs_MarketPlanLogService();
        IUcs_MarketPlan3Service _ucs_MarketPlan3Service = new Ucs_MarketPlan3Service();
        IUcs_MarketPlanMonitorService _ucs_MarketPlanMonitorService = new Ucs_MarketPlanMonitorService();

        // GET: MarketPlan
        public ActionResult StatiIndex()
        {
            return View();
        }
        public ActionResult StatiEdit(Guid? Id, string type)
        {
            ViewData["Id"] = Id;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult PlanList()
        {
            return View();
        }
        public ActionResult MonitorLog()
        {
            return View();
        }
        public ActionResult StatiLog()
        {
            return View();
        }
        #region 营销方案可办理数统计
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, MarketPlanQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ucs_MarketPlan2Service.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<Ucs_MarketPlan2>;
            return Json(new { items = modelList, count = count });

        }
        public ActionResult Delete(Guid[] Ids)
        {
            bool isSuccess = false;
            int successCount = 0;
            var objs = _ucs_MarketPlan2Service.List().Where(u => Ids.Contains(u.Id)).ToList();
            if (objs.Count > 0)
            {
                isSuccess = _ucs_MarketPlan2Service.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }
            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });
        }

        /// <summary>
        /// 获取下拉框信息
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetDropList()
        {
            var ChannelList = GetDictListByDDName("办理渠道");
            var PlanTypeList = GetDictListByDDName("方案类型");
            var TargetUsersList = GetDictListByDDName("目标用户群");
            return Json(new
            {
                ChannelList,
                PlanTypeList,
                TargetUsersList
            });
        }

        public ActionResult marketInfoGetByID(Guid Id)
        {
            var marketInfo = _ucs_MarketPlan2Service.FindById(Id);

            return Json(new
            {
                marketInfo.Id,
                marketInfo.Channel,
                marketInfo.Coding,
                marketInfo.DetialInfo,
                StartTime = marketInfo.StartTime.HasValue ? marketInfo.StartTime.Value.ToString("yyyy-MM-dd") : "",
                EndTime = marketInfo.EndTime.HasValue ? marketInfo.EndTime.Value.ToString("yyyy-MM-dd") : "",
                marketInfo.IsMarketing,
                marketInfo.Name,
                marketInfo.NumCount,
                marketInfo.Orders,
                marketInfo.PaysRlues,
                marketInfo.PlanType,
                marketInfo.RegPort,
                marketInfo.Remark,
                marketInfo.TargetUsers,
                marketInfo.Templet1,
                marketInfo.Templet2,
                marketInfo.Templet3,
                marketInfo.Templet4

            });
        }
        public ActionResult Save_marketInfo(Ucs_MarketPlan2 market)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            string tip = "";
            Ucs_MarketPlanLog log = new Ucs_MarketPlanLog();
            log.Id = Guid.NewGuid();
            log.Name = market.Name;
            log.Coding = market.Coding;
            log.Creator = this.WorkContext.CurrentUser.Creator;
            log.Creattime = DateTime.Now;
            log.Department = this.WorkContext.CurrentUser.Dept.DpName;
            if (market.Id == null || market.Id.ToString() == "00000000-0000-0000-0000-000000000000")//新增
            {
                bool isValid = false;//是否验证通过
                #region 验证数据是否合法
                var Coding = _ucs_MarketPlan2Service.List().Where(c => c.Coding == market.Coding).ToList();
                if (Coding.Count() > 0)
                {
                    tip = "营销方案编号不能重复";
                }
                else
                {
                    isValid = true;
                }
                #endregion

                if (isValid)
                {
                    market.Id = Guid.NewGuid();
                    result.IsSuccess = _ucs_MarketPlan2Service.Insert(market);
                    if (result.IsSuccess)
                    {
                        log.Remark = "插入成功";
                    }
                    else
                    {
                        log.Remark = "插入失败";
                    }
                    _ucs_MarketPlanLogService.Insert(log);

                }
            }
            else
            {//编辑
                result.IsSuccess = _ucs_MarketPlan2Service.Update(market);
                if (result.IsSuccess)
                {
                    log.Remark = "修改成功";
                }
                else
                {
                    log.Remark = "修改失败";
                }
                _ucs_MarketPlanLogService.Insert(log);
            }
            result.Message = tip;
            return Json(result);
        }

        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<MarketPlanQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _ucs_MarketPlan2Service.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<Ucs_MarketPlan2>;
                var list = modelList.Select(s => new
                {
                    s.Id,
                    s.Channel,
                    s.Coding,
                    s.DetialInfo,
                    s = s.StartTime.HasValue ? s.StartTime.Value.ToString("yyyy-MM-dd") : "",
                    EndTime = s.EndTime.HasValue ? s.EndTime.Value.ToString("yyyy-MM-dd") : "",
                    s.IsMarketing,
                    s.Name,
                    s.NumCount,
                    s.Orders,
                    s.PaysRlues,
                    s.PlanType,
                    s.RegPort,
                    s.Remark,
                    s.TargetUsers,
                    s.Templet1,
                    s.Templet2,
                    s.Templet3,
                    s.Templet4

                }).ToList<object>();
                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Market + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Market);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Market, list);
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


        #region 优惠方案查询
        public ActionResult GetPlanListData(int pageIndex = 1, int pageSize = 5, MarketPlanQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ucs_MarketPlan3Service.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<Ucs_MarketPlan3>;
            return Json(new { items = modelList, count = count });

        }
        /// <summary>
        /// 下载数据
        /// </summary>
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult DownloadPlanList(string queryBuilder = null)
        {
            string path = "";
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<MarketPlanQueryBuilder>(queryBuilder);
                int pageIndex = 0;
                int pageSize = int.MaxValue;

                int count = 0;
                var modelList = _ucs_MarketPlan3Service.GetForPaging(out count, QueryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<Ucs_MarketPlan3>;
                var list = modelList.Select(s => new
                {
                    s.Id,
                    s.Channel,
                    s.Coding,
                    s.DetialInfo,
                    s = s.StartTime.HasValue ? s.StartTime.Value.ToString("yyyy-MM-dd") : "",
                    EndTime = s.EndTime.HasValue ? s.EndTime.Value.ToString("yyyy-MM-dd") : "",
                    s.IsMarketing,
                    s.Name,
                    s.Tel,
                    s.Orders,
                    s.PaysRlues,
                    s.PlanType,
                    s.RegPort,
                    s.Remark,
                    s.TargetUsers,
                    s.Templet1,
                    s.Templet2,
                    s.Templet3,
                    s.Templet4

                }).ToList<object>();
                if (count < 1)
                    return View("../Export/Message");
                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.Market + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.Market);
                //设置集合变量
                designer.SetDataSource(ImportFileType.Market, list);
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
        /// <summary>
        /// 优惠方案统计日志查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetStaticLogData(int pageIndex = 1, int pageSize = 5, MarketPlanLogQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ucs_MarketPlanLogService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<Ucs_MarketPlanLog>;
            return Json(new { items = modelList, count = count });

        }
        /// <summary>
        /// 优惠方案日志查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetMonitorLogData(int pageIndex = 1, int pageSize = 5, MarketPlanMonitorQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _ucs_MarketPlanMonitorService.GetForPaging(out count, queryBuilder, pageIndex - 1 <= 0 ? 0 : pageIndex, pageSize) as List<Ucs_MarketPlanMonitor>;
            return Json(new { items = modelList, count = count });

        }
    }
}