using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class Consumable_LevellingMap : EntityTypeConfiguration<Consumable_Levelling>
	{
		public Consumable_LevellingMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 流程单号
		/// <summary>
			  this.Property(t => t.Series)

			.HasMaxLength(200);
		/// <summary>
		/// 部门id
		/// <summary>
			  this.Property(t => t.AppDept)

			.HasMaxLength(500);
		/// <summary>
		/// 申请人手机
		/// <summary>
			  this.Property(t => t.Mobile)

			.HasMaxLength(200);
		/// <summary>
		/// 申请标题
		/// <summary>
			  this.Property(t => t.Title)

			.HasMaxLength(200);
		/// <summary>
		/// 原因
		/// <summary>
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("Consumable_Levelling"); 
			// 编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 流程实例ID
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			// 流程单号
			this.Property(t => t.Series).HasColumnName("Series"); 
			// 申请时间
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			// 部门id
			this.Property(t => t.AppDept).HasColumnName("AppDept"); 
			// 申请人
			this.Property(t => t.AppPerson).HasColumnName("AppPerson"); 
			// 申请人手机
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			// 申请标题
			this.Property(t => t.Title).HasColumnName("Title"); 
			// 原因
			this.Property(t => t.Content).HasColumnName("Content"); 
			// 状态：0保存、1提交
			this.Property(t => t.State).HasColumnName("State");
            
            // Relationships
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.Consumable_Levellings)
                .HasForeignKey(d => d.WorkflowInstanceId);
        }
	 }
}
