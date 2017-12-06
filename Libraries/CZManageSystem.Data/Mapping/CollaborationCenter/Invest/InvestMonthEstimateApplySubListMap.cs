using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class InvestMonthEstimateApplySubListMap : EntityTypeConfiguration<InvestMonthEstimateApplySubList>
	{
		public InvestMonthEstimateApplySubListMap()
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
			  this.Property(t => t.Stop)

			.HasMaxLength(10);
			  this.Property(t => t.Recover)

			.HasMaxLength(10);
			  this.Property(t => t.IsUpdate)

			.HasMaxLength(50);
			  this.Property(t => t.IsUpdate2)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestMonthEstimateApplySubList"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
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
			this.Property(t => t.Stop).HasColumnName("Stop"); 
			this.Property(t => t.Recover).HasColumnName("Recover"); 
			this.Property(t => t.StopTime).HasColumnName("StopTime"); 
			this.Property(t => t.RecoverTime).HasColumnName("RecoverTime"); 
			this.Property(t => t.IsUpdate).HasColumnName("IsUpdate"); 
			this.Property(t => t.IsUpdate2).HasColumnName("IsUpdate2"); 
		 }
	 }
}
