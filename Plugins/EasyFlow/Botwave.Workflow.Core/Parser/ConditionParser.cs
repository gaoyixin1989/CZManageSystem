using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Parser
{
    /* 
     * [or]活动名称1[=]值1;活动名称2[!=]值2
     * [and]活动名称1[=]值1;活动名称2[!=]值2
     */
    /// <summary>
    /// 流程条件解析类.
    /// 条件格式如下("[RULE]")：
    ///     "和"条件：[and]活动名称1[=]值1;活动名称2[!=]值2;
    ///     "或"条件：[or]活动名称1[=]值1;活动名称2[!=]值2;
    /// 规则有:
    /// [and]，[or]，[=]，[!=].
    /// </summary>
    public class ConditionParser
    {
        private string _condition;
        private bool _isAndCondition;
        private IList<ExpressionItem> _expressions;

        /// <summary>
        /// 条件正在表达式.
        /// </summary>
        public static readonly Regex ConditionRegex = new Regex(@"\[(?<equality>[^\]]+)\]", RegexOptions.Compiled);
        
        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="condition"></param>
        public ConditionParser(string condition)
        {
            this._condition = condition;
            this._expressions = new List<ExpressionItem>();
            
            // 获取条件类型（and 或者 or）
            if (condition.StartsWith("[or]"))
            {
                this._isAndCondition = false;
                this._condition = this._condition.Substring(4);
            }
            else if (condition.StartsWith("[and]"))
            {
                this._isAndCondition = true;
                this._condition = this._condition.Substring(5);
            }

            // 获取表达式
            string[] subExpressions = this._condition.Split(new string[] { ";", "；" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string expressionString in subExpressions)
            {
                string[] items = ConditionRegex.Split(expressionString);
                if (items.Length < 3)
                    continue;
                // 获取是否等式("=" 或者 "!=")
                MatchCollection matches = ConditionRegex.Matches(expressionString);
                bool isEquality = true;
                foreach (Match tempMatch in matches)
                {
                    isEquality = (tempMatch.Groups["equality"].Value == "=");
                }
                ExpressionItem expression = new ExpressionItem(items[0], items[2], isEquality);
                if (!this.Contains(expression))
                    this._expressions.Add(expression);
            }
        }

        /// <summary>
        /// 是否是并条件(and)
        /// </summary>
        public bool IsAndCodition
        {
            get { return _isAndCondition; }
        }

        /// <summary>
        /// 表达式列表.
        /// </summary>
        public IList<ExpressionItem> Expressions
        {
            get { return _expressions; }
            set { _expressions = value; }
        }

        /// <summary>
        ///  解析表达式满足条件.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ParseExpression(string name, string value)
        {
            bool result = false;
            foreach (ExpressionItem item in this.Expressions)
            {
                result = ConditionParser.ParseExpression(item, name, value);
                if (result)
                    break;
            }
            return result;
        }

        /// <summary>
        ///  解析表达式满足条件.
        /// </summary>
        /// <param name="targetItem"></param>
        /// <returns></returns>
        public bool ParseExpression(ExpressionItem targetItem)
        {
            return ParseExpression(targetItem.Name, targetItem.Value);
        }

        /// <summary>
        /// 解析指定的目标表达式集合是否满足条件.
        /// </summary>
        /// <param name="targetExpressions"></param>
        /// <returns></returns>
        public bool ParseExpression(IList<ExpressionItem> targetExpressions)
        {
            bool result = false;
            if (IsAndCodition)
            {
                foreach (ExpressionItem item in this.Expressions)
                {
                    result = false;
                    foreach (ExpressionItem targetItem in targetExpressions)
                    {
                        if (ConditionParser.ParseExpression(item, targetItem))
                        {
                            result = true;
                            break;
                        }
                    }
                    if (result == false)
                        break;
                }
            }
            else
            {
                foreach (ExpressionItem targetItem in targetExpressions)
                {
                    if (this.ParseExpression(targetItem))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 解析表达式满足条件.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="targetItem"></param>
        /// <returns></returns>
        protected static bool ParseExpression(ExpressionItem item, ExpressionItem targetItem)
        {
            return ConditionParser.ParseExpression(item, targetItem.Name, targetItem.Value);
        }

        /// <summary>
        /// 解析表达式满足条件.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static bool ParseExpression(ExpressionItem item, string name, string value)
        {
            if (string.Compare(item.Name, name, true) > -1)
            {
                string itemValue = ToLowerTrim(item.Value);
                string tempValue = ToLowerTrim(value);
                if ((itemValue == tempValue && item.IsEquality) || (itemValue != tempValue && !item.IsEquality))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 将字符串转为大写，并移除空白.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ToLowerTrim(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return input.Trim().ToLower();
        }

        /// <summary>
        /// 检查是否已经包含表达式.
        /// </summary>
        /// <param name="item">表达式项.</param>
        /// <returns></returns>
        public bool Contains(ExpressionItem item)
        {
            foreach (ExpressionItem expression in this.Expressions)
            {
                if (item.Name == expression.Name && item.Value == expression.Value && item.IsEquality == expression.IsEquality)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 表达式项类.
        /// </summary>
        public class ExpressionItem
        {
            private string _name;
            private string _value;
            private bool _isEquality;

            /// <summary>
            /// 构造方法.
            /// </summary>
            public ExpressionItem()
                : this(string.Empty, string.Empty)
            { }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="name">表达式的名称.</param>
            /// <param name="value">表达式的值.</param>
            public ExpressionItem(string name, string value)
                : this(name, value, false)
            { }

            /// <summary>
            /// 构造方法.
            /// </summary>
            /// <param name="name">表达式的名称.</param>
            /// <param name="value">表达式的值.</param>
            /// <param name="isEquality">是否等式表达式("!="或者"=").</param>
            public ExpressionItem(string name, string value, bool isEquality)
            {
                this._name = name;
                this._value = value;
                this._isEquality = isEquality;
            }

            /// <summary>
            /// 表达式的名称.
            /// </summary>
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            /// <summary>
            /// 表达式的值.
            /// </summary>
            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }

            /// <summary>
            /// 是否等式表达式("!="或者"=").True 表示 "=". False 则表示 "!=".
            /// </summary>
            public bool IsEquality
            {
                get { return _isEquality; }
                set { _isEquality = value; }
            }
        }
    }
}
