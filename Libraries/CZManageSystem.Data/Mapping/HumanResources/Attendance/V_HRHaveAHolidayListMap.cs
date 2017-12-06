using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
    public class V_HRHaveAHolidayListMap : EntityTypeConfiguration<V_HRHaveAHolidayList>
    {
        public V_HRHaveAHolidayListMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);
            this.Property(t => t.EmployeeId)

          .HasMaxLength(50);
            this.Property(t => t.RealName)

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
            this.ToTable("V_HRHaveAHolidayList");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.DpFullName).HasColumnName("DpFullName");
        }
    }
}
