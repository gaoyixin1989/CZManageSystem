using CZManageSystem.Admin.Base;
using CZManageSystem.Admin.Models;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Data.Domain.SysManger;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.SysManger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 营销订单-营销订单工单
/// </summary>
namespace CZManageSystem.Admin.Controllers.CollaborationCenter.MarketOrder
{
    public class MarketOrder_OrderApplyController : BaseController
    {
        IMarketOrder_OrderApplyService _applyService = new MarketOrder_OrderApplyService();//工单信息
        ISysUserService _userService = new SysUserService();//用户

        ITracking_WorkflowService _tracking_WorkflowService = new Tracking_WorkflowService();//流程

        /// <summary>
        /// 营销订单列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult YXList()
        {
            return View();
        }
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }
        /// <summary>
        /// 办理情况统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Statistics()
        {
            return View();
        }

        public ActionResult Edit_YX(Guid? ApplyID, string type = "look")
        {
            ViewData["workflowName"] = FlowInstance.WorkflowType.MarketOrder_OrderApply_YX;
            ViewData["type"] = type;
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            ViewData["RealName"] = this.WorkContext.CurrentUser.RealName;
            ViewData["Mobile"] = this.WorkContext.CurrentUser.Mobile;
            ViewData["NewTitle"] = this.WorkContext.CurrentUser.RealName + "的营销订单申请" + DateTime.Now.ToString("(yyyy-MM-dd)");
            return View();
        }

        public ActionResult YX_WF(Guid? ApplyID)
        {
            ViewData["ApplyID"] = ApplyID.HasValue ? ApplyID.ToString() : null;
            return View();
        }


        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyOrderStatus()
        {
            return View();
        }

        /// <summary>
        /// 营销订单
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">条件</param>
        /// <returns></returns>
        public ActionResult GetStatisticData(OrderApplyQueryBuilder queryBuilder = null)
        {
            int pageIndex = 1;
            int pageSize = int.MaxValue;
            int count = 0;
            if (queryBuilder.ApplyTime_end.HasValue)
                queryBuilder.ApplyTime_end = queryBuilder.ApplyTime_end.Value.AddDays(1).Date.AddSeconds(-1);
            if (queryBuilder.DealTime_end.HasValue)
                queryBuilder.DealTime_end = queryBuilder.DealTime_end.Value.AddDays(1).Date.AddSeconds(-1);

            var modelList = _applyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(item => new StatisticObject()
            {
                Applicant = item.Applicant,
                RealName = item.ApplicantObj?.RealName,
                OrderStatus = item.OrderStatus
            });
            var UserData = modelList.GroupBy(u => u.Applicant).Select(u => new
            {
                Applicant = u.Key,
                RealName = u.First().RealName,
                Count = u.Count()
            }).ToList();

            var DetailData = modelList.GroupBy(u => new
            {
                u.Applicant,
                u.OrderStatus
            }).OrderBy(u => u.Key.Applicant).Select(u => new
            {
                u.Key.Applicant,
                u.Key.OrderStatus,
                u.First().RealName,
                Count = u.Count()
            }).ToList();

            return Json(new { UserData = UserData, DetailData = DetailData });
        }

        /// <summary>
        /// 营销订单
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">条件</param>
        /// <returns></returns>
        public ActionResult GetListData(int pageIndex = 1, int pageSize = 5, OrderApplyQueryBuilder queryBuilder = null, bool isManager = false)
        {
            int count = 0;
            if (!isManager && this.WorkContext.CurrentUser.UserName != Base.Admin.UserName)
                queryBuilder.Applicant = this.WorkContext.CurrentUser.UserId;

            if (queryBuilder.ApplyTime_end.HasValue)
                queryBuilder.ApplyTime_end = queryBuilder.ApplyTime_end.Value.AddDays(1).Date.AddSeconds(-1);
            if (queryBuilder.DealTime_end.HasValue)
                queryBuilder.DealTime_end = queryBuilder.DealTime_end.Value.AddDays(1).Date.AddSeconds(-1);

            var modelList = _applyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(item => new
            {
                item.ApplyID,
                item.WorkflowInstanceId,
                //item.Tracking_Workflow,
                SerialNo = string.IsNullOrEmpty(item.SerialNo) ? "提交时自动生成" : item.SerialNo,
                item.ApplyTime,
                item.Applicant,
                RealName = item.ApplicantObj?.RealName,
                item.MobilePh,
                item.Status,
                item.OrderStatus,
                item.Title,
                item.MarketID,
                MarketText = item.MarketObj?.Market,
                item.CustomPhone,
                item.CustomName,
                item.CustomPersonID,
                item.CustomAddr,
                item.CustomOther,
                item.EndTypeID,
                EndTypeText = item.EndTypeObj?.EndType,
                item.UseNumber,
                item.SIMNumber,
                item.IMEI,
                item.SetmealID,
                SetmealText = item.SetmealObj?.Setmeal,
                item.BusinessID,
                BusinessText = item.BusinessObj?.Business,
                item.Remark,
                item.AreaID,
                AreaText = item.AreaObj?.DpName,
                item.ProjectID,
                item.YZSubmitTime,
                item.SendStatus,
                item.GDOrderID,
                item.BossOfferID,
                item.MainOrder,
                item.SubOrder,
                item.MailNo,
                item.SendTo
            });

            return Json(new { items = modelList, count = count });
        }


        /// <summary>
        /// 营销订单——查询数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">条件</param>
        /// <returns></returns>
        public ActionResult GetListData_YX(int pageIndex = 1, int pageSize = 5, OrderApplyQueryBuilder queryBuilder = null, bool isManager = false)
        {
            int count = 0;
            queryBuilder.isJK = false;
            if (!isManager && this.WorkContext.CurrentUser.UserName != Base.Admin.UserName)
                queryBuilder.Applicant = this.WorkContext.CurrentUser.UserId;

            var modelList = _applyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => new
            {
                u.ApplyID,
                SerialNo = string.IsNullOrEmpty(u.SerialNo) ? "提交时自动生成" : u.SerialNo,
                u.Title,
                u.ApplyTime,
                u.Status,
                u.OrderStatus
            });

            return Json(new { items = modelList, count = count });
        }

        /// <summary>
        /// 家宽业务订单——查询数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数据量</param>
        /// <param name="queryBuilder">条件</param>
        /// <returns></returns>
        public ActionResult GetListData_JK(int pageIndex = 1, int pageSize = 5, OrderApplyQueryBuilder queryBuilder = null, bool isManager = false)
        {
            int count = 0;
            queryBuilder.isJK = true;
            if (!isManager && this.WorkContext.CurrentUser.UserName != Base.Admin.UserName)
                queryBuilder.Applicant = this.WorkContext.CurrentUser.UserId;

            var modelList = _applyService.GetForPaging(out count, queryBuilder, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize).Select(u => new
            {
                u.ApplyID,
                u.SerialNo,
                u.Title,
                u.ApplyTime,
                u.SendTo,
                SendToText = string.IsNullOrEmpty(u.SendTo) ? null : _userService.FindByFeldName(a => a.UserName == u.SendTo)?.RealName
            });

            return Json(new { items = modelList, count = count });
        }


        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="InputListID">入库单ID</param>
        /// <returns></returns>
        public ActionResult Delete(Guid[] ApplyIDs)
        {
            int allCount = 0;
            int successCount = 0;
            string strMsg = "";

            var listDatas = _applyService.List().Where(u => ApplyIDs.Contains(u.ApplyID)).ToList();//订单信息
            allCount = listDatas.Count;
            listDatas = listDatas.Where(u => !u.WorkflowInstanceId.HasValue).ToList();

            if (allCount > listDatas.Count)
            {
                strMsg = "其中存在已经提交的信息，不能删除";
            }
            else if (listDatas.Count > 0)
            {
                foreach (var item in listDatas)
                {
                    if (_applyService.Delete(item))
                    {
                        AddOperationLog(OperationType.Delete, "营销订单删除数据成功-" + item.ApplyID);
                        successCount++;
                    }
                    else
                        AddOperationLog(OperationType.Delete, "营销订单删除数据失败-" + item.ApplyID);
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
        /// 根据ID获取订单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetDataByID(Guid id)
        {
            var item = _applyService.FindById(id);
            object Apply = new
            {
                item.ApplyID,
                item.WorkflowInstanceId,
                //item.Tracking_Workflow,
                item.SerialNo,
                item.ApplyTime,
                item.Applicant,
                RealName = item.ApplicantObj?.RealName,
                item.MobilePh,
                item.Status,
                item.OrderStatus,
                item.Title,
                item.MarketID,
                MarketText = item.MarketObj?.Market,
                PlanPay=item.MarketObj?.PlanPay,
                MustPay=item.MarketObj?.MustPay,
                item.CustomPhone,
                item.CustomName,
                item.CustomPersonID,
                item.CustomAddr,
                item.CustomOther,
                item.EndTypeID,
                EndTypeText = item.EndTypeObj?.EndType,
                item.UseNumber,
                item.SIMNumber,
                item.IMEI,
                item.SetmealID,
                SetmealText = item.SetmealObj?.Setmeal,
                item.BusinessID,
                BusinessText = item.BusinessObj?.Business,
                item.Remark,
                item.AreaID,
                AreaText=item.AreaObj?.DpName,
                item.ProjectID,
                item.YZSubmitTime,
                item.SendStatus,
                item.GDOrderID,
                item.BossOfferID,
                item.MainOrder,
                item.SubOrder,
                item.MailNo,
                item.SendTo
            };
            return Json(new
            {
                Apply = Apply
            });
        }


        /// <summary>
        /// 保存申请单——营销订单
        /// </summary>
        /// <returns></returns>
        public ActionResult Save_Apply_YX(MarketOrder_OrderApply Apply)
        {
            SystemResult result = new SystemResult() { IsSuccess = false };
            #region 验证数据是否合法
            string tip = "";
            bool isValid = false;//是否验证通过
            if (Apply.Title == null || string.IsNullOrEmpty(Apply.Title.Trim()))
                tip = "标题不能为空";
            else if (!Apply.MarketID.HasValue || Apply.MarketID == Guid.Empty)
                tip = "营销方案不能为空";
            else if (!Apply.EndTypeID.HasValue || Apply.EndTypeID == Guid.Empty)
                tip = "终端机型不能为空";
            else if (Apply.CustomPhone == null || string.IsNullOrEmpty(Apply.CustomPhone.Trim()))
                tip = "目标客户不能为空";
            else if (!CommonFunction.isMobilePhone(Apply.CustomPhone.Trim()))
                tip = "目标客户必须为有效的手机号码";
            else if (Apply.CustomName == null || string.IsNullOrEmpty(Apply.CustomName.Trim()))
                tip = "客户名称不能为空";
            else if (Apply.CustomOther == null || string.IsNullOrEmpty(Apply.CustomOther.Trim()))
                tip = "联系电话不能为空";
            else if (Apply.CustomAddr == null || string.IsNullOrEmpty(Apply.CustomAddr.Trim()))
                tip = "联系地址不能为空";


            if (string.IsNullOrEmpty(tip))
            {
                isValid = true;
                Apply.Title = Apply.Title.Trim();
                Apply.CustomPhone = Apply.CustomPhone.Trim();
                Apply.CustomName = Apply.CustomName.Trim();
                Apply.CustomOther = Apply.CustomOther.Trim();
                Apply.CustomAddr = Apply.CustomAddr.Trim();

            }
            if (!isValid)
            {
                result.IsSuccess = false;
                result.Message = tip;
                return Json(result);
            }
            #endregion

            if (Apply.ApplyID == Guid.Empty)
            {//新增
                Apply.ApplyID = Guid.NewGuid();
                Apply.Status = "编辑";
                Apply.OrderStatus = "草稿";
                Apply.ApplyTime = DateTime.Now;
                Apply.Applicant = this.WorkContext.CurrentUser.UserId;

                result.IsSuccess = _applyService.Insert(Apply);
                if (result.IsSuccess)
                    AddOperationLog(OperationType.Add, "营销订单添加数据成功-" + Apply.ApplyID);
                else
                    AddOperationLog(OperationType.Add, "营销订单添加数据失败");

            }
            else
            {//编辑
                result.IsSuccess = _applyService.Update(Apply);
                if (result.IsSuccess)
                    AddOperationLog(OperationType.Edit, "营销订单编辑数据成功-" + Apply.ApplyID);
                else
                    AddOperationLog(OperationType.Edit, "营销订单编辑数据失败=" + Apply.ApplyID);
            }

            if (!result.IsSuccess)
            {
                result.Message = "保存失败";
            }
            else
            {
                result.Message = Apply.ApplyID.ToString();
            }
            return Json(result);
        }

        //提交申请单——营销订单
        public ActionResult Sumbit_Apply_YX(Guid applyID, string nextActivity = null, string nextActors = null, string nextCC = null)
        {
            SystemResult result = new SystemResult() { IsSuccess = false, Message = "" };

            var curData = _applyService.FindById(applyID);
            if (curData == null || curData.ApplyID == Guid.Empty)
            {
                result.IsSuccess = false;
                result.Message = "该申请单信息不存在";
                return Json(result);
            }

            result = Sumbit_AgoEstimateApply_WF_YX(curData, nextActivity, nextActors);
            if (result.IsSuccess && !string.IsNullOrEmpty(nextCC))
            {//提交成功后抄送
                curData = _applyService.FindById(curData.ApplyID);
                CommonFunction.PendingData(curData.WorkflowInstanceId.Value, nextCC);//抄送
            }

            return Json(result);
        }

        /// <summary>
        /// 提交申请_营销订单
        /// </summary>
        public SystemResult Sumbit_AgoEstimateApply_WF_YX(MarketOrder_OrderApply curData, string nextActivity, string nextActors)
        {
            if (string.IsNullOrEmpty(nextActivity) || string.IsNullOrEmpty(nextActors))
            {
                CommonFunction.GetFirstOperatorInfoAfterSumbit(FlowInstance.WorkflowType.MarketOrder_OrderApply_YX, this.WorkContext.CurrentUser.UserName, out nextActivity, out nextActors);
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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, FlowInstance.WorkflowType.MarketOrder_OrderApply_YX, curData.Title, curData.ApplyID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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
                objectXML = string.Format(objectXML, this.WorkContext.CurrentUser.UserName, tempActivity.ActivityInstanceId.ToString(), curData.Title, curData.ApplyID.ToString(), nextActivity, nextActors, DateTime.Now.AddYears(100).ToString("yyyy-MM-dd"));

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
                    curData.WorkflowInstanceId = Guid.Parse(strWorkflowInstanceId);
                    curData.Status = "审批中";
                    curData.OrderStatus = "待推送";
                    curData.SerialNo = workFlow.SheetId;

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



    }

    public class StatisticObject
    {
        public Guid? Applicant { get; set; }
        public string RealName { get; set; }
        public string OrderStatus { get; set; }


    }
}