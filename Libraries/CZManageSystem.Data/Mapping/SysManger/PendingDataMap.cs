using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
    public class PendingDataMap : EntityTypeConfiguration<PendingData>
    {
        public PendingDataMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            /// <summary>
            /// 数据来源：待办、待阅、已办、已阅...
            /// <summary>
            this.Property(t => t.DataSource).HasMaxLength(100);
            /// <summary>
            /// 数据标识ID
            /// <summary>
            this.Property(t => t.DataID).IsRequired().HasMaxLength(100);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark).HasMaxLength(500);

            this.Property(t => t.Owner).IsRequired().HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("PendingData");
            // 编号
            this.Property(t => t.ID).HasColumnName("ID");
            // 数据来源：待办、待阅、已办、已阅...
            this.Property(t => t.DataSource).HasColumnName("DataSource");
            // 数据标识ID
            this.Property(t => t.DataID).HasColumnName("DataID");
            // 推送ID
            this.Property(t => t.SendID).HasColumnName("SendID");
            // 数据所有者
            this.Property(t => t.Owner).HasColumnName("Owner");
            // 状态：1-未发送，2发送（默认1）
            this.Property(t => t.State).HasColumnName("State");
            // 尝试次数
            this.Property(t => t.TryTime).HasColumnName("TryTime");
            // 处理时间
            this.Property(t => t.DealTime).HasColumnName("DealTime");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
