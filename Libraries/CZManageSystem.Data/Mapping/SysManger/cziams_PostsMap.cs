using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class Cziams_PostsMap : EntityTypeConfiguration<Cziams_Posts>
    {
        public Cziams_PostsMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.PostID)
                .HasMaxLength(32);

            this.Property(t => t.ParentPostID)
                .HasMaxLength(32);

            this.Property(t => t.DepartmentID)
                .HasMaxLength(32);

            this.Property(t => t.RankID)
                .HasMaxLength(32);

            this.Property(t => t.PostSign)
                .HasMaxLength(255);

            this.Property(t => t.PostName)
                .HasMaxLength(255);

            this.Property(t => t.PostDescription)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("cziams_Posts");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.PostID).HasColumnName("PostID");
            this.Property(t => t.ParentPostID).HasColumnName("ParentPostID");
            this.Property(t => t.DepartmentID).HasColumnName("DepartmentID");
            this.Property(t => t.RankID).HasColumnName("RankID");
            this.Property(t => t.PostSign).HasColumnName("PostSign");
            this.Property(t => t.PostName).HasColumnName("PostName");
            this.Property(t => t.PostDescription).HasColumnName("PostDescription");
        }
    }
}
