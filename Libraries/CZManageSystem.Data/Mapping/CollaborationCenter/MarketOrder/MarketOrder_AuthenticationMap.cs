using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-鉴权方式维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_AuthenticationMap : EntityTypeConfiguration<MarketOrder_Authentication>
	{
		public MarketOrder_AuthenticationMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 鉴权方式,不可重复
		/// <summary>
			  this.Property(t => t.Authentication)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Authentication"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 鉴权方式,不可重复
			this.Property(t => t.Authentication).HasColumnName("Authentication"); 
			// 序号,可以重复
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
