using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// 短信发送表
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.SmsManager
{
    //合同信息查询条件
    public class SendSmsQueryBuilder
    {
        public string Context { get; set; }//发送内容
        public string UserName { get; set; }//发送人
        public string DeptName { get; set; }//发送部门
        public DateTime? Date_start { get; set; }//发送日期
        public DateTime? Date_end { get; set; }//发送日期
        public bool? Error { get; set; }//发送状况
    }

    public class SendSms
	{
		/// <summary>
		/// id
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 手机号码
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 短信内容
		/// <summary>
		public string Context { get; set;}
		/// <summary>
		/// 发送时间
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// 发送人
		/// <summary>
		public Nullable<Guid> Sender { get; set;}
		/// <summary>
		/// 是否出错
		/// <summary>
		public Nullable<bool> Error { get; set;}
		/// <summary>
		/// 计数
		/// <summary>
		public Nullable<int> Count { get; set;}
		/// <summary>
		/// 发送日期
		/// <summary>
		public Nullable<DateTime> Date { get; set;}
		/// <summary>
		/// 是否显示名称
		/// <summary>
		public string ShowName { get; set; }
        /// <summary>
        /// 发送部门id
        /// <summary>
        public string Dept { get; set; }

        //外键
        public virtual Users SenderObj { get; set; }
        public virtual Depts DeptObj { get; set; }

    }
}
