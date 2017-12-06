using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    class tbcz_OrderMeal_MealOrderMap : EntityTypeConfiguration<tbcz_OrderMeal_MealOrder>
    {
        public tbcz_OrderMeal_MealOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.UserName)

          .HasMaxLength(50);
            this.Property(t => t.DeptName)

          .HasMaxLength(50);
            this.Property(t => t.MealCardID)

          .HasMaxLength(50);
            this.Property(t => t.MealTimeType)

          .HasMaxLength(50);
            this.Property(t => t.DinningRoomName)

          .HasMaxLength(50);
            this.Property(t => t.MealPlaceName)

          .HasMaxLength(50);
            this.Property(t => t.PackageName)

          .HasMaxLength(50);
            this.Property(t => t.MealTime)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("tbcz_OrderMeal_MealOrder");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.DeptName).HasColumnName("DeptName");
            this.Property(t => t.MealCardID).HasColumnName("MealCardID");
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
            this.Property(t => t.MealTimeType).HasColumnName("MealTimeType");
            this.Property(t => t.DinningRoomName).HasColumnName("DinningRoomName");
            this.Property(t => t.MealPlaceName).HasColumnName("MealPlaceName");
            this.Property(t => t.PackageName).HasColumnName("PackageName");
            this.Property(t => t.PackagePrice).HasColumnName("PackagePrice");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.MealTime).HasColumnName("MealTime");
        }
    }

}
