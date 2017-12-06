using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Binders
{
    /// <summary>
    /// 表单项数据绑定器接口.
    /// </summary>
    public interface IFormItemDataBinder
    {
        /// <summary>
        /// 绑定表单项.
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="formInstanceId"></param>
        /// <param name="template"></param>
        /// <param name="dict"></param>
        void Bind(System.IO.StringWriter sw, Guid formInstanceId, string template, IDictionary<string, object> dict);
    }
}
