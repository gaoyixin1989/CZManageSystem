using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
    public class HRLzUserInfoMap : EntityTypeConfiguration<HRLzUserInfo>
    {
        public HRLzUserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.EmployeeId);
            /// <summary>
            /// 职位职级
            /// <summary>
            this.Property(t => t.PositionRank)

          .HasMaxLength(50);
            /// <summary>
            /// 套入职级
            /// <summary>
            this.Property(t => t.SetIntoTheRanks)

          .HasMaxLength(50);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(50);
            /// <summary>
            /// 修改人
            /// <summary>
            this.Property(t => t.LastModFier)

          .HasMaxLength(50);
            /// <summary>
            /// 档位
            /// <summary>
            this.Property(t => t.Gears)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRLzUserInfo");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // 职位职级
            this.Property(t => t.PositionRank).HasColumnName("PositionRank");
            // 套入职级
            this.Property(t => t.SetIntoTheRanks).HasColumnName("SetIntoTheRanks");
            // 分位值
            this.Property(t => t.Tantile).HasColumnName("Tantile");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            // 更新时间
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
            // 修改人
            this.Property(t => t.LastModFier).HasColumnName("LastModFier");
            // 档位
            this.Property(t => t.Gears).HasColumnName("Gears");
            this.HasOptional(t => t.Users).WithMany ().HasForeignKey(t=>t.UserId );

        }
    }
}
