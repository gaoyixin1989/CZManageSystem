using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationCloseApplyMap : EntityTypeConfiguration<HRVacationCloseApply>
	{
		public HRVacationCloseApplyMap()
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
		/// �ݼ�ԭ��
		/// <summary>
			  this.Property(t => t.Reason)

			.HasMaxLength(200);
		/// <summary>
		/// ����ԭ��
		/// <summary>
			  this.Property(t => t.Note)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("HRVacationCloseApply"); 
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
			// ��������
			this.Property(t => t.ClosedDays).HasColumnName("ClosedDays"); 
			// �ݼ�ԭ��
			this.Property(t => t.Reason).HasColumnName("Reason"); 
			// �ݼ����뵥id
			this.Property(t => t.VacationID).HasColumnName("VacationID"); 
			// ����ԭ��
			this.Property(t => t.Note).HasColumnName("Note"); 
			// ʵ�ʿ�ʼʱ��
			this.Property(t => t.Factst).HasColumnName("Factst"); 
			// ʵ�ʽ���ʱ��
			this.Property(t => t.Factet).HasColumnName("Factet");
            // ʵ������
            this.Property(t => t.Factdays).HasColumnName("Factdays");
            // ״̬��0δ�ύ��1���ύ
            this.Property(t => t.State).HasColumnName("State");

            //���
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.HRVacationCloseApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);
        }
	 }
}
