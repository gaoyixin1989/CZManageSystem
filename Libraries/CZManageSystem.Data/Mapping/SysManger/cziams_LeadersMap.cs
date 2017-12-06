using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class Cziams_LeadersMap : EntityTypeConfiguration<Cziams_Leaders>
    {
        public Cziams_LeadersMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.EmployeeID)
                .HasMaxLength(32);

            this.Property(t => t.DepartmentID)
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("cziams_Leaders");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
            this.Property(t => t.DepartmentID).HasColumnName("DepartmentID");
        }
    }
}
