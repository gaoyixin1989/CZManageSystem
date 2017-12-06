using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ��ʷ��Ŀ�ݹ������
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestAgoEstimateApplyMap : EntityTypeConfiguration<InvestAgoEstimateApply>
	{
		public InvestAgoEstimateApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ���̵���
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(200);
		/// <summary>
		/// ����id
		/// <summary>
			  this.Property(t => t.AppDept)

			.HasMaxLength(500);
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
		/// ԭ��
		/// <summary>
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("InvestAgoEstimateApply"); 
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
			// ԭ��
			this.Property(t => t.Content).HasColumnName("Content"); 
			// ״̬��0���桢1�ύ
			this.Property(t => t.State).HasColumnName("State");


            //���
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.InvestAgoEstimateApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.AppDeptObj).WithMany().HasForeignKey(d => d.AppDept);
            this.HasOptional(t => t.AppPersonObj).WithMany().HasForeignKey(d => d.AppPerson);
        }
    }
}
