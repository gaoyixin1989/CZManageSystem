using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class ScheduleMap : EntityTypeConfiguration<Schedule>
    {
        public ScheduleMap()
        {
            // Primary Key
            this.HasKey(t => t.ScheduleId);

            // Table & Column Mappings
            this.ToTable("Schedule");
            this.Property(t => t.ScheduleId).HasColumnName("ScheduleId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Createdtime).HasColumnName("Createdtime");
            this.Property(t => t.Sms).HasColumnName("Sms");//是否已经发送通知
        }
    }
}
