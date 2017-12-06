using System;
using System.Collections.Generic;
using System.Text;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Validators
{
    /// <summary>
    /// 表单项验证器接口.
    /// </summary>
    public interface IFormItemValidator
    {
        /// <summary>
        /// 验证表单项.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool TryValidate(FormItemInstance fi, out string message);

        /// <summary>
        /// 验证表单项，如不通过则抛出异常.
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        bool Validate(FormItemInstance fi);
    }
}
