using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-���͹��̷���
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderSendCourseMap : EntityTypeConfiguration<MarketOrder_OrderSendCourse>
	{
		public MarketOrder_OrderSendCourseMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ���뵥ID
		/// <summary>
			  this.Property(t => t.ApplyID)

			.HasMaxLength(50);
		/// <summary>
		/// ������ID
		/// <summary>
			  this.Property(t => t.UserID)

			.HasMaxLength(50);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.SendCourse)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderSendCourse"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���뵥ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ������ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// ����ʱ��
			this.Property(t => t.Time).HasColumnName("Time"); 
			// ����ʱ��
			this.Property(t => t.SendTime).HasColumnName("SendTime"); 
			// ��������
			this.Property(t => t.SendCourse).HasColumnName("SendCourse"); 
		 }
	 }
}
