using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ��ͬ��Ϣ
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestContractMap : EntityTypeConfiguration<InvestContract>
	{
		public InvestContractMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��Ŀ���
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
		/// <summary>
		/// ��ͬ���
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// ��ͬ����
		/// <summary>
			  this.Property(t => t.ContractName)

			.HasMaxLength(1000);
		/// <summary>
		/// ��Ӧ��
		/// <summary>
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
		/// <summary>
		/// ��ͬ���첿��
		/// <summary>
			  this.Property(t => t.DpCode)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
		/// <summary>
		/// �Ƿ�MIS����
		/// <summary>
			  this.Property(t => t.IsMIS)

			.HasMaxLength(50);
		/// <summary>
		/// �Ƿ�ɾ��
		/// <summary>
			  this.Property(t => t.IsDel)

			.HasMaxLength(50);
		/// <summary>
		/// ��ͬ��ˮ��
		/// <summary>
			  this.Property(t => t.ContractSeries)

			.HasMaxLength(50);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.Currency)

			.HasMaxLength(50);
		/// <summary>
		/// ��ͬ״̬
		/// <summary>
			  this.Property(t => t.ContractState)

			.HasMaxLength(50);
		/// <summary>
		/// ��ͬ����
		/// <summary>
			  this.Property(t => t.Attribute)

			.HasMaxLength(50);
		/// <summary>
		/// ��ͬ������
		/// <summary>
			  this.Property(t => t.ContractFilesNum)

			.HasMaxLength(50);
		/// <summary>
		/// ӡ��˰��
		/// <summary>
			  this.Property(t => t.StampTaxrate)

			.HasMaxLength(50);
		/// <summary>
		/// ӡ��˰��
		/// <summary>
			  this.Property(t => t.Stamptax)

			.HasMaxLength(50);
			  this.Property(t => t.ContractOpposition)

			.HasMaxLength(500);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.RequestDp)

			.HasMaxLength(50);
		/// <summary>
		/// ��ز���
		/// <summary>
			  this.Property(t => t.RelevantDp)

			.HasMaxLength(50);
		/// <summary>
		/// ��Ŀ��չԭ��
		/// <summary>
			  this.Property(t => t.ProjectCause)

			.HasMaxLength(500);
		/// <summary>
		/// ��ͬ����
		/// <summary>
			  this.Property(t => t.ContractType)

			.HasMaxLength(50);
		/// <summary>
		/// ��ͬ�Է���Դ
		/// <summary>
			  this.Property(t => t.ContractOppositionFrom)

			.HasMaxLength(50);
		/// <summary>
		/// ��ͬ�Է�ѡ��ʽ
		/// <summary>
			  this.Property(t => t.ContractOppositionType)

			.HasMaxLength(50);
		/// <summary>
		/// �ɹ���ʽ
		/// <summary>
			  this.Property(t => t.Purchase)

			.HasMaxLength(50);
		/// <summary>
		/// ���ʽ
		/// <summary>
			  this.Property(t => t.PayType)

			.HasMaxLength(50);
		/// <summary>
		/// ����˵��
		/// <summary>
			  this.Property(t => t.PayRemark)

			.HasMaxLength(1000);
		/// <summary>
		/// ��ܺ�ͬ
		/// <summary>
			  this.Property(t => t.IsFrameContract)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestContract"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ����ʱ�䣬ͬ�����ò���֮һ
			this.Property(t => t.ImportTime).HasColumnName("ImportTime"); 
			// ��Ŀ���
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��ͬ���
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// ��ͬ����
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			// ��Ӧ��
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// ǩ��ʱ��
			this.Property(t => t.SignTime).HasColumnName("SignTime"); 
			// ��ͬ���첿��
			this.Property(t => t.DpCode).HasColumnName("DpCode"); 
			// ������
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// ��ͬ��Ŀ����ͬ����˰���(Ԫ)
			this.Property(t => t.SignTotal).HasColumnName("SignTotal"); 
			// ��ͬ�ܽ��
			this.Property(t => t.AllTotal).HasColumnName("AllTotal"); 
			// ʵ�ʺ�ͬ���
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			// ��ע
			this.Property(t => t.Content).HasColumnName("Content"); 
			// �Ƿ�MIS����
			this.Property(t => t.IsMIS).HasColumnName("IsMIS"); 
			// �Ƿ�ɾ��
			this.Property(t => t.IsDel).HasColumnName("IsDel"); 
			// ��ͬ��ˮ��
			this.Property(t => t.ContractSeries).HasColumnName("ContractSeries"); 
			// ��ͬ˰��
			this.Property(t => t.Tax).HasColumnName("Tax"); 
			// ��ͬ��˰���(Ԫ
			this.Property(t => t.SignTotalTax).HasColumnName("SignTotalTax"); 
			// ����
			this.Property(t => t.Currency).HasColumnName("Currency"); 
			// ��ͬ״̬
			this.Property(t => t.ContractState).HasColumnName("ContractState"); 
			// ��ͬ����
			this.Property(t => t.Attribute).HasColumnName("Attribute"); 
			// ������ʼʱ��
			this.Property(t => t.ApproveStartTime).HasColumnName("ApproveStartTime"); 
			// ��������ʱ��
			this.Property(t => t.ApproveEndTime).HasColumnName("ApproveEndTime"); 
			// ��ͬ������
			this.Property(t => t.ContractFilesNum).HasColumnName("ContractFilesNum"); 
			// ӡ��˰��
			this.Property(t => t.StampTaxrate).HasColumnName("StampTaxrate"); 
			// ӡ��˰��
			this.Property(t => t.Stamptax).HasColumnName("Stamptax"); 
			this.Property(t => t.ContractOpposition).HasColumnName("ContractOpposition"); 
			// ������
			this.Property(t => t.RequestDp).HasColumnName("RequestDp"); 
			// ��ز���
			this.Property(t => t.RelevantDp).HasColumnName("RelevantDp"); 
			// ��Ŀ��չԭ��
			this.Property(t => t.ProjectCause).HasColumnName("ProjectCause"); 
			// ��ͬ����
			this.Property(t => t.ContractType).HasColumnName("ContractType"); 
			// ��ͬ�Է���Դ
			this.Property(t => t.ContractOppositionFrom).HasColumnName("ContractOppositionFrom"); 
			// ��ͬ�Է�ѡ��ʽ
			this.Property(t => t.ContractOppositionType).HasColumnName("ContractOppositionType"); 
			// �ɹ���ʽ
			this.Property(t => t.Purchase).HasColumnName("Purchase"); 
			// ���ʽ
			this.Property(t => t.PayType).HasColumnName("PayType"); 
			// ����˵��
			this.Property(t => t.PayRemark).HasColumnName("PayRemark"); 
			// ��ͬ��Ч������ʼ
			this.Property(t => t.ContractStartTime).HasColumnName("ContractStartTime"); 
			// ��ͬ��Ч������ֹ
			this.Property(t => t.ContractEndTime).HasColumnName("ContractEndTime"); 
			// ��ܺ�ͬ
			this.Property(t => t.IsFrameContract).HasColumnName("IsFrameContract"); 
			// ���ʱ��
			this.Property(t => t.DraftTime).HasColumnName("DraftTime"); 
			// ��Ŀ���
			this.Property(t => t.ProjectTotal).HasColumnName("ProjectTotal"); 
			// ��ǩ����Ŀ�ܶ�
			this.Property(t => t.ProjectAllTotal).HasColumnName("ProjectAllTotal");


            //���
            //this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.DpCode);
            //this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserID);

        }
	 }
}
