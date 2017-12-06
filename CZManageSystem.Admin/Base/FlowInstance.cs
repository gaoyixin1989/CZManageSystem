using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CZManageSystem.Admin.Base
{
    /// <summary>
    /// 流程易注册系统信息
    /// </summary>
    public class FlowInstance
    {
        public static readonly string Workflow_SystemID = System.Configuration.ConfigurationManager.AppSettings["Workflow_SystemID"];//
        public static readonly string Workflow_SystemAcount = System.Configuration.ConfigurationManager.AppSettings["Workflow_SystemAcount"];//
        public static readonly string Workflow_SystemPwd = System.Configuration.ConfigurationManager.AppSettings["Workflow_SystemPwd"];//
        public static readonly string Workflow_SystemUrl = System.Configuration.ConfigurationManager.AppSettings["Workflow_SystemUrl"];

        public class WorkflowType
        {
            /// <summary>
            /// 会议室申请
            /// </summary>
            public static readonly string BoardroomApply = "会议室申请";
            public static readonly string EquipApply = "办公设备申请";
            public static readonly string VoiceApply = "基层心声";
            public static readonly string VoteApply = "投票申请流程";
            public static readonly string ConsumingApply = "耗材领用申请";
            public static readonly string ConsumingCancelling = "耗材退库申请";
            public static readonly string ConsumingLevelling = "耗材调平申请";
            public static readonly string ConsumableScrap = "耗材报废申请";
            public static readonly string ConsumableSporadic = "耗材零星申请";
            public static readonly string ConsumingMakeup = "耗材补录归档";
            public static readonly string UsedCarsApply = "用车申请";
            public static readonly string PaidCarServiceApply = "员工有偿用车服务申请 ";
            public static readonly string CarApplyRent = "租车申请 ";
            public static readonly string UrgentUsedCarsApply = "紧急用车申请";
            public static readonly string InvestCorrectApply = "暂估纠正申请";
            public static readonly string OverTimeApply = "加班申请";
            public static readonly string InvestAgoEstimateApply = "历史项目暂估申请";
            public static readonly string Abnormal = "考勤异常申报";
            public static readonly string VacationApply = "休假申请";
            public static readonly string ReVacationApply = "异常休假申请";
            public static readonly string VacationCloseApply = "销假申请";
            public static readonly string ComebackApply = "成本归口管理申请";
            public static readonly string MarketOrder_OrderApply_YX = "营销订单流程";
        }

        public class MethodName
        {
            public static readonly string SearchWorkflow = "SearchWorkflow";//查询类别方法
            public static readonly string ManageWorkflow = "ManageWorkflow";//处理类别方法
        }
    }


}