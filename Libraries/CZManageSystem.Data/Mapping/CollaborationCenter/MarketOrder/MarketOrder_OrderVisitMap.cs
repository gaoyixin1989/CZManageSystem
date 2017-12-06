using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-回访情况
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
		/// 成功备注
		/// <summary>
			  this.Property(t => t.SuccessRemark)

			.HasMaxLength(2147483647);
		/// <summary>
		/// 失败备注
		/// <summary>
			  this.Property(t => t.FailRemark)

			.HasMaxLength(2147483647);
			  this.Property(t => t.IsAgain)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("MarketOrder_OrderVisit"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 申请单ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 用户ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			this.Property(t => t.Time).HasColumnName("Time"); 
			// 满意度ID
			this.Property(t => t.SatID).HasColumnName("SatID"); 
			// 成功备注
			this.Property(t => t.SuccessRemark).HasColumnName("SuccessRemark"); 
			// 失败回访ID
			this.Property(t => t.VisitID).HasColumnName("VisitID"); 
			// 失败备注
			this.Property(t => t.FailRemark).HasColumnName("FailRemark"); 
			this.Property(t => t.IsAgain).HasColumnName("IsAgain"); 
		 }
	 }
}
