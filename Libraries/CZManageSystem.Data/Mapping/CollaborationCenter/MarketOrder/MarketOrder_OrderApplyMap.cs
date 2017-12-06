using CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// 营销订单-营销订单工单
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
            /// 受理单编号
            /// <summary>
            this.Property(t => t.SerialNo)

          .HasMaxLength(50);
            /// <summary>
            /// 发起人手机号码
            /// <summary>
            this.Property(t => t.MobilePh)

          .HasMaxLength(50);
            /// <summary>
            /// 流程状态
            /// <summary>
            this.Property(t => t.Status)

          .HasMaxLength(50);
            /// <summary>
            /// 受理单状态
            /// <summary>
            this.Property(t => t.OrderStatus)

          .HasMaxLength(50);
            /// <summary>
            /// 标题
            /// <summary>
            this.Property(t => t.Title)

          .HasMaxLength(500);
            /// <summary>
            /// 目标客户号码
            /// <summary>
            this.Property(t => t.CustomPhone)

          .HasMaxLength(50);
            /// <summary>
            /// 客户姓名
            /// <summary>
            this.Property(t => t.CustomName)

          .HasMaxLength(50);
            /// <summary>
            /// 客户身份证号
            /// <summary>
            this.Property(t => t.CustomPersonID)

          .HasMaxLength(50);
            /// <summary>
            /// 客户联系地址
            /// <summary>
            this.Property(t => t.CustomAddr)

          .HasMaxLength(500);
            /// <summary>
            /// 客户联系电话
            /// <summary>
            this.Property(t => t.CustomOther)

          .HasMaxLength(50);
            /// <summary>
            /// 可用号码
            /// <summary>
            this.Property(t => t.UseNumber)

          .HasMaxLength(50);
            /// <summary>
            /// SIM卡号
            /// <summary>
            this.Property(t => t.SIMNumber)

          .HasMaxLength(50);
            /// <summary>
            /// 配送-IMEI码
            /// <summary>
            this.Property(t => t.IMEI)

          .HasMaxLength(50);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(2147483647);
            /// <summary>
            /// 项目编号
            /// <summary>
            this.Property(t => t.ProjectID)

          .HasMaxLength(100);

            /// <summary>
            /// 配送状态（配送超时）
            /// <summary>
            this.Property(t => t.SendStatus).HasMaxLength(50);
            this.Property(t => t.GDOrderID).HasMaxLength(50);
            this.Property(t => t.BossOfferID).HasMaxLength(50);
            this.Property(t => t.MainOrder).HasMaxLength(50);
            this.Property(t => t.SubOrder).HasMaxLength(50);
            this.Property(t => t.MailNo).HasMaxLength(50);
            /// <summary>
            /// 发送对象账号
            /// <summary>
            this.Property(t => t.SendTo).HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("MarketOrder_OrderApply");
            // 工单ID
            this.Property(t => t.ApplyID).HasColumnName("ApplyID");
            // 流程实例ID
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            // 受理单编号
            this.Property(t => t.SerialNo).HasColumnName("SerialNo");
            // 发起时间
            this.Property(t => t.ApplyTime).HasColumnName("ApplyTime");
            // 发起人ID
            this.Property(t => t.Applicant).HasColumnName("Applicant");
            // 发起人手机号码
            this.Property(t => t.MobilePh).HasColumnName("MobilePh");
            // 流程状态
            this.Property(t => t.Status).HasColumnName("Status");
            // 受理单状态
            this.Property(t => t.OrderStatus).HasColumnName("OrderStatus");
            // 标题
            this.Property(t => t.Title).HasColumnName("Title");
            // 营销方案
            this.Property(t => t.MarketID).HasColumnName("MarketID");
            // 目标客户号码
            this.Property(t => t.CustomPhone).HasColumnName("CustomPhone");
            // 客户姓名
            this.Property(t => t.CustomName).HasColumnName("CustomName");
            // 客户身份证号
            this.Property(t => t.CustomPersonID).HasColumnName("CustomPersonID");
            // 客户联系地址
            this.Property(t => t.CustomAddr).HasColumnName("CustomAddr");
            // 客户联系电话
            this.Property(t => t.CustomOther).HasColumnName("CustomOther");
            // 终端机型ID
            this.Property(t => t.EndTypeID).HasColumnName("EndTypeID");
            // 可用号码
            this.Property(t => t.UseNumber).HasColumnName("UseNumber");
            // SIM卡号
            this.Property(t => t.SIMNumber).HasColumnName("SIMNumber");
            // 配送-IMEI码
            this.Property(t => t.IMEI).HasColumnName("IMEI");
            // 基本套餐ID
            this.Property(t => t.SetmealID).HasColumnName("SetmealID");
            // 捆绑业务
            this.Property(t => t.BusinessID).HasColumnName("BusinessID");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            // 所属区域ID
            this.Property(t => t.AreaID).HasColumnName("AreaID");
            // 项目编号
            this.Property(t => t.ProjectID).HasColumnName("ProjectID");
            this.Property(t => t.YZSubmitTime).HasColumnName("YZSubmitTime");
            //配送状态（配送超时）
            this.Property(t => t.SendStatus).HasColumnName("SendStatus");
            this.Property(t => t.GDOrderID).HasColumnName("GDOrderID");
            this.Property(t => t.BossOfferID).HasColumnName("BossOfferID");
            this.Property(t => t.MainOrder).HasColumnName("MainOrder");
            this.Property(t => t.SubOrder).HasColumnName("SubOrder");
            this.Property(t => t.MailNo).HasColumnName("MailNo");
            //发送对象账号
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
