using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteAnserMap : EntityTypeConfiguration<VoteAnser>
    {
        public VoteAnserMap()
        {
            // Primary Key
            this.HasKey(t => t.AnserID);

            // Properties
            this.Property(t => t.AnserContent)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("VoteAnser");
            this.Property(t => t.AnserID).HasColumnName("AnserID");
            this.Property(t => t.QuestionID).HasColumnName("QuestionID");
            this.Property(t => t.AnserContent).HasColumnName("AnserContent");
            this.Property(t => t.AnserScore).HasColumnName("AnserScore");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.MaxValue).HasColumnName("MaxValue");
            this.Property(t => t.MinValue).HasColumnName("MinValue");

            // Relationships
            this.HasOptional(t => t.VoteQuestion)
                .WithMany(t => t.VoteAnsers)
                .HasForeignKey(d => d.QuestionID);

        }
    }
}
