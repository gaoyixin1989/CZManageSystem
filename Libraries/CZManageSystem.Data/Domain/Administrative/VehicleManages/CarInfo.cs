using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarInfo
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid CarId { get; set;}
		/// <summary>
		/// 编辑人
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// 编辑日期
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// 所属单位
		/// <summary>
		public Nullable<int> CorpId { get; set;}
		/// <summary>
		/// 编号
		/// <summary>
		public string SN { get; set;}
		/// <summary>
		/// 车牌号码
		/// <summary>
		public string LicensePlateNum { get; set;}
		/// <summary>
		/// 汽车品牌
		/// <summary>
		public string CarBrand { get; set;}
		/// <summary>
		/// 型号
		/// <summary>
		public string CarModel { get; set;}
		/// <summary>
		/// 发动机号
		/// <summary>
		public string CarEngine { get; set;}
		/// <summary>
		/// 车架号
		/// <summary>
		public string CarNum { get; set;}
		/// <summary>
		/// 车辆类型
		/// <summary>
		public string CarType { get; set;}
		/// <summary>
		/// 吨位/人数
		/// <summary>
		public string CarTonnage { get; set;}
		/// <summary>
		/// 管理部门
		/// <summary>
		public string DeptName { get; set;}
		/// <summary>
		/// 购买日期
		/// <summary>
		public Nullable<DateTime> BuyDate { get; set;}
		/// <summary>
		/// 购买价
		/// <summary>
		public string CarPrice { get; set;}
		/// <summary>
		/// 折旧年限
		/// <summary>
		public string CarLimit { get; set;}
		/// <summary>
		/// 每月折旧
		/// <summary>
		public string Depre { get; set;}
		/// <summary>
		/// 租赁开始时间
		/// <summary>
		public Nullable<DateTime> RentTime1 { get; set;}
		/// <summary>
		/// 租赁结束时间
		/// <summary>
		public Nullable<DateTime> RentTime2 { get; set;}
		/// <summary>
		/// 保险开始时间
		/// <summary>
		public Nullable<DateTime> PolicyTime1 { get; set;}
		/// <summary>
		/// 保险结束时间
		/// <summary>
		public Nullable<DateTime> PolicyTime2 { get; set;}
		/// <summary>
		/// 年审开始时间
		/// <summary>
		public Nullable<DateTime> AnnualTime1 { get; set;}
		/// <summary>
		/// 年审结束时间
		/// <summary>
		public Nullable<DateTime> AnnualTime2 { get; set;}
		/// <summary>
		/// 驾驶员
		/// <summary>
		public Nullable<Guid> DriverId { get; set;}
		/// <summary>
		/// 状态
		/// <summary>
		public Nullable<int> Status { get; set;}
		public string Field00 { get; set;}
		public string Field01 { get; set;}
		public string Field02 { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}

        public virtual CarDriverInfo CarDriverInfo { get; set; }

    }


}
