using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 年度投资金额视图
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class V_InvestProjectYearTotalMap : EntityTypeConfiguration<V_InvestProjectYearTotal>
	{
		public V_InvestProjectYearTotalMap()
		{
			// Primary Key
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
			  this.Property(t => t.ProjectName)

			.HasMaxLength(1000);
			// Table & Column Mappings
 			 this.ToTable("V_InvestProjectYearTotal"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			this.Property(t => t.Year).HasColumnName("Year"); 
			this.Property(t => t.YearTotal).HasColumnName("YearTotal"); 
			this.Property(t => t.ProjectName).HasColumnName("ProjectName"); 
			this.Property(t => t.Total).HasColumnName("Total"); 
			this.Property(t => t.ManagerID).HasColumnName("ManagerID"); 
		 }
	 }
}
