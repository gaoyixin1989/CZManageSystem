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
            /// ������Դ�����졢���ġ��Ѱ졢����...
            /// <summary>
            this.Property(t => t.DataSource).HasMaxLength(100);
            /// <summary>
            /// ���ݱ�ʶID
            /// <summary>
            this.Property(t => t.DataID).IsRequired().HasMaxLength(100);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark).HasMaxLength(500);

            this.Property(t => t.Owner).IsRequired().HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("PendingData");
            // ���
            this.Property(t => t.ID).HasColumnName("ID");
            // ������Դ�����졢���ġ��Ѱ졢����...
            this.Property(t => t.DataSource).HasColumnName("DataSource");
            // ���ݱ�ʶID
            this.Property(t => t.DataID).HasColumnName("DataID");
            // ����ID
            this.Property(t => t.SendID).HasColumnName("SendID");
            // ����������
            this.Property(t => t.Owner).HasColumnName("Owner");
            // ״̬��1-δ���ͣ�2���ͣ�Ĭ��1��
            this.Property(t => t.State).HasColumnName("State");
            // ���Դ���
            this.Property(t => t.TryTime).HasColumnName("TryTime");
            // ����ʱ��
            this.Property(t => t.DealTime).HasColumnName("DealTime");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
