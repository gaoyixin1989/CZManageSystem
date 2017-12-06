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
		/// 付款人帐号
		/// <summary>
			  this.Property(t => t.PayerAccount)

			.HasMaxLength(50);
		/// <summary>
		/// 付款人名称
		/// <summary>
			  this.Property(t => t.PayerName)

			.HasMaxLength(50);
		/// <summary>
		/// 付款人分行代码
		/// <summary>
			  this.Property(t => t.PayerBranchCode)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人帐号
		/// <summary>
			  this.Property(t => t.PayeeAccount)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人帐号名称
		/// <summary>
			  this.Property(t => t.PayeeName)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人所属分行
		/// <summary>
			  this.Property(t => t.PayeeBranch)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人分行代码
		/// <summary>
			  this.Property(t => t.PayeeBranchCode)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人开户行
		/// <summary>
			  this.Property(t => t.PayeeOpenBank)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人所属银行
		/// <summary>
			  this.Property(t => t.PayeeBank)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人银行代码
		/// <summary>
			  this.Property(t => t.PayeeBankCode)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人属地代码
		/// <summary>
			  this.Property(t => t.PayeeAddressCode)

			.HasMaxLength(50);
		/// <summary>
		/// 币别
		/// <summary>
			  this.Property(t => t.MoneyType)

			.HasMaxLength(50);
		/// <summary>
		/// 目的
		/// <summary>
			  this.Property(t => t.Purpose)

			.HasMaxLength(100);
		/// <summary>
		/// 收款人区域代码
		/// <summary>
			  this.Property(t => t.PayeeAreaCode)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPayMoney"); 
			// 编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 工单ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// 付款人帐号
			this.Property(t => t.PayerAccount).HasColumnName("PayerAccount"); 
			// 付款人名称
			this.Property(t => t.PayerName).HasColumnName("PayerName"); 
			// 付款人分行代码
			this.Property(t => t.PayerBranchCode).HasColumnName("PayerBranchCode"); 
			// 收款人帐号
			this.Property(t => t.PayeeAccount).HasColumnName("PayeeAccount"); 
			// 收款人帐号名称
			this.Property(t => t.PayeeName).HasColumnName("PayeeName"); 
			// 收款人所属分行
			this.Property(t => t.PayeeBranch).HasColumnName("PayeeBranch"); 
			// 收款人分行代码
			this.Property(t => t.PayeeBranchCode).HasColumnName("PayeeBranchCode"); 
			// 收款人开户行
			this.Property(t => t.PayeeOpenBank).HasColumnName("PayeeOpenBank"); 
			// 收款人所属银行
			this.Property(t => t.PayeeBank).HasColumnName("PayeeBank"); 
			// 收款人银行代码
			this.Property(t => t.PayeeBankCode).HasColumnName("PayeeBankCode"); 
			// 收款人属地代码
			this.Property(t => t.PayeeAddressCode).HasColumnName("PayeeAddressCode"); 
			// 金额
			this.Property(t => t.Money).HasColumnName("Money"); 
			// 币别
			this.Property(t => t.MoneyType).HasColumnName("MoneyType"); 
			// 目的
			this.Property(t => t.Purpose).HasColumnName("Purpose"); 
			// 收款人区域代码
			this.Property(t => t.PayeeAreaCode).HasColumnName("PayeeAreaCode"); 
		 }
	 }
}
