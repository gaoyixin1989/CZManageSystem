using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteAppQidMap : EntityTypeConfiguration<VoteAppQid>
    {
        public VoteAppQidMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ApplyID, t.QuesionID });

            // Properties
            this.Property(t => t.ApplyID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.QuesionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VoteAppQid");
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            this.Property(t => t.QuesionID).HasColumnName("QuesionID");
            this.Property(t => t.ThemeID).HasColumnName("ThemeID");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
        }
    }
}
