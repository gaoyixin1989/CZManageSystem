using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_MenuMap : EntityTypeConfiguration<OrderMeal_Menu>
    {
        public OrderMeal_MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 菜谱名称
            /// <summary>
            this.Property(t => t.MenuName)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 用餐时段
            /// <summary>
            this.Property(t => t.MealTimeType)
           .IsRequired()
          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_Menu");
            this.Property(t => t.Id).HasColumnName("Id");
            // 菜谱名称
            this.Property(t => t.MenuName).HasColumnName("MenuName");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            // 菜谱可用日期
            this.Property(t => t.WorkingDate).HasColumnName("WorkingDate");
            this.Property(t => t.MealTimeID).HasColumnName("MealTimeID");
            // 用餐时段
            this.Property(t => t.MealTimeType).HasColumnName("MealTimeType");
            // 所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.SendTimes).HasColumnName("SendTimes");
            // 是否发送信息
            this.Property(t => t.CanSendSms).HasColumnName("CanSendSms");
            this.Property(t => t.Bookflag).HasColumnName("Bookflag");
            this.Property(t => t.IsCompleted).HasColumnName("IsCompleted");
            this.Property(t => t.IsPreOrder).HasColumnName("IsPreOrder");

        }

    }
}
