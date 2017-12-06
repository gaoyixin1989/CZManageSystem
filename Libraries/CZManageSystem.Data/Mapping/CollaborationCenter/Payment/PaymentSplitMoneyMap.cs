using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentSplitMoneyMap : EntityTypeConfiguration<PaymentSplitMoney>
	{
		public PaymentSplitMoneyMap()
		{
			// Primary Key
			 this.HasKey(t => t.SplitID);
		/// <summary>
		/// ��;
		/// <summary>
			  this.Property(t => t.Purpose)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentSplitMoney"); 
			// ����ID
			this.Property(t => t.SplitID).HasColumnName("SplitID"); 
			// �ܽ��ID
			this.Property(t => t.PayMoneyID).HasColumnName("PayMoneyID"); 
			// ���ʽ��ID
			this.Property(t => t.SplitMoney).HasColumnName("SplitMoney"); 
			// ��;
			this.Property(t => t.Purpose).HasColumnName("Purpose"); 
		 }
	 }
}
