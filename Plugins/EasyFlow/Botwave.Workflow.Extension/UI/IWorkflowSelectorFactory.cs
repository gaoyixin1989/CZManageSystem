using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.UI
{
    /// <summary>
    /// 流程选择器的管理工厂接口.
    /// </summary>
    public interface IWorkflowSelectorFactory
    {
        /// <summary>
        /// 获取指定类型的流程选择器对象.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IWorkflowSelectorProfile GetProfile(string typeName);
    }
}
