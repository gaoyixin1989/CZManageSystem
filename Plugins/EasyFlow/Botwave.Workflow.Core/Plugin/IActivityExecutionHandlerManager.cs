using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Plugin
{
    /// <summary>
    /// 流程活动(步骤)自定义执行处理器管理接口.
    /// </summary>
    public interface IActivityExecutionHandlerManager
    {
        /// <summary>
        /// 获取自定义处理器.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IActivityExecutionHandler GetHandler(string typeName);

        /// <summary>
        /// 执行自定义处理器.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="context"></param>
        void Execute(string typeName, ActivityExecutionContext context);
    }
}
