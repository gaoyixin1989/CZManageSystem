using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
    public class Consumable_MakeupMap : EntityTypeConfiguration<Consumable_Makeup>
    {
        public Consumable_MakeupMap()
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
            /// ���벿��id
            /// <summary>
            this.Property(t => t.AppDept)

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
            this.ToTable("Consumable_Makeup");
            // ���
            this.Property(t => t.ID).HasColumnName("ID");
            // �����ύʱ��
            this.Property(t => t.AppTime).HasColumnName("AppTime");
            // ���벿��id
            this.Property(t => t.AppDept).HasColumnName("AppDept");
            // ������
            this.Property(t => t.AppPerson).HasColumnName("AppPerson");
            // �������ֻ�
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            // �������
            this.Property(t => t.Title).HasColumnName("Title");
            // �˿�ԭ��
            this.Property(t => t.Content).HasColumnName("Content");
            // ״̬��0���桢1�ύ
            this.Property(t => t.State).HasColumnName("State");
            // ʹ����ID
            this.Property(t => t.UsePerson).HasColumnName("UsePerson");

            // Relationships
            this.HasOptional(t => t.UsersForApp).WithMany()
                .HasForeignKey(d => d.AppPerson);
            this.HasOptional(t => t.UsersForUse).WithMany()
                .HasForeignKey(d => d.UsePerson);
            this.HasOptional(t => t.DeptsForApp).WithMany()
                .HasForeignKey(d => d.AppDept);
            this.HasOptional(t => t.Tracking_Workflow).WithMany()
                .HasForeignKey(d => d.WorkflowInstanceId);

        }
    }
}
