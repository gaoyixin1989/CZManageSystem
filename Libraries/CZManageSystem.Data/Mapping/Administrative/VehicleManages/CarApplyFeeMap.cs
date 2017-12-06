using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarApplyFeeMap : EntityTypeConfiguration<CarApplyFee>
	{
		public CarApplyFeeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyFeeId);
		/// <summary>
		/// 结算人
		/// <summary>
			  this.Property(t => t.BalUser)

			.HasMaxLength(100);
            
            this.Property(t => t.ApplySn)
               .HasMaxLength(50);
            /// <summary>
            /// 标题
            /// <summary>
            this.Property(t => t.ApplyTitle)
           .HasMaxLength(150);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.BalRemark)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("CarApplyFee"); 
			// 主键
			this.Property(t => t.ApplyFeeId).HasColumnName("ApplyFeeId");
            // 流程实例Id
            this.Property(t => t.CarApplyId).HasColumnName("CarApplyId");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // 标题
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            // 车辆ID
            this.Property(t => t.CarId).HasColumnName("CarId"); 
			// 结算人
			this.Property(t => t.BalUser).HasColumnName("BalUser"); 
			// 结算时间
			this.Property(t => t.BalTime).HasColumnName("BalTime"); 
			// 起始里程
			this.Property(t => t.KmNum1).HasColumnName("KmNum1"); 
			// 终止里程
			this.Property(t => t.KmNum2).HasColumnName("KmNum2"); 
			// 使用里程
			this.Property(t => t.KmCount).HasColumnName("KmCount"); 
			// 单据数量
			this.Property(t => t.BalCount).HasColumnName("BalCount"); 
			// 合计金额
			this.Property(t => t.BalTotal).HasColumnName("BalTotal"); 
			// 备注
			this.Property(t => t.BalRemark).HasColumnName("BalRemark");

            this.HasOptional(t => t.CarApply).WithMany()
               .HasForeignKey(d => d.CarApplyId);

        }
	 }
}
