using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 条件解析的基础抽象类.
    /// </summary>
    public abstract class AbstractConditionParser : IConditionParser
    {
        #region IConditionParser Members

        /// <summary>
        /// 验证.
        /// </summary>
        /// <param name="condition">条件表达式.</param>
        /// <returns></returns>
        public bool Validate(string condition)
        {
            if (null == condition)
            {
                return true;
            }
            
            condition = condition.Trim();
            if (condition.Length == 0)
            {
                return true;
            }

            bool isValid = ValidateIndividually(condition);
            if (!isValid)
            {
                isValid = ValidateExpression(condition);
            }

            return isValid;
        }

        /// <summary>
        /// 根据选中节点解析条件规则.
        /// </summary>
        /// <param name="condition">条件表达式.</param>
        /// <param name="allActivities">所有活动节点列表.</param>
        /// <param name="selectedActivities">被选中的活动节点列表.</param>
        /// <returns></returns>
        public abstract bool Parse(string condition, IList<string> allActivities, IList<string> selectedActivities);

        #endregion

        /// <summary>
        /// 验证表达式条件.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected virtual bool ValidateIndividually(string condition)
        {
            return true;
        }

        /// <summary>
        /// 验证表达式条件.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected static bool ValidateExpression(string expression)
        {
            //string[] items = expression.Split('+');
            return true;
        }

        private static class AmountExpressionMatcher
        {
            static readonly Regex r = new Regex(@"^\d+(,\d*)?$", RegexOptions.Compiled);

            public static bool IsMatch(string input)
            {
                return r.IsMatch(input);
            }
        }

        /// <summary>
        /// 根据以下定义的条件表达式进行解析：     
        ///     条件表达式 ::= 活动列表 + 数量规则;
        ///     活动列表 ::= 空值 | 活动 + 活动列表;
        ///     数量规则 :: = 空值 | n[,m];
        /// n[,m]表示 大于等于n小于等于m个(如果有定义m)任意数量个活动.
        /// 
        /// 例如： 
        ///         表示需要选择全部;
        /// any     表示任选一个;
        /// 活动A   表示需要选择活动A;
        /// 2       表示至少需要选择2个活动;
        /// 2,3     表示至少需要选择2个活动，至多可以选择3个活动;
        /// 活动A+1 表示需要选择活动A，并且至少还需要选择1个活动;
        /// 活动A+活动B+2,2 表示需要选择活动A与活动B，并且还需要选择2个活动.
        /// </summary>
        /// <param name="expression">条件表达式.</param>
        /// <param name="allActivities">所有活动节点列表.</param>
        /// <param name="selectedActivities">被选中的活动节点列表.</param>
        /// <returns></returns>
        protected static bool ParseExpression(string expression, IList<string> allActivities, IList<string> selectedActivities)
        {
            int n = 0;
            int m = 0;
            IList<string> defActivities = new List<string>();

            string[] items = expression.Split('+');            
            foreach (string item in items)
            {
                string el = item.Trim();
                if (el.Length > 0)
                {
                    //如果有多个数量规则，最终使用最后一个
                    if (AmountExpressionMatcher.IsMatch(el))
                    {
                        string[] ss = el.Split(',');
                        n = int.Parse(ss[0]);
                        if (ss.Length >= 2 && ss[1].Length > 0)
                        {
                            m = int.Parse(ss[1]);
                        }
                    }
                    else
                    {
                        if (!defActivities.Contains(el))
                        {
                            defActivities.Add(el);
                        }
                    }
                }
            }

            //如果有事先定义好的必须要的活动
            if (defActivities.Count > 0)
            {
                foreach (string activity in defActivities)
                {
                    //如果不在选择的活动中
                    if (!selectedActivities.Contains(activity))
                    {
                        return false;
                    }
                }

                n += defActivities.Count;
                if (m > 0)
                {
                    m += defActivities.Count;
                }
            }

            ////验证选择的活动是否全部在目标活动里面
            //foreach (string activity in selectedActivities)
            //{
            //    if (!allActivities.Contains(activity))
            //    {
            //        return false;
            //    }
            //}

            //进行活动数量规则验证
            //如果选择的活动小于最少需要选择的数量
            if (selectedActivities.Count < n)
            {
                return false;
            }

            //如果选择的活动大于最多需要选择的数量
            if (m > 0 && selectedActivities.Count > m)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 需要选择全部的命令解析.
        /// </summary>
        /// <param name="allActivities"></param>
        /// <param name="selectedActivities"></param>
        /// <returns></returns>
        protected static bool ParseAll(IList<string> allActivities, IList<string> selectedActivities)
        {
            int allCount = allActivities.Count;

            //如果选择的分支列表数目与目标分支不一致.
            if (selectedActivities.Count != allCount)
            {
                return false;
            }

            for (int i = 0; i < allCount; i++)
            {
                //如果有活动名称不一致.
                if (!allActivities[i].Equals(selectedActivities[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 任选一个的命令解析.
        /// </summary>
        /// <param name="allActivities"></param>
        /// <param name="selectedActivities"></param>
        /// <returns></returns>
        protected static bool ParseAny(IList<string> allActivities, IList<string> selectedActivities)
        {
            //选择了多于一个的分支
            if (selectedActivities.Count >= 2)
            {
                return false;
            }

            //选择的分支不在目标分支里面
            if (!allActivities.Contains(selectedActivities[0]))
            {
                return false;
            }

            return true;
        }
    }
}
