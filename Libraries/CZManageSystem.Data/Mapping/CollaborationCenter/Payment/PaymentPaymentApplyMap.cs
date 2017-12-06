using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentPaymentApplyMap : EntityTypeConfiguration<PaymentPaymentApply>
	{
		public PaymentPaymentApplyMap()
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
		/// ��ϵ����
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(20);
		/// <summary>
		/// ����״̬
		/// <summary>
			  this.Property(t => t.Status)

			.HasMaxLength(50);
		/// <summary>
		/// ���к�
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPaymentApply"); 
			// ����
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ����
			this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle"); 
			// ����ʵ��Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			this.Property(t => t.ApplySn).HasColumnName("ApplySn"); 
			// ��ϵ����
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// ����ʱ�䡢����ʱ��
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// ��������
			this.Property(t => t.PayDay).HasColumnName("PayDay"); 
			// ���湫˾ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			// ����״̬
			this.Property(t => t.Status).HasColumnName("Status"); 
			// ���к�
			this.Property(t => t.Series).HasColumnName("Series"); 
			// �ύʱ��
			this.Property(t => t.SubmitTime).HasColumnName("SubmitTime"); 
		 }
	 }
}
