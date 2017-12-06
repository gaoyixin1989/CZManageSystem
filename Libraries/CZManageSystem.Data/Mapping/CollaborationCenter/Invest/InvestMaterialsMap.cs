using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ���ʲɹ�
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestMaterialsMap : EntityTypeConfiguration<InvestMaterials>
	{
		public InvestMaterialsMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��Ŀ���
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
		/// <summary>
		/// ��Ŀ����
		/// <summary>
			  this.Property(t => t.ProjectName)

			.HasMaxLength(1000);
		/// <summary>
		/// �������
		/// <summary>
			  this.Property(t => t.OrderID)

			.HasMaxLength(50);
		/// <summary>
		/// ����˵��
		/// <summary>
			  this.Property(t => t.OrderDesc)

			.HasMaxLength(2147483647);
		/// <summary>
		/// ����¼�빫˾
		/// <summary>
			  this.Property(t => t.OrderInCompany)

			.HasMaxLength(1000);
		/// <summary>
		/// ���״̬(��׼)
		/// <summary>
			  this.Property(t => t.AuditStatus)

			.HasMaxLength(50);
		/// <summary>
		/// �������չ�˾
		/// <summary>
			  this.Property(t => t.OrderOutCompany)

			.HasMaxLength(1000);
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
		/// ��Χϵͳ��ͬ���
		/// <summary>
			  this.Property(t => t.OutContractID)

			.HasMaxLength(100);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.OrderTitle)

			.HasMaxLength(1000);
		/// <summary>
		/// ������ע
		/// <summary>
			  this.Property(t => t.OrderNote)

			.HasMaxLength(2147483647);
		/// <summary>
		/// ��Ӧ��
		/// <summary>
			  this.Property(t => t.Apply)

			.HasMaxLength(1000);
			// Table & Column Mappings
 			 this.ToTable("InvestMaterials"); 
			// Ψһ��
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��Ŀ���
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��Ŀ����
			this.Property(t => t.ProjectName).HasColumnName("ProjectName"); 
			// �������
			this.Property(t => t.OrderID).HasColumnName("OrderID"); 
			// ����˵��
			this.Property(t => t.OrderDesc).HasColumnName("OrderDesc"); 
			// ����¼�빫˾
			this.Property(t => t.OrderInCompany).HasColumnName("OrderInCompany"); 
			// ���״̬(��׼)
			this.Property(t => t.AuditStatus).HasColumnName("AuditStatus"); 
			// ����¼����
			this.Property(t => t.OrderInPay).HasColumnName("OrderInPay"); 
			// �������չ�˾
			this.Property(t => t.OrderOutCompany).HasColumnName("OrderOutCompany"); 
			// �������ս��
			this.Property(t => t.OrderOutSum).HasColumnName("OrderOutSum"); 
			// ��������ʱ��
			this.Property(t => t.OrderCreateTime).HasColumnName("OrderCreateTime"); 
			// ��ͬ���
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// ��ͬ����
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			// ��Χϵͳ��ͬ���
			this.Property(t => t.OutContractID).HasColumnName("OutContractID"); 
			// ��������
			this.Property(t => t.OrderTitle).HasColumnName("OrderTitle"); 
			// ������ע
			this.Property(t => t.OrderNote).HasColumnName("OrderNote"); 
			// ��Ӧ��
			this.Property(t => t.Apply).HasColumnName("Apply"); 
			// �������հٷֱ� SUM
			this.Property(t => t.OrderOutRate).HasColumnName("OrderOutRate"); 
			// δ�����豸��Ԫ��
			this.Property(t => t.OrderUnReceived).HasColumnName("OrderUnReceived"); 
		 }
	 }
}
