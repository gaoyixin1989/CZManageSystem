using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-配送时限维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_AreaMap : EntityTypeConfiguration<MarketOrder_Area>
	{
		public MarketOrder_AreaMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 地区编号
		/// <summary>
			  this.Property(t => t.DpCode)

			.HasMaxLength(640);
		/// <summary>
		/// 邮政所属区域
		/// <summary>
			  this.Property(t => t.DpName)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Area"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 地区编号
			this.Property(t => t.DpCode).HasColumnName("DpCode"); 
			// 邮政所属区域
			this.Property(t => t.DpName).HasColumnName("DpName"); 
			// 配送时限（小时）
			this.Property(t => t.LimitTime).HasColumnName("LimitTime"); 
			// 序号
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
