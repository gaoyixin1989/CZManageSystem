using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationTeachingMap : EntityTypeConfiguration<HRVacationTeaching>
	{
		public HRVacationTeachingMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �ڿογ�����
		/// <summary>
			  this.Property(t => t.TeachingPlan)

			.HasMaxLength(500);
		/// <summary>
		/// ��ʦ����
		/// <summary>
			  this.Property(t => t.TeacherType)

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
 			 this.ToTable("HRVacationTeaching"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �ڿογ�����
			this.Property(t => t.TeachingPlan).HasColumnName("TeachingPlan"); 
			// ��ʦ����
			this.Property(t => t.TeacherType).HasColumnName("TeacherType"); 
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
			// ����
			this.Property(t => t.Integral).HasColumnName("Integral"); 
			// ͬ��ı�ʶ
			this.Property(t => t.AgreeFlag).HasColumnName("AgreeFlag"); 
			// ����ID
			this.Property(t => t.ReVacationID).HasColumnName("ReVacationID"); 
			this.Property(t => t.UserId).HasColumnName("UserId"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.Ftst).HasColumnName("Ftst"); 
			this.Property(t => t.Ftet).HasColumnName("Ftet"); 
			this.Property(t => t.Hispt).HasColumnName("Hispt"); 
		 }
	 }
}
