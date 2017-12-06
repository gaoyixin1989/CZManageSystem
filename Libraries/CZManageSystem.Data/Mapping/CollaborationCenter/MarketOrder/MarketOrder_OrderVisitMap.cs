using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-�ط����
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderVisitMap : EntityTypeConfiguration<MarketOrder_OrderVisit>
	{
		public MarketOrder_OrderVisitMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �ɹ���ע
		/// <summary>
			  this.Property(t => t.SuccessRemark)

			.HasMaxLength(2147483647);
		/// <summary>
		/// ʧ�ܱ�ע
		/// <summary>
			  this.Property(t => t.FailRemark)

			.HasMaxLength(2147483647);
			  this.Property(t => t.IsAgain)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderVisit"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���뵥ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// �û�ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			this.Property(t => t.Time).HasColumnName("Time"); 
			// �����ID
			this.Property(t => t.SatID).HasColumnName("SatID"); 
			// �ɹ���ע
			this.Property(t => t.SuccessRemark).HasColumnName("SuccessRemark"); 
			// ʧ�ܻط�ID
			this.Property(t => t.VisitID).HasColumnName("VisitID"); 
			// ʧ�ܱ�ע
			this.Property(t => t.FailRemark).HasColumnName("FailRemark"); 
			this.Property(t => t.IsAgain).HasColumnName("IsAgain"); 
		 }
	 }
}
