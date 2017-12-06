using CZManageSystem.Data.Domain.MarketPlan;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Ucs_MarketPlan2TmpMap : EntityTypeConfiguration<Ucs_MarketPlan2_Tmp>
	{
		public Ucs_MarketPlan2TmpMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
		/// <summary>
		/// 营销方案编码
		/// <summary>
			  this.Property(t => t.Coding)

			.HasMaxLength(64);
		/// <summary>
		/// 营销方案名称
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(200);
		/// <summary>
		/// 办理渠道
		/// <summary>
			  this.Property(t => t.Channel)

			.HasMaxLength(200);
		/// <summary>
		/// 指令
		/// <summary>
			  this.Property(t => t.Orders)

			.HasMaxLength(64);
		/// <summary>
		/// 社会渠道登记端口
		/// <summary>
			  this.Property(t => t.RegPort)

			.HasMaxLength(64);
		/// <summary>
		/// 营销活动细则
		/// <summary>
			  this.Property(t => t.DetialInfo)

			.HasMaxLength(500);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
		/// <summary>
		/// 方案类型
		/// <summary>
			  this.Property(t => t.PlanType)

			.HasMaxLength(100);
		/// <summary>
		/// 目标用户群
		/// <summary>
			  this.Property(t => t.TargetUsers)

			.HasMaxLength(100);
		/// <summary>
		/// 薪金规则提要
		/// <summary>
			  this.Property(t => t.PaysRlues)

			.HasMaxLength(200);
		/// <summary>
		/// 备用模块1
		/// <summary>
			  this.Property(t => t.Templet1)

			.HasMaxLength(200);
		/// <summary>
		/// 备用模块2
		/// <summary>
			  this.Property(t => t.Templet2)

			.HasMaxLength(200);
		/// <summary>
		/// 备用模块3
		/// <summary>
			  this.Property(t => t.Templet3)

			.HasMaxLength(200);
		/// <summary>
		/// 备用模块4
		/// <summary>
			  this.Property(t => t.Templet4)

			.HasMaxLength(200);
		/// <summary>
		/// 是否标准方案
		/// <summary>
			  this.Property(t => t.IsMarketing)

			.HasMaxLength(10);
			// Table & Column Mappings
 			 this.ToTable("Ucs_MarketPlan2_Tmp"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			// 营销方案编码
			this.Property(t => t.Coding).HasColumnName("Coding"); 
			// 营销方案名称
			this.Property(t => t.Name).HasColumnName("Name"); 
			// 开始时间
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// 结束时间
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// 办理渠道
			this.Property(t => t.Channel).HasColumnName("Channel"); 
			// 指令
			this.Property(t => t.Orders).HasColumnName("Orders"); 
			// 社会渠道登记端口
			this.Property(t => t.RegPort).HasColumnName("RegPort"); 
			// 营销活动细则
			this.Property(t => t.DetialInfo).HasColumnName("DetialInfo"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 方案类型
			this.Property(t => t.PlanType).HasColumnName("PlanType"); 
			// 目标用户群
			this.Property(t => t.TargetUsers).HasColumnName("TargetUsers"); 
			// 薪金规则提要
			this.Property(t => t.PaysRlues).HasColumnName("PaysRlues"); 
			// 备用模块1
			this.Property(t => t.Templet1).HasColumnName("Templet1"); 
			// 备用模块2
			this.Property(t => t.Templet2).HasColumnName("Templet2"); 
			// 备用模块3
			this.Property(t => t.Templet3).HasColumnName("Templet3"); 
			// 备用模块4
			this.Property(t => t.Templet4).HasColumnName("Templet4"); 
			// 号码数量
			this.Property(t => t.NumCount).HasColumnName("NumCount"); 
			// 是否标准方案
			this.Property(t => t.IsMarketing).HasColumnName("IsMarketing"); 
		 }
	 }
}
