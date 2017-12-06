using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysUserMap : EntityTypeConfiguration<SysUser>
    {
        public SysUserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id );

            // Properties
            this.Property(t => t.UserName)
                .HasMaxLength(50);

            this.Property(t => t.LoginName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LoginPWD)
                .HasMaxLength(50);

            this.Property(t => t.Mail)
                .HasMaxLength(50);

            this.Property(t => t.Mobile)
                .HasMaxLength(50);

            this.Property(t => t.Tel)
                .HasMaxLength(50);

            this.Property(t => t.EmployeeId)
                .HasMaxLength(50);

            this.Property(t => t.Position)
                .HasMaxLength(50);

            this.Property(t => t.Sex)
                .HasMaxLength(10);

            this.Property(t => t.Address)
                .HasMaxLength(100);

            this.Property(t => t.Remark)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("SysUser");
            this.Property(t => t.Id ).HasColumnName("UserID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.LoginPWD).HasColumnName("LoginPWD");
            this.Property(t => t.Mail).HasColumnName("Mail");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Tel).HasColumnName("Tel");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.JoinDate).HasColumnName("JoinDate");
            this.Property(t => t.IsOuter).HasColumnName("IsOuter");
            this.Property(t => t.DelFlag).HasColumnName("DelFlag");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
