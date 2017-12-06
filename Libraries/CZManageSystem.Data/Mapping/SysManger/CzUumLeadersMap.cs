using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class CzUumLeadersMap : EntityTypeConfiguration<CzUumLeaders>
	{
		public CzUumLeadersMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.DepartmentID)

			.HasMaxLength(50);
			  this.Property(t => t.EmployeeID)

			.HasMaxLength(50);
			  this.Property(t => t.UserName)

			.HasMaxLength(50);
			  this.Property(t => t.EmployeeClass)

			.HasMaxLength(10);
			  this.Property(t => t.JobType)

			.HasMaxLength(6);
			  this.Property(t => t.UserType)

			.HasMaxLength(20);
			  this.Property(t => t.CMCCAccount)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("cz_uum_Leaders"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.DepartmentID).HasColumnName("DepartmentID"); 
			this.Property(t => t.EmployeeID).HasColumnName("EmployeeID"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.EmployeeClass).HasColumnName("EmployeeClass"); 
			this.Property(t => t.JobType).HasColumnName("JobType"); 
			this.Property(t => t.UserType).HasColumnName("UserType"); 
			this.Property(t => t.CMCCAccount).HasColumnName("CMCCAccount"); 
		 }
	 }
}
