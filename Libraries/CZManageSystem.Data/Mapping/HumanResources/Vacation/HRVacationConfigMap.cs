using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationConfigMap : EntityTypeConfiguration<HRVacationConfig>
	{
		public HRVacationConfigMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.VacationName)

			.HasMaxLength(100);
		/// <summary>
		/// �����޶�����
		/// <summary>
			  this.Property(t => t.Limit)

			.HasMaxLength(500);
		/// <summary>
		/// ��Χ
		/// <summary>
			  this.Property(t => t.Scope)

			.HasMaxLength(100);
			  this.Property(t => t.Daycalmethod)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRVacationConfig"); 
			// ����
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��������
			this.Property(t => t.VacationName).HasColumnName("VacationName"); 
			// �����޶�����
			this.Property(t => t.Limit).HasColumnName("Limit"); 
			// ��������
			this.Property(t => t.SpanTime).HasColumnName("SpanTime"); 
			// ��Χ
			this.Property(t => t.Scope).HasColumnName("Scope"); 
			this.Property(t => t.Daycalmethod).HasColumnName("Daycalmethod"); 
		 }
	 }
}
