using System;
using System.Collections.Generic;

namespace Botwave.Security.Domain
{
	/// <summary>
	/// 角色信息类.
    /// </summary>
    [Serializable]
	public class RoleInfo
	{
		#region Getter / Setter
		
		private Guid roleId;
        private Guid parentId;
        private bool isInheritable;
		private string roleName;
		private string comment;
		private DateTime beginTime;
        private DateTime endTime;
        private string creator;
        private string lastModifier;
        private DateTime createdTime;
        private DateTime lastModTime;
        private int sortOrder;

        /// <summary>
        /// 默认构造方法.
        /// </summary>
        public RoleInfo()
            : this(string.Empty)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="roleName"></param>
        public RoleInfo(string roleName)
        {
            this.roleId = Guid.Empty;
            this.parentId = Guid.Empty;
            this.roleName = roleName;
        }
		
		/// <summary>
        /// 角色 ID.
        /// </summary>
		public Guid RoleId
		{
			get{ return roleId; }
			set{ roleId = value; }
        }

        /// <summary>
        /// 父角色 ID.
        /// </summary>
        public Guid ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        /// <summary>
        /// 是否可继承的. true 则表示可继承, false 则表示父子角色只是用于层次性.
        /// 默认值为 false.
        /// </summary>
        public bool IsInheritable
        {
            get { return isInheritable; }
            set { isInheritable = value; }
        }

		/// <summary>
        /// 角色名称.
        /// </summary>
		public string RoleName
		{
			get{ return roleName; }
			set{ roleName = value; }
		}

		/// <summary>
        /// 备注信息.
        /// </summary>
		public string Comment
		{
			get{ return comment; }
			set{ comment = value; }
		}

		/// <summary>
        /// 角色启用时间.
        /// </summary>
		public DateTime BeginTime
		{
			get{ return beginTime; }
			set{ beginTime = value; }
		}

		/// <summary>
        /// 角色使用终止时间.
        /// </summary>
		public DateTime EndTime
		{
			get{ return endTime; }
			set{ endTime = value; }
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
        /// 排序序号.
        /// </summary>
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

		#endregion
	}
}
