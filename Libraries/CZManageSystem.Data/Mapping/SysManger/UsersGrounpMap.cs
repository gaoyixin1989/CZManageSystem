using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class UsersGrounpMap : EntityTypeConfiguration<UsersGrounp>
    {
        public UsersGrounpMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.GroupName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.Remark).HasMaxLength(256);
            
            // Table & Column Mappings
            this.ToTable("UsersGrounp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GroupName).HasColumnName("GroupName");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
