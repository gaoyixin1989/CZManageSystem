using CZManageSystem.Data.Domain.CollaborationCenter.SmsManager;
using CZManageSystem.Data.Domain.SysManger;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 短信发送表
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.SmsManager
{
	public class SendSmsMap : EntityTypeConfiguration<SendSms>
	{
		public SendSmsMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 手机号码
		/// <summary>
			  this.Property(t => t.Mobile)
;
		/// <summary>
		/// 短信内容
		/// <summary>
			  this.Property(t => t.Context)
;
		/// <summary>
		/// 是否显示名称
		/// <summary>
			  this.Property(t => t.ShowName)

			.HasMaxLength(50);
            /// <summary>
            /// 发送部门id
            /// <summary>
            this.Property(t => t.Dept).HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("SendSms"); 
			// id
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 手机号码
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 短信内容
			this.Property(t => t.Context).HasColumnName("Context"); 
			// 发送时间
			this.Property(t => t.Time).HasColumnName("Time"); 
			// 发送人
			this.Property(t => t.Sender).HasColumnName("Sender"); 
			// 是否出错
			this.Property(t => t.Error).HasColumnName("Error"); 
			// 计数
			this.Property(t => t.Count).HasColumnName("Count"); 
			// 发送日期
			this.Property(t => t.Date).HasColumnName("Date"); 
			// 是否显示名称
			this.Property(t => t.ShowName).HasColumnName("ShowName");
            // 发送部门id
            this.Property(t => t.Dept).HasColumnName("Dept");

            //外键
            this.HasOptional(t => t.SenderObj).WithMany().HasForeignKey(d => d.Sender);
            this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.Dept);

        }
	 }
}
