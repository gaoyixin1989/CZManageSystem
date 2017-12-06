using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class InvestEstimateMap : EntityTypeConfiguration<InvestEstimate>
	{
		public InvestEstimateMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.ProjectName)

			.HasMaxLength(500);
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
			  this.Property(t => t.ContractName)

			.HasMaxLength(1000);
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
			  this.Property(t => t.Study)

			.HasMaxLength(500);


			  this.Property(t => t.Course)

			.HasMaxLength(500);

			// Table & Column Mappings
 			 this.ToTable("InvestEstimate"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.Year).HasColumnName("Year"); 
			this.Property(t => t.Month).HasColumnName("Month"); 
			this.Property(t => t.ProjectName).HasColumnName("ProjectName"); 
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			this.Property(t => t.SignTotal).HasColumnName("SignTotal"); 
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			this.Property(t => t.Study).HasColumnName("Study"); 
			this.Property(t => t.ManagerID).HasColumnName("ManagerID"); 
			this.Property(t => t.Course).HasColumnName("Course"); 
			this.Property(t => t.BackRate).HasColumnName("BackRate"); 
			this.Property(t => t.Rate).HasColumnName("Rate"); 
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			this.Property(t => t.NotPay).HasColumnName("NotPay"); 
			this.Property(t => t.EstimateUserID).HasColumnName("EstimateUserID");
            this.HasOptional(t => t.ManagerObj).WithMany().HasForeignKey(d => d.ManagerID);
            this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.EstimateUserID);
        }
	 }
}
