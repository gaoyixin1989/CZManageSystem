using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-受理单状态维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderStatusMap : EntityTypeConfiguration<MarketOrder_OrderStatus>
	{
		public MarketOrder_OrderStatusMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 受理单状态
		/// <summary>
			  this.Property(t => t.OrderStatus)

			.HasMaxLength(50);
		/// <summary>
		/// 说明
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderStatus"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
			// 受理单状态
			this.Property(t => t.OrderStatus).HasColumnName("OrderStatus"); 
			// 说明
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
