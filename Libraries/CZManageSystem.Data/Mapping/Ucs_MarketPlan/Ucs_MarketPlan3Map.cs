using CZManageSystem.Data.Domain.MarketPlan;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Ucs_MarketPlan3Map : EntityTypeConfiguration<Ucs_MarketPlan3>
	{
		public Ucs_MarketPlan3Map()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
			  this.Property(t => t.Coding)

			.HasMaxLength(64);
			  this.Property(t => t.Name)

			.HasMaxLength(200);
			  this.Property(t => t.Channel)

			.HasMaxLength(200);
			  this.Property(t => t.Orders)

			.HasMaxLength(64);
			  this.Property(t => t.RegPort)

			.HasMaxLength(64);
			  this.Property(t => t.DetialInfo)

			.HasMaxLength(500);
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
			  this.Property(t => t.PlanType)

			.HasMaxLength(100);
			  this.Property(t => t.TargetUsers)

			.HasMaxLength(100);
			  this.Property(t => t.PaysRlues)

			.HasMaxLength(200);
			  this.Property(t => t.Templet1)

			.HasMaxLength(200);
			  this.Property(t => t.Templet2)

			.HasMaxLength(200);
			  this.Property(t => t.Templet3)

			.HasMaxLength(200);
			  this.Property(t => t.Templet4)

			.HasMaxLength(200);
			  this.Property(t => t.Tel)

			.HasMaxLength(20);
			  this.Property(t => t.IsMarketing)

			.HasMaxLength(10);
			// Table & Column Mappings
 			 this.ToTable("Ucs_MarketPlan3"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			this.Property(t => t.Coding).HasColumnName("Coding"); 
			this.Property(t => t.Name).HasColumnName("Name"); 
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			this.Property(t => t.Channel).HasColumnName("Channel"); 
			this.Property(t => t.Orders).HasColumnName("Orders"); 
			this.Property(t => t.RegPort).HasColumnName("RegPort"); 
			this.Property(t => t.DetialInfo).HasColumnName("DetialInfo"); 
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			this.Property(t => t.PlanType).HasColumnName("PlanType"); 
			this.Property(t => t.TargetUsers).HasColumnName("TargetUsers"); 
			this.Property(t => t.PaysRlues).HasColumnName("PaysRlues"); 
			this.Property(t => t.Templet1).HasColumnName("Templet1"); 
			this.Property(t => t.Templet2).HasColumnName("Templet2"); 
			this.Property(t => t.Templet3).HasColumnName("Templet3"); 
			this.Property(t => t.Templet4).HasColumnName("Templet4"); 
			this.Property(t => t.Tel).HasColumnName("Tel"); 
			this.Property(t => t.IsMarketing).HasColumnName("IsMarketing"); 
			this.Property(t => t.status).HasColumnName("status"); 
		 }
	 }
}
