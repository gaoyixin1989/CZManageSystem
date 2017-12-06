using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-捆绑业务维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_BusinessMap : EntityTypeConfiguration<MarketOrder_Business>
	{
		public MarketOrder_BusinessMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 捆绑业务
		/// <summary>
			  this.Property(t => t.Business)

			.HasMaxLength(1000);
		/// <summary>
		/// 说明
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Business"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 捆绑业务
			this.Property(t => t.Business).HasColumnName("Business"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
			// 说明
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
