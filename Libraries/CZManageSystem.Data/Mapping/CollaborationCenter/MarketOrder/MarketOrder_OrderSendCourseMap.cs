using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-配送过程反馈
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
		/// 申请单ID
		/// <summary>
			  this.Property(t => t.ApplyID)

			.HasMaxLength(50);
		/// <summary>
		/// 操作人ID
		/// <summary>
			  this.Property(t => t.UserID)

			.HasMaxLength(50);
		/// <summary>
		/// 反馈内容
		/// <summary>
			  this.Property(t => t.SendCourse)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderSendCourse"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 申请单ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 操作人ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// 操作时间
			this.Property(t => t.Time).HasColumnName("Time"); 
			// 反馈时间
			this.Property(t => t.SendTime).HasColumnName("SendTime"); 
			// 反馈内容
			this.Property(t => t.SendCourse).HasColumnName("SendCourse"); 
		 }
	 }
}
