using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Validators
{
    /// <summary>
    /// 表单验证器接口.
    /// </summary>
    public interface IFormValidator
    {
        /// <summary>
        /// 验证表单.
        /// </summary>
        /// <param name="formInstance"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool TryValidate(FormInstance formInstance, out string message);

        /// <summary>
        /// 验证表单，如不通过则抛出异常.
        /// </summary>
        /// <param name="formInstance"></param>
        /// <returns></returns>
        bool Validate(FormInstance formInstance);
    }
}
