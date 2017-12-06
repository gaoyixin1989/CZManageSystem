using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Enums
{
    /// <summary>
    /// 查询命令的集合
    /// </summary>
    public enum SearchCommandEnum
    {
        /// <summary>
        /// 高级查询的条件
        /// </summary>
        searchquery,

        /// <summary>
        /// 高级查询
        /// </summary>
        searchlist,

        /// <summary>
        /// 获取指定用户的待办列表
        /// </summary>
        todolist,

        /// <summary>
        /// 获取指定用户的已办列表
        /// </summary>
        donelist,

        /// <summary>
        /// 获取指定用户的“我的工单”列表信息
        /// </summary>
        mytasklist,

        /// <summary>
        /// 获取指定类别的表单以及步骤和处理人列表
        /// </summary>
        infolist,

        /// <summary>
        /// 获取指定需求单处理列表
        /// </summary>
        recordlist,

        /// <summary>
        /// 获取流程分组信息
        /// </summary>
        menugrouplist,

        /// <summary>
        /// 获取流程列表
        /// </summary>
        workflowlist,

        /// <summary>
        /// 评论列表
        /// </summary>
        commentlist,

        /// <summary>
        /// 获取指定工单标识的需求单明细信息
        /// </summary>
        detail,

        /// <summary>
        /// 获取工单信息信息
        /// </summary>
        info,

        /// <summary>
        /// 获取指定需求单工单的当前步骤以及处理状态（正在处理，已完成，已取消等）
        /// </summary>
        state,

        /// <summary>
        /// 获取流程定义
        /// </summary>
        definition,

        /// <summary>
        /// 获取步骤定义(当前步骤，上行步骤，下行步骤等)
        /// </summary>
        activitieslist,

        /// <summary>
        /// 获取步骤和处理人
        /// </summary>
        activity
    }
}
