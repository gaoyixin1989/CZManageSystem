using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// Allocator 信息项.
    /// </summary>
    [Serializable]
    public class AllocatorItem
    {
        #region gets / sets

        private string key;
        private string name;

        /// <summary>
        /// 分派器键名(英文字母组成，如：users, resource 等).
        /// </summary>
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 分派器显示名称(为中文名).
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion
    }
}
