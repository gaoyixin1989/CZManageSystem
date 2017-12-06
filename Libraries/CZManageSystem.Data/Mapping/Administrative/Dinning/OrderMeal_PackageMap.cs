using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_PackageMap : EntityTypeConfiguration<OrderMeal_Package>
    {
        public OrderMeal_PackageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 套餐名称
            /// <summary>
            this.Property(t => t.PackageName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 套餐所属餐时
            /// <summary>
            this.Property(t => t.MealTimeType)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 描述
            /// <summary>
            this.Property(t => t.Discription)

          .HasMaxLength(255);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_Package");
            this.Property(t => t.Id).HasColumnName("Id");
            // 套餐名称
            this.Property(t => t.PackageName).HasColumnName("PackageName");
            // 套餐价格
            this.Property(t => t.PackagePrice).HasColumnName("PackagePrice");
            // 套餐所属餐时ID
            this.Property(t => t.MealTimeID).HasColumnName("MealTimeID");
            // 套餐所属餐时
            this.Property(t => t.MealTimeType).HasColumnName("MealTimeType");
            // 套餐所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            // 描述
            this.Property(t => t.Discription).HasColumnName("Discription");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");


        }
    }
}
