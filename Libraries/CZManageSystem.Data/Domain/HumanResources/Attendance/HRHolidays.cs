using System;
using System.ComponentModel.DataAnnotations;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRHolidays
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 编辑者ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// 编辑者
		/// <summary>
		public string Editor { get; set;}
		/// <summary>
		/// 编辑时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
        /// <summary>
        /// 假日名称
        /// <summary>
        [Required(ErrorMessage = "假日名称不能为空")]
        public string HolidayName { get; set;}
        /// <summary>
        /// 年度
        /// <summary>
        [Required(ErrorMessage = "年度不能为空且为整数")]
        public Nullable<DateTime> HolidayYear { get; set;}
        /// <summary>
        /// 开始时间
        /// <summary>
        [Required(ErrorMessage = "开始时间不能为空且为日期格式")]
        public Nullable<DateTime> StartTime { get; set;}
        /// <summary>
        /// 结束时间 
        /// <summary>
        [Required(ErrorMessage = "结束时间不能为空且为日期格式")]
        public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
        /// <summary>
        /// 假期类别。1、公休假日；2、法定假日
        /// <summary>
        [Required(ErrorMessage = "假期类别不能为空")]
        public string HolidayClass { get; set;}

	}
}
