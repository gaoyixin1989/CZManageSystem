using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-ʧ�ܻط�ά��
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
		/// ʧ�ܻط����
		/// <summary>
			  this.Property(t => t.Visit)

			.HasMaxLength(1000);
		/// <summary>
		/// ˵��
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Visit"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ʧ�ܻط����
			this.Property(t => t.Visit).HasColumnName("Visit"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
			// ˵��
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
