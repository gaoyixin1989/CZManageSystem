using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HROverTimeApplyMap : EntityTypeConfiguration<HROverTimeApply>
	{
		public HROverTimeApplyMap()
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
		/// ְ��
		/// <summary>
			  this.Property(t => t.ApplyPost)

			.HasMaxLength(100);
		/// <summary>
		/// ֱ����������
		/// <summary>
			  this.Property(t => t.ManageName)

			.HasMaxLength(100);
		/// <summary>
		/// ֱ������ְ��
		/// <summary>
			  this.Property(t => t.ManagePost)

			.HasMaxLength(100);
		/// <summary>
		/// �Ӱ�ص�
		/// <summary>
			  this.Property(t => t.Address)

			.HasMaxLength(500);
		/// <summary>
		/// �Ӱ�����
		/// <summary>
			  this.Property(t => t.OvertimeType)

			.HasMaxLength(200);
		/// <summary>
		/// �Ӱ�ԭ��
		/// <summary>
			  this.Property(t => t.Reason)

			.HasMaxLength(200);
			  this.Property(t => t.Newpt)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HROverTimeApply"); 
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
			// ְ��
			this.Property(t => t.ApplyPost).HasColumnName("ApplyPost"); 
			// ֱ����������
			this.Property(t => t.ManageName).HasColumnName("ManageName"); 
			// ֱ������ְ��
			this.Property(t => t.ManagePost).HasColumnName("ManagePost"); 
			// �Ӱ�ص�
			this.Property(t => t.Address).HasColumnName("Address"); 
			// �Ӱ�����
			this.Property(t => t.OvertimeType).HasColumnName("OvertimeType"); 
			// �Ӱ�ԭ��
			this.Property(t => t.Reason).HasColumnName("Reason"); 
			// �༭��
			this.Property(t => t.Editor).HasColumnName("Editor");
            // ����ʱ��
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Newpt).HasColumnName("Newpt"); 
		 }
	 }
}
