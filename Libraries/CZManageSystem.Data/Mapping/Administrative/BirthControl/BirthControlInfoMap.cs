using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
    public class BirthControlInfoMap : EntityTypeConfiguration<BirthControlInfo>
    {
        public BirthControlInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.id);
            this.Property(t => t.Sex)

          .HasMaxLength(1);
            this.Property(t => t.Nation)

          .HasMaxLength(50);
            this.Property(t => t.IdCardNum)

          .HasMaxLength(50);
            this.Property(t => t.StreetBelong)

          .HasMaxLength(50);
            this.Property(t => t.MaritalStatus)

          .HasMaxLength(50);
            this.Property(t => t.PhoneNum)

          .HasMaxLength(50);
            this.Property(t => t.Havebear)

          .HasMaxLength(50);
            this.Property(t => t.Latemarriage)

          .HasMaxLength(50);
            this.Property(t => t.BRemark)

          .HasMaxLength(250);
            this.Property(t => t.SpouseName)

          .HasMaxLength(50);
            this.Property(t => t.Spousesex)

          .HasMaxLength(50);
            this.Property(t => t.SpouseIdCardNum)

          .HasMaxLength(50);
            this.Property(t => t.SpouseAccountbelong)

          .HasMaxLength(50);
            this.Property(t => t.SpousePhone)

          .HasMaxLength(50);
            this.Property(t => t.SpouseMaritalStatus)

          .HasMaxLength(50);
            this.Property(t => t.FixedJob)

          .HasMaxLength(50);
            this.Property(t => t.SpouseWorkingAddress)

          .HasMaxLength(50);
            this.Property(t => t.OrganizeGE)

          .HasMaxLength(50);
            this.Property(t => t.SameWorkPlace)

          .HasMaxLength(50);
            this.Property(t => t.ForeMarriageBore)

          .HasMaxLength(50);
            this.Property(t => t.ConfirmStatus)

          .HasMaxLength(25);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("BirthControlInfo");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Nation).HasColumnName("Nation");
            this.Property(t => t.IdCardNum).HasColumnName("IdCardNum");
            this.Property(t => t.StreetBelong).HasColumnName("StreetBelong");
            this.Property(t => t.MaritalStatus).HasColumnName("MaritalStatus");
            this.Property(t => t.PhoneNum).HasColumnName("PhoneNum");
            this.Property(t => t.Lastupdatedate).HasColumnName("Lastupdatedate");
            this.Property(t => t.Havebear).HasColumnName("Havebear");
            this.Property(t => t.Latemarriage).HasColumnName("Latemarriage");
            this.Property(t => t.FirstMarryDate).HasColumnName("FirstMarryDate");
            this.Property(t => t.DivorceDate).HasColumnName("DivorceDate");
            this.Property(t => t.RemarryDate).HasColumnName("RemarryDate");
            this.Property(t => t.WidowedDate).HasColumnName("WidowedDate");
            this.Property(t => t.LigationDate).HasColumnName("LigationDate");
            this.Property(t => t.BRemark).HasColumnName("BRemark");
            this.Property(t => t.SpouseName).HasColumnName("SpouseName");
            this.Property(t => t.Spousesex).HasColumnName("Spousesex");
            this.Property(t => t.SpouseBirthday).HasColumnName("SpouseBirthday");
            this.Property(t => t.SpouseIdCardNum).HasColumnName("SpouseIdCardNum");
            this.Property(t => t.SpouseAccountbelong).HasColumnName("SpouseAccountbelong");
            this.Property(t => t.SpousePhone).HasColumnName("SpousePhone");
            this.Property(t => t.SpouseMaritalStatus).HasColumnName("SpouseMaritalStatus");
            this.Property(t => t.FixedJob).HasColumnName("FixedJob");
            this.Property(t => t.SpouseWorkingAddress).HasColumnName("SpouseWorkingAddress");
            this.Property(t => t.SpouseLigationDate).HasColumnName("SpouseLigationDate");
            this.Property(t => t.OrganizeGE).HasColumnName("OrganizeGE");
            this.Property(t => t.SameWorkPlace).HasColumnName("SameWorkPlace");
            this.Property(t => t.ForeMarriageBore).HasColumnName("ForeMarriageBore");
            this.Property(t => t.ConfirmStatus).HasColumnName("ConfirmStatus");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");

        }
    }
}
