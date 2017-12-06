using CZManageSystem.Data.Domain.HumanResources.Vacation;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationCloseApplyMap : EntityTypeConfiguration<HRVacationCloseApply>
	{
		public HRVacationCloseApplyMap()
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
		/// 休假原因
		/// <summary>
			  this.Property(t => t.Reason)

			.HasMaxLength(200);
		/// <summary>
		/// 销假原因
		/// <summary>
			  this.Property(t => t.Note)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("HRVacationCloseApply"); 
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
			// 销假天数
			this.Property(t => t.ClosedDays).HasColumnName("ClosedDays"); 
			// 休假原因
			this.Property(t => t.Reason).HasColumnName("Reason"); 
			// 休假申请单id
			this.Property(t => t.VacationID).HasColumnName("VacationID"); 
			// 销假原因
			this.Property(t => t.Note).HasColumnName("Note"); 
			// 实际开始时间
			this.Property(t => t.Factst).HasColumnName("Factst"); 
			// 实际结束时间
			this.Property(t => t.Factet).HasColumnName("Factet");
            // 实际天数
            this.Property(t => t.Factdays).HasColumnName("Factdays");
            // 状态：0未提交，1已提交
            this.Property(t => t.State).HasColumnName("State");

            //外键
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.HRVacationCloseApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.EditorObj).WithMany().HasForeignKey(d => d.Editor);
        }
	 }
}
