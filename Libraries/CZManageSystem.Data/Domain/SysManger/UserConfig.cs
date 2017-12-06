using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class UserConfig
	{
		/// <summary>
		/// 编号
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 用户id
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// 配置标识
		/// <summary>
		public string ConfigName { get; set;}
		/// <summary>
		/// 配置值
		/// <summary>
		public string ConfigValue { get; set;}


        public virtual Users UserObj { get; set; }

    }
}
