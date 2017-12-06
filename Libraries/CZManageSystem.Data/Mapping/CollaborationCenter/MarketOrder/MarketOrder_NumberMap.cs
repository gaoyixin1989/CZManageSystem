using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-�����ά��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_NumberMap : EntityTypeConfiguration<MarketOrder_Number>
	{
		public MarketOrder_NumberMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �����
		/// <summary>
			  this.Property(t => t.Number)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Number"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
			// �����
			this.Property(t => t.Number).HasColumnName("Number"); 
		 }
	 }
}
