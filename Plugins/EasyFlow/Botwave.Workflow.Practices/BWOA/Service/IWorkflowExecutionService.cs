using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using Botwave.Workflow;
using Botwave.Workflow.Practices.BWOA.Domain;
using Botwave.Workflow.Domain;
using Botwave.Security.Domain;

namespace Botwave.Workflow.Practices.BWOA.Service
{
    public interface IWorkflowExecutionService
    {
        /// <summary>
        /// 获取日常报销流水帐

        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetApplyInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup, string fieldOrder, string stWhere, ref int recordCount);

        /// <summary>
        /// 获取申请流水帐

        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strwhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetApplicationInfoPage(Guid? workflowId, string startDT, string endDT, int pageIndex, int pageSize, string strwhere, ref int recordCount);

         /// <summary>
        /// 获取采购数据
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetPurchaseInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup, string fieldOrder, string stWhere, ref int recordCount);

        /// <summary>
        /// 获取系统部署数据
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetSysDeployInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup, string fieldOrder, string stWhere, ref int recordCount);

        /// <summary>
        /// 获取文档验收数据
        /// </summary>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="fieldShow"></param>
        /// <param name="fieldGroup"></param>
        /// <param name="stWhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetDocInspectInfoPage(string startDT, string endDT, int pageIndex, int pageSize, string fieldShow, string fieldGroup, string fieldOrder, string stWhere, ref int recordCount);

        /// <summary>
        /// 费用报销申请流水帐

        /// </summary>
        /// <param name="context"></param>
        void SaveFlowWaterAccount(WorkflowInstance workflowInstance, ActivityExecutionContext context);

        /// <summary>
        /// 获取项目/部门对应的PM关系
        /// </summary>
        /// <returns></returns>
        DataTable GetDeptProjectInfo();

        //根据用户名获取采购工单

        DataTable GetPurchaseInfoByUserId(string userId);

        //获取年假天数
        DataSet GetTotalYearInfo(string userName);

        /// <summary>
        /// 获取所有的流程定义(除加班申请流程和请假申请流程)
        /// </summary>
        /// <returns></returns>
        IList<WorkflowDefinition> GetWorkflowDefinitionListNew();

         /// <summary>
        /// 获取待阅的通知人

        /// </summary>
        /// <param name="workFlowID"></param>
        /// <returns></returns>
        IList<UserInfo> GetNotifyActors(Guid workFlowID);



        /// <summary>
        /// 保存假期流水账

        /// </summary>
        /// <param name="creator"></param>
        /// <param name="workflowId"></param>
        /// <param name="context"></param>
        void SaveFlowLeaveWaterAccount(string creator,Guid workflowId, Guid workflowInstanceId, ActivityExecutionContext context);

        //保存采购流水账

        void SavePurchaseWaterAccount(Guid workflowId, Guid workflowInstanceId, ActivityExecutionContext context);

        //保存系统部署流水账

        void SaveSysDeployWaterAccount(Guid workflowId, Guid workflowInstanceId,string title, ActivityExecutionContext context);

         //保存文档验收流水账

        void SaveDocInspectWaterAccount(Guid workflowId, Guid workflowInstanceId, string title, ActivityExecutionContext context);

        /// <summary>
        /// 获取请假数据
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strwhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetLeaveInfoPage(Guid? workflowId, string startDT, string endDT, int pageIndex, int pageSize, string strwhere, ref int recordCount);

        /// <summary>
        /// 获取加班数据
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="startDT"></param>
        /// <param name="endDT"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="strwhere"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        DataTable GetOverTimeInfoPage(Guid? workflowId, string startDT, string endDT, int pageIndex, int pageSize, string strwhere, ref int recordCount);

        /// <summary>
        /// 发送指定流程步骤的邮件提醒和短信提醒.
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromMobile"></param>
        /// <param name="nextActivityIntances"></param>
        /// <param name="_workflowSetting">流程提醒设置.</param>
        /// <param name="operateType"></param>
        void SendMessage(string fromEmail, string fromMobile, IList<ActivityInstance> nextActivityIntances, WorkflowSetting _workflowSetting, int operateType,string nextUserName);

       
        /// <summary>
        /// 保存还款流水帐

        /// </summary>
        /// <param name="ActivityID"></param>
        /// <param name="context"></param>
        void SaveRepaymentWaterAccount(Guid ActivityID, ActivityExecutionContext context);

        DataTable GetTaskListByUserName(string userName, string workflowName, string condition, int pageIndex, int pageSize, ref int recordCount);

        DataTable GetWorkflowTrackingPager(string workflowName, string keywords, string startDT, string endDT, int pageIndex, int pageSize, ref int recordCount);


        void DeleteApplyInfo(Guid workflowinstanceid);

        void DeleteApplicationInfo(Guid workflowinstanceid);

        void DeleteLeaveInfo(Guid workflowinstanceid);

        //删除采购流水账

        void DeletePurchaseInfo(Guid workflowinstanceid);

        void DeleteTrackingWorkflows(Guid workflowinstanceid);

        void DeleteTrackingActivity(Guid workflowinstanceid);

        void ReSaveFlowWaterAccount(Guid workflowID, Guid workflowInstanceID, ActivityExecutionContext context, string creator, string workflowName);

        /// <summary>
        /// 判断是否存在流水账

        /// </summary>
        /// <param name="ApplyName"></param>
        /// <param name="ApplyType"></param>
        /// <param name="Depts"></param>
        /// <param name="InvoiceNum"></param>
        /// <param name="HappenDate"></param>
        /// <param name="CashNum"></param>
        /// <returns></returns>
        int ExistApplyInfo(string ApplyName, string ApplyType, string Depts, string InvoiceNum, string HappenDate, string CashNum);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        DataTable GetUserList();
    }
}
