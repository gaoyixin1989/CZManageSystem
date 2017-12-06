using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.OperatingFloor.ComeBack;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CZManageSystem.Admin.Controllers.OperatingFloor
{
    public class ComeBackApplyController : BaseController
    {

        // GET: ComeBackApply

        IComebackApplyService _comebackApplyService = new ComebackApplyService();
        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();
        ISysDeptmentService _sysDeptmentService = new SysDeptmentService();
        IComebackSourceService _comebackSourceService = new ComebackSourceService();
        IComebackTypeService _comebackTypeService = new ComebackTypeService();

        #region 成本归口申请
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ApplyView(Guid? ApplyId, string type = "look")
        {
            ViewData["ApplyId"] = ApplyId;
            ViewData["type"] = type;
            return View();
        }
        public ActionResult Edit(Guid? ApplyId, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.ComebackApply;
            ViewData["type"] = type;
            ViewData["ApplyId"] = ApplyId.HasValue ? ApplyId.ToString() : null;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的成本归口管理申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }

        
        public ActionResult ApplyInfo()
        {
            return View();
        }

        public ActionResult getApplyInfo()
        {
            object AppInfo = new object();
            if (this.WorkContext.CurrentUser != null)
            {
                AppInfo = new
                {
                   // Applylicant = this.WorkContext.CurrentUser.UserId,
                    ApplyUser = this.WorkContext.CurrentUser.RealName,
                    ApplyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                   // ApplyDpCode = this.WorkContext.CurrentUser.Dept.DpId,
                    ApplyDept = this.WorkContext.CurrentUser.Dept.DpName,
                    Mobile = this.WorkContext.CurrentUser.Mobile,
                    Series = "流程单号待生成",
                    Title = this.WorkContext.CurrentUser.RealName + "的成本归口申请(" + DateTime.Now.ToString("yyyy-MM-dd") + ")"
                };
            }
            return Json(AppInfo);
        }
        /// <summary>
        /// 成本归口申请列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="withDelData"></param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, ComebackApplyQueryBuilder queryBuilder = null, bool withDelData = false)
        {
            int count = 0;
            var modelList = _comebackApplyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }
        /// <summary>
        /// 删除成本归口
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteComebackApply(Guid?[] IDs)
        {
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var ListDatas = _comebackApplyService.List().Where(u => IDs.Contains(u.ApplyId)).ToList();
            allCount = ListDatas.Count;
            ListDatas = ListDatas.Where(u => u.Status != 1).ToList();

            if (allCount > ListDatas.Count)
            {
                strMsg = "其中存在已经提交的申请信息，不能删除";
            }
            else if (ListDatas.Count > 0)
            {
                foreach (var item in ListDatas)
                {
                    successCount++;
                    _comebackApplyService.Delete(item);
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
        /// <param name="ApplyId"></param>
        /// <returns></returns>
        public ActionResult ComebackApplyGetByID(Guid ApplyId)
        {
            var model = _comebackApplyService.FindById(ApplyId);
            var modelList = new
            {
                model.ApplyId,
                model.WorkflowInstanceId,
                model.Title,
                model.Series,
                model.Mobile,
                model.Status,
                ApplyTime= model.ApplyTime.HasValue ? model.ApplyTime.Value.ToString("yyyy-MM-dd") : "",
                model.ApplyDept,
                model.ApplyUser,
                model.BudgetDept,
                model.SourceTypeID,
                TimeStart = model.TimeStart.HasValue ? model.TimeStart.Value.ToString("yyyy-MM-dd") : "",
                TimeEnd = model.TimeEnd.HasValue ? model.TimeEnd.Value.ToString("yyyy-MM-dd") : "",
                model.SourceChildId,
                model.ProjName,
                model.PrevProjName,
                model.PrevProjCode,
                model.ProjManager,
                model.AppAmount,
                model.ProjAnalysis,
                model.Year,
                model.Remark,
                model.AppAmountHanshui
            };
            return Json(new { items = modelList });
        }

        public ActionResult Save_ComebackApply(ComebackApply dataObj)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (dataObj.Title == null || string.IsNullOrEmpty(dataObj.Title.Trim()))
                tip = "标题不能为空";
            else
            {
                isValid = true;
                dataObj.Title = dataObj.Title.Trim();
            }

            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (dataObj.ApplyId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {//新增
                dataObj.ApplyId = Guid.NewGuid();
                dataObj.Status = 0;
                dataObj.ApplyUser = this.WorkContext.CurrentUser.RealName;
                dataObj.ApplyDept = this.WorkContext.CurrentUser.Dept.DpName;
                dataObj.Mobile = this.WorkContext.CurrentUser.Mobile;
                result.IsSuccess = _comebackApplyService.Insert(dataObj);
            }
            else
            {//编辑
                result.IsSuccess = _comebackApplyService.Update(dataObj);
            }

            if (result.IsSuccess)
                result.Message = dataObj.ApplyId.ToString();
            return Json(result);
        }

        /// <summary>
        /// 发起流程工单
        /// </summary>
        /// <param name="ApplyId">ApplyId</param>
        /// <returns></returns>
        public ActionResult Sumbit_ComebackApply(Guid ApplyId, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _comebackApplyService.FindById(ApplyId);
            if (curData == null || curData.ApplyId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                result.IsSuccess = false;
                result.Message = "该暂估成本归口单信息不存在";
                return Json(result);
            }

            result = Sumbit_ComebackApply_WF(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _comebackApplyService.FindById(ApplyId);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }
            return Json(result);
        }

        #region 成本归口的流程处理
        /// <summary>
        /// 提交成本归口
        /// </summary>
        public SystemResult Sumbit_ComebackApply_WF(ComebackApply curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.ComebackApply, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.ComebackApply, curData.Title, curData.ApplyId.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ApplyId.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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
                    curData.Status = 1;
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Series = workFlow.SheetId;
                    curData.ApplyTime = DateTime.Now;

                    result.IsSuccess = _comebackApplyService.Update(curData);
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

        #region
        /// <summary>
        /// 成本归口申请信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryBuilder"></param>
        /// <param name="withDelData"></param>
        /// <returns></returns>
        public ActionResult GetApplyInfoListData(int pageIndex = 1, int pageSize = 5, ComebackInfoQueryBuilder queryBuilder = null, bool withDelData = false)
        {
            int count = 0;
            var modelList = _comebackApplyService.GetApplyInfoListData(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize);

            return Json(new { items = modelList, count = count });
        }

        #endregion

        #region 其他

        /// <summary>
        ///获取资源类别
        /// </summary>
        /// <param name="CorpList"></param>
        public ActionResult GetSourceList(string BudgetDept,int? Year)
        {
            var NameList = _comebackTypeService.List().Select(s => new {
                Name=s.ComebackSource.Name,
                ID = s.ID,
                Year= s.ComebackSource.Year,
                BudgetDept= s.BudgetDept
            }).ToList();
            if (Year!=0)
            {
                NameList= NameList.Where(s => s.Year == Year).ToList();
            }
            if (!string.IsNullOrEmpty(BudgetDept))
            {
                NameList= NameList.Where(s => s.BudgetDept == BudgetDept).ToList();
            }
            return Json(new
            {
                NameList = NameList
            });
        }
        #endregion
    }
}