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
		/// 标题
		/// <summary>
			  this.Property(t => t.ApplyTitle)

			.HasMaxLength(200);
			  this.Property(t => t.ApplySn)

			.HasMaxLength(50);
		/// <summary>
		/// 联系号码
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(20);
		/// <summary>
		/// 工单状态
		/// <summary>
			  this.Property(t => t.Status)

			.HasMaxLength(50);
		/// <summary>
		/// 序列号
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPaymentApply"); 
			// 主键
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 标题
			this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle"); 
			// 流程实例Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			this.Property(t => t.ApplySn).HasColumnName("ApplySn"); 
			// 联系号码
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 创建时间、申请时间
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// 代垫日期
			this.Property(t => t.PayDay).HasColumnName("PayDay"); 
			// 代垫公司ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			// 工单状态
			this.Property(t => t.Status).HasColumnName("Status"); 
			// 序列号
			this.Property(t => t.Series).HasColumnName("Series"); 
			// 提交时间
			this.Property(t => t.SubmitTime).HasColumnName("SubmitTime"); 
		 }
	 }
}
