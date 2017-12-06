using System;
using System.Collections.Generic;

namespace Botwave.Security.Domain
{
	/// <summary>
    /// 记录实体级别权限类.
    /// </summary>
    [Serializable]
	public class EntityPermission
	{
		#region Getter / Setter
		
		private int id;
		private string entityType;
		private string entityId;
		private string permissionType;
		private string permissionValue;

        /// <summary>
        /// 默认构造方法.创建 EntityPermissions 实例对象.
        /// </summary>
        public EntityPermission()
        {
            this.entityType = string.Empty;
            this.entityId = string.Empty;
            this.permissionType = string.Empty;
            this.permissionValue = string.Empty;
        }
		
		/// <summary>
        /// 实体权限 ID.
        /// </summary>
		public int Id
		{
			get{ return id; }
			set{ id = value; }
		}

		/// <summary>
        /// 实体类型.
        /// </summary>
		public string EntityType
		{
			get{ return entityType; }
			set{ entityType = value; }
		}

		/// <summary>
        /// 实体 ID.
        /// </summary>
		public string EntityId
		{
			get{ return entityId; }
			set{ entityId = value; }
		}

		/// <summary>
        /// 权限类型.
        /// </summary>
		public string PermissionType
		{
			get{ return permissionType; }
			set{ permissionType = value; }
		}

		/// <summary>
        /// 权限值.
        /// </summary>
		public string PermissionValue
		{
			get{ return permissionValue; }
			set{ permissionValue = value; }
		}
		
		#endregion
	}
}
