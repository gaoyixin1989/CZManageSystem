using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class TB_Log_ExceptionCatalogMap : EntityTypeConfiguration<TB_Log_ExceptionCatalog>
    {
        public TB_Log_ExceptionCatalogMap()
        {
            // Primary Key
            this.HasKey(t => t.exceptionID);

            // Properties
            this.Property(t => t.exceptionID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.parentID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.exceptionName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("TB_Log_ExceptionCatalog");
            this.Property(t => t.exceptionID).HasColumnName("exceptionID");
            this.Property(t => t.parentID).HasColumnName("parentID");
            this.Property(t => t.exceptionName).HasColumnName("exceptionName");
            this.Property(t => t.startTime).HasColumnName("startTime");
            this.Property(t => t.endTime).HasColumnName("endTime");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.available).HasColumnName("available");
        }
    }
}
