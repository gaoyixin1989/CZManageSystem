using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_UserAccountRechargeMap : EntityTypeConfiguration<OrderMeal_UserAccountRecharge>
    {
        public OrderMeal_UserAccountRechargeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.OrderNum)

          .HasMaxLength(50);
            this.Property(t => t.FeedbackID)

          .HasMaxLength(50);
            this.Property(t => t.UserName)

          .HasMaxLength(50);
            this.Property(t => t.LoginName)

          .HasMaxLength(50);
            this.Property(t => t.MealCardID)

          .HasMaxLength(50);
            this.Property(t => t.RechargeAdminName)

          .HasMaxLength(50);
            this.Property(t => t.RechargeAdminloginname)

          .HasMaxLength(50);
            this.Property(t => t.Discription)

          .HasMaxLength(255);
            // Table & Column Mappings
            this.ToTable("OrderMeal_UserAccountRecharge");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderNum).HasColumnName("OrderNum");
            this.Property(t => t.FeedbackID).HasColumnName("FeedbackID");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.UserBaseinfoID).HasColumnName("UserBaseinfoID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.MealCardID).HasColumnName("MealCardID");
            this.Property(t => t.UpDateTime).HasColumnName("UpDateTime");
            this.Property(t => t.RechargeType).HasColumnName("RechargeType");
            this.Property(t => t.RechargeState).HasColumnName("RechargeState");
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.BeforeRechargeBalance).HasColumnName("BeforeRechargeBalance");
            this.Property(t => t.AfterRechargeBalance).HasColumnName("AfterRechargeBalance");
            this.Property(t => t.RechargeAdminName).HasColumnName("RechargeAdminName");
            this.Property(t => t.RechargeAdminloginname).HasColumnName("RechargeAdminloginname");
            this.Property(t => t.Discription).HasColumnName("Discription");

        }
    }

}
