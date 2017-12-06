using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 命令解析规则抽象类.
    /// </summary>
    public abstract class AbstractCommandRulesParser : ICommandRulesParser
    {
        #region ICommandRulesParser Members

        /// <summary>
        /// 解析命令规则.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public abstract IDictionary<string, ICollection<string>> Parse(string rules, string command);

        /// <summary>
        /// 解析命令规则.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public abstract IDictionary<string, ICollection<string>> Parse(string rules, ActivityExecutionContext context);

        #endregion

        /// <summary>
        /// expression格式
        /// 活动A1:用户A1-U,用户A2-U;活动2:用户用户A1-U,用户A2-U.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected static IDictionary<string, ICollection<string>> ParseExpression(string expression)
        {
            if (String.IsNullOrEmpty(expression))
            {
                return null;
            }

            IDictionary<string, ICollection<string>> dict = new Dictionary<string, ICollection<string>>();
            string[] subExpressions = expression.Split(';');//半角分号
            foreach (string subExpression in subExpressions)
            {
                string subExp = subExpression.Trim();//活动A1:用户A1-U,用户A2-U
                if (subExp.Length > 0)
                {
                    string[] arrActivityAndUsers = subExp.Split(':');
                    string activityName = arrActivityAndUsers[0].Trim();
                    if (activityName.Length > 0)
                    {
                        ICollection<string> users = null;
                        if (arrActivityAndUsers.Length == 2
                            && arrActivityAndUsers[1].Length > 0)
                        {
                            users = new List<string>();
                            string[] usernames = arrActivityAndUsers[1].Split(',');
                            foreach (string username in usernames)
                            {
                                string uname = username.Trim();
                                if (uname.Length > 0)
                                {
                                    users.Add(uname);
                                }
                            }
                        }
                        dict[activityName] = users;
                    }
                }
            }

            return dict;
        }
    }
}
