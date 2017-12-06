using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationAnnualLeaveConfigMap : EntityTypeConfiguration<HRVacationAnnualLeaveConfig>
	{
		public HRVacationAnnualLeaveConfigMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ���������ʹ���·�
		/// <summary>
			  this.Property(t => t.LimitMonth)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HRVacationAnnualLeaveConfig"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���
			this.Property(t => t.Annual).HasColumnName("Annual"); 
			// ��������
			this.Property(t => t.SpanTime).HasColumnName("SpanTime"); 
			// ���������ʹ���·�
			this.Property(t => t.LimitMonth).HasColumnName("LimitMonth"); 
		 }
	 }
}
