using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
    public class VW_Birthcontrol_DataMap : EntityTypeConfiguration<VW_Birthcontrol_Data>
    {
        public VW_Birthcontrol_DataMap()
        {
            // Primary Key
            this.Property(t => t.Id)

          .HasMaxLength(30);
            this.Property(t => t.InfoStatus)

          .HasMaxLength(6);
            this.Property(t => t.StatusColor)

          .HasMaxLength(11);
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
            this.Property(t => t.Email)

          .HasMaxLength(512);
            this.Property(t => t.Mobile)

          .HasMaxLength(32);
            this.Property(t => t.Status)

          .HasMaxLength(6);
            this.Property(t => t.IsFormal)
           .IsRequired()
          .HasMaxLength(2);
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
            this.Property(t => t.fixedjob)

          .HasMaxLength(50);
            this.Property(t => t.SpouseWorkingAddress)

          .HasMaxLength(50);
            this.Property(t => t.organizeGE)

          .HasMaxLength(50);
            this.Property(t => t.sameworkplace)

          .HasMaxLength(50);
            this.Property(t => t.Latemarriage)

          .HasMaxLength(50);
            this.Property(t => t.foremarriagebore)

          .HasMaxLength(50);
            this.Property(t => t.confirmstatus)

          .HasMaxLength(25);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("vw_birthcontrol_data");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.InfoStatus).HasColumnName("InfoStatus");
            this.Property(t => t.StatusColor).HasColumnName("StatusColor");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.DpId).HasColumnName("DpId");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.DpfullName).HasColumnName("DpfullName");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.JoinTime).HasColumnName("JoinTime");
            this.Property(t => t.IsFormal).HasColumnName("IsFormal");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Nation).HasColumnName("Nation");
            this.Property(t => t.IdCardNum).HasColumnName("IdCardNum");
            this.Property(t => t.StreetBelong).HasColumnName("StreetBelong");
            this.Property(t => t.MaritalStatus).HasColumnName("MaritalStatus");
            this.Property(t => t.PhoneNum).HasColumnName("PhoneNum");
            this.Property(t => t.Lastupdatedate).HasColumnName("Lastupdatedate");
            this.Property(t => t.Havebear).HasColumnName("Havebear");
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
            this.Property(t => t.fixedjob).HasColumnName("fixedjob");
            this.Property(t => t.SpouseWorkingAddress).HasColumnName("SpouseWorkingAddress");
            this.Property(t => t.SpouseLigationDate).HasColumnName("SpouseLigationDate");
            this.Property(t => t.organizeGE).HasColumnName("organizeGE");
            this.Property(t => t.sameworkplace).HasColumnName("sameworkplace");
            this.Property(t => t.Latemarriage).HasColumnName("Latemarriage");
            this.Property(t => t.foremarriagebore).HasColumnName("foremarriagebore");
            this.Property(t => t.confirmstatus).HasColumnName("confirmstatus");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }
    }
}
