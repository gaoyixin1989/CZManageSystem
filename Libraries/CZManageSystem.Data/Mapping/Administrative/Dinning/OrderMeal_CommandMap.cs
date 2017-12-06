using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_CommandMap : EntityTypeConfiguration<OrderMeal_Command>
    {
        public OrderMeal_CommandMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 订餐命令
            /// <summary>
            this.Property(t => t.Command)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_Command");
            this.Property(t => t.Id).HasColumnName("Id");
            // 用餐地点ID
            this.Property(t => t.PlaceId).HasColumnName("PlaceId");
            // 套餐ID
            this.Property(t => t.PackageId).HasColumnName("PackageId");
            // 订餐命令
            this.Property(t => t.Command).HasColumnName("Command");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");


        }
    }
}
