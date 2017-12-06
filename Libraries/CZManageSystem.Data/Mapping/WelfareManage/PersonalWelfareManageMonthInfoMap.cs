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
		/// 员工编号
		/// <summary>
			  this.Property(t => t.Employee)

			.HasMaxLength(20);
		/// <summary>
		/// 员工名字
		/// <summary>
			  this.Property(t => t.EmployeeName)

			.HasMaxLength(100);
		/// <summary>
		/// 年月
		/// <summary>
			  this.Property(t => t.CYearAndMonth)

			.HasMaxLength(20);
		/// <summary>
		/// 福利套餐
		/// <summary>
			  this.Property(t => t.WelfarePackage)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PersonalWelfareManageMonthInfo"); 
			// 月福利ID
			this.Property(t => t.MID).HasColumnName("MID"); 
			// 员工编号
			this.Property(t => t.Employee).HasColumnName("Employee"); 
			// 员工名字
			this.Property(t => t.EmployeeName).HasColumnName("EmployeeName"); 
			// 年月
			this.Property(t => t.CYearAndMonth).HasColumnName("CYearAndMonth"); 
			// 福利套餐
			this.Property(t => t.WelfarePackage).HasColumnName("WelfarePackage"); 
			// 总额度
			this.Property(t => t.WelfareMonthTotalAmount).HasColumnName("WelfareMonthTotalAmount"); 
			// 已用额度
			this.Property(t => t.WelfareMonthAmountUsed).HasColumnName("WelfareMonthAmountUsed"); 
			// 创建时间
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// 编辑人
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			// 编辑时间
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
		 }
	 }
}
