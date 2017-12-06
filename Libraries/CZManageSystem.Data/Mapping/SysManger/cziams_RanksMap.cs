using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class Cziams_RanksMap : EntityTypeConfiguration<Cziams_Ranks>
    {
        public Cziams_RanksMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.RankID)
                .HasMaxLength(32);

            this.Property(t => t.ParentRankID)
                .HasMaxLength(32);

            this.Property(t => t.RankName)
                .HasMaxLength(255);

            this.Property(t => t.RankDescription)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("cziams_Ranks");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RankID).HasColumnName("RankID");
            this.Property(t => t.ParentRankID).HasColumnName("ParentRankID");
            this.Property(t => t.RankName).HasColumnName("RankName");
            this.Property(t => t.RankDescription).HasColumnName("RankDescription");
        }
    }
}
