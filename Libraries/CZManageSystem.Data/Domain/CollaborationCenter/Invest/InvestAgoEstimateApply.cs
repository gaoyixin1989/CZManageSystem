using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// 历史项目暂估申请表
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //暂估申请查询
    public class AgoEstimateApplyQueryBuilder
    {
        public string Title { get; set; }
        public int? State { get; set; }
        public Guid? AppPerson { get; set; }
        public DateTime? ApplyTime_start { get; set; }
        public DateTime? ApplyTime_end { get; set; }
        public int? WF_State { get; set; }
    }

    public class InvestAgoEstimateApply
	{
		/// <summary>
		/// 编号
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// 流程实例ID
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		/// <summary>
		/// 流程单号
		/// <summary>
		public string Series { get; set;}
		/// <summary>
		/// 申请时间
		/// <summary>
		public Nullable<DateTime> ApplyTime { get; set;}
		/// <summary>
		/// 部门id
		/// <summary>
		public string AppDept { get; set;}
		/// <summary>
		/// 申请人
		/// <summary>
		public Nullable<Guid> AppPerson { get; set;}
		/// <summary>
		/// 申请人手机
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 申请标题
		/// <summary>
		public string Title { get; set;}
		/// <summary>
		/// 原因
		/// <summary>
		public string Content { get; set;}
		/// <summary>
		/// 状态：0保存、1提交
		/// <summary>
		public Nullable<int> State { get; set;}

        //外键
        public virtual Tracking_Workflow Tracking_Workflow { get; set; }
        public virtual Depts AppDeptObj { get; set; }
        public virtual Users AppPersonObj { get; set; }

    }
}
