using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
    public class HRVacationApplyMap : EntityTypeConfiguration<HRVacationApply>
    {
        public HRVacationApplyMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplyId);
            /// <summary>
            /// 流程单号
            /// <summary>
            this.Property(t => t.ApplySn)

          .HasMaxLength(50);
            /// <summary>
            /// 标题
            /// <summary>
            this.Property(t => t.ApplyTitle)

          .HasMaxLength(150);
            /// <summary>
            /// 休假类型
            /// <summary>
            this.Property(t => t.VacationType)

          .HasMaxLength(50);
            /// <summary>
            /// 公假类型
            /// <summary>
            this.Property(t => t.VacationClass)

          .HasMaxLength(50);
            /// <summary>
            /// 异常休假原因
            /// <summary>
            this.Property(t => t.Reason)

          .HasMaxLength(200);
            /// <summary>
            /// 已销假天数
            /// <summary>
            this.Property(t => t.CancelDays)

           .HasPrecision(18, 1);
            this.Property(t => t.Newpt)

          .HasMaxLength(100);
            this.Property(t => t.Newst)

          .HasMaxLength(100);
            this.Property(t => t.Newet)

          .HasMaxLength(100);
            /// <summary>
            /// 外出地点
            /// <summary>
            this.Property(t => t.OutAddress)

          .HasMaxLength(200);
            /// <summary>
            /// 加班时间
            /// <summary>
            this.Property(t => t.OverTime)

          .HasMaxLength(128);
            /// <summary>
            /// 附件IDs
            /// <summary>
            this.Property(t => t.Attids)
;
            // Table & Column Mappings
            this.ToTable("HRVacationApply");
            // 主键
            this.Property(t => t.ApplyId).HasColumnName("ApplyId");
            // 流程实例Id
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            // 流程单号
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // 标题
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            // 编辑者
            this.Property(t => t.Editor).HasColumnName("Editor");
            // 编辑时间、申请时间
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // 休假类型
            this.Property(t => t.VacationType).HasColumnName("VacationType");
            // 公假类型
            this.Property(t => t.VacationClass).HasColumnName("VacationClass");
            // 开始时间
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // 结束时间
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            // 天数
            this.Property(t => t.PeriodTime).HasColumnName("PeriodTime");
            // 异常休假原因
            this.Property(t => t.Reason).HasColumnName("Reason");
            // 销假
            this.Property(t => t.CancelVacation).HasColumnName("CancelVacation");
            // 已销假天数
            this.Property(t => t.CancelDays).HasColumnName("CancelDays");
            this.Property(t => t.Newpt).HasColumnName("Newpt");
            this.Property(t => t.Newst).HasColumnName("Newst");
            this.Property(t => t.Newet).HasColumnName("Newet");
            // 外出地点
            this.Property(t => t.OutAddress).HasColumnName("OutAddress");
            // 加班时间
            this.Property(t => t.OverTime).HasColumnName("OverTime");
            // 附件IDs
            this.Property(t => t.Attids).HasColumnName("Attids");
            // 状态：0未提交，1已提交
            this.Property(t => t.State).HasColumnName("State");

            //外键
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.HRVacationApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);

            this.HasMany(t => t.Meetings).WithOptional().HasForeignKey(d => d.VacationID);
            this.HasMany(t => t.Courses).WithOptional().HasForeignKey(d => d.VacationID);
            this.HasMany(t => t.Teachings).WithOptional().HasForeignKey(d => d.VacationID);
            this.HasMany(t => t.Others).WithOptional().HasForeignKey(d => d.VacationID);

        }
    }
}
