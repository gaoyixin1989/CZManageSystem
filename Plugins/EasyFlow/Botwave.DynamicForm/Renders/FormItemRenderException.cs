using System;
using System.Runtime.Serialization;
using Botwave.DynamicForm.Domain;

namespace Botwave.DynamicForm.Renders
{
    /// <summary>
    /// 表单呈现异常类.
    /// </summary>
    public class FormItemRenderException : Exception
    {
        private bool isUnknowException = false;
        private FormItemDefinition formItem;

        /// <summary>
        /// 是否未知异常类.
        /// </summary>
        public bool IsUnknowException
        {
            get { return isUnknowException; }
            set { isUnknowException = value; }
        }

        /// <summary>
        /// 异常的表单项对象.
        /// </summary>
        public FormItemDefinition FormItem
        {
            get { return formItem; }
            set { formItem = value; }
        }

        /// <summary>
        /// 构造方法,
        /// </summary>
        public FormItemRenderException()
            : base()
        { }

        /// <summary>
        /// 构造方法,
        /// </summary>
        /// <param name="message"></param>
        public FormItemRenderException(string message)
            : base(message)
        { }

        /// <summary>
        /// 构造方法,
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FormItemRenderException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// 构造方法,
        /// </summary>
        /// <param name="item"></param>
        /// <param name="innerException"></param>
        public FormItemRenderException(FormItemDefinition item, Exception innerException)
            : this(item, innerException, false)
        { }

        /// <summary>
        /// 构造方法,
        /// </summary>
        /// <param name="item"></param>
        /// <param name="innerException"></param>
        /// <param name="isUnknowException">是否未知异常.</param>
        public FormItemRenderException(FormItemDefinition item, Exception innerException, bool isUnknowException)
            : base(string.Format("表单生成出错，出错字段名称：{0}；字段意义：{1}。请重新设置字段的行列与高宽属性。", item.FName, item.Name), innerException)
        {
            this.isUnknowException = isUnknowException;
            this.formItem = item;
        }

        /// <summary>
        /// 构造方法,
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public FormItemRenderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
