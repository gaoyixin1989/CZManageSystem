using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ͷ����Ŀ��Ϣ
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
            /// �ƻ��������ĺ�
            /// <summary>
            this.Property(t => t.TaskID)

          .HasMaxLength(1000);
            /// <summary>
            /// ��Ŀ����
            /// <summary>
            this.Property(t => t.ProjectName)

          .HasMaxLength(1000);
            /// <summary>
            /// ��ֹ����
            /// <summary>
            this.Property(t => t.BeginEnd)

          .HasMaxLength(50);
            /// <summary>
            /// ��Ƚ�������
            /// <summary>
            this.Property(t => t.Content)

          .HasMaxLength(2147483647);
            /// <summary>
            /// Ҫ�����ʱ��
            /// <summary>
            this.Property(t => t.FinishDate)

          .HasMaxLength(200);
            /// <summary>
            /// ����רҵ��
            /// <summary>
            this.Property(t => t.DpCode)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("InvestProject");
            this.Property(t => t.ID).HasColumnName("ID");
            // ��Ŀ��ţ�Ψһ��
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            // �´����,Ҳ�ǵ���ʱ������
            this.Property(t => t.Year).HasColumnName("Year");
            // �ƻ��������ĺ�
            this.Property(t => t.TaskID).HasColumnName("TaskID");
            // ��Ŀ����
            this.Property(t => t.ProjectName).HasColumnName("ProjectName");
            // ��ֹ����
            this.Property(t => t.BeginEnd).HasColumnName("BeginEnd");
            // ��Ŀ��Ͷ��
            this.Property(t => t.Total).HasColumnName("Total");
            // �����ĿͶ��
            this.Property(t => t.YearTotal).HasColumnName("YearTotal");
            // ��Ƚ�������
            this.Property(t => t.Content).HasColumnName("Content");
            // Ҫ�����ʱ��
            this.Property(t => t.FinishDate).HasColumnName("FinishDate");
            // ����רҵ��
            this.Property(t => t.DpCode).HasColumnName("DpCode");
            // �Ҹ�����
            this.Property(t => t.UserID).HasColumnName("UserID");
            // ��Ŀ����
            this.Property(t => t.ManagerID).HasColumnName("ManagerID");


            //���
            //this.HasOptional(t => t.DeptObj).WithMany().HasForeignKey(d => d.DpCode);
            //this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserID);
            //this.HasOptional(t => t.ManagerObj).WithMany().HasForeignKey(d => d.ManagerID);

        }
    }
}
