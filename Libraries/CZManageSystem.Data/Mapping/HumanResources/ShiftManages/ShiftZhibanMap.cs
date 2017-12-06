using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// 排班信息表
    /// </summary>
	public class ShiftZhibanMap : EntityTypeConfiguration<ShiftZhiban>
    {
        public ShiftZhibanMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 部门ID
            /// <summary>
            this.Property(t => t.DeptId)

          .HasMaxLength(50);
            /// <summary>
            /// 标题
            /// <summary>
            this.Property(t => t.Title).HasMaxLength(100);
            /// <summary>
            /// 年
            /// <summary>
            this.Property(t => t.Year)

          .HasMaxLength(50);
            /// <summary>
            /// 月
            /// <summary>
            this.Property(t => t.Month)

          .HasMaxLength(50);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            /// <summary>
            /// 状态：0-未提交，1-提交
            /// <summary>
            this.Property(t => t.State)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("ShiftZhiban");
            // 排班ID
            this.Property(t => t.Id).HasColumnName("Id");
            //标题
            this.Property(t => t.Title).HasColumnName("Title");
            // 编辑人
            this.Property(t => t.Editor).HasColumnName("Editor");
            // 编辑时间
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // 部门ID
            this.Property(t => t.DeptId).HasColumnName("DeptId");
            // 年
            this.Property(t => t.Year).HasColumnName("Year");
            // 月
            this.Property(t => t.Month).HasColumnName("Month");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            // 状态：0-未提交，1-提交
            this.Property(t => t.State).HasColumnName("State");

            // Relationships
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);
            this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.DeptId);
            this.HasMany(t => t.ShiftBancis).WithOptional().HasForeignKey(d => d.ZhibanId);
            this.HasMany(t => t.ShiftLunbans).WithOptional().HasForeignKey(d => d.ZhibanId);

            
        }
    }
}
