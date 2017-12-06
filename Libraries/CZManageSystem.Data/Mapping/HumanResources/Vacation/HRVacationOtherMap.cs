using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationOtherMap : EntityTypeConfiguration<HRVacationOther>
	{
		public HRVacationOtherMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.OtherName)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			// Table & Column Mappings
 			 this.ToTable("HRVacationOther"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ����
			this.Property(t => t.OtherName).HasColumnName("OtherName"); 
			// ����
			this.Property(t => t.PeriodTime).HasColumnName("PeriodTime"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// ���ڱ��ID
			this.Property(t => t.VacationID).HasColumnName("VacationID"); 
			// ͬ��ı�ʶ
			this.Property(t => t.AgreeFlag).HasColumnName("AgreeFlag"); 
			// ����ID
			this.Property(t => t.ReVacationID).HasColumnName("ReVacationID"); 
		 }
	 }
}
