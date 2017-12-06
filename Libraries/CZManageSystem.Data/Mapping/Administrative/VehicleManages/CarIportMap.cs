using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarIportMap : EntityTypeConfiguration<CarIport>
	{
		public CarIportMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarIportId);
		/// <summary>
		/// 用户名
		/// <summary>
			  this.Property(t => t.UserName)

			.HasMaxLength(50);
		/// <summary>
		/// 手机
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("CarIport"); 
			// 主键
			this.Property(t => t.CarIportId).HasColumnName("CarIportId"); 
			// 结束时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 用户名
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			// 手机
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 口？
			this.Property(t => t.Iport).HasColumnName("Iport"); 
			// 车辆申请ID
			this.Property(t => t.CarApplyId).HasColumnName("CarApplyId"); 
			// 车辆ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// 状态
			this.Property(t => t.Status).HasColumnName("Status"); 
		 }
	 }
}
