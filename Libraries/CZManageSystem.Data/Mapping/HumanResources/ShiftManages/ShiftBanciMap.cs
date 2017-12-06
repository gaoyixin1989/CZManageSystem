using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// 班次信息
    /// </summary>
	public class ShiftBanciMap : EntityTypeConfiguration<ShiftBanci>
    {
        public ShiftBanciMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 班次名称
            /// <summary>
            this.Property(t => t.BcName)

          .HasMaxLength(50);
            /// <summary>
            /// 开始小时值
            /// <summary>
            this.Property(t => t.StartHour)

          .HasMaxLength(50);
            /// <summary>
            /// 开始分钟值
            /// <summary>
            this.Property(t => t.StartMinute)

          .HasMaxLength(50);
            /// <summary>
            /// 结束小时值
            /// <summary>
            this.Property(t => t.EndHour)

          .HasMaxLength(50);
            /// <summary>
            /// 结束分钟值
            /// <summary>
            this.Property(t => t.EndMinute)

          .HasMaxLength(50);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("ShiftBanci");
            // 班次ID
            this.Property(t => t.Id).HasColumnName("Id");
            // 编辑人
            this.Property(t => t.Editor).HasColumnName("Editor");
            // 编辑时间
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // 排班信息表Id
            this.Property(t => t.ZhibanId).HasColumnName("ZhibanId");
            // 班次名称
            this.Property(t => t.BcName).HasColumnName("BcName");
            // 开始小时值
            this.Property(t => t.StartHour).HasColumnName("StartHour");
            // 开始分钟值
            this.Property(t => t.StartMinute).HasColumnName("StartMinute");
            // 结束小时值
            this.Property(t => t.EndHour).HasColumnName("EndHour");
            // 结束分钟值
            this.Property(t => t.EndMinute).HasColumnName("EndMinute");
            // 值班人数
            this.Property(t => t.StaffNum).HasColumnName("StaffNum");
            // 班次排序
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");

            // Relationships
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);
            this.HasMany(t => t.ShiftRichs).WithOptional().HasForeignKey(d => d.BanciId);
        }
    }
}
