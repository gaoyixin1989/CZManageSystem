using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
    public class V_HROtherListMap : EntityTypeConfiguration<V_HROtherList>
    {
        public V_HROtherListMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);
            this.Property(t => t.RealName)

          .HasMaxLength(50);
            this.Property(t => t.EmployeeId)

          .HasMaxLength(50);
            this.Property(t => t.DpId)

          .HasMaxLength(255);
            this.Property(t => t.DpName)

          .HasMaxLength(50);
            this.Property(t => t.DpFullName)

          .HasMaxLength(256);
            // Table & Column Mappings
            this.ToTable("V_HROtherList");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.DpFullName).HasColumnName("DpFullName");
            this.Property(t => t.AtDate).HasColumnName("AtDate");
        }
    }
}
