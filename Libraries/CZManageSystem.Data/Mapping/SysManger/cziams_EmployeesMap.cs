using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class Cziams_EmployeesMap : EntityTypeConfiguration<Cziams_Employees>
    {
        public Cziams_EmployeesMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.EmployeeID)
                .HasMaxLength(32);

            this.Property(t => t.DepartmentID)
                .HasMaxLength(32);

            this.Property(t => t.EmployeeSign)
                .HasMaxLength(64);

            this.Property(t => t.EmployeeName)
                .HasMaxLength(64);

            this.Property(t => t.CardID)
                .HasMaxLength(32);

            this.Property(t => t.Email)
                .HasMaxLength(512);

            this.Property(t => t.MobileNo)
                .HasMaxLength(32);

            this.Property(t => t.WorkTelNo)
                .HasMaxLength(32);

            this.Property(t => t.Address)
                .HasMaxLength(512);

            this.Property(t => t.OrderNo)
                .HasMaxLength(32);

            this.Property(t => t.EmployeeEx1)
                .HasMaxLength(255);

            this.Property(t => t.EmployeeEx2)
                .HasMaxLength(255);

            this.Property(t => t.EmployeeEx3)
                .HasMaxLength(255);

            this.Property(t => t.EmployeeEx4)
                .HasMaxLength(255);

            this.Property(t => t.EmployeeEx5)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("cziams_Employees");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
            this.Property(t => t.DepartmentID).HasColumnName("DepartmentID");
            this.Property(t => t.EmployeeSign).HasColumnName("EmployeeSign");
            this.Property(t => t.EmployeeName).HasColumnName("EmployeeName");
            this.Property(t => t.CardID).HasColumnName("CardID");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.MobileNo).HasColumnName("MobileNo");
            this.Property(t => t.WorkTelNo).HasColumnName("WorkTelNo");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Gender).HasColumnName("Gender");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.EmployeeEx1).HasColumnName("EmployeeEx1");
            this.Property(t => t.EmployeeEx2).HasColumnName("EmployeeEx2");
            this.Property(t => t.EmployeeEx3).HasColumnName("EmployeeEx3");
            this.Property(t => t.EmployeeEx4).HasColumnName("EmployeeEx4");
            this.Property(t => t.EmployeeEx5).HasColumnName("EmployeeEx5");
        }
    }
}
