using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-�ն˻���ά��
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
		/// ��������
		/// <summary>
			  this.Property(t => t.EndType)

			.HasMaxLength(1000);
		/// <summary>
		/// ˵��
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_EndType"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��������
			this.Property(t => t.EndType).HasColumnName("EndType"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
			// ��������
			this.Property(t => t.MarketID).HasColumnName("MarketID"); 
			// ˵��
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
