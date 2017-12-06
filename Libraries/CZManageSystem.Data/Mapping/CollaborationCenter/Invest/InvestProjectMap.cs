using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 投资项目信息
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
    public class InvestProjectMap : EntityTypeConfiguration<InvestProject>
    {
        public InvestProjectMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.ProjectID).HasMaxLength(50);
            /// <summary>
            /// 计划任务书文号
            /// <summary>
            this.Property(t => t.TaskID)

          .HasMaxLength(1000);
            /// <summary>
            /// 项目名称
            /// <summary>
            this.Property(t => t.ProjectName)

          .HasMaxLength(1000);
            /// <summary>
            /// 起止年限
            /// <summary>
            this.Property(t => t.BeginEnd)

          .HasMaxLength(50);
            /// <summary>
            /// 年度建设内容
            /// <summary>
            this.Property(t => t.Content)

          .HasMaxLength(2147483647);
            /// <summary>
            /// 要求完成时限
            /// <summary>
            this.Property(t => t.FinishDate)

          .HasMaxLength(200);
            /// <summary>
            /// 负责专业室
            /// <summary>
            this.Property(t => t.DpCode)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("InvestProject");
            this.Property(t => t.ID).HasColumnName("ID");
            // 项目编号（唯一）
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            // 下达年份,也是导入时间的年份
            this.Property(t => t.Year).HasColumnName("Year");
            // 计划任务书文号
            this.Property(t => t.TaskID).HasColumnName("TaskID");
            // 项目名称
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            // 起止年限
            this.Property(t => t.BeginEnd).HasColumnName("BeginEnd");
            // 项目总投资
            this.Property(t => t.Total).HasColumnName("Total");
            // 年度项目投资
            this.Property(t => t.YearTotal).HasColumnName("YearTotal");
            // 年度建设内容
            this.Property(t => t.Content).HasColumnName("Content");
            // 要求完成时限
            this.Property(t => t.FinishDate).HasColumnName("FinishDate");
            // 负责专业室
            this.Property(t => t.DpCode).HasColumnName("DpCode");
            // 室负责人
            this.Property(t => t.UserID).HasColumnName("UserID");
            // 项目经理
            this.Property(t => t.ManagerID).HasColumnName("ManagerID");


            //外键
            //this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.DpCode);
            //this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserID);
            //this.HasOptional(t => t.ManagerObj).WithMany().HasForeignKey(d => d.ManagerID);

        }
    }
}
