using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationAnnualLeaveMap : EntityTypeConfiguration<HRVacationAnnualLeave>
	{
        public HRVacationAnnualLeaveMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.YearDate)

          .HasMaxLength(50);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(1073741823);
            this.Property(t => t.Toflag)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRVacationAnnualLeave");
            // 主键ID
            this.Property(t => t.ID).HasColumnName("ID");
            // 用户ID
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.ComID).HasColumnName("ComID");
            this.Property(t => t.YearDate).HasColumnName("YearDate");
            this.Property(t => t.AnnualLeave).HasColumnName("AnnualLeave");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            // 创建者ID
            this.Property(t => t.CreateID).HasColumnName("CreateID");
            // 使用日期
            this.Property(t => t.UseDate).HasColumnName("UseDate");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            //申请单ID
            this.Property(t => t.AppID).HasColumnName("AppID");
            // 年休假天数
            this.Property(t => t.SpendDays).HasColumnName("SpendDays");
            // 开始时间
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // 结束时间
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.Toflag).HasColumnName("Toflag");
        }

    }
}
