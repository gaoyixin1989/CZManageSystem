using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentPaymentApplySubMap : EntityTypeConfiguration<PaymentPaymentApplySub>
	{
		public PaymentPaymentApplySubMap()
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
		/// ���к�
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(50);
		/// <summary>
		/// ״̬
		/// <summary>
			  this.Property(t => t.Status)

			.HasMaxLength(50);
		/// <summary>
		/// ��Ӫ��ID
		/// <summary>
			  this.Property(t => t.HallID)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPaymentApplySub"); 
			// ����
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ����
			this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle"); 
			// ����ʵ��Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			this.Property(t => t.ApplySn).HasColumnName("ApplySn"); 
			// ��ϵ����
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// ������ID
			this.Property(t => t.MainApplyID).HasColumnName("MainApplyID"); 
			// ��������
			this.Property(t => t.PayDay).HasColumnName("PayDay"); 
			// ���к�
			this.Property(t => t.Series).HasColumnName("Series"); 
			// ������ID
			this.Property(t => t.AppliCant).HasColumnName("AppliCant"); 
			// ����ʱ��
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			// ״̬
			this.Property(t => t.Status).HasColumnName("Status"); 
			// ��Ӫ��ID
			this.Property(t => t.HallID).HasColumnName("HallID"); 
			// ���湫˾
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
