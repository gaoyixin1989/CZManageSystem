using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-����ʱ��ά��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_AreaMap : EntityTypeConfiguration<MarketOrder_Area>
	{
		public MarketOrder_AreaMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �������
		/// <summary>
			  this.Property(t => t.DpCode)

			.HasMaxLength(640);
		/// <summary>
		/// ������������
		/// <summary>
			  this.Property(t => t.DpName)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Area"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �������
			this.Property(t => t.DpCode).HasColumnName("DpCode"); 
			// ������������
			this.Property(t => t.DpName).HasColumnName("DpName"); 
			// ����ʱ�ޣ�Сʱ��
			this.Property(t => t.LimitTime).HasColumnName("LimitTime"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
