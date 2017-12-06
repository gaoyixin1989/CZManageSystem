using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentRewardScaleMap : EntityTypeConfiguration<PaymentRewardScale>
	{
		public PaymentRewardScaleMap()
		{
			// Primary Key
			 this.HasKey(t => t.ScaleID);
			  this.Property(t => t.CompanyName)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentRewardScale"); 
			// ±ÈÀýID
			this.Property(t => t.ScaleID).HasColumnName("ScaleID"); 
			this.Property(t => t.Scale).HasColumnName("Scale"); 
			this.Property(t => t.BeginTime).HasColumnName("BeginTime"); 
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			this.Property(t => t.CompanyName).HasColumnName("CompanyName"); 
		 }
	 }
}
