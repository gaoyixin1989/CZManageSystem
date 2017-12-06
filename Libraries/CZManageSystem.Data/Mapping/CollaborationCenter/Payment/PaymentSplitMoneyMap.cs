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
		/// 用途
		/// <summary>
			  this.Property(t => t.Purpose)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentSplitMoney"); 
			// 拆账ID
			this.Property(t => t.SplitID).HasColumnName("SplitID"); 
			// 总金额ID
			this.Property(t => t.PayMoneyID).HasColumnName("PayMoneyID"); 
			// 拆帐金额ID
			this.Property(t => t.SplitMoney).HasColumnName("SplitMoney"); 
			// 用途
			this.Property(t => t.Purpose).HasColumnName("Purpose"); 
		 }
	 }
}
