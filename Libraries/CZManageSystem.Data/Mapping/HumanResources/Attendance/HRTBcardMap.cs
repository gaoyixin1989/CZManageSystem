using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Integral
{
	public class HRTBcardMap : EntityTypeConfiguration<HRTBcard>
	{
		public HRTBcardMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 账号
		/// <summary>
			  this.Property(t => t.EmployeeId)

			.HasMaxLength(50);
		/// <summary>
		/// 用户ID
		/// <summary>
			  this.Property(t => t.EmpNo)

			.HasMaxLength(50);
		/// <summary>
		/// 通宝卡号
		/// <summary>
			  this.Property(t => t.CardNo)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("HRTBcard"); 
			// 主键
			this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Tid).HasColumnName("Tid");
            // 账号
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId"); 
			// 时间
			this.Property(t => t.SkTime).HasColumnName("SkTime"); 
			// 状态
			this.Property(t => t.ActionStatus).HasColumnName("ActionStatus"); 
			// 用户ID
			this.Property(t => t.EmpNo).HasColumnName("EmpNo"); 
			// 通宝卡号
			this.Property(t => t.CardNo).HasColumnName("CardNo"); 
		 }
	 }
}
