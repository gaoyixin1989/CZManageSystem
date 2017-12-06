using System;
using System.Collections.Generic;
using System.Web;
using Botwave.Workflow.Domain;

namespace Botwave.Workflow.Extension.UI
{
    /// <summary>
    /// 流程选择器界面接口.
    /// </summary>
    public interface IWorkflowSelectorProfile
    {
        /// <summary>
        /// 生成指定流程活动的活动选择器 Html.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext">选择器上下文对象.</param>
        /// <returns></returns>
        string BuildActivitySelectorHtml(HttpContext webContext,  WorkflowSelectorContext selecotrContext);
    }
}
