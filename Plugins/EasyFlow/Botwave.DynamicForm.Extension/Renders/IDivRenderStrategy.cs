using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Extension.Renders
{
    /// <summary>
    /// 表单呈现策略接口.
    /// </summary>
    public interface IDivRenderStrategy
    {
        /// <summary>
        /// 渲染表单定义.
        /// </summary>
        /// <param name="htw"></param>
        /// <param name="form"></param>
        void Render(HtmlTextWriter htw, FormDefinition form);

        /// <summary>
        /// 渲染表单实例.
        /// </summary>
        /// <param name="htw"></param>
        /// <param name="form"></param>
        void Render(HtmlTextWriter htw, FormInstance form);
    }
}
