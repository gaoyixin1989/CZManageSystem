using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-失败回访维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_VisitMap : EntityTypeConfiguration<MarketOrder_Visit>
	{
		public MarketOrder_VisitMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 失败回访情况
		/// <summary>
			  this.Property(t => t.Visit)

			.HasMaxLength(1000);
		/// <summary>
		/// 说明
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Visit"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 失败回访情况
			this.Property(t => t.Visit).HasColumnName("Visit"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
			// 说明
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
