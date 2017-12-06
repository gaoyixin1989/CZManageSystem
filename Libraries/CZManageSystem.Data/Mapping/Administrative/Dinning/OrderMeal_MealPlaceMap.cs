using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_MealPlaceMap : EntityTypeConfiguration<OrderMeal_MealPlace>
    {
        public OrderMeal_MealPlaceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 用餐地点
            /// <summary>
            this.Property(t => t.MealPlaceName)

          .HasMaxLength(50);
            /// <summary>
            /// 用餐地点简称
            /// <summary>
            this.Property(t => t.MealPlaceShortName)

          .HasMaxLength(50);
            /// <summary>
            /// 用餐地点描述
            /// <summary>
            this.Property(t => t.Discription)

          .HasMaxLength(255);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_MealPlace");
            this.Property(t => t.Id).HasColumnName("Id");
            // 用餐地点
            this.Property(t => t.MealPlaceName).HasColumnName("MealPlaceName");
            // 用餐地点简称
            this.Property(t => t.MealPlaceShortName).HasColumnName("MealPlaceShortName");
            // 所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            // 用餐地点描述
            this.Property(t => t.Discription).HasColumnName("Discription");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }
    }
}
