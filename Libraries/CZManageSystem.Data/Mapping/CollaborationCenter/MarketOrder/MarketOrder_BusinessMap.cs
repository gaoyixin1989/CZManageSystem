using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-����ҵ��ά��
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
		/// ����ҵ��
		/// <summary>
			  this.Property(t => t.Business)

			.HasMaxLength(1000);
		/// <summary>
		/// ˵��
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Business"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ����ҵ��
			this.Property(t => t.Business).HasColumnName("Business"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
			// ˵��
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
