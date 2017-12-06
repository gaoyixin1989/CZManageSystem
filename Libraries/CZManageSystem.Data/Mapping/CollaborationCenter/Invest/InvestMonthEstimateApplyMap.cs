using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class InvestMonthEstimateApplyMap : EntityTypeConfiguration<InvestMonthEstimateApply>
	{
		public InvestMonthEstimateApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyID);
			  this.Property(t => t.Series)

			.HasMaxLength(50);
			  this.Property(t => t.ApplyDpCode)

			.HasMaxLength(640);
			  this.Property(t => t.Applicant)

			.HasMaxLength(50);
			  this.Property(t => t.Mobile)

			.HasMaxLength(50);
			  this.Property(t => t.Status)

			.HasMaxLength(50);
			  this.Property(t => t.Title)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("InvestMonthEstimateApply"); 
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			this.Property(t => t.Series).HasColumnName("Series"); 
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			this.Property(t => t.ApplyDpCode).HasColumnName("ApplyDpCode"); 
			this.Property(t => t.Applicant).HasColumnName("Applicant"); 
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			this.Property(t => t.Status).HasColumnName("Status"); 
			this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.HasOptional(t => t.TrackingWorkflow).WithMany().HasForeignKey(d => d.WorkflowInstanceId);
           // this.HasOptional(t => t.DeptInfo).WithMany().HasForeignKey(d => d.ApplyDpCode);
        }
	 }
}
