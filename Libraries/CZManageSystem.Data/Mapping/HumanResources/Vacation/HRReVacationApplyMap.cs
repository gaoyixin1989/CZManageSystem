using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRReVacationApplyMap : EntityTypeConfiguration<HRReVacationApply>
	{
		public HRReVacationApplyMap()
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
		/// 口头申请领导
		/// <summary>
			  this.Property(t => t.Leader)

			.HasMaxLength(50);
		/// <summary>
		/// 异常休假原因
		/// <summary>
			  this.Property(t => t.ReApplyReason)

			.HasMaxLength(200);
		/// <summary>
		/// 附件ID
		/// <summary>
			  this.Property(t => t.Attids)
;
			// Table & Column Mappings
 			 this.ToTable("HRReVacationApply"); 
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
			// 是否已口头申请
			this.Property(t => t.Boral).HasColumnName("Boral"); 
			// 口头申请领导
			this.Property(t => t.Leader).HasColumnName("Leader"); 
			// 异常休假原因
			this.Property(t => t.ReApplyReason).HasColumnName("ReApplyReason"); 
			// 附件ID
			this.Property(t => t.Attids).HasColumnName("Attids");
            // 状态：0未提交，1已提交
            this.Property(t => t.State).HasColumnName("State");

            //外键
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.HRReVacationApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);

            this.HasMany(t => t.Meetings).WithOptional().HasForeignKey(d => d.ReVacationID);
            this.HasMany(t => t.Courses).WithOptional().HasForeignKey(d => d.ReVacationID);
            this.HasMany(t => t.Teachings).WithOptional().HasForeignKey(d => d.ReVacationID);
            this.HasMany(t => t.Others).WithOptional().HasForeignKey(d => d.ReVacationID);
        }
	 }
}
