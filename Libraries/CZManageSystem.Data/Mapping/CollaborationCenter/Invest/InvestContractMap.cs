using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 合同信息
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestContractMap : EntityTypeConfiguration<InvestContract>
	{
		public InvestContractMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 项目编号
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
		/// <summary>
		/// 合同编号
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// 合同名称
		/// <summary>
			  this.Property(t => t.ContractName)

			.HasMaxLength(1000);
		/// <summary>
		/// 供应商
		/// <summary>
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
		/// <summary>
		/// 合同主办部门
		/// <summary>
			  this.Property(t => t.DpCode)

			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
		/// <summary>
		/// 是否MIS单类
		/// <summary>
			  this.Property(t => t.IsMIS)

			.HasMaxLength(50);
		/// <summary>
		/// 是否删除
		/// <summary>
			  this.Property(t => t.IsDel)

			.HasMaxLength(50);
		/// <summary>
		/// 合同流水号
		/// <summary>
			  this.Property(t => t.ContractSeries)

			.HasMaxLength(50);
		/// <summary>
		/// 币种
		/// <summary>
			  this.Property(t => t.Currency)

			.HasMaxLength(50);
		/// <summary>
		/// 合同状态
		/// <summary>
			  this.Property(t => t.ContractState)

			.HasMaxLength(50);
		/// <summary>
		/// 合同属性
		/// <summary>
			  this.Property(t => t.Attribute)

			.HasMaxLength(50);
		/// <summary>
		/// 合同档案号
		/// <summary>
			  this.Property(t => t.ContractFilesNum)

			.HasMaxLength(50);
		/// <summary>
		/// 印花税率
		/// <summary>
			  this.Property(t => t.StampTaxrate)

			.HasMaxLength(50);
		/// <summary>
		/// 印花税金
		/// <summary>
			  this.Property(t => t.Stamptax)

			.HasMaxLength(50);
			  this.Property(t => t.ContractOpposition)

			.HasMaxLength(500);
		/// <summary>
		/// 需求部门
		/// <summary>
			  this.Property(t => t.RequestDp)

			.HasMaxLength(50);
		/// <summary>
		/// 相关部门
		/// <summary>
			  this.Property(t => t.RelevantDp)

			.HasMaxLength(50);
		/// <summary>
		/// 项目开展原因
		/// <summary>
			  this.Property(t => t.ProjectCause)

			.HasMaxLength(500);
		/// <summary>
		/// 合同类型
		/// <summary>
			  this.Property(t => t.ContractType)

			.HasMaxLength(50);
		/// <summary>
		/// 合同对方来源
		/// <summary>
			  this.Property(t => t.ContractOppositionFrom)

			.HasMaxLength(50);
		/// <summary>
		/// 合同对方选择方式
		/// <summary>
			  this.Property(t => t.ContractOppositionType)

			.HasMaxLength(50);
		/// <summary>
		/// 采购方式
		/// <summary>
			  this.Property(t => t.Purchase)

			.HasMaxLength(50);
		/// <summary>
		/// 付款方式
		/// <summary>
			  this.Property(t => t.PayType)

			.HasMaxLength(50);
		/// <summary>
		/// 付款说明
		/// <summary>
			  this.Property(t => t.PayRemark)

			.HasMaxLength(1000);
		/// <summary>
		/// 框架合同
		/// <summary>
			  this.Property(t => t.IsFrameContract)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestContract"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 导入时间，同步调用参数之一
			this.Property(t => t.ImportTime).HasColumnName("ImportTime"); 
			// 项目编号
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// 合同编号
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// 合同名称
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			// 供应商
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// 签订时间
			this.Property(t => t.SignTime).HasColumnName("SignTime"); 
			// 合同主办部门
			this.Property(t => t.DpCode).HasColumnName("DpCode"); 
			// 主办人
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// 合同项目金额、合同不含税金额(元)
			this.Property(t => t.SignTotal).HasColumnName("SignTotal"); 
			// 合同总金额
			this.Property(t => t.AllTotal).HasColumnName("AllTotal"); 
			// 实际合同金额
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			// 备注
			this.Property(t => t.Content).HasColumnName("Content"); 
			// 是否MIS单类
			this.Property(t => t.IsMIS).HasColumnName("IsMIS"); 
			// 是否删除
			this.Property(t => t.IsDel).HasColumnName("IsDel"); 
			// 合同流水号
			this.Property(t => t.ContractSeries).HasColumnName("ContractSeries"); 
			// 合同税金
			this.Property(t => t.Tax).HasColumnName("Tax"); 
			// 合同含税金额(元
			this.Property(t => t.SignTotalTax).HasColumnName("SignTotalTax"); 
			// 币种
			this.Property(t => t.Currency).HasColumnName("Currency"); 
			// 合同状态
			this.Property(t => t.ContractState).HasColumnName("ContractState"); 
			// 合同属性
			this.Property(t => t.Attribute).HasColumnName("Attribute"); 
			// 审批开始时间
			this.Property(t => t.ApproveStartTime).HasColumnName("ApproveStartTime"); 
			// 审批结束时间
			this.Property(t => t.ApproveEndTime).HasColumnName("ApproveEndTime"); 
			// 合同档案号
			this.Property(t => t.ContractFilesNum).HasColumnName("ContractFilesNum"); 
			// 印花税率
			this.Property(t => t.StampTaxrate).HasColumnName("StampTaxrate"); 
			// 印花税金
			this.Property(t => t.Stamptax).HasColumnName("Stamptax"); 
			this.Property(t => t.ContractOpposition).HasColumnName("ContractOpposition"); 
			// 需求部门
			this.Property(t => t.RequestDp).HasColumnName("RequestDp"); 
			// 相关部门
			this.Property(t => t.RelevantDp).HasColumnName("RelevantDp"); 
			// 项目开展原因
			this.Property(t => t.ProjectCause).HasColumnName("ProjectCause"); 
			// 合同类型
			this.Property(t => t.ContractType).HasColumnName("ContractType"); 
			// 合同对方来源
			this.Property(t => t.ContractOppositionFrom).HasColumnName("ContractOppositionFrom"); 
			// 合同对方选择方式
			this.Property(t => t.ContractOppositionType).HasColumnName("ContractOppositionType"); 
			// 采购方式
			this.Property(t => t.Purchase).HasColumnName("Purchase"); 
			// 付款方式
			this.Property(t => t.PayType).HasColumnName("PayType"); 
			// 付款说明
			this.Property(t => t.PayRemark).HasColumnName("PayRemark"); 
			// 合同有效区间起始
			this.Property(t => t.ContractStartTime).HasColumnName("ContractStartTime"); 
			// 合同有效区间终止
			this.Property(t => t.ContractEndTime).HasColumnName("ContractEndTime"); 
			// 框架合同
			this.Property(t => t.IsFrameContract).HasColumnName("IsFrameContract"); 
			// 起草时间
			this.Property(t => t.DraftTime).HasColumnName("DraftTime"); 
			// 项目金额
			this.Property(t => t.ProjectTotal).HasColumnName("ProjectTotal"); 
			// 已签署项目总额
			this.Property(t => t.ProjectAllTotal).HasColumnName("ProjectAllTotal");


            //外键
            //this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.DpCode);
            //this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserID);

        }
	 }
}
