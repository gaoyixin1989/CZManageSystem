using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentLoginLogMap : EntityTypeConfiguration<PaymentLoginLog>
	{
		public PaymentLoginLogMap()
		{
			// Primary Key
			 this.HasKey(t => t.LogID);
		/// <summary>
		/// 登录ID
		/// <summary>
			  this.Property(t => t.LoginID)

			.HasMaxLength(50);
		/// <summary>
		/// 姓名
		/// <summary>
			  this.Property(t => t.LoginName)

			.HasMaxLength(50);
		/// <summary>
		/// 公司名称
		/// <summary>
			  this.Property(t => t.CompanyName)

			.HasMaxLength(50);
		/// <summary>
		/// 登录IP
		/// <summary>
			  this.Property(t => t.LoginIP)

			.HasMaxLength(50);
			  this.Property(t => t.Result)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentLoginLog"); 
			// 日志ID
			this.Property(t => t.LogID).HasColumnName("LogID"); 
			// 登录时间
			this.Property(t => t.LoginTime).HasColumnName("LoginTime"); 
			// 登录ID
			this.Property(t => t.LoginID).HasColumnName("LoginID"); 
			// 姓名
			this.Property(t => t.LoginName).HasColumnName("LoginName"); 
			// 公司名称
			this.Property(t => t.CompanyName).HasColumnName("CompanyName"); 
			// 公司ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			// 登录IP
			this.Property(t => t.LoginIP).HasColumnName("LoginIP"); 
			this.Property(t => t.Result).HasColumnName("Result"); 
		 }
	 }
}
