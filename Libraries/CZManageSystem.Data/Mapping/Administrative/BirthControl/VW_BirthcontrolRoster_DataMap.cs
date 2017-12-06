using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping
{
    public class VW_BirthcontrolRoster_DataMap : EntityTypeConfiguration<VW_BirthcontrolRoster_Data>

    {
        public VW_BirthcontrolRoster_DataMap()
        {
            // Primary Key
            this.Property(t => t.Id)

            .HasMaxLength(30);
            this.Property(t => t.InfoStatus)

          .HasMaxLength(6);
            this.Property(t => t.UserName)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.DpId)

          .HasMaxLength(255);
            this.Property(t => t.DpName)

          .HasMaxLength(50);
            this.Property(t => t.DpfullName)

          .HasMaxLength(256);
            this.Property(t => t.RealName)

          .HasMaxLength(50);
            this.Property(t => t.EmployeeId)

          .HasMaxLength(50);
            this.Property(t => t.FemaleName)

          .HasMaxLength(50);
            this.Property(t => t.FemaleWorkingPlace)

          .HasMaxLength(50);
            this.Property(t => t.MaleName)

          .HasMaxLength(50);
            this.Property(t => t.MaleWorkingPlace)

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
            this.Property(t => t.femeleLigation)

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
            this.ToTable("vw_birthcontrolroster_data");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InfoStatus).HasColumnName("InfoStatus");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.DpfullName).HasColumnName("DpfullName");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
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
            this.Property(t => t.femeleLigation).HasColumnName("femeleLigation");
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
