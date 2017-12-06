namespace Botwave.Algorithms.Apriori
{
    using System;

    /// <summary>
    /// 常量类.
    /// </summary>
    public class LibConst
    {
        /// <summary>
        /// 日期类型枚举.
        /// </summary>
        public enum DateType
        {
            /// <summary>
            /// 日.
            /// </summary>
            Day,
            /// <summary>
            /// 周.
            /// </summary>
            Week,
            /// <summary>
            /// 月.
            /// </summary>
            Month,
            /// <summary>
            /// 刻.
            /// </summary>
            Quarter,
            /// <summary>
            /// 年.
            /// </summary>
            Year,
            /// <summary>
            /// 小时.
            /// </summary>
            Hour,
            /// <summary>
            /// 分.
            /// </summary>
            Minute,
            /// <summary>
            /// 秒.
            /// </summary>
            Second,
            /// <summary>
            /// 毫秒.
            /// </summary>
            MilliSecond
        }

        /// <summary>
        /// 操作类型枚举.
        /// </summary>
        public enum OperateType
        {
            /// <summary>
            /// 更新操作.
            /// </summary>
            Update,
            /// <summary>
            /// 插入操作.
            /// </summary>
            Insert
        }

        /// <summary>
        /// 水印位置枚举.
        /// </summary>
        public enum WatermarkPosition
        {
            /// <summary>
            /// 左上位置.
            /// </summary>
            WM_TOP_LEFT,
            /// <summary>
            /// 右上位置.
            /// </summary>
            WM_TOP_RIGHT,
            /// <summary>
            /// 右下位置.
            /// </summary>
            WM_BOTTOM_RIGHT,
            /// <summary>
            /// 左下位置.
            /// </summary>
            WM_BOTTOM_LEFT
        }

        /// <summary>
        /// 水印类型枚举.
        /// </summary>
        public enum WatermarkType
        {
            /// <summary>
            /// 文本字符串水印.
            /// </summary>
            WM_TEXT,
            /// <summary>
            /// 图片水印.
            /// </summary>
            WM_IMAGE
        }
    }
}
