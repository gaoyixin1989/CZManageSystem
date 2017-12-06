using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-配送结果信息
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
		/// 配送结果
		/// <summary>
			  this.Property(t => t.IsSuccess)

			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.SendInfo)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderSendInfo"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 申请单ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 操作人ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// 操作时间
			this.Property(t => t.Time).HasColumnName("Time"); 
			// 配送结果
			this.Property(t => t.IsSuccess).HasColumnName("IsSuccess"); 
			// 备注
			this.Property(t => t.SendInfo).HasColumnName("SendInfo"); 
		 }
	 }
}
