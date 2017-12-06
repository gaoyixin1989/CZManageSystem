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
		/// �տ��ʺ�
		/// <summary>
			  this.Property(t => t.PayeeAccount)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentReportReward"); 
			// �ճ��ID
			this.Property(t => t.RewardID).HasColumnName("RewardID"); 
			// �ϱ�ʱ��
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			// ��Ӫ��ID
			this.Property(t => t.HallID).HasColumnName("HallID"); 
			// �տ��ʺ�
			this.Property(t => t.PayeeAccount).HasColumnName("PayeeAccount"); 
			// ���
			this.Property(t => t.PayMoney).HasColumnName("PayMoney"); 
			// ���
			this.Property(t => t.Reward).HasColumnName("Reward"); 
			// ��˾ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
		 }
	 }
}
