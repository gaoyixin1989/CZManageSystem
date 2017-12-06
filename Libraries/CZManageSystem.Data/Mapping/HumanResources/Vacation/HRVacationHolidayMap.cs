using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationHolidayMap : EntityTypeConfiguration<HRVacationHoliday>
	{
		public HRVacationHolidayMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �ݼ�����
		/// <summary>
			  this.Property(t => t.VacationType)

			.HasMaxLength(50);
		/// <summary>
		/// ��������
		/// <summary>
			  this.Property(t => t.VacationClass)

			.HasMaxLength(50);
		/// <summary>
		/// ԭ��
		/// <summary>
			  this.Property(t => t.Reason)

			.HasMaxLength(1000);
			// Table & Column Mappings
 			 this.ToTable("HRVacationHoliday"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �û�ID
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			// ���
			this.Property(t => t.YearDate).HasColumnName("YearDate"); 
			// �ݼ�����
			this.Property(t => t.VacationType).HasColumnName("VacationType"); 
			// ��������
			this.Property(t => t.VacationClass).HasColumnName("VacationClass"); 
			// ����
			this.Property(t => t.PeriodTime).HasColumnName("PeriodTime"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ԭ��
			this.Property(t => t.Reason).HasColumnName("Reason");

            this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserId);
        }
	 }
}
