using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class view_XF_AmountLogMap : EntityTypeConfiguration<view_XF_AmountLog>
    {
        public view_XF_AmountLogMap()
        {
            // Primary Key
            this.HasKey(t => t.LogID);
            this.Property(t => t.DepartmentName)

          .HasMaxLength(128);
            this.Property(t => t.Name)

          .HasMaxLength(32);
            this.Property(t => t.JobNumber)

          .HasMaxLength(128);
            this.Property(t => t.TypeContent)
;
            this.Property(t => t.Memo)
;
            this.Property(t => t.Operator)
;
            this.Property(t => t.BankName)

          .HasMaxLength(32);
            // Table & Column Mappings
            this.ToTable("view_XF_AmountLog");
            this.Property(t => t.DepartmentName).HasColumnName("DepartmentName");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
            this.Property(t => t.JobNumber).HasColumnName("JobNumber");
            this.Property(t => t.LogID).HasColumnName("LogID");
            this.Property(t => t.TypeID).HasColumnName("TypeID");
            this.Property(t => t.AccountID).HasColumnName("AccountID");
            this.Property(t => t.AddAmount).HasColumnName("AddAmount");
            this.Property(t => t.BelAmount).HasColumnName("BelAmount");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.TypeContent).HasColumnName("TypeContent");
            this.Property(t => t.Memo).HasColumnName("Memo");
            this.Property(t => t.Operator).HasColumnName("Operator");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.BankName).HasColumnName("BankName");
            this.Property(t => t.Expr1).HasColumnName("Expr1");
        }

    }
}
