using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Converters
{
    /// <summary>
    /// 默认的表单实例转换器实现类.
    /// </summary>
    public class DefaultFormInstanceConverter : IFormInstanceConverter
    {
        #region IFormInstanceConverter Members

        /// <summary>
        /// 转换.
        /// </summary>
        /// <param name="formDefinition"></param>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public FormInstance Convert(FormDefinition formDefinition, NameValueCollection nvc)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
