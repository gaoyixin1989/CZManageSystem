using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebService_WF.Base
{
    /// <summary>
    /// 流程审核结果类
    /// </summary>
    public class CommandName
    {
        public static string approve = "approve";//通过审核
        public static string reject = "reject";//退回工单
        public static string cancel = "cancel";//取消工单
        public static string save = "save";//保存工单
    }

    public class WfSectionGroup
    {
        //耗材退库申请
        public static IDictionary ConsumableCancelling = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/ConsumableCancelling");

        //耗材领用申请
        public static IDictionary ConsumableApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/ConsumableApply");

        //耗材调平申请
        public static IDictionary ConsumableLevelling = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/ConsumableLevelling");

        //耗材报废申请
        public static IDictionary ConsumableScrap = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/ConsumableScrap");

        //耗材补录归档
        public static IDictionary ConsumableMakeup = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/ConsumableMakeup");

        //设备申请
        public static IDictionary EquipApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/EquipApply");

        //会议室申请
        public static IDictionary BoardroomApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/BoardroomApply");
        //暂估每月流程跟踪
        public static IDictionary InvestMonthEstimateApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/InvestMonthEstimateApply");

        //历史项目暂估申请
        public static IDictionary InvestAgoEstimateApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/InvestAgoEstimateApply");

        //休假申请
        public static IDictionary VacationApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/VacationApply");
        //异常休假申请
        public static IDictionary ReVacationApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/ReVacationApply");
        //销假申请
        public static IDictionary VacationCloseApply = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/VacationCloseApply");
        //营销订单流程
        public static IDictionary MarketOrder_OrderApply_YX = (IDictionary)ConfigurationManager.GetSection("WfSectionGroup/MarketOrder_OrderApply_YX");

    }
}