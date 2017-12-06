using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class Admin_AttachmentMap : EntityTypeConfiguration<Admin_Attachment>
    {
        public Admin_AttachmentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.FileName)

          .HasMaxLength(255);
            this.Property(t => t.Fileupload)
;
            this.Property(t => t.MimeType)

          .HasMaxLength(150);
            this.Property(t => t.FileSize)

          .HasMaxLength(150);
            // Table & Column Mappings
            this.ToTable("Admin_Attachment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.FileName).HasColumnName("FileName");
            this.Property(t => t.Fileupload).HasColumnName("Fileupload");
            this.Property(t => t.MimeType).HasColumnName("MimeType");
            this.Property(t => t.FileSize).HasColumnName("FileSize");
            this.Property(t => t.Upguid).HasColumnName("Upguid");

        }
    }

}
