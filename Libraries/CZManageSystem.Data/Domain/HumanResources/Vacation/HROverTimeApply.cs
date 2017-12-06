using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HROverTimeApply
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid ApplyID { get; set;}
		/// <summary>
		/// 标题
		/// <summary>
		public string ApplyTitle { get; set;}
		/// <summary>
		/// 流程实例Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		public string ApplySn { get; set;}
		/// <summary>
		/// 申请人
		/// <summary>
		public string ApplyUserName { get; set;}
		/// <summary>
		/// 开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 天数
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// 职务
		/// <summary>
		public string ApplyPost { get; set;}
		/// <summary>
		/// 直接主管姓名
		/// <summary>
		public string ManageName { get; set;}
		/// <summary>
		/// 直接主管职务
		/// <summary>
		public string ManagePost { get; set;}
		/// <summary>
		/// 加班地点
		/// <summary>
		public string Address { get; set;}
		/// <summary>
		/// 加班类型
		/// <summary>
		public string OvertimeType { get; set;}
		/// <summary>
		/// 加班原因
		/// <summary>
		public string Reason { get; set;}
		/// <summary>
		/// 编辑者
		/// <summary>
		public Nullable<Guid> Editor { get; set;}
        /// <summary>
        /// 申请时间
        /// <summary>
        public Nullable<DateTime> CreateTime { get; set; }
        public string Newpt { get; set;}

        public virtual Tracking_Workflow TrackingWorkflow { get; set; }
    }
}
