using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-开户结果信息
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
		/// 是否开户成功
		/// <summary>
			  this.Property(t => t.IsSuccess)

			.HasMaxLength(50);
		/// <summary>
		/// 是否回访
		/// <summary>
			  this.Property(t => t.IsVisit)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderBossDeal"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 申请单ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 操作人ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// 操作时间
			this.Property(t => t.Time).HasColumnName("Time"); 
			// 是否开户成功
			this.Property(t => t.IsSuccess).HasColumnName("IsSuccess"); 
			// 是否回访
			this.Property(t => t.IsVisit).HasColumnName("IsVisit"); 
		 }
	 }
}
