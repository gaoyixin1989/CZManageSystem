using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class VoteQuestionMap : EntityTypeConfiguration<VoteQuestion>
    {
        public VoteQuestionMap()
        {
            // Primary Key
            this.HasKey(t => t.QuestionID);

            // Properties
            this.Property(t => t.QuestionTitle)
                .HasMaxLength(200);

            this.Property(t => t.QuestionType)
                .HasMaxLength(200);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VoteQuestion");
            this.Property(t => t.QuestionID).HasColumnName("QuestionID");
            this.Property(t => t.QuestionTitle).HasColumnName("QuestionTitle");
            this.Property(t => t.QuestionType).HasColumnName("QuestionType");
            this.Property(t => t.AnswerNum).HasColumnName("AnswerNum");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatorID).HasColumnName("CreatorID");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.IsDel).HasColumnName("IsDel");
            this.Property(t => t.MaxValue).HasColumnName("MaxValue");
            this.Property(t => t.MinValue).HasColumnName("MinValue"); 
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
        }
    }
}
