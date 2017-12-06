
using System.Data.Entity.ModelConfiguration;
using CZManageSystem.Data.Domain.Administrative;

namespace CZManageSystem.Data.Mapping.Administrative
{
    public class BoardroomInfoMap : EntityTypeConfiguration<BoardroomInfo>
    {
        public BoardroomInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.BoardroomID);
            /// <summary>
            /// �༭��
            /// <summary>
            this.Property(t => t.Editor)
          .HasMaxLength(50);
            /// <summary>
            /// ���
            /// <summary>
            this.Property(t => t.Code)
          .HasMaxLength(50);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.Name)
            .HasMaxLength(50);
            /// <summary>
            /// �ص�
            /// <summary>
            this.Property(t => t.Address)
          .HasMaxLength(200);
            /// <summary>
            /// �豸
            /// <summary>
            this.Property(t => t.Equip)
          .HasMaxLength(50);
            /// <summary>
            /// �����豸
            /// <summary>
            this.Property(t => t.EquipOther)
          .HasMaxLength(200);
            /// <summary>
            /// ��;
            /// <summary>
            this.Property(t => t.Purpose)
          .HasMaxLength(200);
            /// <summary>
            /// Ԥ��ģʽ
            /// <summary>
            this.Property(t => t.BookMode)
          .HasMaxLength(200);
          //  /// <summary>
          //  /// ����λ
          //  /// <summary>
          //  this.Property(t => t.ManagerUnit)
          //.HasMaxLength(200);
          //  /// <summary>
          //  /// ������
          //  /// <summary>
          //  this.Property(t => t.ManagerPerson)
          //.HasMaxLength(200);
            /// <summary>
            /// ״̬
            /// <summary>
            this.Property(t => t.State)
          .HasMaxLength(50);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark)
          .HasMaxLength(500);
            /// <summary>
            /// �Զ����ֶ�
            /// <summary>
            this.Property(t => t.Field00)
          .HasMaxLength(200);
            this.Property(t => t.Field01)
          .HasMaxLength(200);
            this.Property(t => t.Field02)
          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("BoardroomInfo");
            // ������ID
            this.Property(t => t.BoardroomID).HasColumnName("BoardroomID");
            // �༭��
            this.Property(t => t.Editor).HasColumnName("Editor");
            // �༭ʱ��
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // ������λ
            this.Property(t => t.CorpID).HasColumnName("CorpID");
            // ���
            this.Property(t => t.Code).HasColumnName("Code");
            // ����
            this.Property(t => t.Name).HasColumnName("Name");
            // �ص�
            this.Property(t => t.Address).HasColumnName("Address");
            // �������
            this.Property(t => t.MaxMan).HasColumnName("MaxMan");
            // �豸
            this.Property(t => t.Equip).HasColumnName("Equip");
            // �����豸
            this.Property(t => t.EquipOther).HasColumnName("EquipOther");
            // ��;
            this.Property(t => t.Purpose).HasColumnName("Purpose");
            // Ԥ��ģʽ
            this.Property(t => t.BookMode).HasColumnName("BookMode");
            // ����λ
            this.Property(t => t.ManagerUnit).HasColumnName("ManagerUnit");
            // ������
            this.Property(t => t.ManagerPerson).HasColumnName("ManagerPerson");
            // ״̬
            this.Property(t => t.State).HasColumnName("State");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            // ͣ�ÿ�ʼʱ��
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // ͣ�ý���ʱ��
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            // �Զ����ֶ�
            this.Property(t => t.Field00).HasColumnName("Field00");
            this.Property(t => t.Field01).HasColumnName("Field01");
            this.Property(t => t.Field02).HasColumnName("Field02");
        }
    }
}
