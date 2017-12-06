using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarInfoMap : EntityTypeConfiguration<CarInfo>
	{
		public CarInfoMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarId);
		/// <summary>
		/// 编号
		/// <summary>
			  this.Property(t => t.SN)

			.HasMaxLength(100);
		/// <summary>
		/// 车牌号码
		/// <summary>
			  this.Property(t => t.LicensePlateNum)

			.HasMaxLength(100);
		/// <summary>
		/// 汽车品牌
		/// <summary>
			  this.Property(t => t.CarBrand)

			.HasMaxLength(100);
		/// <summary>
		/// 型号
		/// <summary>
			  this.Property(t => t.CarModel)

			.HasMaxLength(100);
		/// <summary>
		/// 发动机号
		/// <summary>
			  this.Property(t => t.CarEngine)

			.HasMaxLength(100);
		/// <summary>
		/// 车架号
		/// <summary>
			  this.Property(t => t.CarNum)

			.HasMaxLength(100);
		/// <summary>
		/// 车辆类型
		/// <summary>
			  this.Property(t => t.CarType)

			.HasMaxLength(100);
		/// <summary>
		/// 吨位/人数
		/// <summary>
			  this.Property(t => t.CarTonnage)

			.HasMaxLength(100);
		/// <summary>
		/// 管理部门
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
		/// <summary>
		/// 购买价
		/// <summary>
			  this.Property(t => t.CarPrice)

			.HasMaxLength(100);
		/// <summary>
		/// 折旧年限
		/// <summary>
			  this.Property(t => t.CarLimit)

			.HasMaxLength(100);
		/// <summary>
		/// 每月折旧
		/// <summary>
			  this.Property(t => t.Depre)

			.HasMaxLength(100);
			  this.Property(t => t.Field00)

			.HasMaxLength(200);
			  this.Property(t => t.Field01)

			.HasMaxLength(200);
			  this.Property(t => t.Field02)

			.HasMaxLength(200);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarInfo"); 
			// 主键
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// 编辑人
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 编辑日期
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 所属单位
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// 编号
			this.Property(t => t.SN).HasColumnName("SN"); 
			// 车牌号码
			this.Property(t => t.LicensePlateNum).HasColumnName("LicensePlateNum"); 
			// 汽车品牌
			this.Property(t => t.CarBrand).HasColumnName("CarBrand"); 
			// 型号
			this.Property(t => t.CarModel).HasColumnName("CarModel"); 
			// 发动机号
			this.Property(t => t.CarEngine).HasColumnName("CarEngine"); 
			// 车架号
			this.Property(t => t.CarNum).HasColumnName("CarNum"); 
			// 车辆类型
			this.Property(t => t.CarType).HasColumnName("CarType"); 
			// 吨位/人数
			this.Property(t => t.CarTonnage).HasColumnName("CarTonnage"); 
			// 管理部门
			this.Property(t => t.DeptName).HasColumnName("DeptName"); 
			// 购买日期
			this.Property(t => t.BuyDate).HasColumnName("BuyDate"); 
			// 购买价
			this.Property(t => t.CarPrice).HasColumnName("CarPrice"); 
			// 折旧年限
			this.Property(t => t.CarLimit).HasColumnName("CarLimit"); 
			// 每月折旧
			this.Property(t => t.Depre).HasColumnName("Depre"); 
			// 租赁开始时间
			this.Property(t => t.RentTime1).HasColumnName("RentTime1"); 
			// 租赁结束时间
			this.Property(t => t.RentTime2).HasColumnName("RentTime2"); 
			// 保险开始时间
			this.Property(t => t.PolicyTime1).HasColumnName("PolicyTime1"); 
			// 保险结束时间
			this.Property(t => t.PolicyTime2).HasColumnName("PolicyTime2"); 
			// 年审开始时间
			this.Property(t => t.AnnualTime1).HasColumnName("AnnualTime1"); 
			// 年审结束时间
			this.Property(t => t.AnnualTime2).HasColumnName("AnnualTime2"); 
			// 驾驶员
			this.Property(t => t.DriverId).HasColumnName("DriverId"); 
			// 状态
			this.Property(t => t.Status).HasColumnName("Status"); 
			this.Property(t => t.Field00).HasColumnName("Field00"); 
			this.Property(t => t.Field01).HasColumnName("Field01"); 
			this.Property(t => t.Field02).HasColumnName("Field02"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark");

            this.HasOptional(t => t.CarDriverInfo).WithMany().HasForeignKey(t => t.DriverId);


         }
	 }
}
