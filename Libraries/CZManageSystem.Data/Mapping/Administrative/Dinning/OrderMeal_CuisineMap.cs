using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_CuisineMap : EntityTypeConfiguration<OrderMeal_Cuisine>
    {
        public OrderMeal_CuisineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 菜式名称
            /// <summary>
            this.Property(t => t.CuisineName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 菜式类型
            /// <summary>
            this.Property(t => t.CuisineType)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_Cuisine");
            this.Property(t => t.Id).HasColumnName("Id");
            // 菜式名称
            this.Property(t => t.CuisineName).HasColumnName("CuisineName");
            // 菜式类型
            this.Property(t => t.CuisineType).HasColumnName("CuisineType");
            // 所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }
    }
}
