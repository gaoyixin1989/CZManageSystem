using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_WF.Domain
{
    /// <summary>
    /// 表单字段实体
    /// </summary>
    [Serializable]
    public class Field
    {
        /// <summary>
        /// 表单键名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 表单名称（中文名称）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 表单值
        /// </summary>
        public string Value { get; set; }
    }
}