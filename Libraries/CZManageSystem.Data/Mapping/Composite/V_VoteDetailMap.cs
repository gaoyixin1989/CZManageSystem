using CZManageSystem.Data.Domain.Composite; 
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Composite
{
    public class V_VoteDetailMap : EntityTypeConfiguration<V_VoteDetail>
    {
        public V_VoteDetailMap()
        {
            // Primary Key new { t.ApplyID, t.QuestionID, t.UserID,t.RealName ,t.ThemeID ,t.QuestionTitle ,t.CreatorID ,t.ApplyTitle ,t.CreateTime ,t.Creator }
            this.HasKey(t => new { t.ApplyID, t.QuestionID, t.UserID});
            this.Property(t => t.ApplyTitle)

          .HasMaxLength(200);
            this.Property(t => t.ThemeType)

          .HasMaxLength(100);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.IsNiming)

          .HasMaxLength(50);
            this.Property(t => t.QuestionTitle)

          .HasMaxLength(200);
            this.Property(t => t.UserName)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.RealName)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("V_VoteDetail");
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            this.Property(t => t.ThemeType).HasColumnName("ThemeType");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.IsNiming).HasColumnName("IsNiming");
            this.Property(t => t.QuestionID).HasColumnName("QuestionID");
            this.Property(t => t.QuestionTitle).HasColumnName("QuestionTitle");
            this.Property(t => t.AnswerNum).HasColumnName("AnswerNum");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.CreatorID).HasColumnName("CreatorID");
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ThemeID).HasColumnName("ThemeID");
        }
    }
}