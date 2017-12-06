using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ∫œÕ¨≤È—Ø
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class V_InvestContractQueryMap : EntityTypeConfiguration<V_InvestContractQuery>
	{
		public V_InvestContractQueryMap()
		{
            // Primary Key
            this.HasKey(t => t.ID);
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
			  this.Property(t => t.ProjectName)

			.HasMaxLength(1000);
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
			  this.Property(t => t.ContractName)

			.HasMaxLength(1000);
			  this.Property(t => t.SignTime)

			.HasMaxLength(10);
			  this.Property(t => t.DpCode)

			.HasMaxLength(50);
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
			  this.Property(t => t.IsMIS)

			.HasMaxLength(50);
			  this.Property(t => t.IsDel)

			.HasMaxLength(50);
			  this.Property(t => t.ContractState)

			.HasMaxLength(50);
			  this.Property(t => t.Attribute)

			.HasMaxLength(50);
			  this.Property(t => t.ContractFilesNum)

			.HasMaxLength(50);
			  this.Property(t => t.StampTaxrate)

			.HasMaxLength(50);
			  this.Property(t => t.Stamptax)

			.HasMaxLength(50);
			  this.Property(t => t.ContractOpposition)

			.HasMaxLength(500);
			  this.Property(t => t.RequestDp)

			.HasMaxLength(50);
			  this.Property(t => t.RelevantDp)

			.HasMaxLength(50);
			  this.Property(t => t.ProjectCause)

			.HasMaxLength(500);
			  this.Property(t => t.ContractType)

			.HasMaxLength(50);
			  this.Property(t => t.ContractOppositionFrom)

			.HasMaxLength(50);
			  this.Property(t => t.ContractOppositionType)

			.HasMaxLength(50);
			  this.Property(t => t.Purchase)

			.HasMaxLength(50);
			  this.Property(t => t.PayType)

			.HasMaxLength(50);
			  this.Property(t => t.PayRemark)

			.HasMaxLength(1000);
			  this.Property(t => t.IsFrameContract)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("V_InvestContractQuery"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.ImportTime).HasColumnName("ImportTime"); 
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			this.Property(t => t.ProjectName).HasColumnName("ProjectName"); 
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			this.Property(t => t.SignTime).HasColumnName("SignTime"); 
			this.Property(t => t.DpCode).HasColumnName("DpCode"); 
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			this.Property(t => t.SignTotal).HasColumnName("SignTotal"); 
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			this.Property(t => t.Content).HasColumnName("Content"); 
			this.Property(t => t.ManagerID).HasColumnName("ManagerID"); 
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			this.Property(t => t.AllTotal).HasColumnName("AllTotal"); 
			this.Property(t => t.IsMIS).HasColumnName("IsMIS"); 
			this.Property(t => t.IsDel).HasColumnName("IsDel"); 
			this.Property(t => t.ContractState).HasColumnName("ContractState"); 
			this.Property(t => t.Attribute).HasColumnName("Attribute"); 
			this.Property(t => t.ProjectTotal).HasColumnName("ProjectTotal"); 
			this.Property(t => t.ApproveStartTime).HasColumnName("ApproveStartTime"); 
			this.Property(t => t.ApproveEndTime).HasColumnName("ApproveEndTime"); 
			this.Property(t => t.ContractFilesNum).HasColumnName("ContractFilesNum"); 
			this.Property(t => t.StampTaxrate).HasColumnName("StampTaxrate"); 
			this.Property(t => t.Stamptax).HasColumnName("Stamptax"); 
			this.Property(t => t.ContractOpposition).HasColumnName("ContractOpposition"); 
			this.Property(t => t.RequestDp).HasColumnName("RequestDp"); 
			this.Property(t => t.RelevantDp).HasColumnName("RelevantDp"); 
			this.Property(t => t.ProjectCause).HasColumnName("ProjectCause"); 
			this.Property(t => t.ContractType).HasColumnName("ContractType"); 
			this.Property(t => t.ContractOppositionFrom).HasColumnName("ContractOppositionFrom"); 
			this.Property(t => t.ContractOppositionType).HasColumnName("ContractOppositionType"); 
			this.Property(t => t.Purchase).HasColumnName("Purchase"); 
			this.Property(t => t.PayType).HasColumnName("PayType"); 
			this.Property(t => t.PayRemark).HasColumnName("PayRemark"); 
			this.Property(t => t.ContractStartTime).HasColumnName("ContractStartTime"); 
			this.Property(t => t.ContractEndTime).HasColumnName("ContractEndTime"); 
			this.Property(t => t.IsFrameContract).HasColumnName("IsFrameContract"); 
			this.Property(t => t.DraftTime).HasColumnName("DraftTime"); 
			this.Property(t => t.ProjectAllTotal).HasColumnName("ProjectAllTotal"); 
			this.Property(t => t.MISMoney).HasColumnName("MISMoney"); 
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			this.Property(t => t.NotPay).HasColumnName("NotPay"); 
			this.Property(t => t.MustPay).HasColumnName("MustPay"); 
		 }
	 }
}
