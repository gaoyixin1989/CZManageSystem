
using CZManageSystem.Data.Domain.ITSupport;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class Consumable_InputListMap : EntityTypeConfiguration<Consumable_InputList>
	{
		public Consumable_InputListMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.Title)
			.HasMaxLength(200);
		/// <summary>
		/// ��ⵥ��
		/// <summary>
			  this.Property(t => t.Code)
			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)
			.HasMaxLength(200);
			// Table & Column Mappings
 			 this.ToTable("Consumable_InputList"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ����
			this.Property(t => t.Title).HasColumnName("Title"); 
			// ��ⵥ��
			this.Property(t => t.Code).HasColumnName("Code"); 
			// ����ʱ��
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// ���ʱ��
			this.Property(t => t.InputTime).HasColumnName("InputTime"); 
			// ������
			this.Property(t => t.Operator).HasColumnName("Operator");
            // ������
            this.Property(t => t.SumbitUser).HasColumnName("SumbitUser");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            // ��ⵥ״̬��0-���棬1-�ύ���
            this.Property(t => t.State).HasColumnName("State");
        }
	 }
}
