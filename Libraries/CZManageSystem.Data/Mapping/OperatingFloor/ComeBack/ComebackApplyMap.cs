using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ComebackApplyMap : EntityTypeConfiguration<ComebackApply>
	{
		public ComebackApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyId);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.Title)  

			.HasMaxLength(500);
		/// <summary>
		/// ���̵���
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(50);
		/// <summary>
		/// �绰
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(50);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.ApplyDept)

			.HasMaxLength(100);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.ApplyUser)

			.HasMaxLength(100);
		/// <summary>
		/// Ԥ��������
		/// <summary>
			  this.Property(t => t.BudgetDept)

			.HasMaxLength(100);
            /// <summary>
            /// ������Դ���
            /// <summary>
            this.Property(t => t.SourceTypeID);

			  this.Property(t => t.SourceChildId)

			.HasMaxLength(100);
		/// <summary>
		/// �⿪չ��Ŀ����
		/// <summary>
			  this.Property(t => t.ProjName)

			.HasMaxLength(100);
		/// <summary>
		/// ����������Ԥ����Ŀ����
		/// <summary>
			  this.Property(t => t.PrevProjName)

			.HasMaxLength(100);
		/// <summary>
		/// ����������Ԥ����Ŀ���
		/// <summary>
			  this.Property(t => t.PrevProjCode)

			.HasMaxLength(100);
		/// <summary>
		/// ��Ŀ������
		/// <summary>
			  this.Property(t => t.ProjManager)

			.HasMaxLength(100);
		/// <summary>
		/// ��Ŀ��չ��Ҫ�Լ�Ч���Է���
		/// <summary>
			  this.Property(t => t.ProjAnalysis)

			.HasMaxLength(1073741823);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("ComebackApply"); 
			this.Property(t => t.ApplyId).HasColumnName("ApplyId"); 
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			// ����
			this.Property(t => t.Title).HasColumnName("Title"); 
			// ���̵���
			this.Property(t => t.Series).HasColumnName("Series"); 
			// �绰
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// ״̬
			this.Property(t => t.Status).HasColumnName("Status"); 
			// ����ʱ��
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			// ����
			this.Property(t => t.ApplyDept).HasColumnName("ApplyDept"); 
			// ������
			this.Property(t => t.ApplyUser).HasColumnName("ApplyUser"); 
			// Ԥ��������
			this.Property(t => t.BudgetDept).HasColumnName("BudgetDept"); 
			// ������Դ���
			this.Property(t => t.SourceTypeID).HasColumnName("SourceTypeID"); 
			// ��Ŀ��ʼʱ��
			this.Property(t => t.TimeStart).HasColumnName("TimeStart"); 
			// ��Ŀ����ʱ��
			this.Property(t => t.TimeEnd).HasColumnName("TimeEnd"); 
			this.Property(t => t.SourceChildId).HasColumnName("SourceChildId"); 
			// �⿪չ��Ŀ����
			this.Property(t => t.ProjName).HasColumnName("ProjName"); 
			// ����������Ԥ����Ŀ����
			this.Property(t => t.PrevProjName).HasColumnName("PrevProjName"); 
			// ����������Ԥ����Ŀ���
			this.Property(t => t.PrevProjCode).HasColumnName("PrevProjCode"); 
			// ��Ŀ������
			this.Property(t => t.ProjManager).HasColumnName("ProjManager"); 
			// ����˰���������
			this.Property(t => t.AppAmount).HasColumnName("AppAmount"); 
			// ��Ŀ��չ��Ҫ�Լ�Ч���Է���
			this.Property(t => t.ProjAnalysis).HasColumnName("ProjAnalysis"); 
			// Ԥ�����
			this.Property(t => t.Year).HasColumnName("Year"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// ��˰���������
			this.Property(t => t.AppAmountHanshui).HasColumnName("AppAmountHanshui"); 
		 }
	 }
}
