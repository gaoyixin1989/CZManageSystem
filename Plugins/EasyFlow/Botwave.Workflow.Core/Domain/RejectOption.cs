using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 退回选项类.
    /// </summary>
    public static class RejectOption
    {
        /// <summary>
        /// 起始(流程发起/提单)状态.
        /// </summary>
        public static readonly string Initial = "initial";

        /// <summary>
        /// 上一步.
        /// </summary>
        public static readonly string Previous = "previous";

        /// <summary>
        /// 无(不允许退回).
        /// </summary>
        public static readonly string None = "none";

        /// <summary>
        /// 是否退回到初始状态.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool IsInitial(string option)
        {
            return Initial.CompareTo(option) == 0;
        }

        /// <summary>
        /// 是否退回到前一活动.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool IsPrevious(string option)
        {
            return Previous.CompareTo(option) == 0;
        }

        /// <summary>
        /// 是否不退回.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool IsNone(string option)
        {
            return None.CompareTo(option) == 0;
        }

        /// <summary>
        /// 是否自定义退回.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool IsCustom(string option)
        {
            return !(String.IsNullOrEmpty(option)
                || IsInitial(option)
                || IsPrevious(option)
                || IsNone(option));
        }
    }
}
