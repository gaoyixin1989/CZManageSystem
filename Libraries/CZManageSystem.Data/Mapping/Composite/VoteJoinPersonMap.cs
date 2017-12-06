using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteJoinPersonMap : EntityTypeConfiguration<VoteJoinPerson>
    {
        public VoteJoinPersonMap()
        {
            // Primary Key
            this.HasKey(t => t.JoinPersonID);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.RealName)
               .IsRequired()
               .HasMaxLength(50);
            this.Property(t => t.Remark)
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("VoteJoinPerson");
            this.Property(t => t.JoinPersonID).HasColumnName("JoinPersonID");
            this.Property(t => t.ThemeID).HasColumnName("ThemeID");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.Remark).HasColumnName("Remark");

            // Relationships
            this.HasRequired(t => t.VoteThemeInfo)
                .WithMany(t => t.VoteJoinPersons)
                .HasForeignKey(d => d.ThemeID);

        }
    }
}
