using CZManageSystem.Data.Domain.HumanResources.Vacation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Vacation
{
	public class HRVacationAnnualLeaveMap : EntityTypeConfiguration<HRVacationAnnualLeave>
	{
        public HRVacationAnnualLeaveMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            this.Property(t => t.YearDate)

          .HasMaxLength(50);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(1073741823);
            this.Property(t => t.Toflag)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRVacationAnnualLeave");
            // ����ID
            this.Property(t => t.ID).HasColumnName("ID");
            // �û�ID
            this.Property(t => t.UserID).HasColumnName("UserID");
            this.Property(t => t.ComID).HasColumnName("ComID");
            this.Property(t => t.YearDate).HasColumnName("YearDate");
            this.Property(t => t.AnnualLeave).HasColumnName("AnnualLeave");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            // ������ID
            this.Property(t => t.CreateID).HasColumnName("CreateID");
            // ʹ������
            this.Property(t => t.UseDate).HasColumnName("UseDate");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            //���뵥ID
            this.Property(t => t.AppID).HasColumnName("AppID");
            // ���ݼ�����
            this.Property(t => t.SpendDays).HasColumnName("SpendDays");
            // ��ʼʱ��
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            // ����ʱ��
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.Toflag).HasColumnName("Toflag");
        }

    }
}
