using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ComebackApplyMap : EntityTypeConfiguration<ComebackApply>
	{
		public ComebackApplyMap()
		{
			// Primary Key
			 this.HasKey(t => t.ApplyId);
		/// <summary>
		/// 标题
		/// <summary>
			  this.Property(t => t.Title)  

			.HasMaxLength(500);
		/// <summary>
		/// 流程单号
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(50);
		/// <summary>
		/// 电话
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(50);
		/// <summary>
		/// 部门
		/// <summary>
			  this.Property(t => t.ApplyDept)

			.HasMaxLength(100);
		/// <summary>
		/// 申请人
		/// <summary>
			  this.Property(t => t.ApplyUser)

			.HasMaxLength(100);
		/// <summary>
		/// 预算需求部门
		/// <summary>
			  this.Property(t => t.BudgetDept)

			.HasMaxLength(100);
            /// <summary>
            /// 申请资源类别
            /// <summary>
            this.Property(t => t.SourceTypeID);

			  this.Property(t => t.SourceChildId)

			.HasMaxLength(100);
		/// <summary>
		/// 拟开展项目名称
		/// <summary>
			  this.Property(t => t.ProjName)

			.HasMaxLength(100);
		/// <summary>
		/// 拟立或已立预算项目名称
		/// <summary>
			  this.Property(t => t.PrevProjName)

			.HasMaxLength(100);
		/// <summary>
		/// 拟立或已立预算项目编号
		/// <summary>
			  this.Property(t => t.PrevProjCode)

			.HasMaxLength(100);
		/// <summary>
		/// 项目经办人
		/// <summary>
			  this.Property(t => t.ProjManager)

			.HasMaxLength(100);
		/// <summary>
		/// 项目开展必要性及效益性分析
		/// <summary>
			  this.Property(t => t.ProjAnalysis)

			.HasMaxLength(1073741823);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("ComebackApply"); 
			this.Property(t => t.ApplyId).HasColumnName("ApplyId"); 
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			// 标题
			this.Property(t => t.Title).HasColumnName("Title"); 
			// 流程单号
			this.Property(t => t.Series).HasColumnName("Series"); 
			// 电话
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 状态
			this.Property(t => t.Status).HasColumnName("Status"); 
			// 申请时间
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			// 部门
			this.Property(t => t.ApplyDept).HasColumnName("ApplyDept"); 
			// 申请人
			this.Property(t => t.ApplyUser).HasColumnName("ApplyUser"); 
			// 预算需求部门
			this.Property(t => t.BudgetDept).HasColumnName("BudgetDept"); 
			// 申请资源类别
			this.Property(t => t.SourceTypeID).HasColumnName("SourceTypeID"); 
			// 项目开始时间
			this.Property(t => t.TimeStart).HasColumnName("TimeStart"); 
			// 项目结束时间
			this.Property(t => t.TimeEnd).HasColumnName("TimeEnd"); 
			this.Property(t => t.SourceChildId).HasColumnName("SourceChildId"); 
			// 拟开展项目名称
			this.Property(t => t.ProjName).HasColumnName("ProjName"); 
			// 拟立或已立预算项目名称
			this.Property(t => t.PrevProjName).HasColumnName("PrevProjName"); 
			// 拟立或已立预算项目编号
			this.Property(t => t.PrevProjCode).HasColumnName("PrevProjCode"); 
			// 项目经办人
			this.Property(t => t.ProjManager).HasColumnName("ProjManager"); 
			// 不含税金额申请额度
			this.Property(t => t.AppAmount).HasColumnName("AppAmount"); 
			// 项目开展必要性及效益性分析
			this.Property(t => t.ProjAnalysis).HasColumnName("ProjAnalysis"); 
			// 预算年度
			this.Property(t => t.Year).HasColumnName("Year"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 含税金额申请额度
			this.Property(t => t.AppAmountHanshui).HasColumnName("AppAmountHanshui"); 
		 }
	 }
}
