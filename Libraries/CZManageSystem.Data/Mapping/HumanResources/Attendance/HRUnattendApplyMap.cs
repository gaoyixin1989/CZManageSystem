using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
    public class HRUnattendApplyMap : EntityTypeConfiguration<HRUnattendApply>
    {
        public HRUnattendApplyMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplyID);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.ApplyTitle)

          .HasMaxLength(200);
            this.Property(t => t.ApplySn)

          .HasMaxLength(50);
            /// <summary>
            /// ������
            /// <summary>
            this.Property(t => t.ApplyUserName)

          .HasMaxLength(100);
            /// <summary>
            /// ���뵥λ
            /// <summary>
            this.Property(t => t.ApplyUnit)

          .HasMaxLength(200);
            /// <summary>
            /// ��������
            /// <summary>
            this.Property(t => t.ApplyDept)

          .HasMaxLength(200);
            /// <summary>
            /// ��ϵ����
            /// <summary>
            this.Property(t => t.Mobile)

          .HasMaxLength(20);
            /// <summary>
            /// ����ԭ��
            /// <summary>
            this.Property(t => t.Reason)

          .HasMaxLength(200);
            /// <summary>
            /// �쳣��¼
            /// <summary>
            //this.Property(t => t.RecordContent).HasMaxLength(250);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            /// <summary>
            /// ����IDs
            /// <summary>
            this.Property(t => t.AccessoryIds)
;
            /// <summary>
            /// ְλ��
            /// <summary>
            this.Property(t => t.UnattendPost)

          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("HRUnattendApply");
            // ����
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            // ����
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            // ����ʵ��Id
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // ������
            this.Property(t => t.ApplyUserName).HasColumnName("ApplyUserName");
            // ��ʼʱ��
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // ����ʱ��
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            // ����
            this.Property(t => t.PeriodTime).HasColumnName("PeriodTime");
            // ����ʱ�䡢����ʱ��
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            // ���뵥λ
            this.Property(t => t.ApplyUnit).HasColumnName("ApplyUnit");
            // ��������
            this.Property(t => t.ApplyDept).HasColumnName("ApplyDept");
            // ��ϵ����
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            // ����ԭ��
            this.Property(t => t.Reason).HasColumnName("Reason");
            // �쳣��¼
            this.Property(t => t.RecordContent).HasColumnName("RecordContent");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            // ����IDs
            this.Property(t => t.AccessoryIds).HasColumnName("AccessoryIds");
            // ְλ��
            this.Property(t => t.UnattendPost).HasColumnName("UnattendPost");
            // ����ID
            this.Property(t => t.AttendanceIds).HasColumnName("AttendanceIds");

            this.HasOptional(t => t.TrackingWorkflow)
               .WithMany()
               .HasForeignKey(d => d.WorkflowInstanceId);
        }
    }
}
