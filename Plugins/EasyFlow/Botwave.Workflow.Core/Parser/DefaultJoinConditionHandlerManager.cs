using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Botwave.Workflow.Parser
{
    /// <summary>
    /// 合并条件处理器的默认管理类.
    /// </summary>
    public class DefaultJoinConditionHandlerManager : IJoinConditionHandlerManager
    {
        private static IDictionary<string, IJoinConditionHandler> cache = new Dictionary<string, IJoinConditionHandler>();

        /// <summary>
        /// 可以由其它实现来重写此方法.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        protected virtual IJoinConditionHandler DoGetHandler(string typeName)
        {
            return GetHandlerByTypeName(typeName);
        }

        /// <summary>
        /// 获取指定类型名称的合并条件处理器对象.
        /// </summary>
        /// <param name="typeName">指定类型名称.</param>
        /// <returns></returns>
        protected static IJoinConditionHandler GetHandlerByTypeName(string typeName)
        {
            if (cache.ContainsKey(typeName))
            {
                return cache[typeName];
            }

            Type objType = Type.GetType(typeName, true, true);
            IJoinConditionHandler parser = Activator.CreateInstance(objType) as IJoinConditionHandler;
            if (null == parser)
            {
                throw new ArgumentException(String.Format("{0} 不存在", typeName));
            }
            cache.Add(typeName, parser);
            return parser;
        }

        #region IJoinConditionHandlerManager Members

        /// <summary>
        /// 条件是否通过验证.
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public bool IsValid(string condition)
        {
            return (!String.IsNullOrEmpty(condition)
                && condition.StartsWith("type:"));
        }

        /// <summary>
        /// 解析合并条件.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="typeName"></param>
        /// <param name="ifSelectedActivities"></param>
        /// <param name="mustCompletedActivities"></param>
        public void ParseCondition(string condition, out string typeName, out IList<string> ifSelectedActivities, out IList<string> mustCompletedActivities)
        {
            ifSelectedActivities = null;
            mustCompletedActivities = null;
            const int lengthOfTypePrefix = 5;//type:
            const int lengthOfArgsPrefix = 5;//args:
            string[] ss = condition.Split(';');
            typeName = ss[0].Substring(lengthOfTypePrefix).Trim();
            if (ss.Length >= 2)
            {
                string args = ss[1].Substring(lengthOfArgsPrefix);
                string[] argsArray = args.Split(',');
                string[] ifSelectedActivitiesArray = argsArray[0].Split('+');
                ifSelectedActivities = new List<string>();
                for (int i = 0, ilen = ifSelectedActivitiesArray.Length; i < ilen; i++)
                {
                    ifSelectedActivities.Add(ifSelectedActivitiesArray[i].Trim());
                }

                if (argsArray.Length >= 2)
                {
                    string[] mustCompletedActivitiesArray = argsArray[1].Split('+');
                    mustCompletedActivities = new List<string>();
                    for (int i = 0, ilen = mustCompletedActivitiesArray.Length; i < ilen; i++)
                    {
                        mustCompletedActivities.Add(mustCompletedActivitiesArray[i].Trim());
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定类型名称的合并条件处理器对象.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IJoinConditionHandler GetHandler(string typeName)
        {
            if (String.IsNullOrEmpty(typeName))
            {
                return null;
            }

            return DoGetHandler(typeName);
        }

        #endregion
    }
}
