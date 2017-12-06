using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-基本套餐维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_SetmealMap : EntityTypeConfiguration<MarketOrder_Setmeal>
	{
		public MarketOrder_SetmealMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 套餐名称
		/// <summary>
			  this.Property(t => t.Setmeal)

			.HasMaxLength(50);
		/// <summary>
		/// 说明备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Setmeal"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 套餐名称
			this.Property(t => t.Setmeal).HasColumnName("Setmeal"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
			// 说明备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
