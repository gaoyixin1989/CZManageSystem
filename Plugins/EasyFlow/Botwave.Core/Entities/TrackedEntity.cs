using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Entities
{
    /// <summary>
    /// 跟踪实体类.
    /// </summary>
    public class TrackedEntity
    {
        private DateTime createdTime;
        private DateTime lastModTime;
        private string creator = String.Empty;
        private string lastModifier = String.Empty;

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 最后更新时间.
        /// </summary>
        public DateTime LastModTime
        {
            get { return lastModTime; }
            set { lastModTime = value; }
        }

        /// <summary>
        /// 创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 最后更新人.
        /// </summary>
        public string LastModifier
        {
            get { return lastModifier; }
            set { lastModifier = value; }
        }
    }
}
