using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
    public class BirthControlRosterMap : EntityTypeConfiguration<BirthControlRoster>
    {
        public BirthControlRosterMap()
        {
            // Primary Key
            this.HasKey(t => t.id);
            this.Property(t => t.FemaleName)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.FemaleWorkingPlace)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.MaleName)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.MaleWorkingPlace)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.FirstEmbryoSex)

          .HasMaxLength(50);
            this.Property(t => t.SecondEmbryoSex)

          .HasMaxLength(50);
            this.Property(t => t.OverThreeChildrenMele)

          .HasMaxLength(50);
            this.Property(t => t.OverThreeChildrenFemele)

          .HasMaxLength(50);
            this.Property(t => t.MeleLigation)

          .HasMaxLength(50);
            this.Property(t => t.FemeleLigation)

          .HasMaxLength(50);
            this.Property(t => t.PutAnnulus)

          .HasMaxLength(50);
            this.Property(t => t.Others)

          .HasMaxLength(50);
            this.Property(t => t.Remark)

          .HasMaxLength(255);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("BirthControlRoster");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.FemaleName).HasColumnName("FemaleName");
            this.Property(t => t.FemaleBirthday).HasColumnName("FemaleBirthday");
            this.Property(t => t.FemaleWorkingPlace).HasColumnName("FemaleWorkingPlace");
            this.Property(t => t.MaleName).HasColumnName("MaleName");
            this.Property(t => t.MaleWorkingPlace).HasColumnName("MaleWorkingPlace");
            this.Property(t => t.FirstEmbryoSex).HasColumnName("FirstEmbryoSex");
            this.Property(t => t.FirstEmbryoBirthday).HasColumnName("FirstEmbryoBirthday");
            this.Property(t => t.SecondEmbryoSex).HasColumnName("SecondEmbryoSex");
            this.Property(t => t.SecondEmbryoBirthday).HasColumnName("SecondEmbryoBirthday");
            this.Property(t => t.OverThreeChildrenMele).HasColumnName("OverThreeChildrenMele");
            this.Property(t => t.OverThreeChildrenFemele).HasColumnName("OverThreeChildrenFemele");
            this.Property(t => t.MeleLigation).HasColumnName("MeleLigation");
            this.Property(t => t.FemeleLigation).HasColumnName("FemeleLigation");
            this.Property(t => t.PutAnnulus).HasColumnName("PutAnnulus");
            this.Property(t => t.Others).HasColumnName("Others");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");


        }
    }
}
