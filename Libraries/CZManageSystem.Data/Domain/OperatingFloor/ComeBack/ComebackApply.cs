using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ComebackApply
	{  
		public Guid ApplyId { get; set;} 
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		/// <summary>
		/// 标题
		/// <summary>
		public string Title { get; set;} 
		/// <summary>
		/// 流程单号
		/// <summary>
		public string Series { get; set;}
		/// <summary>
		/// 电话
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 状态
		/// <summary>
		public int Status { get; set;}
		/// <summary>
		/// 申请时间
		/// <summary>
		public Nullable<DateTime> ApplyTime { get; set;}
		/// <summary>
		/// 部门
		/// <summary>
		public string ApplyDept { get; set;}
		/// <summary>
		/// 申请人
		/// <summary>
		public string ApplyUser { get; set;}
		/// <summary>
		/// 预算需求部门
		/// <summary>
		public string BudgetDept { get; set;}
		/// <summary>
		/// 申请资源类别
		/// <summary>
		public Nullable<Guid> SourceTypeID { get; set;}
		/// <summary>
		/// 项目开始时间
		/// <summary>
		public Nullable<DateTime> TimeStart { get; set;}
		/// <summary>
		/// 项目结束时间
		/// <summary>
		public Nullable<DateTime> TimeEnd { get; set;}
		public string SourceChildId { get; set;}
		/// <summary>
		/// 拟开展项目名称
		/// <summary>
		public string ProjName { get; set;}
		/// <summary>
		/// 拟立或已立预算项目名称
		/// <summary>
		public string PrevProjName { get; set;}
		/// <summary>
		/// 拟立或已立预算项目编号
		/// <summary>
		public string PrevProjCode { get; set;}
		/// <summary>
		/// 项目经办人
		/// <summary>
		public string ProjManager { get; set;}
		/// <summary>
		/// 不含税金额申请额度
		/// <summary>
		public Nullable<decimal> AppAmount { get; set;}
		/// <summary>
		/// 项目开展必要性及效益性分析
		/// <summary>
		public string ProjAnalysis { get; set;}
		/// <summary>
		/// 预算年度
		/// <summary>
		public Nullable<int> Year { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 含税金额申请额度
		/// <summary>
		public Nullable<decimal> AppAmountHanshui { get; set;}

	}

    public class ComebackInfoQueryBuilder
    {
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string Name { get; set; }

        public string BudgetDept { get; set; }
        /// <summary>
		/// 申请人
		/// <summary>
		public string ApplyUser { get; set; }

    }
}
