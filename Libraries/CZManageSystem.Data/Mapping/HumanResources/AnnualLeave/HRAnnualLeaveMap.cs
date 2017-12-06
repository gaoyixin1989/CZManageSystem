using CZManageSystem.Data.Domain.HumanResources.AnnualLeave;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.HumanResources.AnnualLeave
{
   public class HRAnnualLeaveMap : EntityTypeConfiguration<HRAnnualLeave>
    {
        public  HRAnnualLeaveMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 姓名
            /// <summary>
            this.Property(t => t.UserName)

          .HasMaxLength(200);
            /// <summary>
            /// 年度
            /// <summary>
            this.Property(t => t.VYears)

          .HasMaxLength(200);
            /// <summary>
            /// 年休假天数
            /// <summary>
            this.Property(t => t.VDays)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRAnnualLeave");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // 姓名
            this.Property(t => t.UserName).HasColumnName("UserName");
            // 年度
            this.Property(t => t.VYears).HasColumnName("VYears");
            // 年休假天数
            this.Property(t => t.VDays).HasColumnName("VDays");
            // 上年度法定年休假剩余天数
            this.Property(t => t.FdLastYearVDays).HasColumnName("FdLastYearVDays");
            // 本年度法定年休假天数
            this.Property(t => t.FdYearVDays).HasColumnName("FdYearVDays");
            // 本年度补充年休假天数
            this.Property(t => t.BcYearVDays).HasColumnName("BcYearVDays");


        }
    }

}
