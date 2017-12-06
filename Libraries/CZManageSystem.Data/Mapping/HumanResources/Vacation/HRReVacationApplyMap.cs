using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRReVacationApplyMap : EntityTypeConfiguration<HRReVacationApply>
	{
		public HRReVacationApplyMap()
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
		/// ��ͷ�����쵼
		/// <summary>
			  this.Property(t => t.Leader)

			.HasMaxLength(50);
		/// <summary>
		/// �쳣�ݼ�ԭ��
		/// <summary>
			  this.Property(t => t.ReApplyReason)

			.HasMaxLength(200);
		/// <summary>
		/// ����ID
		/// <summary>
			  this.Property(t => t.Attids)
;
			// Table & Column Mappings
 			 this.ToTable("HRReVacationApply"); 
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
			// �Ƿ��ѿ�ͷ����
			this.Property(t => t.Boral).HasColumnName("Boral"); 
			// ��ͷ�����쵼
			this.Property(t => t.Leader).HasColumnName("Leader"); 
			// �쳣�ݼ�ԭ��
			this.Property(t => t.ReApplyReason).HasColumnName("ReApplyReason"); 
			// ����ID
			this.Property(t => t.Attids).HasColumnName("Attids");
            // ״̬��0δ�ύ��1���ύ
            this.Property(t => t.State).HasColumnName("State");

            //���
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.HRReVacationApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);

            this.HasMany(t => t.Meetings).WithOptional().HasForeignKey(d => d.ReVacationID);
            this.HasMany(t => t.Courses).WithOptional().HasForeignKey(d => d.ReVacationID);
            this.HasMany(t => t.Teachings).WithOptional().HasForeignKey(d => d.ReVacationID);
            this.HasMany(t => t.Others).WithOptional().HasForeignKey(d => d.ReVacationID);
        }
	 }
}
