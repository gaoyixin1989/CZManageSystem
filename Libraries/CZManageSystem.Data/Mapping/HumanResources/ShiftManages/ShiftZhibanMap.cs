using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// �Ű���Ϣ��
    /// </summary>
	public class ShiftZhibanMap : EntityTypeConfiguration<ShiftZhiban>
    {
        public ShiftZhibanMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// ����ID
            /// <summary>
            this.Property(t => t.DeptId)

          .HasMaxLength(50);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.Title).HasMaxLength(100);
            /// <summary>
            /// ��
            /// <summary>
            this.Property(t => t.Year)

          .HasMaxLength(50);
            /// <summary>
            /// ��
            /// <summary>
            this.Property(t => t.Month)

          .HasMaxLength(50);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            /// <summary>
            /// ״̬��0-δ�ύ��1-�ύ
            /// <summary>
            this.Property(t => t.State)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("ShiftZhiban");
            // �Ű�ID
            this.Property(t => t.Id).HasColumnName("Id");
            //����
            this.Property(t => t.Title).HasColumnName("Title");
            // �༭��
            this.Property(t => t.Editor).HasColumnName("Editor");
            // �༭ʱ��
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // ����ID
            this.Property(t => t.DeptId).HasColumnName("DeptId");
            // ��
            this.Property(t => t.Year).HasColumnName("Year");
            // ��
            this.Property(t => t.Month).HasColumnName("Month");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            // ״̬��0-δ�ύ��1-�ύ
            this.Property(t => t.State).HasColumnName("State");

            // Relationships
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);
            this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.DeptId);
            this.HasMany(t => t.ShiftBancis).WithOptional().HasForeignKey(d => d.ZhibanId);
            this.HasMany(t => t.ShiftLunbans).WithOptional().HasForeignKey(d => d.ZhibanId);

            
        }
    }
}
