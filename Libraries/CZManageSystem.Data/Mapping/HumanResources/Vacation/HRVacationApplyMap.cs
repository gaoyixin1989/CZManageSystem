using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
    public class HRVacationApplyMap : EntityTypeConfiguration<HRVacationApply>
    {
        public HRVacationApplyMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplyId);
            /// <summary>
            /// ���̵���
            /// <summary>
            this.Property(t => t.ApplySn)

          .HasMaxLength(50);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.ApplyTitle)

          .HasMaxLength(150);
            /// <summary>
            /// �ݼ�����
            /// <summary>
            this.Property(t => t.VacationType)

          .HasMaxLength(50);
            /// <summary>
            /// ��������
            /// <summary>
            this.Property(t => t.VacationClass)

          .HasMaxLength(50);
            /// <summary>
            /// �쳣�ݼ�ԭ��
            /// <summary>
            this.Property(t => t.Reason)

          .HasMaxLength(200);
            /// <summary>
            /// ����������
            /// <summary>
            this.Property(t => t.CancelDays)

           .HasPrecision(18, 1);
            this.Property(t => t.Newpt)

          .HasMaxLength(100);
            this.Property(t => t.Newst)

          .HasMaxLength(100);
            this.Property(t => t.Newet)

          .HasMaxLength(100);
            /// <summary>
            /// ����ص�
            /// <summary>
            this.Property(t => t.OutAddress)

          .HasMaxLength(200);
            /// <summary>
            /// �Ӱ�ʱ��
            /// <summary>
            this.Property(t => t.OverTime)

          .HasMaxLength(128);
            /// <summary>
            /// ����IDs
            /// <summary>
            this.Property(t => t.Attids)
;
            // Table & Column Mappings
            this.ToTable("HRVacationApply");
            // ����
            this.Property(t => t.ApplyId).HasColumnName("ApplyId");
            // ����ʵ��Id
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            // ���̵���
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // ����
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            // �༭��
            this.Property(t => t.Editor).HasColumnName("Editor");
            // �༭ʱ�䡢����ʱ��
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // �ݼ�����
            this.Property(t => t.VacationType).HasColumnName("VacationType");
            // ��������
            this.Property(t => t.VacationClass).HasColumnName("VacationClass");
            // ��ʼʱ��
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // ����ʱ��
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            // ����
            this.Property(t => t.PeriodTime).HasColumnName("PeriodTime");
            // �쳣�ݼ�ԭ��
            this.Property(t => t.Reason).HasColumnName("Reason");
            // ����
            this.Property(t => t.CancelVacation).HasColumnName("CancelVacation");
            // ����������
            this.Property(t => t.CancelDays).HasColumnName("CancelDays");
            this.Property(t => t.Newpt).HasColumnName("Newpt");
            this.Property(t => t.Newst).HasColumnName("Newst");
            this.Property(t => t.Newet).HasColumnName("Newet");
            // ����ص�
            this.Property(t => t.OutAddress).HasColumnName("OutAddress");
            // �Ӱ�ʱ��
            this.Property(t => t.OverTime).HasColumnName("OverTime");
            // ����IDs
            this.Property(t => t.Attids).HasColumnName("Attids");
            // ״̬��0δ�ύ��1���ύ
            this.Property(t => t.State).HasColumnName("State");

            //���
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.HRVacationApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);

            this.HasMany(t => t.Meetings).WithOptional().HasForeignKey(d => d.VacationID);
            this.HasMany(t => t.Courses).WithOptional().HasForeignKey(d => d.VacationID);
            this.HasMany(t => t.Teachings).WithOptional().HasForeignKey(d => d.VacationID);
            this.HasMany(t => t.Others).WithOptional().HasForeignKey(d => d.VacationID);

        }
    }
}
