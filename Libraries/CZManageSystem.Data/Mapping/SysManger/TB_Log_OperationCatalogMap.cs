using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class TB_Log_OperationCatalogMap : EntityTypeConfiguration<TB_Log_OperationCatalog>
    {
        public TB_Log_OperationCatalogMap()
        {
            // Primary Key
            this.HasKey(t => t.operationID);

            // Properties
            this.Property(t => t.operationID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.parentID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.operationName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.description)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("TB_Log_OperationCatalog");
            this.Property(t => t.operationID).HasColumnName("operationID");
            this.Property(t => t.parentID).HasColumnName("parentID");
            this.Property(t => t.operationName).HasColumnName("operationName");
            this.Property(t => t.startTime).HasColumnName("startTime");
            this.Property(t => t.endTime).HasColumnName("endTime");
            this.Property(t => t.description).HasColumnName("description");
            this.Property(t => t.available).HasColumnName("available");
        }
    }
}
