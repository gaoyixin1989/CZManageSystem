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
		/// 收款人帐号
		/// <summary>
			  this.Property(t => t.Account)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人名称
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(50);
		/// <summary>
		/// 收款人所属分行代码
		/// <summary>
			  this.Property(t => t.BranchCode)

			.HasMaxLength(50);
		/// <summary>
		/// 开户行
		/// <summary>
			  this.Property(t => t.Branch)

			.HasMaxLength(50);
		/// <summary>
		/// 开户行名称
		/// <summary>
			  this.Property(t => t.OpenBank)

			.HasMaxLength(50);
		/// <summary>
		/// 所属银行名称
		/// <summary>
			  this.Property(t => t.Bank)

			.HasMaxLength(50);
		/// <summary>
		/// 所属银行代码
		/// <summary>
			  this.Property(t => t.BankCode)

			.HasMaxLength(50);
		/// <summary>
		/// 属地代码
		/// <summary>
			  this.Property(t => t.AddressCode)

			.HasMaxLength(50);
		/// <summary>
		/// 区域代码
		/// <summary>
			  this.Property(t => t.AreaCode)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPayee"); 
			// 收款人帐号ID
			this.Property(t => t.PayeeID).HasColumnName("PayeeID"); 
			// 收款人帐号
			this.Property(t => t.Account).HasColumnName("Account"); 
			// 收款人名称
			this.Property(t => t.Name).HasColumnName("Name"); 
			// 收款人所属分行代码
			this.Property(t => t.BranchCode).HasColumnName("BranchCode"); 
			// 开户行
			this.Property(t => t.Branch).HasColumnName("Branch"); 
			// 开户行名称
			this.Property(t => t.OpenBank).HasColumnName("OpenBank"); 
			// 所属银行名称
			this.Property(t => t.Bank).HasColumnName("Bank"); 
			// 所属银行代码
			this.Property(t => t.BankCode).HasColumnName("BankCode"); 
			// 属地代码
			this.Property(t => t.AddressCode).HasColumnName("AddressCode"); 
			// 区域代码
			this.Property(t => t.AreaCode).HasColumnName("AreaCode"); 
			// 服营厅ID
			this.Property(t => t.HallID).HasColumnName("HallID"); 
		 }
	 }
}
