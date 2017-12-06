using System;
using System.Collections.Generic;

namespace Botwave.Security.Domain
{
	/// <summary>
    /// 部门信息类.
    /// </summary>
    [Serializable]
	public class Department
	{
		#region Getter / Setter
		
		private string dpId;
		private string dpName = string.Empty;
		private string parentDpId = string.Empty;
		private string dpFullName = string.Empty;
		private int dpLevel;
		private int deptOrderNo;
		private bool isTmpDp;
		private byte type;
        private string creator = string.Empty;
        private string lastModifier = string.Empty;
        private DateTime createdTime;
        private DateTime lastModTime;
		
		/// <summary>
        /// 部门 ID.
        /// </summary>
		public string DpId
		{
			get{ return dpId; }
			set{ dpId = value; }
		}

		/// <summary>
        /// 部门名称.
        /// </summary>
		public string DpName
		{
			get{ return dpName; }
			set{ dpName = value; }
		}

		/// <summary>
        /// 部门的上级部门 ID.
        /// </summary>
		public string ParentDpId
		{
			get{ return parentDpId; }
			set{ parentDpId = value; }
		}

		/// <summary>
        /// 部门的全名.
        /// </summary>
		public string DpFullName
		{
			get{ return dpFullName; }
			set{ dpFullName = value; }
		}

		/// <summary>
        /// 部门的等级级别.
        /// </summary>
		public int DpLevel
		{
			get{ return dpLevel; }
			set{ dpLevel = value; }
		}

		/// <summary>
        /// 部门排序号.
        /// </summary>
		public int DeptOrderNo
		{
			get{ return deptOrderNo; }
			set{ deptOrderNo = value; }
		}

		/// <summary>
        /// 是否为临时部门.
        /// </summary>
		public bool IsTmpDp
		{
			get{ return isTmpDp; }
			set{ isTmpDp = value; }
		}

		/// <summary>
        /// 部门类型.
        /// </summary>
		public byte Type
		{
			get{ return type; }
			set{ type = value; }
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

		#endregion
	}
}	
