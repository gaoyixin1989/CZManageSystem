using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Plugin;

namespace Botwave.Workflow.Extension.Service.Plugins
{
    /// <summary>
    /// 权限资源转换器实现类.
    /// </summary>
    public class ResourceTranslator : IResourceTranslator
    {
        #region IResourceTranslator 成员

        /// <summary>
        /// 获取权限资源编号.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public string Alias2Name(string alias)
        {
            object result = IBatisMapper.Mapper.QueryForObject("bwwf_Resources_Select_ResourceId_ByAlias", alias);
            if (result == null)
                return string.Empty;
            return result.ToString();
        }

        /// <summary>
        /// 获取权限资源别名.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Name2Alias(string name)
        {
            object result = IBatisMapper.Mapper.QueryForObject("bwwf_Resources_Select_Alias_ByResourceId", name);
            if (result == null)
                return string.Empty;
            return result.ToString();
        }

        #endregion

        /// <summary>
        /// 去除空白.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ClearEmpty(string input)
        {
            return input.Trim('\r', '\n', '\t', ' ');
        }
    }
}
