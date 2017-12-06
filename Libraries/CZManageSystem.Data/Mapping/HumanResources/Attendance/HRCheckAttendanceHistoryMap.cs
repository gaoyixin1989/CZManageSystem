using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.HumanResources.Attendance
{
    public class HRCheckAttendanceHistoryMap : EntityTypeConfiguration<HRCheckAttendanceHistory>
    {
        public HRCheckAttendanceHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.AttendanceHistoryId);
            /// <summary>
            /// �ϰ�Ǽ�IP
            /// <summary>
            this.Property(t => t.IpOn)

          .HasMaxLength(50);
            /// <summary>
            /// �°�Ǽ�IP
            /// <summary>
            this.Property(t => t.IpOff)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("HRCheckAttendanceHistory");
            this.Property(t => t.AttendanceHistoryId).HasColumnName("AttendanceHistoryId");
            // �û�ID
            this.Property(t => t.UserId).HasColumnName("UserId");
            // ����
            this.Property(t => t.AtDate).HasColumnName("AtDate");
            this.Property(t => t.OffDate).HasColumnName("OffDate");
            // �ϰ�ʱ��
            this.Property(t => t.DoTime).HasColumnName("DoTime");
            this.Property(t => t.OffTime).HasColumnName("OffTime");
            this.Property(t => t.DoReallyTime).HasColumnName("DoReallyTime");
            this.Property(t => t.OffReallyTime).HasColumnName("OffReallyTime");
            this.Property(t => t.DoReallyDate).HasColumnName("DoReallyDate");
            this.Property(t => t.OffReallyDate).HasColumnName("OffReallyDate");
            // �ϰ�Ǽ�IP
            this.Property(t => t.IpOn).HasColumnName("IpOn");
            // �°�Ǽ�IP
            this.Property(t => t.IpOff).HasColumnName("IpOff");
            // ����
            this.Property(t => t.Minute).HasColumnName("Minute");
            // 1�����걨��2���ݼ٣�3�����
            this.Property(t => t.DoFlag).HasColumnName("DoFlag");
            // ���ݱ�־��1��Ϊ����״̬
            this.Property(t => t.RotateDaysOffFlag).HasColumnName("RotateDaysOffFlag");
            // �ϰࣺ1��ָ�ƵǼǣ�2���ֻ��Ǽǣ�3��ͨ�����Ǽǣ�4���Ž����Ǽ�
            this.Property(t => t.FlagOn).HasColumnName("FlagOn");
            // �°ࣺ1��ָ�ƵǼǣ�2���ֻ��Ǽǣ�3��ͨ�����Ǽǣ�4���Ž����Ǽ�
            this.Property(t => t.FlagOff).HasColumnName("FlagOff");
        }
    }
}
