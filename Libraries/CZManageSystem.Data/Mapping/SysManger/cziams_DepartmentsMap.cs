using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class Cziams_DepartmentsMap : EntityTypeConfiguration<Cziams_Departments>
    {
        public Cziams_DepartmentsMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.DepartmentID)
                .HasMaxLength(32);

            this.Property(t => t.ParentDepartmentID)
                .HasMaxLength(32);

            this.Property(t => t.DepartmentSign)
                .HasMaxLength(255);

            this.Property(t => t.DepartmentName)
                .HasMaxLength(255);

            this.Property(t => t.DepartmentFullName)
                .HasMaxLength(255);

            this.Property(t => t.DepartmentDescription)
                .HasMaxLength(1024);

            this.Property(t => t.DepartmentEx1)
                .HasMaxLength(255);

            this.Property(t => t.DepartmentEx2)
                .HasMaxLength(255);

            this.Property(t => t.DepartmentEx3)
                .HasMaxLength(255);

            this.Property(t => t.DepartmentEx4)
                .HasMaxLength(255);

            this.Property(t => t.DepartmentEx5)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("cziams_Departments");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.DepartmentID).HasColumnName("DepartmentID");
            this.Property(t => t.ParentDepartmentID).HasColumnName("ParentDepartmentID");
            this.Property(t => t.DepartmentSign).HasColumnName("DepartmentSign");
            this.Property(t => t.DepartmentName).HasColumnName("DepartmentName");
            this.Property(t => t.DepartmentFullName).HasColumnName("DepartmentFullName");
            this.Property(t => t.DepartmentDescription).HasColumnName("DepartmentDescription");
            this.Property(t => t.DepartmentLevel).HasColumnName("DepartmentLevel");
            this.Property(t => t.DepartmentEx1).HasColumnName("DepartmentEx1");
            this.Property(t => t.DepartmentEx2).HasColumnName("DepartmentEx2");
            this.Property(t => t.DepartmentEx3).HasColumnName("DepartmentEx3");
            this.Property(t => t.DepartmentEx4).HasColumnName("DepartmentEx4");
            this.Property(t => t.DepartmentEx5).HasColumnName("DepartmentEx5");
        }
    }
}
