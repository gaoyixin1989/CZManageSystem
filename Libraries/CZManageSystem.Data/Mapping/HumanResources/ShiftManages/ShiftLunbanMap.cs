using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// 轮班信息
    /// </summary>
	public class ShiftLunbanMap : EntityTypeConfiguration<ShiftLunban>
    {
        public ShiftLunbanMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 开始日
            /// <summary>
            this.Property(t => t.StartDay)

          .HasMaxLength(50);
            /// <summary>
            /// 结束日
            /// <summary>
            this.Property(t => t.EndDay)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("ShiftLunban");
            // 轮班ID
            this.Property(t => t.Id).HasColumnName("Id");
            // 排班信息表ID
            this.Property(t => t.ZhibanId).HasColumnName("ZhibanId");
            // 开始日
            this.Property(t => t.StartDay).HasColumnName("StartDay");
            // 结束日
            this.Property(t => t.EndDay).HasColumnName("EndDay");

            // Relationships
            this.HasMany(t => t.ShiftLbusers).WithOptional().HasForeignKey(d => d.LunbanId);
        }
    }
}
