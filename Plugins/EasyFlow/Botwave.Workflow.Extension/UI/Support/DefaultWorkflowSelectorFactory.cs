using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.UI.Support
{
    /// <summary>
    /// 流程选择器的管理工厂的默认实现类.
    /// </summary>
    public class DefaultWorkflowSelectorFactory : IWorkflowSelectorFactory
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DefaultWorkflowSelectorFactory));

        private IDictionary<string, IWorkflowSelectorProfile> cache = new Dictionary<string, IWorkflowSelectorProfile>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// 适配spring
        /// srping中的dictionary的类型为HybridDictionary
        /// </summary>
        public System.Collections.Specialized.HybridDictionary Profiles
        {
            set
            {
                if (null != value && value.Count > 0)
                {
                    foreach (string key in value.Keys)
                    {
                        string name = key.Trim();
                        //直接转换，如果有异常则说明配置不正确
                        cache.Add(name, value[key] as IWorkflowSelectorProfile);
                    }
                }
            }
        }

        #region IWorkflowSelectorFactory 成员

        /// <summary>
        /// 获取指定类型的流程选择器对象.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IWorkflowSelectorProfile GetProfile(string typeName)
        {
            typeName = (string.IsNullOrEmpty(typeName) ? "default" : typeName.Trim());
            if (cache.ContainsKey(typeName))
                return cache[typeName];
            return null;
        }

        #endregion
    }
}
