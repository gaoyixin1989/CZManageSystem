using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class ActivitySetMap : EntityTypeConfiguration<ActivitySet>
    {
        public ActivitySetMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SetId, t.ActivityId });
            // Properties
            this.Property(t => t.SetId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ActivityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("bwwf_ActivitySet");
            this.Property(t => t.SetId).HasColumnName("SetId");
            this.Property(t => t.ActivityId).HasColumnName("ActivityId");


            // Relationships
            //this.HasOptional(t => t.Activities).WithMany(t => t.ActivitySets)
            //    .HasForeignKey(d => d.ActivityId);
        }
    }
}
