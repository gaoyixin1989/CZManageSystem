using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-营销方案维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_MarketMap : EntityTypeConfiguration<MarketOrder_Market>
	{
		public MarketOrder_MarketMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 营销方案名称
		/// <summary>
			  this.Property(t => t.Market)

			.HasMaxLength(1000);
		/// <summary>
		/// 序号
		/// <summary>
			  this.Property(t => t.Order)

			.HasMaxLength(50);
		/// <summary>
		/// 备注说明
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Market"); 
			// 编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 营销方案名称
			this.Property(t => t.Market).HasColumnName("Market"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
			// 生效时间
			this.Property(t => t.AbleTime).HasColumnName("AbleTime"); 
			// 失效时间
			this.Property(t => t.DisableTime).HasColumnName("DisableTime"); 
			// 备注说明
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 优化费用
			this.Property(t => t.PlanPay).HasColumnName("PlanPay"); 
			// 实收费用
			this.Property(t => t.MustPay).HasColumnName("MustPay"); 
			// 是否家宽业务
			this.Property(t => t.isJK).HasColumnName("isJK"); 
		 }
	 }
}
