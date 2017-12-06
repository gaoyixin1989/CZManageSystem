using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
    public class BirthControlLogMap : EntityTypeConfiguration<BirthControlLog>
    {
        public BirthControlLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.UserName)

          .HasMaxLength(50);
            this.Property(t => t.UserIp)

          .HasMaxLength(150);
            this.Property(t => t.OpType)

          .HasMaxLength(50);
            this.Property(t => t.Description)
;
            // Table & Column Mappings
            this.ToTable("BirthControlLog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.UserIp).HasColumnName("UserIp");
            this.Property(t => t.OpType).HasColumnName("OpType");
            this.Property(t => t.OpTime).HasColumnName("OpTime");
            this.Property(t => t.Description).HasColumnName("Description");

        }
    }
}
