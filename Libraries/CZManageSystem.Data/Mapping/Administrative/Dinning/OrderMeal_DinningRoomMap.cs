using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_DinningRoomMap : EntityTypeConfiguration<OrderMeal_DinningRoom>
    {
        public OrderMeal_DinningRoomMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 食堂名称
            /// <summary>
            this.Property(t => t.DinningRoomName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 食堂简介
            /// <summary>
            this.Property(t => t.Discription)

          .HasMaxLength(255);
            // Table & Column Mappings
            this.ToTable("OrderMeal_DinningRoom");
            this.Property(t => t.Id).HasColumnName("Id");
            // 食堂名称
            this.Property(t => t.DinningRoomName).HasColumnName("DinningRoomName");
            // 食堂简介
            this.Property(t => t.Discription).HasColumnName("Discription");
        }
    }

}
