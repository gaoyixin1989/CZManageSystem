using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
	public class V_HRHaveHolidaysByTurnsListMap : EntityTypeConfiguration<V_HRHaveHolidaysByTurnsList>
	{
		public V_HRHaveHolidaysByTurnsListMap()
		{
            // Primary Key
            this.HasKey(t=>t.AttendanceId );
			  this.Property(t => t.IpOn)

			.HasMaxLength(50);
			  this.Property(t => t.IpOff)

			.HasMaxLength(50);
			  this.Property(t => t.RealName)

			.HasMaxLength(50);
			  this.Property(t => t.EmployeeId)

			.HasMaxLength(50);
			  this.Property(t => t.DpId)
			 .IsRequired()
			.HasMaxLength(255);
			  this.Property(t => t.DpName)
			 .IsRequired()
			.HasMaxLength(50);
			  this.Property(t => t.DpFullName)
			 .IsRequired()
			.HasMaxLength(256);
			// Table & Column Mappings
 			 this.ToTable("V_HRHaveHolidaysByTurnsList"); 
			this.Property(t => t.AttendanceId).HasColumnName("AttendanceId"); 
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			this.Property(t => t.AtDate).HasColumnName("AtDate"); 
			this.Property(t => t.OffDate).HasColumnName("OffDate"); 
			this.Property(t => t.DoTime).HasColumnName("DoTime"); 
			this.Property(t => t.OffTime).HasColumnName("OffTime"); 
			this.Property(t => t.DoReallyTime).HasColumnName("DoReallyTime"); 
			this.Property(t => t.OffReallyTime).HasColumnName("OffReallyTime"); 
			this.Property(t => t.IpOn).HasColumnName("IpOn"); 
			this.Property(t => t.IpOff).HasColumnName("IpOff"); 
			this.Property(t => t.Minute).HasColumnName("Minute"); 
			this.Property(t => t.DoFlag).HasColumnName("DoFlag"); 
			this.Property(t => t.RotateDaysOffFlag).HasColumnName("RotateDaysOffFlag"); 
			this.Property(t => t.FlagOn).HasColumnName("FlagOn"); 
			this.Property(t => t.FlagOff).HasColumnName("FlagOff"); 
			this.Property(t => t.RealName).HasColumnName("RealName"); 
			this.Property(t => t.EmployeeId).HasColumnName("EmployeeId"); 
			this.Property(t => t.DpId).HasColumnName("DpId"); 
			this.Property(t => t.DpName).HasColumnName("DpName"); 
			this.Property(t => t.DpFullName).HasColumnName("DpFullName"); 
		 }
	 }
}
