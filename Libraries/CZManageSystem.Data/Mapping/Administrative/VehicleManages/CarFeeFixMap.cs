using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeFixMap : EntityTypeConfiguration<CarFeeFix>
	{
		public CarFeeFixMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeFixId);
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
 			 this.ToTable("CarFeeFix"); 
			// 主键
			this.Property(t => t.CarFeeFixId).HasColumnName("CarFeeFixId"); 
			// 编辑人ID
			this.Property(t => t.EditorId).HasColumnName("EditorId"); 
			// 编辑日期
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			// 所属单位
			this.Property(t => t.CorpId).HasColumnName("CorpId"); 
			// 车辆ID
			this.Property(t => t.CarId).HasColumnName("CarId"); 
			// 缴费日期
			this.Property(t => t.PayTime).HasColumnName("PayTime"); 
			// 保险费
			this.Property(t => t.FolicyFee).HasColumnName("FolicyFee"); 
			// 车船税
			this.Property(t => t.TaxFee).HasColumnName("TaxFee"); 
			// 公路基金
			this.Property(t => t.RoadFee).HasColumnName("RoadFee"); 
			// 其它杂费
			this.Property(t => t.OtherFee).HasColumnName("OtherFee"); 
			// 费用小计
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// 计费开始日期
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束日期
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 经手人
			this.Property(t => t.Person).HasColumnName("Person"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark");
            // Relationships
            this.HasOptional(t => t.CarInfo).WithMany()
                .HasForeignKey(d => d.CarId);
        }
	 }
}
