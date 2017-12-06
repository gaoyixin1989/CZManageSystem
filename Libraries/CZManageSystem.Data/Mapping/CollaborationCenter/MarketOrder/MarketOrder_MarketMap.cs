using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-Ӫ������ά��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_MarketMap : EntityTypeConfiguration<MarketOrder_Market>
	{
		public MarketOrder_MarketMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// Ӫ����������
		/// <summary>
			  this.Property(t => t.Market)

			.HasMaxLength(1000);
		/// <summary>
		/// ���
		/// <summary>
			  this.Property(t => t.Order)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע˵��
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_Market"); 
			// ���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// Ӫ����������
			this.Property(t => t.Market).HasColumnName("Market"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
			// ��Чʱ��
			this.Property(t => t.AbleTime).HasColumnName("AbleTime"); 
			// ʧЧʱ��
			this.Property(t => t.DisableTime).HasColumnName("DisableTime"); 
			// ��ע˵��
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// �Ż�����
			this.Property(t => t.PlanPay).HasColumnName("PlanPay"); 
			// ʵ�շ���
			this.Property(t => t.MustPay).HasColumnName("MustPay"); 
			// �Ƿ�ҿ�ҵ��
			this.Property(t => t.isJK).HasColumnName("isJK"); 
		 }
	 }
}
