using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentSplitMoneySettingMap : EntityTypeConfiguration<PaymentSplitMoneySetting>
	{
		public PaymentSplitMoneySettingMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			// Table & Column Mappings
 			 this.ToTable("PaymentSplitMoneySetting"); 
			// ID
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ²ðÕÊ½ð¶î
			this.Property(t => t.SplitMoney).HasColumnName("SplitMoney"); 
		 }
	 }
}
