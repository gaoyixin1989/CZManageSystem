using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentPayeeMap : EntityTypeConfiguration<PaymentPayee>
	{
		public PaymentPayeeMap()
		{
			// Primary Key
			 this.HasKey(t => t.PayeeID);
		/// <summary>
		/// �տ����ʺ�
		/// <summary>
			  this.Property(t => t.Account)

			.HasMaxLength(50);
		/// <summary>
		/// �տ�������
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(50);
		/// <summary>
		/// �տ����������д���
		/// <summary>
			  this.Property(t => t.BranchCode)

			.HasMaxLength(50);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.Branch)

			.HasMaxLength(50);
		/// <summary>
		/// ����������
		/// <summary>
			  this.Property(t => t.OpenBank)

			.HasMaxLength(50);
		/// <summary>
		/// ������������
		/// <summary>
			  this.Property(t => t.Bank)

			.HasMaxLength(50);
		/// <summary>
		/// �������д���
		/// <summary>
			  this.Property(t => t.BankCode)

			.HasMaxLength(50);
		/// <summary>
		/// ���ش���
		/// <summary>
			  this.Property(t => t.AddressCode)

			.HasMaxLength(50);
		/// <summary>
		/// �������
		/// <summary>
			  this.Property(t => t.AreaCode)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPayee"); 
			// �տ����ʺ�ID
			this.Property(t => t.PayeeID).HasColumnName("PayeeID"); 
			// �տ����ʺ�
			this.Property(t => t.Account).HasColumnName("Account"); 
			// �տ�������
			this.Property(t => t.Name).HasColumnName("Name"); 
			// �տ����������д���
			this.Property(t => t.BranchCode).HasColumnName("BranchCode"); 
			// ������
			this.Property(t => t.Branch).HasColumnName("Branch"); 
			// ����������
			this.Property(t => t.OpenBank).HasColumnName("OpenBank"); 
			// ������������
			this.Property(t => t.Bank).HasColumnName("Bank"); 
			// �������д���
			this.Property(t => t.BankCode).HasColumnName("BankCode"); 
			// ���ش���
			this.Property(t => t.AddressCode).HasColumnName("AddressCode"); 
			// �������
			this.Property(t => t.AreaCode).HasColumnName("AreaCode"); 
			// ��Ӫ��ID
			this.Property(t => t.HallID).HasColumnName("HallID"); 
		 }
	 }
}
