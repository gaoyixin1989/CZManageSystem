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
		/// 序列号
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(50);
		/// <summary>
		/// 状态
		/// <summary>
			  this.Property(t => t.Status)

			.HasMaxLength(50);
		/// <summary>
		/// 服营厅ID
		/// <summary>
			  this.Property(t => t.HallID)

			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPaymentApplySub"); 
			// 主键
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 标题
			this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle"); 
			// 流程实例Id
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			this.Property(t => t.ApplySn).HasColumnName("ApplySn"); 
			// 联系号码
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 主工单ID
			this.Property(t => t.MainApplyID).HasColumnName("MainApplyID"); 
			// 代垫日期
			this.Property(t => t.PayDay).HasColumnName("PayDay"); 
			// 序列号
			this.Property(t => t.Series).HasColumnName("Series"); 
			// 申请者ID
			this.Property(t => t.AppliCant).HasColumnName("AppliCant"); 
			// 申请时间
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			// 状态
			this.Property(t => t.Status).HasColumnName("Status"); 
			// 服营厅ID
			this.Property(t => t.HallID).HasColumnName("HallID"); 
			// 代垫公司
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
		 }
	 }
}
