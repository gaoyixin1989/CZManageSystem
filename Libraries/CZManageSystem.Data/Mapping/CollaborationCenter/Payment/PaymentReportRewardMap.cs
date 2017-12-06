using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentReportRewardMap : EntityTypeConfiguration<PaymentReportReward>
	{
		public PaymentReportRewardMap()
		{
			// Primary Key
			 this.HasKey(t => t.RewardID);
		/// <summary>
		/// 收款帐号
		/// <summary>
			  this.Property(t => t.PayeeAccount)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentReportReward"); 
			// 日酬金ID
			this.Property(t => t.RewardID).HasColumnName("RewardID"); 
			// 上报时间
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			// 服营厅ID
			this.Property(t => t.HallID).HasColumnName("HallID"); 
			// 收款帐号
			this.Property(t => t.PayeeAccount).HasColumnName("PayeeAccount"); 
			// 金额
			this.Property(t => t.PayMoney).HasColumnName("PayMoney"); 
			// 酬金
			this.Property(t => t.Reward).HasColumnName("Reward"); 
			// 公司ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
		 }
	 }
}
