﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 默认的分支条件解析. 
    /// 
    /// 1.如果condition为空，则认为任选一分支;
    /// 2.如果condition为all，则认为需要选择全部分支;
    /// 否则根据以下定义的条件表达式进行解析：    
    ///     (1)条件表达式 ::= 活动列表 + 数量规则;
    ///     (2)活动列表 ::= 空值 | 活动 + 活动列表;
    ///     (3)数量规则 :: = 空值 | n[,m];
    /// n[,m]表示 大于等于n小于等于m个(如果有定义m)任意数量个活动.
    /// 
    /// 例如： 
    ///         表示任选一个;
    /// all     表示需要选择全部;
    /// 活动A   表示需要选择活动A;
    /// 2       表示至少需要选择2个活动;
    /// 2,3     表示至少需要选择2个活动，至多可以选择3个活动;
    /// 活动A+1 表示需要选择活动A，并且至少还需要选择1个活动;
    /// 活动A+活动B+2,2 表示需要选择活动A与活动B，并且还需要选择2个活动.
    /// </summary>
    public class DefaultSplitConditionParser : AbstractConditionParser
    {
        /// <summary>
        /// 根据选中节点解析条件规则.
        /// </summary>
        /// <param name="condition">条件表达式.</param>
        /// <param name="allNodes">所有活动节点列表.</param>
        /// <param name="selectedNodes">被选中的活动节点列表.</param>
        /// <returns></returns>
        public override bool Parse(string condition, IList<string> allNodes, IList<string> selectedNodes)
        {
            //如果目标分支为空或数目为0
            if (null == allNodes || allNodes.Count == 0)
            {
                return true;
            }

            //如果选择的分支数为空或者为0
            if (null == selectedNodes || selectedNodes.Count == 0)
            {
                return false;
            }

            //任选一分支
            if (String.IsNullOrEmpty(condition))
            {
                return ParseAny(allNodes, selectedNodes);
            }

            //全部分支
            if (condition.Equals("all", StringComparison.OrdinalIgnoreCase))
            {
                return ParseAll(allNodes, selectedNodes);
            }

            return ParseExpression(condition, allNodes, selectedNodes);
        }

        /// <summary>
        /// 个性化验证.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override bool ValidateIndividually(string condition)
        {
            return condition.Equals("all", StringComparison.OrdinalIgnoreCase);
        }
    }
}
