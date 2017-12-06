using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentUserMap : EntityTypeConfiguration<PaymentUser>
	{
		public PaymentUserMap()
		{
			// Primary Key
			 this.HasKey(t => t.UserID);
			  this.Property(t => t.LoginID)

			.HasMaxLength(50);
			  this.Property(t => t.PassWord)

			.HasMaxLength(50);
			  this.Property(t => t.UserName)

			.HasMaxLength(50);
			  this.Property(t => t.Mobile)

			.HasMaxLength(50);
			  this.Property(t => t.Phone)

			.HasMaxLength(50);
			  this.Property(t => t.Status)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentUser"); 
			// ÓÃ»§ID
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			this.Property(t => t.LoginID).HasColumnName("LoginID"); 
			this.Property(t => t.PassWord).HasColumnName("PassWord"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			this.Property(t => t.Phone).HasColumnName("Phone"); 
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			this.Property(t => t.Status).HasColumnName("Status"); 
		 }
	 }
}
