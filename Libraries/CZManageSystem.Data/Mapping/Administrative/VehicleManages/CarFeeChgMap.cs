using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeChgMap : EntityTypeConfiguration<CarFeeChg>
	{
		public CarFeeChgMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeChgId);
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
 			 this.ToTable("CarFeeChg"); 
			this.Property(t => t.CarFeeChgId).HasColumnName("CarFeeChgId"); 
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
			// 上月公里数
			this.Property(t => t.RoadLast).HasColumnName("RoadLast"); 
			// 本月公里数
			this.Property(t => t.RoadThis).HasColumnName("RoadThis"); 
			// 本月行驶公里数
			this.Property(t => t.RoadCount).HasColumnName("RoadCount"); 
			// 实用油量
			this.Property(t => t.OilCount).HasColumnName("OilCount"); 
			// 油价
			this.Property(t => t.OilPrice).HasColumnName("OilPrice"); 
			// 汽油费
			this.Property(t => t.OilFee).HasColumnName("OilFee"); 
			// 维修费
			this.Property(t => t.FixFee).HasColumnName("FixFee"); 
			// 路桥/停车费
			this.Property(t => t.RoadFee).HasColumnName("RoadFee"); 
			// 住宿费
			this.Property(t => t.LiveFee).HasColumnName("LiveFee"); 
			// 餐费
			this.Property(t => t.EatFee).HasColumnName("EatFee"); 
			// 其它杂费
			this.Property(t => t.OtherFee).HasColumnName("OtherFee"); 
			// 费用小计
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// 经手人
			this.Property(t => t.Person).HasColumnName("Person"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark");

            this.HasOptional(t => t.CarInfo).WithMany()
               .HasForeignKey(d => d.CarId);
        }
	 }
}
