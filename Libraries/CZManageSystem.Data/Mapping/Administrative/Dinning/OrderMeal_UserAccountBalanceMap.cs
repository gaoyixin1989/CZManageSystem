using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_UserAccountBalanceMap : EntityTypeConfiguration<OrderMeal_UserAccountBalance>
    {
        public OrderMeal_UserAccountBalanceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.UserName)

          .HasMaxLength(50);
            this.Property(t => t.LoginName)

          .HasMaxLength(50);
            this.Property(t => t.MealCardID)

          .HasMaxLength(50);
            this.Property(t => t.RecordType)

          .HasMaxLength(50);
            this.Property(t => t.Reason)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_UserAccountBalance");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Balance).HasColumnName("Balance");
            this.Property(t => t.RecordTime).HasColumnName("RecordTime");
            this.Property(t => t.UserBaseinfoID).HasColumnName("UserBaseinfoID");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.MealCardID).HasColumnName("MealCardID");
            this.Property(t => t.RecordType).HasColumnName("RecordType");
            this.Property(t => t.Payments).HasColumnName("Payments");
            this.Property(t => t.Reason).HasColumnName("Reason");

        }
    }

}
