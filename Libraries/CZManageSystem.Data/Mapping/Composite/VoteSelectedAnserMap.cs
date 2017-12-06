using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteSelectedAnserMap : EntityTypeConfiguration<VoteSelectedAnser>
    {
        public VoteSelectedAnserMap()
        {
            // Primary Key
            this.HasKey(t => t.SelectedAnserID);

            // Properties
            this.Property(t => t.OtherContent)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("VoteSelectedAnser");
            this.Property(t => t.SelectedAnserID).HasColumnName("SelectedAnserID"); 
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.Respondent).HasColumnName("Respondent");
            this.Property(t => t.ThemeID).HasColumnName("ThemeID");
            this.Property(t => t.QuestionID).HasColumnName("QuestionID");
            this.Property(t => t.AnserID).HasColumnName("AnserID");
            this.Property(t => t.OtherContent).HasColumnName("OtherContent");

            // Relationships
            this.HasOptional(t => t.VoteAnser)
                .WithMany(t => t.VoteSelectedAnsers)
                .HasForeignKey(d => d.AnserID);
            this.HasOptional(t => t.VoteQuestion)
                .WithMany(t => t.VoteSelectedAnsers)
                .HasForeignKey(d => d.QuestionID); 
        }
    }
}
