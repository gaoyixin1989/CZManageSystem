using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class InvestTempEstimateMap : EntityTypeConfiguration<InvestTempEstimate>
	{
		public InvestTempEstimateMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目ID
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// 合同ID
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// 供应商
		/// <summary>
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
		/// <summary>
		/// 所属专业
		/// <summary>
			  this.Property(t => t.Study)

			.HasMaxLength(500);
		/// <summary>
		/// 科目
		/// <summary>
			  this.Property(t => t.Course)

			.HasMaxLength(500);
		/// <summary>
		/// 是否锁定
		/// <summary>
			  this.Property(t => t.IsLock)

			.HasMaxLength(50);
		/// <summary>
		/// 当前状态
		/// <summary>
			  this.Property(t => t.Status)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestTempEstimate"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 项目ID
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 合同ID
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// 供应商
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// 合同金额
			this.Property(t => t.SignTotal).HasColumnName("SignTotal"); 
			// 合同实际金额
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			// 所属专业
			this.Property(t => t.Study).HasColumnName("Study"); 
			// 负责人ID
			this.Property(t => t.ManagerID).HasColumnName("ManagerID"); 
			// 科目
			this.Property(t => t.Course).HasColumnName("Course"); 
			// 上个月进度
			this.Property(t => t.BackRate).HasColumnName("BackRate"); 
			// 项目形象进度
			this.Property(t => t.Rate).HasColumnName("Rate"); 
			// 已付金额
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			// 暂估金额
			this.Property(t => t.NotPay).HasColumnName("NotPay"); 
			// 是否锁定
			this.Property(t => t.IsLock).HasColumnName("IsLock"); 
			// 暂估人员ID
			this.Property(t => t.EstimateUserID).HasColumnName("EstimateUserID"); 
			// 当前状态
			this.Property(t => t.Status).HasColumnName("Status"); 
			// 状态操作时间
			this.Property(t => t.StatusTime).HasColumnName("StatusTime");

            this.HasOptional(t => t.ManagerObj).WithMany().HasForeignKey(d => d.ManagerID);
        }
	 }
}
