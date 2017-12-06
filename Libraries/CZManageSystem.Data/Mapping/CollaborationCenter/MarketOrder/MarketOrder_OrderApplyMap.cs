using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// Ӫ������-Ӫ����������
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.MarketOrder
{
    public class MarketOrder_OrderApplyMap : EntityTypeConfiguration<MarketOrder_OrderApply>
    {
        public MarketOrder_OrderApplyMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplyID);
            /// <summary>
            /// �������
            /// <summary>
            this.Property(t => t.SerialNo)

          .HasMaxLength(50);
            /// <summary>
            /// �������ֻ�����
            /// <summary>
            this.Property(t => t.MobilePh)

          .HasMaxLength(50);
            /// <summary>
            /// ����״̬
            /// <summary>
            this.Property(t => t.Status)

          .HasMaxLength(50);
            /// <summary>
            /// ����״̬
            /// <summary>
            this.Property(t => t.OrderStatus)

          .HasMaxLength(50);
            /// <summary>
            /// ����
            /// <summary>
            this.Property(t => t.Title)

          .HasMaxLength(500);
            /// <summary>
            /// Ŀ��ͻ�����
            /// <summary>
            this.Property(t => t.CustomPhone)

          .HasMaxLength(50);
            /// <summary>
            /// �ͻ�����
            /// <summary>
            this.Property(t => t.CustomName)

          .HasMaxLength(50);
            /// <summary>
            /// �ͻ����֤��
            /// <summary>
            this.Property(t => t.CustomPersonID)

          .HasMaxLength(50);
            /// <summary>
            /// �ͻ���ϵ��ַ
            /// <summary>
            this.Property(t => t.CustomAddr)

          .HasMaxLength(500);
            /// <summary>
            /// �ͻ���ϵ�绰
            /// <summary>
            this.Property(t => t.CustomOther)

          .HasMaxLength(50);
            /// <summary>
            /// ���ú���
            /// <summary>
            this.Property(t => t.UseNumber)

          .HasMaxLength(50);
            /// <summary>
            /// SIM����
            /// <summary>
            this.Property(t => t.SIMNumber)

          .HasMaxLength(50);
            /// <summary>
            /// ����-IMEI��
            /// <summary>
            this.Property(t => t.IMEI)

          .HasMaxLength(50);
            /// <summary>
            /// ��ע
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(2147483647);
            /// <summary>
            /// ��Ŀ���
            /// <summary>
            this.Property(t => t.ProjectID)

          .HasMaxLength(100);

            /// <summary>
            /// ����״̬�����ͳ�ʱ��
            /// <summary>
            this.Property(t => t.SendStatus).HasMaxLength(50);
            this.Property(t => t.GDOrderID).HasMaxLength(50);
            this.Property(t => t.BossOfferID).HasMaxLength(50);
            this.Property(t => t.MainOrder).HasMaxLength(50);
            this.Property(t => t.SubOrder).HasMaxLength(50);
            this.Property(t => t.MailNo).HasMaxLength(50);
            /// <summary>
            /// ���Ͷ����˺�
            /// <summary>
            this.Property(t => t.SendTo).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("MarketOrder_OrderApply");
            // ����ID
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            // ����ʵ��ID
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            // �������
            this.Property(t => t.SerialNo).HasColumnName("SerialNo");
            // ����ʱ��
            this.Property(t => t.ApplyTime).HasColumnName("ApplyTime");
            // ������ID
            this.Property(t => t.Applicant).HasColumnName("Applicant");
            // �������ֻ�����
            this.Property(t => t.MobilePh).HasColumnName("MobilePh");
            // ����״̬
            this.Property(t => t.Status).HasColumnName("Status");
            // ����״̬
            this.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
            // ����
            this.Property(t => t.Title).HasColumnName("Title");
            // Ӫ������
            this.Property(t => t.MarketID).HasColumnName("MarketID");
            // Ŀ��ͻ�����
            this.Property(t => t.CustomPhone).HasColumnName("CustomPhone");
            // �ͻ�����
            this.Property(t => t.CustomName).HasColumnName("CustomName");
            // �ͻ����֤��
            this.Property(t => t.CustomPersonID).HasColumnName("CustomPersonID");
            // �ͻ���ϵ��ַ
            this.Property(t => t.CustomAddr).HasColumnName("CustomAddr");
            // �ͻ���ϵ�绰
            this.Property(t => t.CustomOther).HasColumnName("CustomOther");
            // �ն˻���ID
            this.Property(t => t.EndTypeID).HasColumnName("EndTypeID");
            // ���ú���
            this.Property(t => t.UseNumber).HasColumnName("UseNumber");
            // SIM����
            this.Property(t => t.SIMNumber).HasColumnName("SIMNumber");
            // ����-IMEI��
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            // �����ײ�ID
            this.Property(t => t.SetmealID).HasColumnName("SetmealID");
            // ����ҵ��
            this.Property(t => t.BusinessID).HasColumnName("BusinessID");
            // ��ע
            this.Property(t => t.Remark).HasColumnName("Remark");
            // ��������ID
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            // ��Ŀ���
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.YZSubmitTime).HasColumnName("YZSubmitTime");
            //����״̬�����ͳ�ʱ��
            this.Property(t => t.SendStatus).HasColumnName("SendStatus");
            this.Property(t => t.GDOrderID).HasColumnName("GDOrderID");
            this.Property(t => t.BossOfferID).HasColumnName("BossOfferID");
            this.Property(t => t.MainOrder).HasColumnName("MainOrder");
            this.Property(t => t.SubOrder).HasColumnName("SubOrder");
            this.Property(t => t.MailNo).HasColumnName("MailNo");
            //���Ͷ����˺�
            this.Property(t => t.SendTo).HasColumnName("SendTo");

            // Relationships
            this.HasOptional(t => t.Tracking_Workflow).WithMany(t => t.MarketOrder_OrderApplys).HasForeignKey(d => d.WorkflowInstanceId);
            this.HasOptional(t => t.ApplicantObj).WithMany().HasForeignKey(d => d.Applicant);
            this.HasOptional(t => t.MarketObj).WithMany().HasForeignKey(d => d.MarketID);
            this.HasOptional(t => t.EndTypeObj).WithMany().HasForeignKey(d => d.EndTypeID);
            this.HasOptional(t => t.SetmealObj).WithMany().HasForeignKey(d => d.SetmealID);
            this.HasOptional(t => t.BusinessObj).WithMany().HasForeignKey(d => d.BusinessID);
            this.HasOptional(t => t.AreaObj).WithMany().HasForeignKey(d => d.AreaID);
            this.HasOptional(t => t.AuthenticationObj).WithMany().HasForeignKey(d => d.AuthenticationID);
        }
    }
}
