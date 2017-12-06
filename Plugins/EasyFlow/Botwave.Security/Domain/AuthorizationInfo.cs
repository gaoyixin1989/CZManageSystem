using System;
using System.Collections.Generic;

namespace Botwave.Security.Domain
{
	/// <summary>
	/// 授权信息类.
	/// </summary>
    [Serializable]
	public class AuthorizationInfo
	{
		#region Getter / Setter
		
		private int id;
		private Guid fromUserId;
        private Guid toUserId;
        private bool isFullAuthorized;
		private DateTime beginTime;
        private DateTime endTime;
		private bool enabled;

        /// <summary>
        /// 构造方法.创建 Authorizations 实例对象.
        /// </summary>
        public AuthorizationInfo()
        {
            this.beginTime = DateTime.Now;
            this.EndTime = DateTime.Now.AddDays(1);
            this.enabled = true;
        }
		
		/// <summary>
        /// 授权 ID.
        /// </summary>
		public int Id
		{
			get{ return id; }
			set{ id = value; }
		}

		/// <summary>
        /// 授权用户.
        /// </summary>
        public Guid FromUserId
		{
			get{ return fromUserId; }
			set{ fromUserId = value; }
		}

		/// <summary>
        /// 被授权用户.
        /// </summary>
        public Guid ToUserId
		{
			get{ return toUserId; }
			set{ toUserId = value; }
        }

        /// <summary>
        /// 是否完全授权.
        /// </summary>
        public bool IsFullAuthorized
        {
            get { return isFullAuthorized; }
            set { isFullAuthorized = value; }
        }

		/// <summary>
        /// 授权的开始时间.
        /// </summary>
        public DateTime BeginTime
		{
			get{ return beginTime; }
			set{ beginTime = value; }
		}

		/// <summary>
        /// 授权的结束时间.
        /// </summary>
        public DateTime EndTime
		{
			get{ return endTime; }
			set{ endTime = value; }
		}

		/// <summary>
        /// 授权是否有效.
        /// </summary>
		public bool Enabled
		{
			get{ return enabled; }
			set{ enabled = value; }
		}
		
		#endregion		

        #region 非持久属性
        private string _fromRealName;
        private string _toRealName;
        private string _toUserName;
        private string _toDpFullName;

        /// <summary>
        /// 授权用户的真实姓名.
        /// </summary>
        public string FromRealName
        {
            get { return _fromRealName; }
            set { _fromRealName = value; }
        }

        /// <summary>
        /// 被授权用户的真实姓名.
        /// </summary>
        public string ToRealName
        {
            get { return _toRealName; }
            set { _toRealName = value; }
        }

        /// <summary>
        /// 被授权用户的用户名.
        /// </summary>
        public string ToUserName
        {
            get { return _toUserName; }
            set { _toUserName = value; }
        }

        /// <summary>
        /// 被授权的用户所属部门.
        /// </summary>
        public string ToDpFullName
        {
            get { return _toDpFullName; }
            set { _toDpFullName = value; }
        }

        #endregion
    }
}
