using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class PersonalWelfareManageMonthInfoMap : EntityTypeConfiguration<PersonalWelfareManageMonthInfo>
	{
		public PersonalWelfareManageMonthInfoMap()
		{
			// Primary Key
			 this.HasKey(t => t.MID);
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
		/// ����
		/// <summary>
			  this.Property(t => t.CYearAndMonth)

			.HasMaxLength(20);
		/// <summary>
		/// �����ײ�
		/// <summary>
			  this.Property(t => t.WelfarePackage)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PersonalWelfareManageMonthInfo"); 
			// �¸���ID
			this.Property(t => t.MID).HasColumnName("MID"); 
			// Ա�����
			this.Property(t => t.Employee).HasColumnName("Employee"); 
			// Ա������
			this.Property(t => t.EmployeeName).HasColumnName("EmployeeName"); 
			// ����
			this.Property(t => t.CYearAndMonth).HasColumnName("CYearAndMonth"); 
			// �����ײ�
			this.Property(t => t.WelfarePackage).HasColumnName("WelfarePackage"); 
			// �ܶ��
			this.Property(t => t.WelfareMonthTotalAmount).HasColumnName("WelfareMonthTotalAmount"); 
			// ���ö��
			this.Property(t => t.WelfareMonthAmountUsed).HasColumnName("WelfareMonthAmountUsed"); 
			// ����ʱ��
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// �༭��
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// �༭ʱ��
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
		 }
	 }
}
