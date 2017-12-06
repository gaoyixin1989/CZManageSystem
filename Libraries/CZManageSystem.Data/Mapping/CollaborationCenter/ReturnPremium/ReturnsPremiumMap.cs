using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ReturnsPremiumMap : EntityTypeConfiguration<ReturnsPremium>
	{
		public ReturnsPremiumMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 手机号码
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(50);
		/// <summary>
		/// 退费区间
		/// <summary>
			  this.Property(t => t.Range)

			.HasMaxLength(50);
		/// <summary>
		/// 退费方式
		/// <summary>
			  this.Property(t => t.Type)

			.HasMaxLength(50);
		/// <summary>
		/// 退费原因
		/// <summary>
			  this.Property(t => t.Causation)

			.HasMaxLength(50);
		/// <summary>
		/// 情况说明
		/// <summary>
			  this.Property(t => t.Explain)

			.HasMaxLength(2147483647);
		/// <summary>
		/// 月份
		/// <summary>
			  this.Property(t => t.Month)

			.HasMaxLength(50);
		/// <summary>
		/// 登记渠道
		/// <summary>
			  this.Property(t => t.Channel)

			.HasMaxLength(50);
		/// <summary>
		/// SP端口号
		/// <summary>
			  this.Property(t => t.SpPort)

			.HasMaxLength(50);
		/// <summary>
		/// 退费详细原因
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			  this.Property(t => t.Series)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("ReturnsPremium"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 手机号码
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 退费金额
			this.Property(t => t.Money).HasColumnName("Money"); 
			// 退费区间
			this.Property(t => t.Range).HasColumnName("Range"); 
			// 退费方式
			this.Property(t => t.Type).HasColumnName("Type"); 
			// 退费原因
			this.Property(t => t.Causation).HasColumnName("Causation"); 
			// 情况说明
			this.Property(t => t.Explain).HasColumnName("Explain"); 
			// 月份
			this.Property(t => t.Month).HasColumnName("Month"); 
			// 日期
			this.Property(t => t.Date).HasColumnName("Date"); 
			// 登记渠道
			this.Property(t => t.Channel).HasColumnName("Channel"); 
			// SP端口号
			this.Property(t => t.SpPort).HasColumnName("SpPort"); 
			// 退费详细原因
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			this.Property(t => t.Series).HasColumnName("Series"); 
		 }
	 }
}
