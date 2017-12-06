using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ÏîÄ¿²éÑ¯
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
    public class V_InvestProjectQueryMap : EntityTypeConfiguration<V_InvestProjectQuery>
    {
        public V_InvestProjectQueryMap()
        {
            // Primary Key
            this.HasKey(t => t.ProjectID);
            this.Property(t => t.TaskID)

          .HasMaxLength(1000);
          //  this.Property(t => t.ProjectID)

          //.HasMaxLength(50);
            this.Property(t => t.ProjectName)

          .HasMaxLength(1000);
            this.Property(t => t.BeginEnd)

          .HasMaxLength(50);
            this.Property(t => t.Content)

          .HasMaxLength(2147483647);
            this.Property(t => t.FinishDate)

          .HasMaxLength(200);
            this.Property(t => t.DpCode)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("V_InvestProjectQuery");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.TaskID).HasColumnName("TaskID");
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            this.Property(t => t.BeginEnd).HasColumnName("BeginEnd");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.YearTotal).HasColumnName("YearTotal");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.FinishDate).HasColumnName("FinishDate");
            this.Property(t => t.DpCode).HasColumnName("DpCode");
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.ManagerID).HasColumnName("ManagerID");
            this.Property(t => t.SignTotal).HasColumnName("SignTotal");
            this.Property(t => t.NotPay).HasColumnName("NotPay");
            this.Property(t => t.Pay).HasColumnName("Pay");
            this.Property(t => t.MISMoney).HasColumnName("MISMoney");
            this.Property(t => t.MustPay).HasColumnName("MustPay");
            this.Property(t => t.ProjectRate).HasColumnName("ProjectRate");
            this.Property(t => t.TransferRate).HasColumnName("TransferRate");
            this.Property(t => t.BackYearTotal).HasColumnName("BackYearTotal");
            this.Property(t => t.YearMustPay).HasColumnName("YearMustPay");
            this.Property(t => t.YearRate).HasColumnName("YearRate");
        }
    }
}
