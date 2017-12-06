using CZManageSystem.Data.Domain.HumanResources.Employees;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Employees
{
    public class HRLzUserInfoMap : EntityTypeConfiguration<HRLzUserInfo>
    {
        public HRLzUserInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.EmployeeId);
            /// <summary>
            /// ְλְ��
            /// <summary>
            this.Property(t => t.PositionRank)

          .HasMaxLength(50);
            /// <summary>
            /// ����ְ��
            /// <summary>
            this.Property(t => t.SetIntoTheRanks)

          .HasMaxLength(50);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(50);
            /// <summary>
            /// �޸���
            /// <summary>
            this.Property(t => t.LastModFier)

          .HasMaxLength(50);
            /// <summary>
            /// ��λ
            /// <summary>
            this.Property(t => t.Gears)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRLzUserInfo");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // ְλְ��
            this.Property(t => t.PositionRank).HasColumnName("PositionRank");
            // ����ְ��
            this.Property(t => t.SetIntoTheRanks).HasColumnName("SetIntoTheRanks");
            // ��λֵ
            this.Property(t => t.Tantile).HasColumnName("Tantile");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            // ����ʱ��
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
            // �޸���
            this.Property(t => t.LastModFier).HasColumnName("LastModFier");
            // ��λ
            this.Property(t => t.Gears).HasColumnName("Gears");
            this.HasOptional(t => t.Users).WithMany ().HasForeignKey(t=>t.UserId );

        }
    }
}
