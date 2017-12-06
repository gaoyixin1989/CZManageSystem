using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
    public class Consumable_CancellingMap : EntityTypeConfiguration<Consumable_Cancelling>
    {
        public Consumable_CancellingMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            /// <summary>
            /// ����ʵ��ID
            /// <summary>
            this.Property(t => t.WorkflowInstanceId);
            /// <summary>
            /// ���̵���
            /// <summary>
            this.Property(t => t.Series)

          .HasMaxLength(200);
            /// <summary>
            /// ����id
            /// <summary>
            this.Property(t => t.AppDept)

          .HasMaxLength(640);
            /// <summary>
            /// ������
            /// <summary>
            this.Property(t => t.AppPerson)

          .HasMaxLength(200);
            /// <summary>
            /// �������ֻ�
            /// <summary>
            this.Property(t => t.Mobile)

          .HasMaxLength(200);
            /// <summary>
            /// �������
            /// <summary>
            this.Property(t => t.Title)

          .HasMaxLength(200);
            /// <summary>
            /// �˿�ԭ��
            /// <summary>
            this.Property(t => t.Content)

            .HasMaxLength(2147483647);
            // Table & Column Mappings
            this.ToTable("Consumable_Cancelling");
            // ���
            this.Property(t => t.ID).HasColumnName("ID");
            // ����ʵ��ID
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            // ���̵���
            this.Property(t => t.Series).HasColumnName("Series");
            // ����ʱ��
            this.Property(t => t.ApplyTime).HasColumnName("ApplyTime");
            // ����id
            this.Property(t => t.AppDept).HasColumnName("AppDept");
            // ������
            this.Property(t => t.AppPerson).HasColumnName("AppPerson");
            // �������ֻ�
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            // �������
            this.Property(t => t.Title).HasColumnName("Title");
            // ����ԭ��
            this.Property(t => t.Content).HasColumnName("Content");
            // ״̬��0���桢1�ύ
            this.Property(t => t.State).HasColumnName("State");

            // Relationships
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t=>t.Consumable_Cancellings)
                .HasForeignKey(d => d.WorkflowInstanceId);
        }
    }
}
