using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// 休假申请
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
    //休假申请查询
    public class VacationApplyQueryBuilder
    {
        public string ApplyTitle { get; set; }//标题
        public int? State { get; set; }//状态
        public Guid? Editor { get; set; }//提交人
        public DateTime? EditTime_start { get; set; }//提交时间
        public DateTime? EditTime_end { get; set; }
        public List<int> WF_State { get; set; }
    }
    public class HRVacationApply
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid ApplyId { get; set;}
		/// <summary>
		/// 流程实例Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		/// <summary>
		/// 流程单号
		/// <summary>
		public string ApplySn { get; set;}
		/// <summary>
		/// 标题
		/// <summary>
		public string ApplyTitle { get; set;}
		/// <summary>
		/// 编辑者
		/// <summary>
		public Nullable<Guid> Editor { get; set; }
        /// <summary>
        /// 编辑时间、申请时间
        /// <summary>
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// 休假类型
        /// <summary>
        public string VacationType { get; set;}
		/// <summary>
		/// 公假类型
		/// <summary>
		public string VacationClass { get; set;}
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
		/// 休假原因
		/// <summary>
		public string Reason { get; set;}
		/// <summary>
		/// 销假
		/// <summary>
		public Nullable<short> CancelVacation { get; set;}
		/// <summary>
		/// 已销假天数
		/// <summary>
		public Nullable<decimal> CancelDays { get; set;}
		public string Newpt { get; set;}
		public string Newst { get; set;}
		public string Newet { get; set;}
		/// <summary>
		/// 外出地点
		/// <summary>
		public string OutAddress { get; set;}
		/// <summary>
		/// 加班时间
		/// <summary>
		public string OverTime { get; set;}
		/// <summary>
		/// 附件IDs
		/// <summary>
		public string Attids { get; set;}

        /// <summary>
        /// 状态：0未提交，1已提交
        /// <summary>
        public Nullable<int> State { get; set; }


        //外键
        public virtual Tracking_Workflow Tracking_Workflow { get; set; }
        public virtual Users EditorObj { get; set; }
        
        public virtual ICollection<HRVacationMeeting> Meetings { get; set; }
        public virtual ICollection<HRVacationCourses> Courses { get; set; }
        public virtual ICollection<HRVacationTeaching> Teachings { get; set; }
        public virtual ICollection<HRVacationOther> Others { get; set; }

    }
}
