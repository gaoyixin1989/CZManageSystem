using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public  class OrderMeal_BookOrderMap : EntityTypeConfiguration<OrderMeal_BookOrder>
    {
        public OrderMeal_BookOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 订单号
            /// <summary>
            this.Property(t => t.OrderNum)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 用户名
            /// <summary>
            this.Property(t => t.UserName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 登录名
            /// <summary>
            this.Property(t => t.LoginName)
           .IsRequired()
          .HasMaxLength(50);
            this.Property(t => t.MealCardID)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 订餐食堂
            /// <summary>
            this.Property(t => t.DinningRoomName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 套餐
            /// <summary>
            this.Property(t => t.PackageName)

          .HasMaxLength(50);
            /// <summary>
            /// 餐时
            /// <summary>
            this.Property(t => t.MealTimeType)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 用餐地点
            /// <summary>
            this.Property(t => t.MealPlaceName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 订单状态
            /// <summary>
            this.Property(t => t.OrderStateName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 描述
            /// <summary>
            this.Property(t => t.Discription)

          .HasMaxLength(255);
            // Table & Column Mappings
            this.ToTable("OrderMeal_BookOrder");
            this.Property(t => t.Id).HasColumnName("Id");
            // 订单号
            this.Property(t => t.OrderNum).HasColumnName("OrderNum");
            // 订餐系统用户ID
            this.Property(t => t.UserBaseinfoID).HasColumnName("UserBaseinfoID");
            // 用户名
            this.Property(t => t.UserName).HasColumnName("UserName");
            // 登录名
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            this.Property(t => t.MealCardID).HasColumnName("MealCardID");
            // 订餐食堂id
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            // 订餐食堂
            this.Property(t => t.DinningRoomName).HasColumnName("DinningRoomName");
            // 套餐ID
            this.Property(t => t.PackageID).HasColumnName("PackageID");
            // 套餐
            this.Property(t => t.PackageName).HasColumnName("PackageName");
            // 套餐价格
            this.Property(t => t.PackagePrice).HasColumnName("PackagePrice");
            this.Property(t => t.MealTimeID).HasColumnName("MealTimeID");
            // 餐时
            this.Property(t => t.MealTimeType).HasColumnName("MealTimeType");
            // 用餐地点ID
            this.Property(t => t.MealPlaceID).HasColumnName("MealPlaceID");
            // 用餐地点
            this.Property(t => t.MealPlaceName).HasColumnName("MealPlaceName");
            // 预定时间
            this.Property(t => t.OrderTime).HasColumnName("OrderTime");
            // 预定状态
            this.Property(t => t.OrderState).HasColumnName("OrderState");
            // 订单状态
            this.Property(t => t.OrderStateName).HasColumnName("OrderStateName");
            // 描述
            this.Property(t => t.Discription).HasColumnName("Discription");
            // 用餐时间
            this.Property(t => t.DinningDate).HasColumnName("DinningDate");
            // 有效时间-开始
            this.Property(t => t.StartedDate).HasColumnName("StartedDate");
            // 有效时间-结束
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            // 预定类型（一周，一个月）
            this.Property(t => t.BookType).HasColumnName("BookType");
            // 预定状态
            this.Property(t => t.Flag).HasColumnName("Flag");
            // 预定后的余额
            this.Property(t => t.AfterOrderBalance).HasColumnName("AfterOrderBalance");
        }

    }
}
