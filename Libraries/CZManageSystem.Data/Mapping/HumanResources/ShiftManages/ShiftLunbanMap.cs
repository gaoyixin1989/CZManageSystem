using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// �ְ���Ϣ
    /// </summary>
	public class ShiftLunbanMap : EntityTypeConfiguration<ShiftLunban>
    {
        public ShiftLunbanMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// ��ʼ��
            /// <summary>
            this.Property(t => t.StartDay)

          .HasMaxLength(50);
            /// <summary>
            /// ������
            /// <summary>
            this.Property(t => t.EndDay)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("ShiftLunban");
            // �ְ�ID
            this.Property(t => t.Id).HasColumnName("Id");
            // �Ű���Ϣ��ID
            this.Property(t => t.ZhibanId).HasColumnName("ZhibanId");
            // ��ʼ��
            this.Property(t => t.StartDay).HasColumnName("StartDay");
            // ������
            this.Property(t => t.EndDay).HasColumnName("EndDay");

            // Relationships
            this.HasMany(t => t.ShiftLbusers).WithOptional().HasForeignKey(d => d.LunbanId);
        }
    }
}
