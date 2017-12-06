using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class UsersMap : EntityTypeConfiguration<Users>
    {
        public UsersMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Email)
                .HasMaxLength(512);

            this.Property(t => t.Mobile)
                .HasMaxLength(32);

            this.Property(t => t.Tel)
                .HasMaxLength(32);

            this.Property(t => t.EmployeeId)
                .HasMaxLength(50);

            this.Property(t => t.RealName)
                .HasMaxLength(50);

            this.Property(t => t.DpId)
                .HasMaxLength(255);

            this.Property(t => t.Ext_Str1)
                .HasMaxLength(256);

            this.Property(t => t.Ext_Str2)
                .HasMaxLength(256);

            this.Property(t => t.Ext_Str3)
                .HasMaxLength(256);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("bw_Users");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Tel).HasColumnName("Tel");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.Ext_Int).HasColumnName("Ext_Int");
            this.Property(t => t.Ext_Decimal).HasColumnName("Ext_Decimal");
            this.Property(t => t.Ext_Str1).HasColumnName("Ext_Str1");
            this.Property(t => t.Ext_Str2).HasColumnName("Ext_Str2");
            this.Property(t => t.Ext_Str3).HasColumnName("Ext_Str3");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.JoinTime).HasColumnName("JoinTime");
            this.Property(t => t.UserType).HasColumnName("UserType");
            this.Property(t => t.CheckIP).HasColumnName("CheckIP");

            this.HasOptional(t => t.Dept)
                .WithMany()
                .HasForeignKey(d => d.DpId);
        }
    }
}
