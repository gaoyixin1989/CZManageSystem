using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarApplyRent
	{
		public Guid ApplyRentId { get; set;}
		/// <summary>
		/// 流程实例Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
        /// <summary>
        /// 流程单号
        /// </summary>
        public string ApplySn { get; set; }
        /// <summary>
        /// 标题
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// 所属单位
        /// <summary>
        public Nullable<int> CorpId { get; set;}
		/// <summary>
		/// 申请时间
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// 所在部门
		/// <summary>
		public string DeptName { get; set;}
		/// <summary>
		/// 申请人
		/// <summary>
		public string ApplyCant { get; set;}
		/// <summary>
		/// 驾驶人、使用人
		/// <summary>
		public string Driver { get; set;}
		/// <summary>
		/// 联系电话
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 预计结束时间
		/// <summary>
		public Nullable<DateTime> TimeOut { get; set;}
		/// <summary>
		/// 出车开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 出发地点
		/// <summary>
		public string Starting { get; set;}
		/// <summary>
		/// 目的地1
		/// <summary>
		public string Destination1 { get; set;}
		/// <summary>
		/// 目的地2
		/// <summary>
		public string Destination2 { get; set;}
		/// <summary>
		/// 目的地3
		/// <summary>
		public string Destination3 { get; set;}
		/// <summary>
		/// 目的地4
		/// <summary>
		public string Destination4 { get; set;}
		/// <summary>
		/// 目的地5
		/// <summary>
		public string Destination5 { get; set;}
		/// <summary>
		/// 总人数
		/// <summary>
		public string PersonCount { get; set;}
		/// <summary>
		/// 路途类别
		/// <summary>
		public string Road { get; set;}
		/// <summary>
		/// 车辆用途
		/// <summary>
		public string UseType { get; set;}
		/// <summary>
		/// 请求？
		/// <summary>
		public string Request { get; set;}
		/// <summary>
		/// 附加
		/// <summary>
		public string Attach { get; set;}
		/// <summary>
		/// 领域00
		/// <summary>
		public string Field00 { get; set;}
		/// <summary>
		/// 领域01
		/// <summary>
		public string Field01 { get; set;}
		/// <summary>
		/// 领域02
		/// <summary>
		public string Field02 { get; set;}
		/// <summary>
		/// 安排人、分配者
		/// <summary>
		public string Allocator { get; set;}
		/// <summary>
		/// 分配人审批的时间
		/// <summary>
		public Nullable<DateTime> AllotTime { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 租车类型
		/// <summary>
		public string Htype { get; set;}
		/// <summary>
		/// 吨位/人数
		/// <summary>
		public string CarTonnage { get; set;}
		/// <summary>
		/// 租车询价
		/// <summary>
		public string Enquiry { get; set;}
		/// <summary>
		/// 是否在费用预算额度内
		/// <summary>
		public Nullable<bool> AllotRight { get; set;}

        public virtual Tracking_Workflow TrackingWorkflow { get; set; }

    }
}
