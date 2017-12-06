using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-商品维护
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_ProductMap : EntityTypeConfiguration<MarketOrder_Product>
	{
		public MarketOrder_ProductMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 序号
		/// <summary>
			  this.Property(t => t.ProductID)

			.HasMaxLength(50);
		/// <summary>
		/// 商品名称
		/// <summary>
			  this.Property(t => t.ProductName)

			.HasMaxLength(50);
		/// <summary>
		/// 说明
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Product"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 序号
			this.Property(t => t.ProductID).HasColumnName("ProductID"); 
			// 商品名称
			this.Property(t => t.ProductName).HasColumnName("ProductName"); 
			// 商品机型
			this.Property(t => t.ProductTypeID).HasColumnName("ProductTypeID"); 
			// 说明
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
