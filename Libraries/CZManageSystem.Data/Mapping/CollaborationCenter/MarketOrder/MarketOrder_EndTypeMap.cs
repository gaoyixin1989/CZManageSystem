using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-终端机型维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_EndTypeMap : EntityTypeConfiguration<MarketOrder_EndType>
	{
		public MarketOrder_EndTypeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 机型名称
		/// <summary>
			  this.Property(t => t.EndType)

			.HasMaxLength(1000);
		/// <summary>
		/// 说明
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_EndType"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 机型名称
			this.Property(t => t.EndType).HasColumnName("EndType"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
			// 所属方案
			this.Property(t => t.MarketID).HasColumnName("MarketID"); 
			// 说明
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
