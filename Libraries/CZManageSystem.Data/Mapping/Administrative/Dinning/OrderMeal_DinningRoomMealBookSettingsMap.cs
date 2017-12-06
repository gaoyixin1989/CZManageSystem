using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_DinningRoomMealBookSettingsMap : EntityTypeConfiguration<OrderMeal_DinningRoomMealBookSettings>
    {
        public OrderMeal_DinningRoomMealBookSettingsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 用餐时段
            /// <summary>
            this.Property(t => t.MealTimeType)
           .IsRequired()
          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_DinningRoomMealBookSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MealTimeID).HasColumnName("MealTimeID");
            // 所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            // 用餐时段
            this.Property(t => t.MealTimeType).HasColumnName("MealTimeType");
            // 是否提供预约
            this.Property(t => t.State).HasColumnName("State");
            // 预约设置 一周内（7天） 
            this.Property(t => t.Week).HasColumnName("Week");
            // 预约设置一个月（7天） 
            this.Property(t => t.Month).HasColumnName("Month");

        }
    }
}
