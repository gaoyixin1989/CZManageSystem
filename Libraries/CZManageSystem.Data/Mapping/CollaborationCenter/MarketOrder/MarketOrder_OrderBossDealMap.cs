using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-���������Ϣ
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderBossDealMap : EntityTypeConfiguration<MarketOrder_OrderBossDeal>
	{
		public MarketOrder_OrderBossDealMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �Ƿ񿪻��ɹ�
		/// <summary>
			  this.Property(t => t.IsSuccess)

			.HasMaxLength(50);
		/// <summary>
		/// �Ƿ�ط�
		/// <summary>
			  this.Property(t => t.IsVisit)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderBossDeal"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���뵥ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ������ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// ����ʱ��
			this.Property(t => t.Time).HasColumnName("Time"); 
			// �Ƿ񿪻��ɹ�
			this.Property(t => t.IsSuccess).HasColumnName("IsSuccess"); 
			// �Ƿ�ط�
			this.Property(t => t.IsVisit).HasColumnName("IsVisit"); 
		 }
	 }
}
