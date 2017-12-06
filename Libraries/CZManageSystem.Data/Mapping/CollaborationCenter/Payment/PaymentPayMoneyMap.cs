using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentPayMoneyMap : EntityTypeConfiguration<PaymentPayMoney>
	{
		public PaymentPayMoneyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �������ʺ�
		/// <summary>
			  this.Property(t => t.PayerAccount)

			.HasMaxLength(50);
		/// <summary>
		/// ����������
		/// <summary>
			  this.Property(t => t.PayerName)

			.HasMaxLength(50);
		/// <summary>
		/// �����˷��д���
		/// <summary>
			  this.Property(t => t.PayerBranchCode)

			.HasMaxLength(50);
		/// <summary>
		/// �տ����ʺ�
		/// <summary>
			  this.Property(t => t.PayeeAccount)

			.HasMaxLength(50);
		/// <summary>
		/// �տ����ʺ�����
		/// <summary>
			  this.Property(t => t.PayeeName)

			.HasMaxLength(50);
		/// <summary>
		/// �տ�����������
		/// <summary>
			  this.Property(t => t.PayeeBranch)

			.HasMaxLength(50);
		/// <summary>
		/// �տ��˷��д���
		/// <summary>
			  this.Property(t => t.PayeeBranchCode)

			.HasMaxLength(50);
		/// <summary>
		/// �տ��˿�����
		/// <summary>
			  this.Property(t => t.PayeeOpenBank)

			.HasMaxLength(50);
		/// <summary>
		/// �տ�����������
		/// <summary>
			  this.Property(t => t.PayeeBank)

			.HasMaxLength(50);
		/// <summary>
		/// �տ������д���
		/// <summary>
			  this.Property(t => t.PayeeBankCode)

			.HasMaxLength(50);
		/// <summary>
		/// �տ������ش���
		/// <summary>
			  this.Property(t => t.PayeeAddressCode)

			.HasMaxLength(50);
		/// <summary>
		/// �ұ�
		/// <summary>
			  this.Property(t => t.MoneyType)

			.HasMaxLength(50);
		/// <summary>
		/// Ŀ��
		/// <summary>
			  this.Property(t => t.Purpose)

			.HasMaxLength(100);
		/// <summary>
		/// �տ����������
		/// <summary>
			  this.Property(t => t.PayeeAreaCode)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPayMoney"); 
			// ���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ����ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// �������ʺ�
			this.Property(t => t.PayerAccount).HasColumnName("PayerAccount"); 
			// ����������
			this.Property(t => t.PayerName).HasColumnName("PayerName"); 
			// �����˷��д���
			this.Property(t => t.PayerBranchCode).HasColumnName("PayerBranchCode"); 
			// �տ����ʺ�
			this.Property(t => t.PayeeAccount).HasColumnName("PayeeAccount"); 
			// �տ����ʺ�����
			this.Property(t => t.PayeeName).HasColumnName("PayeeName"); 
			// �տ�����������
			this.Property(t => t.PayeeBranch).HasColumnName("PayeeBranch"); 
			// �տ��˷��д���
			this.Property(t => t.PayeeBranchCode).HasColumnName("PayeeBranchCode"); 
			// �տ��˿�����
			this.Property(t => t.PayeeOpenBank).HasColumnName("PayeeOpenBank"); 
			// �տ�����������
			this.Property(t => t.PayeeBank).HasColumnName("PayeeBank"); 
			// �տ������д���
			this.Property(t => t.PayeeBankCode).HasColumnName("PayeeBankCode"); 
			// �տ������ش���
			this.Property(t => t.PayeeAddressCode).HasColumnName("PayeeAddressCode"); 
			// ���
			this.Property(t => t.Money).HasColumnName("Money"); 
			// �ұ�
			this.Property(t => t.MoneyType).HasColumnName("MoneyType"); 
			// Ŀ��
			this.Property(t => t.Purpose).HasColumnName("Purpose"); 
			// �տ����������
			this.Property(t => t.PayeeAreaCode).HasColumnName("PayeeAreaCode"); 
		 }
	 }
}
