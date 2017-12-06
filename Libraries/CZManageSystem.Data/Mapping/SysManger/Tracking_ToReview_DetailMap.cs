using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class Tracking_ToReview_DetailMap : EntityTypeConfiguration<Tracking_ToReview_Detail>
    {
        public Tracking_ToReview_DetailMap()
        {
            // Primary Key
            this.HasKey(t => t.ActivityInstanceId);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);
            this.Property(t => t.State)
               .IsRequired();
           
            this.ToTable("vw_bwwf_Tracking_ToReview_Detail");
        }
    }
}
