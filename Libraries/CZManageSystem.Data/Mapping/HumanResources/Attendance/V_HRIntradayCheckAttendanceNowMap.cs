using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
    public class V_HRIntradayCheckAttendanceNowMap : EntityTypeConfiguration<V_HRIntradayCheckAttendanceNow>
    {
        public V_HRIntradayCheckAttendanceNowMap()
        {
            // Primary Key
            // Table & Column Mappings
            this.HasKey(t => t.AttendanceId);
            this.ToTable("V_HRIntradayCheckAttendanceNow");
            this.Property(t => t.AttendanceId).HasColumnName("AttendanceId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AtDate).HasColumnName("AtDate");
            this.Property(t => t.DoTime).HasColumnName("DoTime");
            this.Property(t => t.OffDate).HasColumnName("OffDate");
            this.Property(t => t.OffTime).HasColumnName("OffTime");
            this.Property(t => t.Minute).HasColumnName("Minute");
            this.Property(t => t.DoReallyTime).HasColumnName("DoReallyTime");
            this.Property(t => t.OffReallyTime).HasColumnName("OffReallyTime");
            this.Property(t => t.DoFlag).HasColumnName("DoFlag");
            this.Property(t => t.RotateDaysOffFlag).HasColumnName("RotateDaysOffFlag");
        }
    }
}
