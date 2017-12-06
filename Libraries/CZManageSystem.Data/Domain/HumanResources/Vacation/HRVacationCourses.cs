using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 假期培训
/// </summary>
namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
	public class HRVacationCourses
	{
		public Guid CoursesId { get; set;}
		/// <summary>
		/// 课程名称
		/// <summary>
		public string CoursesName { get; set;}
		/// <summary>
		/// 课程类别
		/// <summary>
		public string CoursesType { get; set;}
		/// <summary>
		/// 主办单位
		/// <summary>
		public string ProvinceCity { get; set;}
		/// <summary>
		/// 开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 培训天数
		/// <summary>
		public Nullable<decimal> PeriodTime { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 假期表ID
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
		public Nullable<Guid> UserId { get; set;}
		public Nullable<int> DaoId { get; set;}
		public string UserName { get; set;}
		public string Ftst { get; set;}
		public string Ftet { get; set;}
		public string Hispt { get; set;}
		/// <summary>
		/// 销假ID
		/// <summary>
		public Nullable<Guid> ReVacationID { get; set;}

	}
    public class HRVacationCoursesQueryBuilder
    {
        public string EmployeeID { get; set; }//地点
        public string[] DpId { get; set; }//部门ID
        public string RealName { get; set; }//名称

        public string CoursesType { get; set; }//地点
        public string ProvinceCity { get; set; }//名称
        public string Year { get; set; }
    }
}
