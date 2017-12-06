using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// 短信发送统计视图
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.SmsManager
{
    //合同信息查询条件
    public class V_SendSmsCountQueryBuilder
    {
        public string SenderName { get; set; }//发送人
        public string DeptFullName { get; set; }//发送部门
        public DateTime? Date_start { get; set; }//发送日期
        public DateTime? Date_end { get; set; }//发送日期
    }

    public class V_SendSmsCount
	{
		public string Dept { get; set;}
		public Nullable<Guid> Sender { get; set;}
		public Nullable<DateTime> Date { get; set;}
		public Nullable<int> Count { get; set; }
        public string SenderName { get; set; }
        public string DeptFullName { get; set; }
        

    }
}
