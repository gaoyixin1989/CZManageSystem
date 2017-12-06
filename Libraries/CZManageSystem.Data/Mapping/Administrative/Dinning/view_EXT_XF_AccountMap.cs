using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class view_EXT_XF_AccountMap : EntityTypeConfiguration<view_EXT_XF_Account>
    {
        public view_EXT_XF_AccountMap()
        {
            // Primary Key
            this.HasKey(t => t.JobNumber);
            this.Property(t => t.BankName)

          .HasMaxLength(32);
            this.Property(t => t.JobNumber)

          .HasMaxLength(128);
            this.Property(t => t.SystemNumber)

          .HasMaxLength(128);
            this.Property(t => t.Name)

          .HasMaxLength(32);
            this.Property(t => t.DepartmentName)

          .HasMaxLength(128);
            // Table & Column Mappings
            this.ToTable("view_EXT_XF_Account");
            this.Property(t => t.AccountID).HasColumnName("AccountID");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.BelAmount).HasColumnName("BelAmount");
            this.Property(t => t.JobNumber).HasColumnName("JobNumber");
            this.Property(t => t.SystemNumber).HasColumnName("SystemNumber");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.DepartmentName).HasColumnName("DepartmentName");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Updatetime).HasColumnName("Updatetime");
        }
    }
}
