using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-�����ײ�ά��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_SetmealMap : EntityTypeConfiguration<MarketOrder_Setmeal>
	{
		public MarketOrder_SetmealMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �ײ�����
		/// <summary>
			  this.Property(t => t.Setmeal)

			.HasMaxLength(50);
		/// <summary>
		/// ˵����ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Setmeal"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �ײ�����
			this.Property(t => t.Setmeal).HasColumnName("Setmeal"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
			// ˵����ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
