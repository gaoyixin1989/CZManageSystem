using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Workflow.Routing.Domain;
using System.Data;

namespace Botwave.Workflow.Routing.Implements
{
    /// <summary>
    /// 路由规则业务接口
    /// </summary>
    public interface IActivityRulesService
    {
        /// <summary>
        /// 获取一条规则
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        RulesDetail GetActivityRule(Guid ruleId);

        /// <summary>
        /// 获取当前步骤到下行步骤的所有规则
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        DataTable GetActivityRules(string workflowId, string activityName, int pageIndex, int pageSize, ref int recordCount);

        /// <summary>
        /// 获取起始步骤规则
        /// </summary>
        /// <param name="workflowid">流程实例ID</param>
        /// <param name="nextActivityid">下行步骤ID</param>
        /// <returns></returns>
        RulesDetail GetStartActivityRules(string workflowid, string activityName, string nextActivityName);

        /// <summary>
        /// 获取下行步骤规则
        /// </summary>
        /// <param name="workflowinstanceid">表单实例ID</param>
        /// <param name="activityid">当前步骤ID</param>
        /// <param name="nextActivityid">下行步骤ID</param>
        /// <returns></returns>
        RulesDetail GetNextActivityRules(string workflowid, string activityName, string nextActivityName);

        /// <summary>
        /// 获取步骤规则集合
        /// </summary>
        /// <param name="WorkflowId"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        IList<RulesDetail> GetActivityRules(string WorkflowId, string activityName);
        /// <summary>
        /// 获取指定流程步骤的子流程字段规则
        /// </summary>
        /// <param name="WorkflowId"></param>
        /// <param name="activityName"></param>
        /// <returns></returns>
        IList<RulesDetail> GetRelationRules(string WorkflowId, string activityName);
        /// <summary>
        /// 创建规则实体
        /// </summary>
        /// <param name="rulesDetail"></param>
        /// <returns></returns>
        void ActivityRulesDetailInsert(RulesDetail rulesDetail);

        /// <summary>
        /// 更新规则实体
        /// </summary>
        /// <param name="rulesDetail"></param>
        /// <returns></returns>
        int ActivityRulesDetailUpdate(RulesDetail rulesDetail);

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int ActivityRulesDetailDelete(string id);

        /// <summary>
        /// 插入起始步骤规则
        /// </summary>
        /// <param name="rulesDetail"></param>
        /// <returns></returns>
        int ActivityRulesInsertForStart(RulesDetail rulesDetail);

        /// <summary>
        /// 规则解析
        /// </summary>
        /// <param name="rulesDetail"></param>
        /// <returns></returns>
        //int ActivityRulesAnalysis(RulesDetail rulesDetail);

        bool GetActivityRulesAnalysisResult(RulesDetail rulesDetail, DataTable dtPreview);

        /// <summary>
        /// 获取下行步骤
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        DataTable GetNextActivitys(string activityid);

        /// <summary>
        /// 获取表单定义
        /// </summary>
        /// <param name="workflowid"></param>
        /// <returns></returns>
        DataTable GetFormItemDefinitions(string workflowid);

        /// <summary>
        /// 根据下行步骤名称更新规则
        /// </summary>
        /// <param name="rulesDetail"></param>
        /// <returns></returns>
        int ActivityRulesDetailUpdateByActName(RulesDetail rulesDetail);

        /// <summary>
        /// 判断是否存在相同规则
        /// </summary>
        /// <param name="nextActivityid"></param>
        /// <returns></returns>
        int ExistActivityRules(RulesDetail rulesDetail);

        /// <summary>
        /// 获取表单项的DataSource
        /// </summary>
        /// <param name="workflowid"></param>
        /// <param name="fName"></param>
        /// <returns></returns>
        DataRow GetFormItemDataSource(string workflowid, string fName);

        /// <summary>
        /// 获取表单内容并合并成DataTable
        /// </summary>
        /// <param name="workflowid">流程ID</param>
        /// <param name="workflowinstanceid">流程实例ID</param>
        /// <param name="currentActor">当前用户</param>
        /// <param name="formVariables">表单集合</param>
        /// <param name="isInit">是否第一次加载</param>
        /// <returns></returns>
        DataTable GetInstanceTable(Guid workflowid ,Guid workflowinstanceid, string currentActor, IDictionary<string, object> formVariables, bool isInit);

        /// <summary>
        /// 插入组织控制类型
        /// </summary>
        /// <param name="string"></param>
        /// <param name="int"></param>
        void ActivityOrganizationTypeInsert(string Activityid, int OrganizationType);

        /// <summary>
        /// 更新组织控制类型
        /// </summary>
        /// <param name="string"></param>
        /// <param name="int"></param>
        /// <returns></returns>
        int ActivityOrganizationTypeUpdate(string Activityid, int OrganizationType);

        /// <summary>
        /// 删除组织控制类型
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        int ActivityOrganizationTypeDelete(string activityid);

        /// <summary>
        /// 获取组织控制类型
        /// </summary>
        /// <param name="activityid"></param>
        /// <returns></returns>
        int GetActivityOrganizationType(string activityid);

        int ExistActivityOrganizationType(string activityid);

        
    }
}
