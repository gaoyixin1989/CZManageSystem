using System;

namespace Botwave.Workflow
{
    /// <summary>
    /// 活动命令.
    /// </summary>
    public static class ActivityCommands
    {
        /// <summary>
        /// 指定（分配）
        /// </summary>
        public static readonly string Assign = "assign";

        /// <summary>
        /// 拒绝.
        /// </summary>
        public static readonly string Reject = "reject";

        /// <summary>
        /// 同意（通过）.
        /// </summary>
        public static readonly string Approve = "approve";

        /// <summary>
        /// 取消.
        /// </summary>
        public static readonly string Cancel = "cancel";

        /// <summary>
        /// 完成.
        /// </summary>
        public static readonly string Complete = "complete";

        /// <summary>
        /// 保存.
        /// </summary>
        public static readonly string Save = "save";

        /// <summary>
        /// 移除.
        /// </summary>
        public static readonly string Remove = "remove";

        /// <summary>
        /// 退回草稿/提单人.
        /// </summary>
        public static readonly string ReturnToDraft = "return-to-draft";

        /// <summary>
        /// 撤回.
        /// </summary>
        public static readonly string Withdraw = "withdraw";
    }
}
