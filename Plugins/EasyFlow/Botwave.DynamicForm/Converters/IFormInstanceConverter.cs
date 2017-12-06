using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Converters
{
    /// <summary>
    /// 表单实例转换器.
    /// </summary>
    public interface IFormInstanceConverter
    {
        /// <summary>
        /// 转换表单实例.
        /// </summary>
        /// <param name="formDefinition"></param>
        /// <param name="nvc"></param>
        /// <returns></returns>
        FormInstance Convert(FormDefinition formDefinition, NameValueCollection nvc);
    }
}
