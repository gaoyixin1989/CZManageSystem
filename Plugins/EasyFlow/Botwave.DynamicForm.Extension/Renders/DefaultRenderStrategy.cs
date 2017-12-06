using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Botwave.Commons;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Extension.Renders
{
    /// <summary>
    /// 默认表单呈现策略.
    /// </summary>
    public class DefaultRenderStrategy : IDivRenderStrategy
    {       
        #region IRenderStrategy Members

        /// <summary>
        /// 呈现表单定义.
        /// </summary>
        /// <param name="htw"></param>
        /// <param name="form"></param>
        public void Render(HtmlTextWriter htw, FormDefinition form)
        {
            DivRender.Render(htw, form, false);
        }

        /// <summary>
        /// 呈现表单实例.
        /// </summary>
        /// <param name="htw"></param>
        /// <param name="form"></param>
        public void Render(HtmlTextWriter htw, FormInstance form)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
