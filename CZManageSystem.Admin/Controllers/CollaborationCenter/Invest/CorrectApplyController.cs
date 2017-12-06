using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.Invest;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.CollaborationCenter.Invest
{
    public class CorrectApplyController : BaseController
    {
        IInvestCorrectApplySubListService _investCorrectApplySubListService = new InvestCorrectApplySubListService();
        IInvestCorrectApplyService _investCorrectApplyService = new InvestCorrectApplyService();
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
        IInvestContractService _contractService = new InvestContractService();//合同信息
        // GET: CorrectApply

        #region 纠正申请
        public ActionResult ApplyList()
        {
            return View();
        }
        public ActionResult ApplyView(Guid? ID)
        {
            ViewData["ID"] = ID;
            return View();
        }
        public ActionResult Apply(Guid? ID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ConsumableSporadic;
            ViewData["type"] = type;
            ViewData["ID"] = ID.HasValue ? ID.ToString() : null;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的暂估纠正申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }
        public ActionResult getApplyInfo()
        {
            object AppInfo = new object();
            if (this.WorkContext.CurrentUser != null)
            {
                AppInfo = new
                {
                    Applylicant = this.WorkContext.CurrentUser.UserId,
                    ApplyName = this.WorkContext.CurrentUser.RealName,
                    ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ApplyDpCode = this.WorkContext.CurrentUser.Dept.DpId,
                    DpName = this.WorkContext.CurrentUser.Dept.DpName,
                    Mobile = this.WorkContext.CurrentUser.Mobile,
                    // Series = "流程单号待生成",
                    Title = this.WorkContext.CurrentUser.RealName + "的纠正申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")"
                };
            }
            return Json(AppInfo);
        }
        /// <summary>
        /// 纠正申请列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="withDelData"></param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, CorrectApplyQueryBuilder queryBuilder = null, bool withDelData = false)
        {
            int count = 0;
            var modelList = _investCorrectApplyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 删除纠正申请
        /// </summary>
        /// <param name="ConsumingIDs">报废申请ID</param>
        /// <returns></returns>
        public ActionResult DeleteCorrectApply(Guid?[] IDs)
        {
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var ListDatas = _investCorrectApplyService.List().Where(u => IDs.Contains(u.ID)).ToList();//领用申请信息
            allCount = ListDatas.Count;
            ListDatas = ListDatas.Where(u => u.State != 1).ToList();

            if (allCount > ListDatas.Count)
            {
                strMsg = "其中存在已经提交的申请信息，不能删除";
            }
            else if (ListDatas.Count > 0)
            {
                foreach (var item in ListDatas)
                {
                    if (_investCorrectApplyService.Delete(item))
                    {
                        successCount++;
                        var detailData = _investCorrectApplySubListService.List().Where(u => u.ApplyID == item.ID).ToList();
                        _investCorrectApplySubListService.DeleteByList(detailData);
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
        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult CorrectApplyGetListByApplyID(Guid ID)
        {
            var model = _investCorrectApplySubListService.List().Where(u => u.ApplyID == ID).ToList();
            decimal? TaxCount = 0;
            var modelList = model.ToList().Select(u => new
            {
                u.Month,
                u.Year,
                u.ID,
                u.EstimateID,
                u.ProjectID,
                u.ContractID,
                u.ProjectName,
                u.ContractName,
                u.Supply,
                u.SignTotal,
                u.PayTotal,
                u.Study,
                //ManagerName = u.ManagerID == null ? "" : u.ManagerObj?.RealName,
                //Dpfullname = u.EstimateUserID == null ? "" : u.UserObj?.Dept?.DpName,
                u.Course,
                u.BackRate,
                u.Rate,
                u.Pay,
                u.NotPay,
                Tax = GetTax(u.ProjectID, u.ContractID, ref TaxCount)
            });
          var  total = new
            {
                SignTotalCount = model.Sum(u => u.SignTotal ?? 0),//合同金额
                PayTotalCount = model.Sum(u => u.PayTotal ?? 0),//实际合同金额
                TaxCount = TaxCount,//合同税金额
                NotPayCount = model.Sum(u => u.NotPay ?? 0),//暂估金额
                PayCount = model.Sum(u => u.Pay ?? 0)//已付款金额

            };
           
            return Json(new { items = modelList, Total= total });
        }

        public ActionResult CorrectApplyGetByID(Guid ID)
        {
            var modelList = _investCorrectApplyService.FindById(ID);
            List<object> listResult = new List<object>();
            listResult.Add(new
            {
                modelList.ID,
                modelList.WorkflowInstanceId,
                modelList.Series,
                modelList.ApplyTime,
                DpName = CommonFunction.getDeptNamesByIDs(modelList.ApplyDpCode),
                ApplyName = CommonFunction.getUserRealNamesByIDs(modelList.Applicant),
                modelList.Mobile,
                modelList.Title,
                modelList.Content,
                modelList.State
            });

            return Json(new { items = listResult });
        }

        public ActionResult DetaildeleteByID(Guid ID)
        {
            var detail = _investCorrectApplySubListService.FindById(ID);
            var isSuccess = _investCorrectApplySubListService.Delete(detail);
            return Json(new { IsSuccess = isSuccess });
        }
        public ActionResult Save_CorrectApply(InvestCorrectApply dataObj, List<InvestEstimate> details = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else if (dataObj.Content == null || string.IsNullOrEmpty(dataObj.Content.Trim()))
                tip = "备注不能为空";
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

            if (dataObj.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {//新增
                dataObj.ID = Guid.NewGuid();
                dataObj.State = 0;
                dataObj.Applicant = this.WorkContext.CurrentUser.UserId.ToString();
                dataObj.ApplyDpCode = this.WorkContext.CurrentUser.DpId;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _investCorrectApplyService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _investCorrectApplyService.Update(dataObj);
            }

            //信息保存成功，则继续保存更新明细
            if (result.IsSuccess)
            {
                int count = 0;
                //先删除原来的明细数据
                var oldData = _investCorrectApplySubListService.GetForPaging(out count, new { ApplyID = dataObj.ID }).Select(u => (InvestCorrectApplySubList)u).ToList();
                _investCorrectApplySubListService.DeleteByList(oldData);
                if (details != null && details.Count > 0)
                {
                    List<InvestCorrectApplySubList> listDetail = new List<InvestCorrectApplySubList>();
                    foreach (var item in details)
                    {
                        InvestCorrectApplySubList tempData = new InvestCorrectApplySubList();
                        tempData.ID = Guid.NewGuid();
                        tempData.ApplyID = dataObj.ID;
                        tempData.EstimateID = item.ID;
                        tempData.Year = item.Year;

                        tempData.Month = item.Month;
                        tempData.ProjectName = item.ProjectName;
                        tempData.ProjectID = item.ProjectID;
                        tempData.ContractName = item.ContractName;
                        tempData.ContractID = item.ContractID;
                        tempData.Supply = item.Supply;
                        tempData.SignTotal = item.SignTotal;
                        tempData.PayTotal = item.PayTotal;
                        tempData.Study = item.Study;

                        tempData.ManagerID = item.ManagerID;
                        tempData.Course = item.Course;
                        tempData.BackRate = item.BackRate;
                        tempData.Rate = item.Rate;
                        tempData.Pay = item.Pay;

                        tempData.NotPay = item.NotPay;
                        tempData.EstimateUserID = item.EstimateUserID;
                        listDetail.Add(tempData);
                    }
                    if (listDetail.Count > 0)
                        result.IsSuccess = _investCorrectApplySubListService.InsertByList(listDetail);
                    else
                        result.IsSuccess = true;
                    if (!result.IsSuccess)
                    {
                        result.Message = "保存暂估申请明细信息失败";
                    }
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "保存暂估申请信息失败";
            }

            if (result.IsSuccess)
                result.Message = dataObj.ID.ToString();
            return Json(result);
        }

        /// <summary>
        /// 暂估纠正(发起流程工单)
        /// </summary>
        /// <param name="ID">纠正ID</param>
        /// <returns></returns>
        public ActionResult Sumbit_CorrectApply(Guid ID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _investCorrectApplyService.FindById(ID);
            if (curData == null || curData.ID == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该暂估纠正申请单信息不存在";
                return Json(result);
            }

            //int count = 0;
            ////获取当前暂估纠正的明细

            //var detailData = _investCorrectApplySubListService.List().Where(u => u.ApplyID == curData.ID).ToList();
            //int detailDataAmount = detailData.Sum(u => u.ApplyCount ?? 0);
            //if (detailDataAmount <= 0)
            //{
            //    result.IsSuccess = false;
            //    result.Message = "该报纠正耗材中的总量小于1，请正确填写信息";
            //    return Json(result);
            //}
            result = Sumbit_CorrectApply_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _investCorrectApplyService.FindById(ID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }
            return Json(result);
        }

        #region 耗材纠正申请的流程处理
        /// <summary>
        /// 提交暂估纠正申请
        /// </summary>
        public SystemResult Sumbit_CorrectApply_WF(InvestCorrectApply curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.InvestCorrectApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.InvestCorrectApply, curData.Title, curData.ID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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

                    result.IsSuccess = _investCorrectApplyService.Update(curData);
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
        #endregion
        #endregion

        decimal? GetTax(string ProjectID, string ContractID, ref decimal? TaxCount)
        {
            var model = _contractService.FindByFeldName(t => t.ProjectID == ProjectID && t.ContractID == ContractID);
            if (model == null)
                return 0;
            TaxCount += model.Tax;
            return model.Tax;
        }
    }
}