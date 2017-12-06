using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteRoleMap : EntityTypeConfiguration<VoteRole>
    {
        public VoteRoleMap()
        {
            // Primary Key
            this.HasKey(t => t.RoleID);

            // Properties
            // Table & Column Mappings
            this.ToTable("VoteRole");
            this.Property(t => t.RoleID).HasColumnName("RoleID");
            this.Property(t => t.GroupID).HasColumnName("GroupID");
            this.Property(t => t.Editor).HasColumnName("Editor");
        }
    }
}
