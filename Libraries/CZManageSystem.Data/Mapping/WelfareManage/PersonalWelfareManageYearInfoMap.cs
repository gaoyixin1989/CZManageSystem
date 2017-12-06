using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class PersonalWelfareManageYearInfoMap : EntityTypeConfiguration<PersonalWelfareManageYearInfo>
	{
		public PersonalWelfareManageYearInfoMap()
		{
			// Primary Key
			 this.HasKey(t => t.YID);
		/// <summary>
		/// Ա�����
		/// <summary>
			  this.Property(t => t.Employee)

			.HasMaxLength(20);
		/// <summary>
		/// Ա������
		/// <summary>
			  this.Property(t => t.EmployeeName)

			.HasMaxLength(100);
		/// <summary>
		/// ��
		/// <summary>
			  this.Property(t => t.CYear)
			 .IsRequired()
			.HasMaxLength(10);
			// Table & Column Mappings
 			 this.ToTable("PersonalWelfareManageYearInfo"); 
			// �긣��ID
			this.Property(t => t.YID).HasColumnName("YID"); 
			// Ա�����
			this.Property(t => t.Employee).HasColumnName("Employee"); 
			// Ա������
			this.Property(t => t.EmployeeName).HasColumnName("EmployeeName"); 
			// ��
			this.Property(t => t.CYear).HasColumnName("CYear"); 
			// �긣���ܶ�
			this.Property(t => t.WelfareYearTotalAmount).HasColumnName("WelfareYearTotalAmount"); 
			// ����ʱ��
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// �༭��
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// �༭ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
		 }
	 }
}
