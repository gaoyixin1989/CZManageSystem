using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ���Ͷ�ʽ��
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
		/// ��ĿID
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestProjectYearTotal"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��ĿID
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ���
			this.Property(t => t.Year).HasColumnName("Year"); 
			// ���ܼ�
			this.Property(t => t.YearTotal).HasColumnName("YearTotal"); 
		 }
	 }
}
