using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-满意度维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_SatMap : EntityTypeConfiguration<MarketOrder_Sat>
	{
		public MarketOrder_SatMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 满意度
		/// <summary>
			  this.Property(t => t.Sat)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Sat"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 满意度
			this.Property(t => t.Sat).HasColumnName("Sat"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
