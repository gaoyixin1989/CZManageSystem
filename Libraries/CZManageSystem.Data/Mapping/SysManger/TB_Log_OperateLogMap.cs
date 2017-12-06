using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Models.Mapping
{
    public class TB_Log_OperateLogMap : EntityTypeConfiguration<TB_Log_OperateLog>
    {
        public TB_Log_OperateLogMap()
        {
            // Primary Key
            this.HasKey(t => t.uid);

            // Properties
            this.Property(t => t.portalID)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.clientIP)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.clientComputerName)
                .HasMaxLength(50);

            this.Property(t => t.serverIP)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.operationID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.exceptionID)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.description)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("TB_Log_OperateLog");
            this.Property(t => t.uid).HasColumnName("uid");
            this.Property(t => t.opStartTime).HasColumnName("opStartTime");
            this.Property(t => t.portalID).HasColumnName("portalID");
            this.Property(t => t.clientIP).HasColumnName("clientIP");
            this.Property(t => t.clientComputerName).HasColumnName("clientComputerName");
            this.Property(t => t.serverIP).HasColumnName("serverIP");
            this.Property(t => t.opEndTime).HasColumnName("opEndTime");
            this.Property(t => t.operationID).HasColumnName("operationID");
            this.Property(t => t.exceptionID).HasColumnName("exceptionID");
            this.Property(t => t.description).HasColumnName("description");

            // Relationships
            this.HasRequired(t => t.TB_Log_ExceptionCatalog)
                .WithMany(t => t.TB_Log_OperateLog)
                .HasForeignKey(d => d.exceptionID);
            this.HasRequired(t => t.TB_Log_OperationCatalog)
                .WithMany(t => t.TB_Log_OperateLog)
                .HasForeignKey(d => d.operationID);

        }
    }
}
