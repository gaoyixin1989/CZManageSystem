using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentSplitAccountMap : EntityTypeConfiguration<PaymentSplitAccount>
	{
		public PaymentSplitAccountMap()
		{
			// Primary Key
			 this.HasKey(t => t.AccountID);
		/// <summary>
		/// �����տ����ʻ�
		/// <summary>
			  this.Property(t => t.Account)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentSplitAccount"); 
			// �ʻ�ID
			this.Property(t => t.AccountID).HasColumnName("AccountID"); 
			// �����տ����ʻ�
			this.Property(t => t.Account).HasColumnName("Account"); 
		 }
	 }
}
