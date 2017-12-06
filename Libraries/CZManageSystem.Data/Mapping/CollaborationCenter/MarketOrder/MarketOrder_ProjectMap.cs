using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-项目编号维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_ProjectMap : EntityTypeConfiguration<MarketOrder_Project>
	{
		public MarketOrder_ProjectMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目编号，不可重复
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Project"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 项目编号，不可重复
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 序号，可重复
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
