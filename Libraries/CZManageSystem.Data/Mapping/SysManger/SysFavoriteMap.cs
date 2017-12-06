using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class SysFavoriteMap : EntityTypeConfiguration<SysFavorite>
    {
        public SysFavoriteMap()
        {
            // Primary Key
            this.HasKey(t => t.FavoriteId);

            // Properties
            this.Property(t => t.WorkflowId);
            this.Property(t => t.WorkflowName)
                .HasMaxLength(50);

            this.Property(t => t.UserId);

            this.Property(t => t.Remark)
                .HasMaxLength(300);

            this.Property(t => t.Type).IsRequired();

            // Table & Column Mappings
            this.ToTable("SysFavorite");
            this.Property(t => t.FavoriteId).HasColumnName("FavoriteId");
            this.Property(t => t.WorkflowId).HasColumnName("WorkflowId");
            this.Property(t => t.WorkflowName).HasColumnName("WorkflowName");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.EnableFlag).HasColumnName("EnableFlag");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Type).HasColumnName("Type");
        }
    }
}
