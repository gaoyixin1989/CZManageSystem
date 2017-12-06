using CZManageSystem.Data.Domain.CollaborationCenter.SmsManager;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 短信发送统计视图
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.SmsManager
{
    public class V_SendSmsCountMap : EntityTypeConfiguration<V_SendSmsCount>
    {
        public V_SendSmsCountMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Sender, t.Date });
            //this.HasKey(t => t.Date);
            this.Property(t => t.Dept)

            .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("V_SendSmsCount");
            this.Property(t => t.Dept).HasColumnName("Dept");
            this.Property(t => t.Sender).HasColumnName("Sender");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Count).HasColumnName("Count");
            this.Property(t => t.SenderName).HasColumnName("SenderName");
            this.Property(t => t.DeptFullName).HasColumnName("DeptFullName");

        }
    }
}
