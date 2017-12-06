using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 历史项目暂估
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestAgoEstimateMap : EntityTypeConfiguration<InvestAgoEstimate>
	{
		public InvestAgoEstimateMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目名称
		/// <summary>
			  this.Property(t => t.ProjectName)

			.HasMaxLength(500);
		/// <summary>
		/// 项目ID
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// 合同名称
		/// <summary>
			  this.Property(t => t.ContractName)

			.HasMaxLength(1000);
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
			// Table & Column Mappings
 			 this.ToTable("InvestAgoEstimate"); 
			// 唯一ID
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 年份
			this.Property(t => t.Year).HasColumnName("Year"); 
			// 月份
			this.Property(t => t.Month).HasColumnName("Month"); 
			// 项目名称
			this.Property(t => t.ProjectName).HasColumnName("ProjectName"); 
			// 项目ID
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 合同名称
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			// 合同ID
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// 供应商
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// 合同总金额
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
			// 暂估人员ID
			this.Property(t => t.EstimateUserID).HasColumnName("EstimateUserID");


            //外键
            this.HasOptional(t => t.ManagerObj).WithMany().HasForeignKey(d => d.ManagerID);
            this.HasOptional(t => t.EstimateUserObj).WithMany().HasForeignKey(d => d.EstimateUserID);

        }
    }
}
