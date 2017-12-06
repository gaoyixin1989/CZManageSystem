using System;
using System.Collections.Generic;

namespace Botwave.Security.Domain
{
	/// <summary>
	/// 用户信息类.
    /// </summary>
    [Serializable]
	public class UserInfo
	{
		#region Getter / Setter
		
		private Guid userId;
        private string userName;
        private string realName;
		private string password = String.Empty;
		private string email = String.Empty;
        private string mobile = String.Empty;
        private string tel;
        private string employeeId;
		private int type;
        private int status;
		private string dpId = String.Empty;
		private int ext_Int;
		private decimal ext_Decimal;
		private string ext_Str1 = String.Empty;
		private string ext_Str2 = String.Empty;
		private string ext_Str3 = String.Empty;
		private DateTime createdTime;
		private DateTime lastModTime;
		private string creator = String.Empty;
		private string lastModifier = String.Empty;

        /// <summary>
        /// 默认构造方法.
        /// </summary>
        public UserInfo()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public UserInfo(string userName, string password)
        {
            this.userId = Guid.NewGuid();
            this.UserName = userName;
            this.password = password;
            this.RealName = userName;
            this.creator = "admin";
            this.lastModifier = "admin";
            this.createdTime = DateTime.Now;
            this.lastModTime = DateTime.Now;
        }

		/// <summary>
        /// 用户 ID.
        /// </summary>
		public Guid UserId
		{
			get{ return userId; }
			set{ userId = value; }
        }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 用户真实姓名.
        /// </summary>
        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }

		/// <summary>
        /// 用户密码.
        /// </summary>
		public string Password
		{
			get{ return password; }
			set{ password = value; }
		}

		/// <summary>
        /// 电子邮箱.
        /// </summary>
		public string Email
		{
			get{ return email; }
			set{ email = value; }
		}

		/// <summary>
        /// 手机号码.
        /// </summary>
		public string Mobile
		{
			get{ return mobile; }
			set{ mobile = value; }
        }

        /// <summary>
        /// 电话号码.
        /// </summary>
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        /// <summary>
        /// 用户的员工ID.
        /// </summary>
        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

		/// <summary>
        /// 用户类型.0:内部; 1:外部.
        /// </summary>
		public int Type
		{
			get{ return type; }
			set{ type = value; }
		}

		/// <summary>
        /// 用户状态.0:正常; -1:禁用.
        /// </summary>
        public int Status
		{
			get{ return status; }
			set{ status = value; }
		}

		/// <summary>
        /// 用户所属的部门 ID.
        /// </summary>
		public string DpId
		{
			get{ return dpId; }
			set{ dpId = value; }
		}

		/// <summary>
        /// 扩展字段(int 类型).
        /// </summary>
		public int Ext_Int
		{
			get{ return ext_Int; }
			set{ ext_Int = value; }
		}

		/// <summary>
        /// 扩展字段(decimal 类型).
        /// </summary>
		public decimal Ext_Decimal
		{
			get{ return ext_Decimal; }
			set{ ext_Decimal = value; }
		}

		/// <summary>
        /// 扩展字段 1(string 类型).
        /// </summary>
		public string Ext_Str1
		{
			get{ return ext_Str1; }
			set{ ext_Str1 = value; }
		}

		/// <summary>
        /// 扩展字段 2(string 类型).
        /// </summary>
		public string Ext_Str2
		{
			get{ return ext_Str2; }
			set{ ext_Str2 = value; }
		}

		/// <summary>
        /// 扩展字段 3(string 类型).
        /// </summary>
		public string Ext_Str3
		{
			get{ return ext_Str3; }
			set{ ext_Str3 = value; }
		}

		/// <summary>
        /// 创建时间.
        /// </summary>
		public DateTime CreatedTime
		{
			get{ return createdTime; }
			set{ createdTime = value; }
		}

		/// <summary>
        /// 最近一次的修改时间.
        /// </summary>
		public DateTime LastModTime
		{
			get{ return lastModTime; }
			set{ lastModTime = value; }
		}

		/// <summary>
        /// 创建人.
        /// </summary>
		public string Creator
		{
			get{ return creator; }
			set{ creator = value; }
		}

		/// <summary>
        /// 最近一次的修改人.
        /// </summary>
		public string LastModifier
		{
			get{ return lastModifier; }
			set{ lastModifier = value; }
		}
		
		#endregion		

        #region 非持久属性

        private string _dpFullName;

        /// <summary>
        /// 用户所在部门的全名.
        /// </summary>
        public string DpFullName
        {
            get { return _dpFullName; }
            set { _dpFullName = value; }
        }

        #endregion
    }
}	
