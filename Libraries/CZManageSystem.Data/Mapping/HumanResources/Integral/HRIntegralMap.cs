using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class HRIntegralMap : EntityTypeConfiguration<HRIntegral>
	{
		public HRIntegralMap()
		{
			// Primary Key
			 this.HasKey(t => t.IntegralId);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
		/// <summary>
		/// 类型
		/// <summary>
			  this.Property(t => t.IntegralType)

			.HasMaxLength(100);
			  this.Property(t => t.FinishTime)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRIntegral"); 
			this.Property(t => t.IntegralId).HasColumnName("IntegralId"); 
			// 用词ID
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			// 积分
			this.Property(t => t.Integral).HasColumnName("Integral"); 
			// 年份
			this.Property(t => t.YearDate).HasColumnName("YearDate"); 
			// 备注
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// 积分来源
			this.Property(t => t.Source).HasColumnName("Source"); 
			// 删除
			this.Property(t => t.Del).HasColumnName("Del"); 
			// 类型
			this.Property(t => t.IntegralType).HasColumnName("IntegralType"); 
			// 培训积分
			this.Property(t => t.CIntegral).HasColumnName("CIntegral"); 
			// 授课积分
			this.Property(t => t.TIntegral).HasColumnName("TIntegral"); 
			// 申请单ID
			this.Property(t => t.ApplyId).HasColumnName("ApplyId"); 
			this.Property(t => t.TPeriodTime).HasColumnName("TPeriodTime"); 
			this.Property(t => t.Daoid).HasColumnName("Daoid"); 
			this.Property(t => t.FinishTime).HasColumnName("FinishTime"); 
		 }
	 }
}
