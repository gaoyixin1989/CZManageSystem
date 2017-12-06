using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_DinningRoomAdminMap : EntityTypeConfiguration<OrderMeal_DinningRoomAdmin>
    {
        public OrderMeal_DinningRoomAdminMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 分食堂管理员登录名
            /// <summary>
            this.Property(t => t.Loginname)

          .HasMaxLength(50);
            /// <summary>
            /// 分食堂管理员
            /// <summary>
            this.Property(t => t.RealName)

          .HasMaxLength(50);
            this.Property(t => t.AdminType)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_DinningRoomAdmin");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            // 分食堂管理员登录名
            this.Property(t => t.Loginname).HasColumnName("Loginname");
            // 所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            // 分食堂管理员
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.AdminType).HasColumnName("AdminType");

        }

    }

}
