using CZManageSystem.Data.Domain.MarketPlan;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Ucs_MarketPlanMonitorMap : EntityTypeConfiguration<Ucs_MarketPlanMonitor>
	{
		public Ucs_MarketPlanMonitorMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
		/// <summary>
		/// 文件名称
		/// <summary>
			  this.Property(t => t.ImportName)

			.HasMaxLength(200);
		/// <summary>
		/// 状态
		/// <summary>
			  this.Property(t => t.Status)

			.HasMaxLength(50);
		/// <summary>
		/// 描述
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1000);
			  this.Property(t => t.ReDownload)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("Ucs_MarketPlanMonitor"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			// 文件名称
			this.Property(t => t.ImportName).HasColumnName("ImportName"); 
			// 操作时间
			this.Property(t => t.Creattime).HasColumnName("Creattime"); 
			// 记录数
			this.Property(t => t.Count).HasColumnName("Count"); 
			// 状态
			this.Property(t => t.Status).HasColumnName("Status"); 
			// 描述
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			this.Property(t => t.ReDownload).HasColumnName("ReDownload"); 
		 }
	 }
}
