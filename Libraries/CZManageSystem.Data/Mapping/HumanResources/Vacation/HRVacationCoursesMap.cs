using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationCoursesMap : EntityTypeConfiguration<HRVacationCourses>
	{
		public HRVacationCoursesMap()
		{
			// Primary Key
			 this.HasKey(t => t.CoursesId);
		/// <summary>
		/// �γ�����
		/// <summary>
			  this.Property(t => t.CoursesName)

			.HasMaxLength(500);
		/// <summary>
		/// �γ����
		/// <summary>
			  this.Property(t => t.CoursesType)

			.HasMaxLength(50);
		/// <summary>
		/// ���쵥λ
		/// <summary>
			  this.Property(t => t.ProvinceCity)

			.HasMaxLength(50);
		/// <summary>
		/// ��ע
		/// <summary>
			  this.Property(t => t.Remark)

			.HasMaxLength(1073741823);
			  this.Property(t => t.UserName)

			.HasMaxLength(100);
			  this.Property(t => t.Ftst)

			.HasMaxLength(100);
			  this.Property(t => t.Ftet)

			.HasMaxLength(100);
			  this.Property(t => t.Hispt)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("HRVacationCourses"); 
			this.Property(t => t.CoursesId).HasColumnName("CoursesId"); 
			// �γ�����
			this.Property(t => t.CoursesName).HasColumnName("CoursesName"); 
			// �γ����
			this.Property(t => t.CoursesType).HasColumnName("CoursesType"); 
			// ���쵥λ
			this.Property(t => t.ProvinceCity).HasColumnName("ProvinceCity"); 
			// ��ʼʱ��
			this.Property(t => t.StartTime).HasColumnName("StartTime"); 
			// ����ʱ��
			this.Property(t => t.EndTime).HasColumnName("EndTime"); 
			// ��ѵ����
			this.Property(t => t.PeriodTime).HasColumnName("PeriodTime"); 
			// ��ע
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			// ���ڱ�ID
			this.Property(t => t.VacationID).HasColumnName("VacationID"); 
			// ����
			this.Property(t => t.Integral).HasColumnName("Integral"); 
			// ͬ��ı�ʶ
			this.Property(t => t.AgreeFlag).HasColumnName("AgreeFlag"); 
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			this.Property(t => t.DaoId).HasColumnName("DaoId"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.Ftst).HasColumnName("Ftst"); 
			this.Property(t => t.Ftet).HasColumnName("Ftet"); 
			this.Property(t => t.Hispt).HasColumnName("Hispt"); 
			// ����ID
			this.Property(t => t.ReVacationID).HasColumnName("ReVacationID"); 
		 }
	 }
}
