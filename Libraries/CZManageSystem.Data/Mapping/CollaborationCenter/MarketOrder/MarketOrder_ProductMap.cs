using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-��Ʒά��
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
		/// ���
		/// <summary>
			  this.Property(t => t.ProductID)

			.HasMaxLength(50);
		/// <summary>
		/// ��Ʒ����
		/// <summary>
			  this.Property(t => t.ProductName)

			.HasMaxLength(50);
		/// <summary>
		/// ˵��
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Product"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���
			this.Property(t => t.ProductID).HasColumnName("ProductID"); 
			// ��Ʒ����
			this.Property(t => t.ProductName).HasColumnName("ProductName"); 
			// ��Ʒ����
			this.Property(t => t.ProductTypeID).HasColumnName("ProductTypeID"); 
			// ˵��
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
