using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarDriverInfoMap : EntityTypeConfiguration<CarDriverInfo>
	{
		public CarDriverInfoMap()
		{
			// Primary Key
			 this.HasKey(t => t.DriverId);
		/// <summary>
		/// 司机编号
		/// <summary>
			  this.Property(t => t.SN)

			.HasMaxLength(100);
			  this.Property(t => t.Name)

			.HasMaxLength(100);
		/// <summary>
		/// 手机号
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(100);
		/// <summary>
		/// 部门名字
		/// <summary>
			  this.Property(t => t.DeptName)

			.HasMaxLength(100);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarDriverInfo"); 
			// 主键
			this.Property(t => t.DriverId).HasColumnName("DriverId"); 
			// 编辑人
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 编辑时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 所属单位
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// 司机编号
			this.Property(t => t.SN).HasColumnName("SN"); 
			this.Property(t => t.Name).HasColumnName("Name"); 
			// 手机号
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 部门名字
			this.Property(t => t.DeptName).HasColumnName("DeptName"); 
			// 开始驾驶时间
			this.Property(t => t.CarAge).HasColumnName("CarAge"); 
			// 生日
			this.Property(t => t.Birthday).HasColumnName("Birthday"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
