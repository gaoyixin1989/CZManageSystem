using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Parser
{
    /* 
     * [or]�����1[=]ֵ1;�����2[!=]ֵ2
     * [and]�����1[=]ֵ1;�����2[!=]ֵ2
     */
    /// <summary>
    /// ��������������.
    /// ������ʽ����("[RULE]")��
    ///     "��"������[and]�����1[=]ֵ1;�����2[!=]ֵ2;
    ///     "��"������[or]�����1[=]ֵ1;�����2[!=]ֵ2;
    /// ������:
    /// [and]��[or]��[=]��[!=].
    /// </summary>
    public class ConditionParser
    {
        private string _condition;
        private bool _isAndCondition;
        private IList<ExpressionItem> _expressions;

        /// <summary>
        /// �������ڱ��ʽ.
        /// </summary>
        public static readonly Regex ConditionRegex = new Regex(@"\[(?<equality>[^\]]+)\]", RegexOptions.Compiled);
        
        /// <summary>
        /// ���췽��.
        /// </summary>
        /// <param name="condition"></param>
        public ConditionParser(string condition)
        {
            this._condition = condition;
            this._expressions = new List<ExpressionItem>();
            
            // ��ȡ�������ͣ�and ���� or��
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

            // ��ȡ���ʽ
            string[] subExpressions = this._condition.Split(new string[] { ";", "��" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string expressionString in subExpressions)
            {
                string[] items = ConditionRegex.Split(expressionString);
                if (items.Length < 3)
                    continue;
                // ��ȡ�Ƿ��ʽ("=" ���� "!=")
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
        /// �Ƿ��ǲ�����(and)
        /// </summary>
        public bool IsAndCodition
        {
            get { return _isAndCondition; }
        }

        /// <summary>
        /// ���ʽ�б�.
        /// </summary>
        public IList<ExpressionItem> Expressions
        {
            get { return _expressions; }
            set { _expressions = value; }
        }

        /// <summary>
        ///  �������ʽ��������.
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
        ///  �������ʽ��������.
        /// </summary>
        /// <param name="targetItem"></param>
        /// <returns></returns>
        public bool ParseExpression(ExpressionItem targetItem)
        {
            return ParseExpression(targetItem.Name, targetItem.Value);
        }

        /// <summary>
        /// ����ָ����Ŀ����ʽ�����Ƿ���������.
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
        /// �������ʽ��������.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="targetItem"></param>
        /// <returns></returns>
        protected static bool ParseExpression(ExpressionItem item, ExpressionItem targetItem)
        {
            return ConditionParser.ParseExpression(item, targetItem.Name, targetItem.Value);
        }

        /// <summary>
        /// �������ʽ��������.
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
        /// ���ַ���תΪ��д�����Ƴ��հ�.
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
        /// ����Ƿ��Ѿ��������ʽ.
        /// </summary>
        /// <param name="item">���ʽ��.</param>
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
        /// ���ʽ����.
        /// </summary>
        public class ExpressionItem
        {
            private string _name;
            private string _value;
            private bool _isEquality;

            /// <summary>
            /// ���췽��.
            /// </summary>
            public ExpressionItem()
                : this(string.Empty, string.Empty)
            { }

            /// <summary>
            /// ���췽��.
            /// </summary>
            /// <param name="name">���ʽ������.</param>
            /// <param name="value">���ʽ��ֵ.</param>
            public ExpressionItem(string name, string value)
                : this(name, value, false)
            { }

            /// <summary>
            /// ���췽��.
            /// </summary>
            /// <param name="name">���ʽ������.</param>
            /// <param name="value">���ʽ��ֵ.</param>
            /// <param name="isEquality">�Ƿ��ʽ���ʽ("!="����"=").</param>
            public ExpressionItem(string name, string value, bool isEquality)
            {
                this._name = name;
                this._value = value;
                this._isEquality = isEquality;
            }

            /// <summary>
            /// ���ʽ������.
            /// </summary>
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            /// <summary>
            /// ���ʽ��ֵ.
            /// </summary>
            public string Value
            {
                get { return _value; }
                set { _value = value; }
            }

            /// <summary>
            /// �Ƿ��ʽ���ʽ("!="����"=").True ��ʾ "=". False ���ʾ "!=".
            /// </summary>
            public bool IsEquality
            {
                get { return _isEquality; }
                set { _isEquality = value; }
            }
        }
    }
}
