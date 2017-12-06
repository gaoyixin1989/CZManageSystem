using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarDriverInfo
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid DriverId { get; set;}
		/// <summary>
		/// 编辑人
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// 编辑时间
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 所属单位
		/// <summary>
		public Nullable<int> CorpId { get; set;}
		/// <summary>
		/// 司机编号
		/// <summary>
		public string SN { get; set;}
		public string Name { get; set;}
		/// <summary>
		/// 手机号
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 部门名字
		/// <summary>
		public string DeptName { get; set;}
		/// <summary>
		/// 开始驾驶时间
		/// <summary>
		public Nullable<DateTime> CarAge { get; set;}
		/// <summary>
		/// 生日
		/// <summary>
		public Nullable<DateTime> Birthday { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}

	}
}
