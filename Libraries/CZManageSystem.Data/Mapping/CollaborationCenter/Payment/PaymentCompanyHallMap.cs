using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentCompanyHallMap : EntityTypeConfiguration<PaymentCompanyHall>
	{
		public PaymentCompanyHallMap()
		{
			// Primary Key
			 this.HasKey(t => t.DcId);
            this.Property(t => t.DpId).HasMaxLength(250);
			// Table & Column Mappings
 			 this.ToTable("PaymentCompanyHall"); 
			this.Property(t => t.DcId).HasColumnName("DcId"); 
			this.Property(t => t.DpId).HasColumnName("DpId"); 
			this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.HasOptional(t => t.Depts).WithMany().HasForeignKey(t => t.DpId );
        }
	 }
}
