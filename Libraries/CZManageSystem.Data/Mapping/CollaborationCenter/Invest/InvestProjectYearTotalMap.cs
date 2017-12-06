using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 年度投资金额
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestProjectYearTotalMap : EntityTypeConfiguration<InvestProjectYearTotal>
	{
		public InvestProjectYearTotalMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目ID
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestProjectYearTotal"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 项目ID
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 年份
			this.Property(t => t.Year).HasColumnName("Year"); 
			// 年总计
			this.Property(t => t.YearTotal).HasColumnName("YearTotal"); 
		 }
	 }
}
