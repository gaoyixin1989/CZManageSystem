using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-��Ŀ���ά��
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
		/// ��Ŀ��ţ������ظ�
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Project"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��Ŀ��ţ������ظ�
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��ţ����ظ�
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
