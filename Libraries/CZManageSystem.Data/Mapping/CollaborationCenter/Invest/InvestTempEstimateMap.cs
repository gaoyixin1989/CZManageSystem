using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class InvestTempEstimateMap : EntityTypeConfiguration<InvestTempEstimate>
	{
		public InvestTempEstimateMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��ĿID
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// ��ͬID
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// ��Ӧ��
		/// <summary>
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
		/// <summary>
		/// ����רҵ
		/// <summary>
			  this.Property(t => t.Study)

			.HasMaxLength(500);
		/// <summary>
		/// ��Ŀ
		/// <summary>
			  this.Property(t => t.Course)

			.HasMaxLength(500);
		/// <summary>
		/// �Ƿ�����
		/// <summary>
			  this.Property(t => t.IsLock)

			.HasMaxLength(50);
		/// <summary>
		/// ��ǰ״̬
		/// <summary>
			  this.Property(t => t.Status)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestTempEstimate"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��ĿID
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��ͬID
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// ��Ӧ��
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// ��ͬ���
			this.Property(t => t.SignTotal).HasColumnName("SignTotal"); 
			// ��ͬʵ�ʽ��
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			// ����רҵ
			this.Property(t => t.Study).HasColumnName("Study"); 
			// ������ID
			this.Property(t => t.ManagerID).HasColumnName("ManagerID"); 
			// ��Ŀ
			this.Property(t => t.Course).HasColumnName("Course"); 
			// �ϸ��½���
			this.Property(t => t.BackRate).HasColumnName("BackRate"); 
			// ��Ŀ�������
			this.Property(t => t.Rate).HasColumnName("Rate"); 
			// �Ѹ����
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			// �ݹ����
			this.Property(t => t.NotPay).HasColumnName("NotPay"); 
			// �Ƿ�����
			this.Property(t => t.IsLock).HasColumnName("IsLock"); 
			// �ݹ���ԱID
			this.Property(t => t.EstimateUserID).HasColumnName("EstimateUserID"); 
			// ��ǰ״̬
			this.Property(t => t.Status).HasColumnName("Status"); 
			// ״̬����ʱ��
			this.Property(t => t.StatusTime).HasColumnName("StatusTime");

            this.HasOptional(t => t.ManagerObj).WithMany().HasForeignKey(d => d.ManagerID);
        }
	 }
}
