using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-回收结果信息
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderReclaimMap : EntityTypeConfiguration<MarketOrder_OrderReclaim>
	{
		public MarketOrder_OrderReclaimMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 回收结果
		/// <summary>
			  this.Property(t => t.IsReclaim)

			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.ReclaimRemark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderReclaim"); 
			// 编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 申请单ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 操作用户ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// 操作时间
			this.Property(t => t.Time).HasColumnName("Time"); 
			// 回收结果
			this.Property(t => t.IsReclaim).HasColumnName("IsReclaim"); 
			// 备注
			this.Property(t => t.ReclaimRemark).HasColumnName("ReclaimRemark"); 
		 }
	 }
}
