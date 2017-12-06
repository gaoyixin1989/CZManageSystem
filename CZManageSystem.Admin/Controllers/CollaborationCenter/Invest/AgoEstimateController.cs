using Aspose.Cells;
using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.Invest;
using CZManageSystem.Service.SysManger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Invest
{
    /// <summary>
    /// 历史项目暂估
    /// </summary>
    public class AgoEstimateController : BaseController
    {
        IInvestAgoEstimateService _agoEstimateService = new InvestAgoEstimateService();//历史项目暂估
        IInvestAgoEstimateLogService _agoEstimateLogService = new InvestAgoEstimateLogService();//历史项目暂估日志

        IInvestAgoEstimateApplyService _applyService = new InvestAgoEstimateApplyService();//历史项目暂估申请
        IInvestAgoEstimateApplyDetailService _applyDetailService = new InvestAgoEstimateApplyDetailService();//历史项目暂估申请明细
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();

        // GET: AgoEstimate
        /// <summary>
        /// 历史项目暂估查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Edit(Guid? ID)
        {
            ViewData["ID"] = ID;
            if (ID.HasValue)
                ViewBag.Title = "暂估编辑";
            else
                ViewBag.Title = "暂估新增";
            return View();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, AgoEstimateQueryBuilder queryBuilder = null)
        {
            int count = 0;
            var modelList = _agoEstimateService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (InvestAgoEstimate)u).Select(u => new
            {
                u.ID,
                u.Year,
                u.Month,
                u.ProjectName,
                u.ProjectID,
                u.ContractName,
                u.ContractID,
                u.Supply,
                u.SignTotal,
                u.PayTotal,
                u.Study,
                u.ManagerID,
                ManagerID_Text = u.ManagerObj?.RealName,
                u.Course,
                u.BackRate,
                u.Rate,
                u.Pay,
                u.NotPay,
                u.EstimateUserID,
                EstimateUserID_Text = u.EstimateUserObj?.RealName

            }).ToList();

            var query = _agoEstimateService.GetForPaging(out count, queryBuilder);
            var total = new
            {
                SignTotal = query.Sum(u => (decimal)(u.SignTotal ?? 0)),
                PayTotal = query.Sum(u => (decimal)(u.PayTotal ?? 0)),
                Pay = query.Sum(u => (decimal)(u.Pay ?? 0)),
                NotPay = query.Sum(u => (decimal)(u.NotPay ?? 0))
            };

            return Json(new { items = modelList, count = count, total = total }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 下载会投资项目信息数据
        /// </summary> 
        /// <param name="queryBuilder">参数</param>
        /// <returns></returns>
        public ActionResult Download_InvestAgoEstimate(string queryBuilder = null)
        {
            try
            {
                //因下载用的是url，对象参数是只好通过JSON格式传入，需要转回对象。
                var QueryBuilder = JsonConvert.DeserializeObject<AgoEstimateQueryBuilder>(queryBuilder);
                int count = 0;

                var listResult = _agoEstimateService.GetForPaging(out count, QueryBuilder).Select(u => (InvestAgoEstimate)u).Select(u => new
                {
                    u.ID,
                    u.Year,
                    u.Month,
                    u.ProjectName,
                    u.ProjectID,
                    u.ContractName,
                    u.ContractID,
                    u.Supply,
                    u.SignTotal,
                    u.PayTotal,
                    u.Study,
                    u.ManagerID,
                    ManagerID_Text = u.ManagerObj?.RealName,
                    ManagerID_DeptText = u.ManagerObj?.Dept?.DpName,
                    u.Course,
                    u.BackRate,
                    u.Rate,
                    u.Pay,
                    u.NotPay,
                    u.EstimateUserID,
                    EstimateUserID_Text = u.EstimateUserObj?.RealName

                }).ToList();


                if (listResult.Count < 1)
                    return View("../Export/Message");

                #region Excel部分
                //生成文件名
                var fileToSaveName = SaveName.InvestAgoEstimate + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";

                WorkbookDesigner designer = new WorkbookDesigner();
                //打开模板
                designer.Open(ExportTempPath.InvestAgoEstimate);
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

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var u = _agoEstimateService.FindById(id);
            return Json(new
            {
                u.ID,
                u.Year,
                u.Month,
                u.ProjectName,
                u.ProjectID,
                u.ContractName,
                u.ContractID,
                u.Supply,
                u.SignTotal,
                u.PayTotal,
                u.Study,
                u.ManagerID,
                ManagerID_Text = u.ManagerObj?.RealName,
                u.Course,
                u.BackRate,
                u.Rate,
                u.Pay,
                u.NotPay,
                u.EstimateUserID,
                EstimateUserID_Text = u.EstimateUserObj?.RealName
            });
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete(Guid[] ids)
        {

            bool isSuccess = false;
            int successCount = 0;

            var objs = _agoEstimateService.List().Where(u => ids.Contains(u.ID)).ToList();

            foreach (var item in objs)
            {
                var log = new InvestAgoEstimateLog();
                log.OpType = "删除";
                log.OpTime = DateTime.Now;
                log.OpName = this.WorkContext.CurrentUser.RealName;
                log.ProjectID = item.ProjectID;
                log.ContractID = item.ContractID;
                log.BfPayTotal = item.PayTotal;
                log.BfRate = item.Rate;
                log.BfPay = item.Pay;
                log.BfNotPay = item.NotPay;
                if (_agoEstimateService.Delete(item))
                {
                    isSuccess = true;
                    successCount++;
                    _agoEstimateLogService.Insert(log);
                }
            }

            return Json(new
            {
                isSuccess = isSuccess,
                successCount = successCount,
            });
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="opType">修改类别</param>
        /// <returns></returns>
        public ActionResult Save(List<InvestAgoEstimate> dataList, string opType)
        {
            if (string.IsNullOrEmpty(opType)) opType = "批量修改";
            SystemResult result = new SystemResult() { IsSuccess = false };

            foreach (var item in dataList)
            {
                item.PayTotal = item.PayTotal ?? 0;
                item.Rate = item.Rate ?? 0;
                item.Pay = item.Pay ?? 0;

                item.NotPay = item.PayTotal * item.Rate / 100 - item.Pay;
                item.NotPay = Math.Round(item.NotPay.Value, 2);
                if (item.NotPay < 0)
                {
                    if (dataList.Count <= 1)
                        result.Message = "暂估金额不能小于0";
                    else
                        result.Message += (string.IsNullOrEmpty(result.Message) ? "" : "<br/>") + string.Format("项目编号-{0},合同编号-{1}:暂估金额不能小于0", item.ProjectID, item.ContractID);
                    continue;
                }

                var log = new InvestAgoEstimateLog();
                var item2 = _agoEstimateService.FindById(item.ID);

                log.OpType = opType;
                log.OpTime = DateTime.Now;
                log.OpName = this.WorkContext.CurrentUser.RealName;
                log.ProjectID = item2.ProjectID;
                log.ContractID = item2.ContractID;

                log.BfPayTotal = item2.PayTotal;
                log.BfRate = item2.Rate;
                log.BfPay = item2.Pay;
                log.BfNotPay = item2.NotPay;

                log.PayTotal = item.PayTotal;
                log.Rate = item.Rate;
                log.Pay = item.Pay;
                log.NotPay = item.NotPay;

                item2.PayTotal = item.PayTotal;
                item2.Rate = item.Rate;
                item2.Pay = item.Pay;
                item2.NotPay = item.NotPay;
                item2.Course = item.Course;

                if (_agoEstimateService.Update(item2))
                {
                    result.IsSuccess = true;
                    _agoEstimateLogService.Insert(log);

                }
                else
                {
                    if (dataList.Count <= 1)
                        result.Message = "保存失败";
                    else
                        result.Message += (string.IsNullOrEmpty(result.Message) ? "" : "<br/>") + string.Format("项目编号-{0},合同编号-{1}:保存失败", item.ProjectID, item.ContractID);
                    continue;
                }

            }

            if (!result.IsSuccess && string.IsNullOrEmpty(result.Message))
            {
                result.Message = "保存失败";
            }
            return Json(result);
        }


        #region 历史项目暂估申请
        //历史项目暂估申请列表页
        public ActionResult AgoEstimateApplyIndex()
        {
            return View();
        }
        //历史项目暂估申请流程查询页
        public ActionResult AgoEstimateApplyQuery()
        {
            return View();
        }

        //历史项目暂估申请流程日志查看
        public ActionResult AgoEstimateApplyWfLook(Guid? ApplyID)
        {
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            return View();
        }

        //历史项目暂估申请编辑页
        public ActionResult AgoEstimateApplyEdit(Guid? ApplyID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.InvestAgoEstimateApply;
            ViewData["type"] = type;
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["DeptName"] = this.WorkContext.CurrentUser.Dept.DpName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的历史项目暂估申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }

        //编辑历史项目暂估申请明细编辑
        public ActionResult AgoEstimateApplyDetailEdit(Guid? ID, Guid? ApplyID, string type = "look")
        {
            ViewData["type"] = type;
            ViewData["ID"] = ID.HasValue ? ID.ToString() : null;
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            return View();
        }

        //历史项目暂估申请信息——流程中使用
        public ActionResult AgoEstimateApplyInfo_WF(Guid? ApplyID)
        {
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            return View();
        }

        /// <summary>
        /// 获取历史项目暂估申请信息数据列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">查询条件</param>
        /// <returns></returns>
        public ActionResult GetListData_AgoEstimateApplyList(int pageIndex = 1, int pageSize = 5, AgoEstimateApplyQueryBuilder queryBuilder = null, bool isManager = false)
        {
            if (!isManager)
            {
                queryBuilder.AppPerson = this.WorkContext.CurrentUser.UserId;
            }
            int count = 0;
            var modelList = _applyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => (InvestAgoEstimateApply)u).ToList();

            List<object> listResult = new List<object>();
            foreach (var item in modelList)
            {
                listResult.Add(new
                {
                    item.ID,
                    item.WorkflowInstanceId,
                    item.Series,
                    item.ApplyTime,
                    item.AppDept,
                    AppDept_Text = item.AppDeptObj?.DpName,// CommonFunction.getDeptNamesByIDs(item.AppDept),
                    item.AppPerson,
                    AppPerson_Text = item.AppPersonObj?.RealName,// CommonFunction.getUserRealNamesByIDs(item.AppPerson?.ToString()),
                    item.Mobile,
                    item.Title,
                    item.Content,
                    item.State,
                    WF_StateText = CommonFunction.GetFlowStateText(item.Tracking_Workflow),
                    WF_CurActivityName = _tracking_WorkflowService.GetCurrentActivityNames(item.WorkflowInstanceId ?? Guid.Parse("00000000-0000-0000-0000-000000000000")).FirstOrDefault(),
                    WF_LastActivityInfo = CommonFunction.GetActivitiesCompletedByID(item.WorkflowInstanceId).Select(u => new
                    {
                        u.Activities.ActivityName,
                        u.ActorDescription,
                        u.FinishedTime,
                        u.Reason,
                        u.Command,
                        CommandText = GetCommandText(u.Command)
                    }).LastOrDefault()
                });
            }

            return Json(new { items = listResult, count = count }, JsonRequestBehavior.AllowGet);

        }

        private string GetCommandText(string value)
        {
            string result = "";
            switch (value)
            {
                case "approve": result = "通过"; break;
                case "reject": result = "退回"; break;
                case "close_activities": result = "作废"; break;
                default: break;
            }
            return result;
        }

        /// <summary>
        /// 删除历史项目暂估申请信息
        /// </summary>
        /// <param name="IDs">退库单ID</param>
        /// <returns></returns>
        public ActionResult Delete_AgoEstimateApplys(Guid[] IDs)
        {
            bool isSuccess = false;
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var applyDatas = _applyService.List().Where(u => IDs.Contains(u.ID)).ToList();//历史项目暂估申请信息
            allCount = applyDatas.Count;

            if (applyDatas.Where(u => u.State == 1).Count() > 0)
            {
                strMsg = "其中存在已提交的记录，不能删除";
            }
            else if (applyDatas.Count > 0)
            {
                if (_applyService.DeleteByList(applyDatas))
                {
                    successCount = applyDatas.Count;
                    foreach (var item in applyDatas)
                    {
                        var temp = _applyDetailService.List().Where(u => u.ApplyID == item.ID).ToList();
                        _applyDetailService.DeleteByList(temp);
                    }
                }
            }
            else
            {
                strMsg = "删除失败";
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount,
                messsage = strMsg
            });

        }

        //保存历史项目暂估申请信息
        public ActionResult Save_AgoEstimateApply(InvestAgoEstimateApply dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "申请原因不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
                dataObj.Content = dataObj.Content.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == Guid.Empty)
            {//新增
                dataObj.ID = Guid.NewGuid();
                dataObj.State = 0;
                dataObj.AppPerson = this.WorkContext.CurrentUser.UserId;
                dataObj.AppDept = this.WorkContext.CurrentUser.DpId;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _applyService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _applyService.Update(dataObj);
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

        public ActionResult Save_AgoEstimateApplyDetail(InvestAgoEstimateApplyDetail dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (!dataObj.ApplyID.HasValue || dataObj.ApplyID.Value == Guid.Empty)
                tip = "申请编号不存在";
            else if (!dataObj.Year.HasValue)
                tip = "年份不能为空";
            else if (!dataObj.Month.HasValue)
                tip = "月份不能为空";
            else if (dataObj.ProjectID == null || string.IsNullOrEmpty(dataObj.ProjectID.Trim()))
                tip = "项目编号不能为空";
            else if (dataObj.ProjectName == null || string.IsNullOrEmpty(dataObj.ProjectName.Trim()))
                tip = "项目名称不能为空";
            else if (dataObj.ContractID == null || string.IsNullOrEmpty(dataObj.ContractID.Trim()))
                tip = "合同编号不能为空";
            else if (dataObj.ContractName == null || string.IsNullOrEmpty(dataObj.ContractName.Trim()))
                tip = "合同名称不能为空";
            else if ((dataObj.SignTotal ?? 0) < 0)
                tip = "合同总金额不能小于0";
            else if ((dataObj.PayTotal ?? 0) < 0)
                tip = "合同实际金额不能小于0";
            else if ((dataObj.Rate ?? 0) < 0)
                tip = "项目形象进度不能小于0";
            else if ((dataObj.Rate ?? 0) > 100)
                tip = "项目形象进度不能大于100";
            else if ((dataObj.Pay ?? 0) < 0)
                tip = "已付金额不能小于0";
            else if ((dataObj.NotPay ?? 0) < 0)
                tip = "暂估金额不能小于0";
            else
            {
                isValid = true;
                dataObj.ProjectID = dataObj.ProjectID.Trim();
                dataObj.ProjectName = dataObj.ProjectName.Trim();
                dataObj.ContractID = dataObj.ContractID.Trim();
                dataObj.ContractName = dataObj.ContractName.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ID == Guid.Empty)
            {//新增
                dataObj.ID = Guid.NewGuid();
                result.IsSuccess = _applyDetailService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _applyDetailService.Update(dataObj);
            }

            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            return Json(result);

        }


        public ActionResult GetDetailDataByID(Guid? id)
        {
            id = id ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var objDta = _applyDetailService.FindById(id);
            return Json(objDta, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除历史项目暂估申请m明细信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult Delete_AgoEstimateApplyDetail(Guid[] ids)
        {
            bool isSuccess = false;
            int successCount = 0;

            var objs = _applyDetailService.List().Where(u => ids.Contains(u.ID)).ToList();//历史项目暂估申请明细信息

            if (objs.Count > 0)
            {
                isSuccess = _applyDetailService.DeleteByList(objs);
                successCount = isSuccess ? objs.Count() : 0;
            }

            return Json(new
            {
                isSuccess = successCount > 0 ? true : false,
                successCount = successCount
            });

        }

        //提交历史项目暂估申请信息
        public ActionResult Sumbit_AgoEstimateApply(Guid applyID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _applyService.FindById(applyID);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该申请单信息不存在";
                return Json(result);
            }

            //获取当前申请的明细
            var detailData = _applyDetailService.List().Where(u => u.ApplyID == curData.ID).ToList();

            if (detailData.Count <= 0)
            {
                result.IsSuccess = false;
                result.Message = "该申请单中的明细为空，请正确填写信息";
                return Json(result);
            }

            result = Sumbit_AgoEstimateApply_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _applyService.FindById(curData.ID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }

        /// <summary>
        /// 提交申请
        /// </summary>
        public SystemResult Sumbit_AgoEstimateApply_WF(InvestAgoEstimateApply curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.InvestAgoEstimateApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
            }

            SystemResult result = new SystemResult() { IsSuccess = false };
            string objectXML = "";

            if (!curData.WorkflowInstanceId.HasValue)
            {//第一次提交
                objectXML = "<Root action=\"Start\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"workflowId\" value=\"{1}\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.InvestAgoEstimateApply, curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }
            else
            {//退回提交
                ITracking_TodoService tempService = new Tracking_TodoService();
                Tracking_Todo tempActivity = new Tracking_Todo();
                Guid guid = curData.WorkflowInstanceId.Value;
                tempActivity = new Tracking_TodoService().Entitys.Where(u => u.WorkflowInstanceId == guid).FirstOrDefault();//当前节点实例
                objectXML = "<Root action=\"execute\" username=\"{0}\" keypassword=\"123\">"
                                + "<parameter>"
                                    + "<item name=\"activityInstanceId\" value=\"{1}\"/>"
                                    + "<item name=\"command\" value=\"Approve\"/>"
                                    + "<item name=\"workflowTitle\" value=\"{2}\"/>"
                                    + "<item name=\"workflowProperties\">"
                                        + "<workflow secrecy=\"0\" urgency=\"0\" importance=\"0\" expectFinishedTime=\"{6}\">"
                                            + "<fields>"
                                                + "<item name=\"F1\" value=\"{3}\"></item>"
                                            + "</fields>"
                                            + "<nextactivities>"
                                                + "<item name=\"{4}\" actors=\"{5}\"/>"
                                            + "</nextactivities>"
                                        + "</workflow>"
                                    + "</item>"
                                + "</parameter>"
                            + "</Root>";
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

            }

            string[] args = new string[4];
            args[0] = FlowInstance.Workflow_SystemID;
            args[1] = FlowInstance.Workflow_SystemAcount;
            args[2] = FlowInstance.Workflow_SystemPwd;
            args[3] = objectXML;

            string resultXml = WebServicesHelper.InvokeWebService(FlowInstance.Workflow_SystemUrl, FlowInstance.MethodName.ManageWorkflow, args).ToString();
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();
            xdoc.LoadXml(resultXml);
            System.Xml.XmlNode resutNode = xdoc.SelectSingleNode("Result");
            string success = resutNode.Attributes["Success"].Value;
            string errmsg = resutNode.Attributes["ErrorMsg"].Value;
            string strWorkflowInstanceId = "";
            string strActivityinstanceId = "";
            int intSuccess = 0;
            int.TryParse(success, out intSuccess);
            if (intSuccess > 0)
            {
                if (!curData.WorkflowInstanceId.HasValue)
                {//第一次提交
                    System.Xml.XmlNodeList xmlList = xdoc.SelectNodes("Result/item/start/item");
                    for (int i = 0; i < xmlList.Count; i++)
                    {
                        switch (xmlList[i].Attributes["name"].Value)
                        {
                            case "WorkflowInstanceId": strWorkflowInstanceId = xmlList[i].Attributes["value"].Value; break;
                            case "ActivityinstanceId": strActivityinstanceId = xmlList[i].Attributes["value"].Value; break;
                            default: break;
                        }
                    }

                    var workFlow = _tracking_WorkflowService.FindById(Guid.Parse(strWorkflowInstanceId));
                    curData.State = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Series = workFlow.SheetId;
                    curData.ApplyTime = DateTime.Now;

                    result.IsSuccess = _applyService.Update(curData);
                }
                else
                {//退回提交
                    result.IsSuccess = true;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = errmsg;
            }

            return result;
        }


        /// <summary>
        /// 根据ID获取历史项目暂估申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID_AgoEstimateApply(Guid? id)
        {
            id = id ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var appData = _applyService.FindById(id);
            Users sumbitUser = new Users();
            if (appData != null)
            {
                Guid userID = appData.AppPerson ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
                sumbitUser = new SysUserService().FindById(userID);
            }
            return Json(new
            {
                appData = new
                {
                    ID = appData.ID,
                    WorkflowInstanceId = appData.WorkflowInstanceId,
                    Series = appData.Series,
                    ApplyTime = appData.ApplyTime,
                    AppDept = appData.AppDept,
                    AppPerson = appData.AppPerson,
                    Mobile = appData.Mobile,
                    Title = appData.Title,
                    Content = appData.Content,
                    State = appData.State
                },
                sumbitUser = sumbitUser == null ? null : new { RealName = sumbitUser.RealName, Mobile = sumbitUser.Mobile, DeptName = sumbitUser.Dept.DpFullName }
            }
                , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据申请ID获取申请明细
        /// </summary>
        /// <param name="ApplyID"></param>
        /// <returns></returns>
        public ActionResult GetDataByApplyID_AgoEstimateApplyDetail(Guid? ApplyID)
        {
            ApplyID = ApplyID ?? Guid.Parse("00000000-0000-0000-0000-000000000000");
            var detailDatas = _applyDetailService.List().Where(u => u.ApplyID == ApplyID).ToList();
            return Json(detailDatas, JsonRequestBehavior.AllowGet);
        }




        #endregion

    }
}