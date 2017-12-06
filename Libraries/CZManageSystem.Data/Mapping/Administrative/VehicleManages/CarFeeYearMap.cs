using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeYearMap : EntityTypeConfiguration<CarFeeYear>
	{
		public CarFeeYearMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeYearId);
		/// <summary>
		/// 使用单位
		/// <summary>
			  this.Property(t => t.CorpName)

			.HasMaxLength(100);
		/// <summary>
		/// 经手人
		/// <summary>
			  this.Property(t => t.Person)

			.HasMaxLength(100);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarFeeYear"); 
			// 主键
			this.Property(t => t.CarFeeYearId).HasColumnName("CarFeeYearId"); 
			// 编辑人ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 编辑日期
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 所属单位
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// 使用单位
			this.Property(t => t.CorpName).HasColumnName("CorpName"); 
			// 车辆ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// 缴费日期
			this.Property(t => t.PayTime).HasColumnName("PayTime"); 
			// 费用小计
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// 经手人
			this.Property(t => t.Person).HasColumnName("Person"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark");

            this.HasOptional(t => t.CarInfo).WithMany().HasForeignKey(t => t.CarId);
        }
	 }
}
