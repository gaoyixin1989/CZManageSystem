using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 命令规则解析.
    /// 格式如下：
    ///     命令A=活动A1:用户A1-U,用户A2-U;活动2:用户用户A1-U,用户A2-U|命令B=活动A2 ...
    /// </summary>
    public class DefaultCommandRulesParser : AbstractCommandRulesParser
    {
        #region ICommandRulesParser Members

        /// <summary>
        ///  解析命令规则.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public override IDictionary<string, ICollection<string>> Parse(string rules, string command)
        {
            if (String.IsNullOrEmpty(rules))
            {
                return null;
            }

            IDictionary<string, string> dict = ParseRules(rules);
            if (!dict.ContainsKey(command))
            {
                return null;                
            }

            return ParseExpression(dict[command]);      
        }

        /// <summary>
        /// 解析命令规则.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IDictionary<string, ICollection<string>> Parse(string rules, ActivityExecutionContext context)
        {
            return Parse(rules, context.Command.ToLower());
        }

        #endregion

        static IDictionary<string, string> ParseRules(string rules)
        {
            IDictionary<string, string> rulesDict = new Dictionary<string, string>();
            string[] ss = rules.Split('|');
            for (int i = 0, ilen = ss.Length; i < ilen; i++)
            {
                string[] arr = ss[i].Split('=');//命令A=活动A1:用户A1-U,用户A2-U;活动2:用户用户A1-U,用户A2-U
                if (arr.Length == 2)
                {
                    string key = arr[0].Trim();//命令A
                    string value = arr[1].Trim();//活动A1:用户A1-U,用户A2-U;活动2:用户用户A1-U,用户A2-U
                    if (key.Length > 0 && value.Length > 0)
                    {                        
                        rulesDict[key] = value;
                    }
                }
            }
            return rulesDict;
        }
    }
}
