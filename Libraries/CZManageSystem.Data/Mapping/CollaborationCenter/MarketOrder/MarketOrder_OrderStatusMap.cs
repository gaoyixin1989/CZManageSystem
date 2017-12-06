using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-����״̬ά��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderStatusMap : EntityTypeConfiguration<MarketOrder_OrderStatus>
	{
		public MarketOrder_OrderStatusMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ����״̬
		/// <summary>
			  this.Property(t => t.OrderStatus)

			.HasMaxLength(50);
		/// <summary>
		/// ˵��
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderStatus"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
			// ����״̬
			this.Property(t => t.OrderStatus).HasColumnName("OrderStatus"); 
			// ˵��
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
