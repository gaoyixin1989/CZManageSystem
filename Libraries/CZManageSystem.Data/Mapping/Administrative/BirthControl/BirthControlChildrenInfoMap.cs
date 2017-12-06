using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
    public class BirthControlChildrenInfoMap : EntityTypeConfiguration<BirthControlChildrenInfo>
    {
        public BirthControlChildrenInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.id);
            this.Property(t => t.Name)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.Sex)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.PolicyPostiton)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.CISingleChildren)

          .HasMaxLength(50);
            this.Property(t => t.CISingleChildNum)

          .HasMaxLength(50);
            this.Property(t => t.Treatment)

          .HasMaxLength(100);
            this.Property(t => t.remark)

          .HasMaxLength(50);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("BirthControlChildrenInfo");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.PolicyPostiton).HasColumnName("PolicyPostiton");
            this.Property(t => t.CISingleChildren).HasColumnName("CISingleChildren");
            this.Property(t => t.CISingleChildNum).HasColumnName("CISingleChildNum");
            this.Property(t => t.Treatment).HasColumnName("Treatment");
            this.Property(t => t.remark).HasColumnName("remark");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }

    }
}
