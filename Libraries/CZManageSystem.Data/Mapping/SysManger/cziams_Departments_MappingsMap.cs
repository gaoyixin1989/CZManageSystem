using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class Cziams_Departments_MappingsMap : EntityTypeConfiguration<Cziams_Departments_Mappings>
    {
        public Cziams_Departments_MappingsMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.DepartmentID)
                .HasMaxLength(50);

            this.Property(t => t.MappingDepartmentID)
                .HasMaxLength(255);

            this.Property(t => t.OUOrder)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("cziams_Departments_Mappings");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DepartmentID).HasColumnName("DepartmentID");
            this.Property(t => t.MappingDepartmentID).HasColumnName("MappingDepartmentID");
            this.Property(t => t.OUOrder).HasColumnName("OUOrder");
        }
    }
}
