using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteTidQieMap : EntityTypeConfiguration<VoteTidQie>
    {
        public VoteTidQieMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ThemeID, t.QuestionID });

            // Properties
            this.Property(t => t.ThemeID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.QuestionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("VoteTidQie");
            this.Property(t => t.ThemeID).HasColumnName("ThemeID");
            this.Property(t => t.QuestionID).HasColumnName("QuestionID");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");

            // Relationships
            this.HasRequired(t => t.VoteQuestion)
                .WithMany(t => t.VoteTidQies)
                .HasForeignKey(d => d.QuestionID);
            this.HasRequired(t => t.VoteThemeInfo)
                .WithMany(t => t.VoteTidQies)
                .HasForeignKey(d => d.ThemeID);

        }
    }
}
