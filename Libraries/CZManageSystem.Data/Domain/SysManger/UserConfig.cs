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
		/// ���
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// �û�id
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// ���ñ�ʶ
		/// <summary>
		public string ConfigName { get; set;}
		/// <summary>
		/// ����ֵ
		/// <summary>
		public string ConfigValue { get; set;}


        public virtual Users UserObj { get; set; }

    }
}
