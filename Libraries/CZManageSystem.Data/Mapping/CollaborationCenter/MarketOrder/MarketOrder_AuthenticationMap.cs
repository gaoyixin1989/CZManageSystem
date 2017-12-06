using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-��Ȩ��ʽά��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_AuthenticationMap : EntityTypeConfiguration<MarketOrder_Authentication>
	{
		public MarketOrder_AuthenticationMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��Ȩ��ʽ,�����ظ�
		/// <summary>
			  this.Property(t => t.Authentication)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Authentication"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��Ȩ��ʽ,�����ظ�
			this.Property(t => t.Authentication).HasColumnName("Authentication"); 
			// ���,�����ظ�
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
