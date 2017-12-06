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
		/// �ֻ�����
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(50);
		/// <summary>
		/// �˷�����
		/// <summary>
			  this.Property(t => t.Range)

			.HasMaxLength(50);
		/// <summary>
		/// �˷ѷ�ʽ
		/// <summary>
			  this.Property(t => t.Type)

			.HasMaxLength(50);
		/// <summary>
		/// �˷�ԭ��
		/// <summary>
			  this.Property(t => t.Causation)

			.HasMaxLength(50);
		/// <summary>
		/// ���˵��
		/// <summary>
			  this.Property(t => t.Explain)

			.HasMaxLength(2147483647);
		/// <summary>
		/// �·�
		/// <summary>
			  this.Property(t => t.Month)

			.HasMaxLength(50);
		/// <summary>
		/// �Ǽ�����
		/// <summary>
			  this.Property(t => t.Channel)

			.HasMaxLength(50);
		/// <summary>
		/// SP�˿ں�
		/// <summary>
			  this.Property(t => t.SpPort)

			.HasMaxLength(50);
		/// <summary>
		/// �˷���ϸԭ��
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(2147483647);
			  this.Property(t => t.Series)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("ReturnsPremium"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �ֻ�����
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// �˷ѽ��
			this.Property(t => t.Money).HasColumnName("Money"); 
			// �˷�����
			this.Property(t => t.Range).HasColumnName("Range"); 
			// �˷ѷ�ʽ
			this.Property(t => t.Type).HasColumnName("Type"); 
			// �˷�ԭ��
			this.Property(t => t.Causation).HasColumnName("Causation"); 
			// ���˵��
			this.Property(t => t.Explain).HasColumnName("Explain"); 
			// �·�
			this.Property(t => t.Month).HasColumnName("Month"); 
			// ����
			this.Property(t => t.Date).HasColumnName("Date"); 
			// �Ǽ�����
			this.Property(t => t.Channel).HasColumnName("Channel"); 
			// SP�˿ں�
			this.Property(t => t.SpPort).HasColumnName("SpPort"); 
			// �˷���ϸԭ��
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			this.Property(t => t.Series).HasColumnName("Series"); 
		 }
	 }
}
