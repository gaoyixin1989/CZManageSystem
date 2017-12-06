
using System.Data.Entity.ModelConfiguration;
using CZManageSystem.Data.Domain.ITSupport;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class ConsumableMap : EntityTypeConfiguration<Consumable>
	{
		public ConsumableMap()
		{
            // Primary Key
            this.HasKey(t=>t.ID);
            this.Property(t => t.ID);
		/// <summary>
		/// �Ĳ����
		/// <summary>
			  this.Property(t => t.Type)
			.HasMaxLength(100);
		/// <summary>
		/// �Ĳ��ͺ�
		/// <summary>
			  this.Property(t => t.Model)
			.HasMaxLength(100);
		/// <summary>
		/// �Ĳ�����
		/// <summary>
			  this.Property(t => t.Name)
			.HasMaxLength(100);
		/// <summary>
		/// �����豸
		/// <summary>
			  this.Property(t => t.Equipment)
			.HasMaxLength(100);
		/// <summary>
		/// ��λ
		/// <summary>
			  this.Property(t => t.Unit)
			.HasMaxLength(100);
		/// <summary>
		/// �Ĳ�Ʒ��
		/// <summary>
			  this.Property(t => t.Trademark)
			.HasMaxLength(100);
            /// <summary>
            /// ��ֵ���ͣ�0�ͼ�ֵ��1�ǵͼ�ֵ
            /// <summary>
            this.Property(t => t.IsValue).HasMaxLength(100);

            //��ע
            this.Property(t => t.Remark).HasMaxLength(100);
            //�Ƿ�ɾ��  0�����  1������
            this.Property(t => t.IsDelete).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Consumable"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �Ĳ����
			this.Property(t => t.Type).HasColumnName("Type"); 
			// �Ĳ��ͺ�
			this.Property(t => t.Model).HasColumnName("Model"); 
			// �Ĳ�����
			this.Property(t => t.Name).HasColumnName("Name"); 
			// �����豸
			this.Property(t => t.Equipment).HasColumnName("Equipment"); 
			// ��λ
			this.Property(t => t.Unit).HasColumnName("Unit"); 
			// �Ĳ�Ʒ��
			this.Property(t => t.Trademark).HasColumnName("Trademark"); 
			// �Ƿ��ֵ
			this.Property(t => t.IsValue).HasColumnName("IsValue");
            // �Ĳĵ�ǰӵ����
            this.Property(t => t.Amount).HasColumnName("Amount");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            //�Ƿ�ɾ��  0�����  1������
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
        }
	 }
}
