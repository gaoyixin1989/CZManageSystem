using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HROverTimeApplyMap : EntityTypeConfiguration<HROverTimeApply>
	{
		public HROverTimeApplyMap()
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
		/// 职务
		/// <summary>
			  this.Property(t => t.ApplyPost)

			.HasMaxLength(100);
		/// <summary>
		/// 直接主管姓名
		/// <summary>
			  this.Property(t => t.ManageName)

			.HasMaxLength(100);
		/// <summary>
		/// 直接主管职务
		/// <summary>
			  this.Property(t => t.ManagePost)

			.HasMaxLength(100);
		/// <summary>
		/// 加班地点
		/// <summary>
			  this.Property(t => t.Address)

			.HasMaxLength(500);
		/// <summary>
		/// 加班类型
		/// <summary>
			  this.Property(t => t.OvertimeType)

			.HasMaxLength(200);
		/// <summary>
		/// 加班原因
		/// <summary>
			  this.Property(t => t.Reason)

			.HasMaxLength(200);
			  this.Property(t => t.Newpt)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HROverTimeApply"); 
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
			// 职务
			this.Property(t => t.ApplyPost).HasColumnName("ApplyPost"); 
			// 直接主管姓名
			this.Property(t => t.ManageName).HasColumnName("ManageName"); 
			// 直接主管职务
			this.Property(t => t.ManagePost).HasColumnName("ManagePost"); 
			// 加班地点
			this.Property(t => t.Address).HasColumnName("Address"); 
			// 加班类型
			this.Property(t => t.OvertimeType).HasColumnName("OvertimeType"); 
			// 加班原因
			this.Property(t => t.Reason).HasColumnName("Reason"); 
			// 编辑者
			this.Property(t => t.Editor).HasColumnName("Editor");
            // 申请时间
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.Newpt).HasColumnName("Newpt"); 
		 }
	 }
}
