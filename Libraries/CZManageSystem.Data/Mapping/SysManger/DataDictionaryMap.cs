using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class DataDictionaryMap : EntityTypeConfiguration<DataDictionary>
    {
        public DataDictionaryMap()
        {
            // Primary Key
            this.HasKey(t => t.DDId).Property(t => t.DDId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Properties
            this.Property(t => t.DDName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DDValue)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.DDText)
                .HasMaxLength(50);

            this.Property(t => t.ValueType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.EnableFlag)
                .IsRequired();

            this.Property(t => t.DefaultFlag)
                .IsRequired();

            this.Property(t => t.OrderNo)
                .IsRequired();

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("DataDictionary");
            this.Property(t => t.DDId).HasColumnName("DDId");
            this.Property(t => t.DDName).HasColumnName("DDName");
            this.Property(t => t.DDValue).HasColumnName("DDValue");
            this.Property(t => t.DDText).HasColumnName("DDText");
            this.Property(t => t.ValueType).HasColumnName("ValueType");
            this.Property(t => t.EnableFlag).HasColumnName("EnableFlag");
            this.Property(t => t.DefaultFlag).HasColumnName("DefaultFlag");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.Createdtime).HasColumnName("Createdtime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }
    }
}
