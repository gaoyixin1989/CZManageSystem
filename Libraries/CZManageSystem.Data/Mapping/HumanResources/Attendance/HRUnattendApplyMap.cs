using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
    public class HRUnattendApplyMap : EntityTypeConfiguration<HRUnattendApply>
    {
        public HRUnattendApplyMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplyID);
            /// <summary>
            /// 标题
            /// <summary>
            this.Property(t => t.ApplyTitle)

          .HasMaxLength(200);
            this.Property(t => t.ApplySn)

          .HasMaxLength(50);
            /// <summary>
            /// 申请人
            /// <summary>
            this.Property(t => t.ApplyUserName)

          .HasMaxLength(100);
            /// <summary>
            /// 申请单位
            /// <summary>
            this.Property(t => t.ApplyUnit)

          .HasMaxLength(200);
            /// <summary>
            /// 所属部门
            /// <summary>
            this.Property(t => t.ApplyDept)

          .HasMaxLength(200);
            /// <summary>
            /// 联系号码
            /// <summary>
            this.Property(t => t.Mobile)

          .HasMaxLength(20);
            /// <summary>
            /// 申请原因
            /// <summary>
            this.Property(t => t.Reason)

          .HasMaxLength(200);
            /// <summary>
            /// 异常记录
            /// <summary>
            //this.Property(t => t.RecordContent).HasMaxLength(250);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            /// <summary>
            /// 附件IDs
            /// <summary>
            this.Property(t => t.AccessoryIds)
;
            /// <summary>
            /// 职位？
            /// <summary>
            this.Property(t => t.UnattendPost)

          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("HRUnattendApply");
            // 主键
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            // 标题
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            // 流程实例Id
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            // 申请人
            this.Property(t => t.ApplyUserName).HasColumnName("ApplyUserName");
            // 开始时间
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // 结束时间
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            // 天数
            this.Property(t => t.PeriodTime).HasColumnName("PeriodTime");
            // 创建时间、申请时间
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            // 申请单位
            this.Property(t => t.ApplyUnit).HasColumnName("ApplyUnit");
            // 所属部门
            this.Property(t => t.ApplyDept).HasColumnName("ApplyDept");
            // 联系号码
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            // 申请原因
            this.Property(t => t.Reason).HasColumnName("Reason");
            // 异常记录
            this.Property(t => t.RecordContent).HasColumnName("RecordContent");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            // 附件IDs
            this.Property(t => t.AccessoryIds).HasColumnName("AccessoryIds");
            // 职位？
            this.Property(t => t.UnattendPost).HasColumnName("UnattendPost");
            // 考勤ID
            this.Property(t => t.AttendanceIds).HasColumnName("AttendanceIds");

            this.HasOptional(t => t.TrackingWorkflow)
               .WithMany()
               .HasForeignKey(d => d.WorkflowInstanceId);
        }
    }
}
