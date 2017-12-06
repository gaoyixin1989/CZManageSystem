using System;
using System.Collections.Generic;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 权限资源转化接口.
    /// </summary>
    public interface IResourceTranslator
    {
        /// <summary>
        /// 将资源别名转换为资源名称/id.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        string Alias2Name(string alias);

        /// <summary>
        /// 将资源名称/id转换为别名.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string Name2Alias(string name);
    }
}
