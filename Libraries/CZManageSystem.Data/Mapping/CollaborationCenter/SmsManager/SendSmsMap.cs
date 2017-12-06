using CZManageSystem.Data.Domain.CollaborationCenter.SmsManager;
using CZManageSystem.Data.Domain.SysManger;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ���ŷ��ͱ�
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
		/// �ֻ�����
		/// <summary>
			  this.Property(t => t.Mobile)
;
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.Context)
;
		/// <summary>
		/// �Ƿ���ʾ����
		/// <summary>
			  this.Property(t => t.ShowName)

			.HasMaxLength(50);
            /// <summary>
            /// ���Ͳ���id
            /// <summary>
            this.Property(t => t.Dept).HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("SendSms"); 
			// id
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �ֻ�����
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// ��������
			this.Property(t => t.Context).HasColumnName("Context"); 
			// ����ʱ��
			this.Property(t => t.Time).HasColumnName("Time"); 
			// ������
			this.Property(t => t.Sender).HasColumnName("Sender"); 
			// �Ƿ����
			this.Property(t => t.Error).HasColumnName("Error"); 
			// ����
			this.Property(t => t.Count).HasColumnName("Count"); 
			// ��������
			this.Property(t => t.Date).HasColumnName("Date"); 
			// �Ƿ���ʾ����
			this.Property(t => t.ShowName).HasColumnName("ShowName");
            // ���Ͳ���id
            this.Property(t => t.Dept).HasColumnName("Dept");

            //���
            this.HasOptional(t => t.SenderObj).WithMany().HasForeignKey(d => d.Sender);
            this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.Dept);

        }
	 }
}
