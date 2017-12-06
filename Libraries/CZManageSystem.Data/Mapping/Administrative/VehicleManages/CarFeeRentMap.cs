using CZManageSystem.Data.Domain.Administrative.VehicleManages;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.VehicleManages
{
	public class CarFeeRentMap : EntityTypeConfiguration<CarFeeRent>
	{
		public CarFeeRentMap()
		{
			// Primary Key
			 this.HasKey(t => t.CarFeeRentId);
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
 			 this.ToTable("CarFeeRent"); 
			// 主键
			this.Property(t => t.CarFeeRentId).HasColumnName("CarFeeRentId"); 
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
			// 排序
			this.Property(t => t.SortId).HasColumnName("SortId"); 
			// 租赁费用
			this.Property(t => t.RentFee).HasColumnName("RentFee"); 
			// 租赁公里
			this.Property(t => t.RentCount).HasColumnName("RentCount"); 
			// 实际行驶公里
			this.Property(t => t.RoadCount).HasColumnName("RoadCount"); 
			// 超包租里程
			this.Property(t => t.MoreRoad).HasColumnName("MoreRoad"); 
			// 超包租里程费用
			this.Property(t => t.MoreFee).HasColumnName("MoreFee"); 
			// 汽油费
			this.Property(t => t.GasFee).HasColumnName("GasFee"); 
			// 路桥/停车费
			this.Property(t => t.RoadFee).HasColumnName("RoadFee"); 
			// 驾驶员补贴
			this.Property(t => t.DriverFee).HasColumnName("DriverFee"); 
			// 费用小计
			this.Property(t => t.TotalFee).HasColumnName("TotalFee"); 
			// 开始日期
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束日期
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 经手人
			this.Property(t => t.Person).HasColumnName("Person"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
