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
		/// 员工编号
		/// <summary>
			  this.Property(t => t.Employee)

			.HasMaxLength(20);
		/// <summary>
		/// 员工姓名
		/// <summary>
			  this.Property(t => t.EmployeeName)

			.HasMaxLength(100);
		/// <summary>
		/// 年
		/// <summary>
			  this.Property(t => t.CYear)
			 .IsRequired()
			.HasMaxLength(10);
			// Table & Column Mappings
 			 this.ToTable("PersonalWelfareManageYearInfo"); 
			// 年福利ID
			this.Property(t => t.YID).HasColumnName("YID"); 
			// 员工编号
			this.Property(t => t.Employee).HasColumnName("Employee"); 
			// 员工姓名
			this.Property(t => t.EmployeeName).HasColumnName("EmployeeName"); 
			// 年
			this.Property(t => t.CYear).HasColumnName("CYear"); 
			// 年福利总额
			this.Property(t => t.WelfareYearTotalAmount).HasColumnName("WelfareYearTotalAmount"); 
			// 创建时间
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// 编辑者
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// 编辑时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
		 }
	 }
}
