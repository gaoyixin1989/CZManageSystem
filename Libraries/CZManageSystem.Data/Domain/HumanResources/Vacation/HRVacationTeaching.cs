using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 内部讲师授课
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationTeaching
	{
        public Guid ID { get; set; }

        /// <summary>
        /// 授课课程名称
        /// <summary>
        public string TeachingPlan { get; set;}
		/// <summary>
		/// 讲师级别
		/// <summary>
		public string TeacherType { get; set;}
		/// <summary>
		/// 天数
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// 开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 假期表的ID
		/// <summary>
		public Nullable<Guid> VacationID { get; set;}
		/// <summary>
		/// 积分
		/// <summary>
		public Nullable<decimal> Integral { get; set;}
		/// <summary>
		/// 同意的标识
		/// <summary>
		public Nullable<int> AgreeFlag { get; set;}
		/// <summary>
		/// 销假ID
		/// <summary>
		public Nullable<Guid> ReVacationID { get; set;}
		public Nullable<Guid> UserId { get; set;}
		public string UserName { get; set;}
		public string Ftst { get; set;}
		public string Ftet { get; set;}
		public string Hispt { get; set;}

	}

    public class HRVacationTeachingQueryBuilder
    {
        public string EmployeeID { get; set; }//地点
        public string[] DpId { get; set; }//部门ID
        public string RealName { get; set; }//名称

        public string TeacherType { get; set; }//地点
        public string Year { get; set; }
    }
}
