using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class HRYKTDataMap : EntityTypeConfiguration<HRYKTData>
	{
		public HRYKTDataMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.loginId)

			.HasMaxLength(64);
			  this.Property(t => t.Employee)

			.HasMaxLength(128);
			  this.Property(t => t.Name)

			.HasMaxLength(50);
			  this.Property(t => t.DepartmentName)

			.HasMaxLength(128);
			  this.Property(t => t.DoorName)

			.HasMaxLength(64);
			  this.Property(t => t.Path)
;
			// Table & Column Mappings
 			 this.ToTable("HRYKTData"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.Tid).HasColumnName("Tid"); 
			this.Property(t => t.CardID).HasColumnName("CardID"); 
			this.Property(t => t.BusinessCardID).HasColumnName("BusinessCardID"); 
			this.Property(t => t.loginId).HasColumnName("loginId"); 
			this.Property(t => t.Employee).HasColumnName("Employee"); 
			this.Property(t => t.Name).HasColumnName("Name"); 
			this.Property(t => t.DepartmentName).HasColumnName("DepartmentName"); 
			this.Property(t => t.ReaderTime).HasColumnName("ReaderTime"); 
			this.Property(t => t.DoorName).HasColumnName("DoorName"); 
			this.Property(t => t.Path).HasColumnName("Path"); 
			this.Property(t => t.ActionStatus).HasColumnName("ActionStatus"); 
		 }
	 }
}
