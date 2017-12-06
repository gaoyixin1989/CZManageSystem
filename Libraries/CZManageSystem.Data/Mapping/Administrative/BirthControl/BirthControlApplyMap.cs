using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.BirthControl
{
    public class BirthControlApplyMap : EntityTypeConfiguration<BirthControlApply>
    {
        public BirthControlApplyMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.SheetId)

          .HasMaxLength(50);
            this.Property(t => t.Title)

          .HasMaxLength(50);
            this.Property(t => t.CurrentActivity)

          .HasMaxLength(50);
            this.Property(t => t.CurrentActors)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("BirthControlApply");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.SheetId).HasColumnName("SheetId");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.ApplyTime).HasColumnName("ApplyTime");
            this.Property(t => t.CurrentActivity).HasColumnName("CurrentActivity");
            this.Property(t => t.CurrentActors).HasColumnName("CurrentActors");
            this.Property(t => t.State).HasColumnName("State");
        }

    }
}
