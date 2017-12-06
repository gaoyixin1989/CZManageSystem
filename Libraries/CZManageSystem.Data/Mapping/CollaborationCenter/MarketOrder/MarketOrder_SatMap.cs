using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-�����ά��
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
		/// �����
		/// <summary>
			  this.Property(t => t.Sat)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Sat"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �����
			this.Property(t => t.Sat).HasColumnName("Sat"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
