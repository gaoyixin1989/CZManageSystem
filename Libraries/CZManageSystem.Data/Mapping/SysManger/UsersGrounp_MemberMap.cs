using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class UsersGrounp_MemberMap : EntityTypeConfiguration<UsersGrounp_Member>
    {
        public UsersGrounp_MemberMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Properties
            this.Property(t => t.GroupId).IsRequired();
            this.Property(t => t.MemberType).HasMaxLength(50);

            this.Property(t => t.MemberId).HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("UsersGrounp_Member");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.MemberType).HasColumnName("MemberType");
            this.Property(t => t.MemberId).HasColumnName("MemberId");
        }
    }
}
