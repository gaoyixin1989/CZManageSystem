using CZManageSystem.Core;
using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Service.CollaborationCenter.MarketOrder;
using CZManageSystem.Service.HumanResources.Vacation;
using System;
using WebService_WF.Base;
using WebService_WF.Domain;

namespace WebService_WF.ExecuteFuns
{
    /// <summary>
    /// 休假申请流程
    /// </summary>
    public class MarketOrder_OrderApply_YX_Execute
    {
        public WorkflowDetail workflowDetail;
        public MarketOrder_OrderApply curApplyData;
        public string F1;//工单数据id
        public string AreaName;//所属地区
        public string BossOfferID;//BOSS商品标识
        public string MailNo;//邮件号码
        public string Authentication;//鉴权方式
        public string IMEI;//IMEI码
        public string SendState;//是否配送成功
        public string SendState_Remark;//配送结果的原因
        public string isOpenState;//是否开户成功
        public string VisitNeed;//是否需要回访
        public string VisitState;//回访情况
        public string VisitState_Remark;//原因
        public string DegreeState;//满意度
        public string DegreeState_Remark;//原因
        public string CallbackState;//是否回收成功
        public string CallbackState_Remark;//回收说明


        IMarketOrder_OrderApplyService _applyService = new MarketOrder_OrderApplyService();//工单信息

        public string step0 = WfSectionGroup.MarketOrder_OrderApply_YX["0"].ToString();
        public string step1 = WfSectionGroup.MarketOrder_OrderApply_YX["1"].ToString();
        public string step2 = WfSectionGroup.MarketOrder_OrderApply_YX["2"].ToString();
        public string step3 = WfSectionGroup.MarketOrder_OrderApply_YX["3"].ToString();
        public string step4 = WfSectionGroup.MarketOrder_OrderApply_YX["4"].ToString();
        public string step5 = WfSectionGroup.MarketOrder_OrderApply_YX["5"].ToString();
        public string step6 = WfSectionGroup.MarketOrder_OrderApply_YX["6"].ToString();
        public string step7 = WfSectionGroup.MarketOrder_OrderApply_YX["7"].ToString();
        public MarketOrder_OrderApply_YX_Execute(WorkflowDetail obj)
        {
            workflowDetail = obj;

            //string confirmStepName = WfSectionGroup.MarketOrder_OrderApply_YX["Confirm"].ToString();
            //if (workflowDetail.PreActivities.Count > 0 && workflowDetail.PreActivities[0].Name == confirmStepName && workflowDetail.PreActivities[0].Command.ToLower() == "approve")
            //{//审核通过
            //    ConfirmStep_Approve();
            //}

            Guid dataID = new Guid();
            F1 = "";
            foreach (Field field in workflowDetail.Fields)
            {
                #region 获取表单字段值
                if (field.Key == "F1")//工单数据id
                {
                    F1 = field.Value;
                }
                else if (field.Key == "AreaName")//所属地区
                {
                    AreaName = field.Value;
                }
                else if (field.Key == "BossOfferID")//BOSS商品标识
                {
                    BossOfferID = field.Value;
                }
                else if (field.Key == "MailNo")//邮件号码
                {
                    MailNo = field.Value;
                }
                else if (field.Key == "Authentication")//鉴权方式
                {
                    Authentication = field.Value;
                }
                else if (field.Key == "IMEI")//IMEI码
                {
                    IMEI = field.Value;
                }
                else if (field.Key == "SendState")//是否配送成功
                {
                    SendState = field.Value;
                }
                else if (field.Key == "SendState_Remark")//配送结果的原因
                {
                    SendState_Remark = field.Value;
                }
                else if (field.Key == "isOpenState")//是否开户成功
                {
                    isOpenState = field.Value;
                }
                else if (field.Key == "VisitNeed")//是否需要回访
                {
                    VisitNeed = field.Value;
                }
                else if (field.Key == "VisitState")//回访情况
                {
                    VisitState = field.Value;
                }
                else if (field.Key == "VisitState_Remark")//原因
                {
                    VisitState_Remark = field.Value;
                }
                else if (field.Key == "DegreeState")//满意度
                {
                    DegreeState = field.Value;
                }
                else if (field.Key == "DegreeState_Remark")//原因
                {
                    DegreeState_Remark = field.Value;
                }
                else if (field.Key == "CallbackState")//是否回收成功
                {
                    CallbackState = field.Value;
                }
                else if (field.Key == "CallbackState_Remark")//回收说明
                {
                    CallbackState_Remark = field.Value;
                }
                #endregion
            }

            if (!Guid.TryParse(F1, out dataID))
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：获取数据ID失败", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }
            curApplyData = _applyService.FindById(dataID);
            if (curApplyData == null || curApplyData.ApplyID == Guid.Empty)
            {
                LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务失败：查询不到对应数据单信息", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.fail);
                return;
            }

            if (workflowDetail.PreActivities.Count > 0)
            {
                if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.cancel)
                    cancelSetp();

                string preStep = workflowDetail.PreActivities[0].Name;
                if (preStep == step0)
                    AfterStep0();
                else if (preStep == step1)
                    AfterStep1();
                else if (preStep == step2)
                    AfterStep2();
                else if (preStep == step3)
                    AfterStep3();
                else if (preStep == step4)
                    AfterStep4();
                else if (preStep == step5)
                    AfterStep5();
                else if (preStep == step6)
                    AfterStep6();
                else if (preStep == step7)
                    AfterStep7();
            }
            LogRecord.WriteLog(string.Format("流程“{0}(受理号：{1})”执行步骤“{2}”后调用服务执行结束", workflowDetail.WorkflowName, workflowDetail.SheetId, workflowDetail.PreActivities[0].Name), LogResult.success);
            return;

        }

        /// <summary>
        /// 取消工单
        /// </summary>
        public void cancelSetp()
        {
            curApplyData.Status = "完成";
            curApplyData.OrderStatus = "失败受理单";
            _applyService.Update(curApplyData);
        }

        /// <summary>
        /// “提单”后处理
        /// </summary>
        public void AfterStep0()
        {
        }

        /// <summary>
        /// “邮政高级人员审核”后处理
        /// </summary>
        public void AfterStep1()
        {
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.approve)//通过审核
            {
                curApplyData.OrderStatus = "配送中";
                curApplyData.YZSubmitTime = DateTime.Now;

                var temp = new MarketOrder_AreaService().FindByFeldName(u => u.DpName == AreaName);
                if (temp != null && temp.ID != Guid.Empty)
                    curApplyData.AreaID = temp.ID;

                curApplyData.BossOfferID = BossOfferID;
                curApplyData.MailNo = MailNo;
                curApplyData.IMEI = IMEI;
                var temp2 = new MarketOrder_AuthenticationService().FindByFeldName(u => u.Authentication == Authentication);
                if (temp2 != null && temp2.ID != Guid.Empty)
                    curApplyData.AuthenticationID = temp2.ID;

                _applyService.Update(curApplyData);
            }
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.reject)//退回工单,回到提单状态
            {
                curApplyData.Status = "编辑";
                curApplyData.OrderStatus = "草稿";
                _applyService.Update(curApplyData);
            }
        }

        /// <summary>
        /// “邮政人员配送处理”后处理
        /// </summary>
        public void AfterStep2()
        {
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.approve)//通过审核
            {
                curApplyData.BossOfferID = BossOfferID;
                curApplyData.MailNo = MailNo;
                curApplyData.IMEI = IMEI;
                var temp = new MarketOrder_AuthenticationService().FindByFeldName(u => u.Authentication == Authentication);
                if (temp != null && temp.ID != Guid.Empty)
                    curApplyData.AuthenticationID = temp.ID;
                _applyService.Update(curApplyData);
            }
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.reject)//退回工单,
            {
                curApplyData.OrderStatus = "待推送";
                _applyService.Update(curApplyData);
            }

        }

        /// <summary>
        /// “邮政人员填写配送情况”后处理
        /// </summary>
        public void AfterStep3()
        {
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.approve)//通过审核
            {
                if (SendState == "是")
                    curApplyData.OrderStatus = "配送成功";
                else
                    curApplyData.OrderStatus = "配送失败";
                _applyService.Update(curApplyData);

                IMarketOrder_OrderSendInfoService _sendInfoService = new MarketOrder_OrderSendInfoService();//配送结果信息

                MarketOrder_OrderSendInfo temp = new MarketOrder_OrderSendInfo();
                temp.ID = Guid.NewGuid();
                temp.ApplyID = curApplyData.ApplyID;
                temp.UserID = BaseFun.FindUserByUserName(workflowDetail.PreActivities[0].Actors)?.UserId;
                temp.Time = DateTime.Now;
                temp.IsSuccess = SendState;
                temp.SendInfo = SendState_Remark;
                _sendInfoService.Insert(temp);

            }
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.reject)//退回工单,
            {
                curApplyData.OrderStatus = "配送中";
                _applyService.Update(curApplyData);
            }
        }

        /// <summary>
        /// “BOSS操作员开户操作”后处理
        /// </summary>
        public void AfterStep4()
        {
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.approve)//通过审核
            {
                if (isOpenState == "是") {
                    curApplyData.OrderStatus = "销售成功";
                    if (VisitNeed == "否")
                    {
                        curApplyData.Status = "完成";
                        curApplyData.OrderStatus = "成功受理单";
                    }
                }
                else
                    curApplyData.OrderStatus = "销售失败";
                _applyService.Update(curApplyData);
                IMarketOrder_OrderBossDealService _bossDealService = new MarketOrder_OrderBossDealService();//开户结果信息
                MarketOrder_OrderBossDeal temp = new MarketOrder_OrderBossDeal();
                temp.ID = Guid.NewGuid();
                temp.ApplyID = curApplyData.ApplyID;
                temp.UserID = BaseFun.FindUserByUserName(workflowDetail.PreActivities[0].Actors)?.UserId;
                temp.Time = DateTime.Now;
                temp.IsSuccess = isOpenState;
                temp.IsVisit = VisitNeed;
                _bossDealService.Insert(temp);
            }
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.reject)//退回工单,
            {
                curApplyData.OrderStatus = "配送中";
                _applyService.Update(curApplyData);
            }

        }

        /// <summary>
        /// “回访员回访”后处理
        /// </summary>
        public void AfterStep5()
        {
            //“回访员回访”该步骤“isOpenState”的值没有正常获取到
            var dd = new MarketOrder_OrderBossDealService().FindByFeldName(u => u.ApplyID == curApplyData.ApplyID);
            if (dd != null)
                isOpenState = dd.IsSuccess;
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.approve)//通过审核
            {

                if (SendState == "是" && isOpenState == "否")
                    curApplyData.OrderStatus = "待回收";
                else if (SendState == "否")
                {
                    curApplyData.Status = "完成";
                    curApplyData.OrderStatus = "失败受理单";
                }
                else if (isOpenState == "是")
                {
                    curApplyData.Status = "完成";
                    curApplyData.OrderStatus = "成功受理单";
                }
                _applyService.Update(curApplyData);
                IMarketOrder_OrderVisitService _visitService = new MarketOrder_OrderVisitService();//开户结果信息

                MarketOrder_OrderVisit temp = new MarketOrder_OrderVisit();
                temp.ID = Guid.NewGuid();
                temp.ApplyID = curApplyData.ApplyID;
                temp.UserID = BaseFun.FindUserByUserName(workflowDetail.PreActivities[0].Actors)?.UserId;
                temp.Time = DateTime.Now;
                if (curApplyData.OrderStatus == "成功受理单")
                {
                    var temp2 = new MarketOrder_SatService().FindByFeldName(u => u.Sat == DegreeState);
                    if (temp2 != null && temp2.ID != Guid.Empty)
                        temp.SatID = temp2.ID;//DegreeState
                    temp.SuccessRemark = DegreeState_Remark;
                }
                else
                {
                    var temp2 = new MarketOrder_VisitService().FindByFeldName(u => u.Visit == VisitState);
                    if (temp2 != null && temp2.ID != Guid.Empty)
                        temp.VisitID = temp2.ID;//VisitState
                    temp.FailRemark = VisitState_Remark;
                }
                _visitService.Insert(temp);
            }
        }

        /// <summary>
        /// “邮政人员回收处理”后处理
        /// </summary>
        public void AfterStep6()
        {
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.approve)//通过审核
            {
                curApplyData.OrderStatus = "回收中";
                _applyService.Update(curApplyData);
            }

        }

        /// <summary>
        /// “邮政人员填写回收情况”后处理
        /// </summary>
        public void AfterStep7()
        {
            if (workflowDetail.PreActivities[0].Command.ToLower() == CommandName.approve)//通过审核
            {
                curApplyData.Status = "完成";
                if (CallbackState == "是")
                    curApplyData.OrderStatus = "回收成功受理单";
                else
                    curApplyData.OrderStatus = "回收失败受理单";
                _applyService.Update(curApplyData);

                IMarketOrder_OrderReclaimService _reclaimService = new MarketOrder_OrderReclaimService();//开户结果信息
                MarketOrder_OrderReclaim temp = new MarketOrder_OrderReclaim();
                temp.ID = Guid.NewGuid();
                temp.ApplyID = curApplyData.ApplyID;
                temp.UserID = BaseFun.FindUserByUserName(workflowDetail.PreActivities[0].Actors)?.UserId;
                temp.Time = DateTime.Now;
                temp.IsReclaim = CallbackState;
                temp.ReclaimRemark = CallbackState_Remark;
                _reclaimService.Insert(temp);
            }

        }


    }

}