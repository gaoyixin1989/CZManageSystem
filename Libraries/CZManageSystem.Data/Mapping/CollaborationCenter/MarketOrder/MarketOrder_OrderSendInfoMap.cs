using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-���ͽ����Ϣ
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderSendInfoMap : EntityTypeConfiguration<MarketOrder_OrderSendInfo>
	{
		public MarketOrder_OrderSendInfoMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ���ͽ��
		/// <summary>
			  this.Property(t => t.IsSuccess)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.SendInfo)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderSendInfo"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���뵥ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ������ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// ����ʱ��
			this.Property(t => t.Time).HasColumnName("Time"); 
			// ���ͽ��
			this.Property(t => t.IsSuccess).HasColumnName("IsSuccess"); 
			// ��ע
			this.Property(t => t.SendInfo).HasColumnName("SendInfo"); 
		 }
	 }
}
