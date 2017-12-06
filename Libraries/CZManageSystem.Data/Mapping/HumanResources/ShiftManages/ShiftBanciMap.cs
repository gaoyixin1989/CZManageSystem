using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
	public class ShiftBanciMap : EntityTypeConfiguration<ShiftBanci>
    {
        public ShiftBanciMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// �������
            /// <summary>
            this.Property(t => t.BcName)

          .HasMaxLength(50);
            /// <summary>
            /// ��ʼСʱֵ
            /// <summary>
            this.Property(t => t.StartHour)

          .HasMaxLength(50);
            /// <summary>
            /// ��ʼ����ֵ
            /// <summary>
            this.Property(t => t.StartMinute)

          .HasMaxLength(50);
            /// <summary>
            /// ����Сʱֵ
            /// <summary>
            this.Property(t => t.EndHour)

          .HasMaxLength(50);
            /// <summary>
            /// ��������ֵ
            /// <summary>
            this.Property(t => t.EndMinute)

          .HasMaxLength(50);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("ShiftBanci");
            // ���ID
            this.Property(t => t.Id).HasColumnName("Id");
            // �༭��
            this.Property(t => t.Editor).HasColumnName("Editor");
            // �༭ʱ��
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // �Ű���Ϣ��Id
            this.Property(t => t.ZhibanId).HasColumnName("ZhibanId");
            // �������
            this.Property(t => t.BcName).HasColumnName("BcName");
            // ��ʼСʱֵ
            this.Property(t => t.StartHour).HasColumnName("StartHour");
            // ��ʼ����ֵ
            this.Property(t => t.StartMinute).HasColumnName("StartMinute");
            // ����Сʱֵ
            this.Property(t => t.EndHour).HasColumnName("EndHour");
            // ��������ֵ
            this.Property(t => t.EndMinute).HasColumnName("EndMinute");
            // ֵ������
            this.Property(t => t.StaffNum).HasColumnName("StaffNum");
            // �������
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");

            // Relationships
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);
            this.HasMany(t => t.ShiftRichs).WithOptional().HasForeignKey(d => d.BanciId);
        }
    }
}
