using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.BirthControl
{
    public class VW_Birthcontrol_SingleChildren_DataMap : EntityTypeConfiguration<VW_Birthcontrol_SingleChildren_Data>
    {
        public VW_Birthcontrol_SingleChildren_DataMap()
        {
            // Primary Key
            this.Property(t => t.Id)

          .HasMaxLength(30);
            this.Property(t => t.RealName)

          .HasMaxLength(50);
            this.Property(t => t.UserName)

          .HasMaxLength(50);
            this.Property(t => t.DpName)

          .HasMaxLength(50);
            this.Property(t => t.Name)

          .HasMaxLength(50);
            this.Property(t => t.sameworkplace)

          .HasMaxLength(50);
            this.Property(t => t.SpouseName)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("vw_Birthcontrol_SingleChildren_Data");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.sameworkplace).HasColumnName("sameworkplace");
            this.Property(t => t.SpouseName).HasColumnName("SpouseName");

        }
    }

}
