using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-���ս����Ϣ
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderReclaimMap : EntityTypeConfiguration<MarketOrder_OrderReclaim>
	{
		public MarketOrder_OrderReclaimMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ���ս��
		/// <summary>
			  this.Property(t => t.IsReclaim)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.ReclaimRemark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderReclaim"); 
			// ���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���뵥ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// �����û�ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// ����ʱ��
			this.Property(t => t.Time).HasColumnName("Time"); 
			// ���ս��
			this.Property(t => t.IsReclaim).HasColumnName("IsReclaim"); 
			// ��ע
			this.Property(t => t.ReclaimRemark).HasColumnName("ReclaimRemark"); 
		 }
	 }
}
