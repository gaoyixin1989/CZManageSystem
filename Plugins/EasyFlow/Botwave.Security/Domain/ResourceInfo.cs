using System;
using System.Collections.Generic;

namespace Botwave.Security.Domain
{
	/// <summary>
	/// 访问资源信息类.
    /// </summary>
    [Serializable]
	public class ResourceInfo
	{
		#region Getter / Setter
		
		private string resourceId;
		private string parentId;
		private string type;
		private string name;
		private string alias;
		private bool enabled;
        private bool visible;
		
		/// <summary>
        /// 资源 ID.
        /// </summary>
		public string ResourceId
		{
			get{ return resourceId; }
			set{ resourceId = value; }
		}

		/// <summary>
        /// 父资源 ID.
        /// </summary>
		public string ParentId
		{
			get{ return parentId; }
			set{ parentId = value; }
		}

		/// <summary>
        /// 资源类型.
        /// </summary>
		public string Type
		{
			get{ return type; }
			set{ type = value; }
		}

		/// <summary>
        /// 资源名称(功能名称).
        /// </summary>
		public string Name
		{
			get{ return name; }
			set{ name = value; }
		}

		/// <summary>
        /// 资源别名(用于界面显示，如:中文).
        /// </summary>
		public string Alias
		{
			get{ return alias; }
			set{ alias = value; }
		}

		/// <summary>
        /// 资源是否可用.
        /// </summary>
		public bool Enabled
		{
			get{ return enabled; }
			set{ enabled = value; }
        }

        /// <summary>
        /// 资源在界面上的可见性.
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        #endregion

        /// <summary>
        /// 构造方法.创建 Resources 实例对象.
        /// </summary>
        public ResourceInfo()
        {
            this.parentId = string.Empty;
            this.type = string.Empty;
            this.name = string.Empty;
            this.alias = string.Empty;
            this.enabled = true;
        }
	}
}	
