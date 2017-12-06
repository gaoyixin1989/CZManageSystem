using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_DinningRoomMealTimeSettingsMap : EntityTypeConfiguration<OrderMeal_DinningRoomMealTimeSettings>
    {
        public OrderMeal_DinningRoomMealTimeSettingsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 用餐时段
            /// <summary>
            this.Property(t => t.MealTimeType)
           .IsRequired()
          .HasMaxLength(50);
            /// <summary>
            /// 创建者
            /// <summary>
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            /// <summary>
            /// 修改者
            /// <summary>
            this.Property(t => t.LastModifier)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_DinningRoomMealTimeSettings");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MealTimeID).HasColumnName("MealTimeID");
            // 所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            // 订餐时间-开始
            this.Property(t => t.BeginTime).HasColumnName("BeginTime");
            // 订餐时间-结束
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            // 最晚退餐时间
            this.Property(t => t.ClosePayBackTime).HasColumnName("ClosePayBackTime");
            // 短信发送时间-开始
            this.Property(t => t.SmsTime).HasColumnName("SmsTime");
            // 短信发送时间-结束
            this.Property(t => t.LastSmsTime).HasColumnName("LastSmsTime");
            // 用餐时段
            this.Property(t => t.MealTimeType).HasColumnName("MealTimeType");
            // 是否提供用餐
            this.Property(t => t.State).HasColumnName("State");
            // 记录推送时间-开始
            this.Property(t => t.OrderMealRecordSendTime).HasColumnName("OrderMealRecordSendTime");
            // 记录推送时间-结束
            this.Property(t => t.LastOrderMealRecordSendTime).HasColumnName("LastOrderMealRecordSendTime");
            // 创建者
            this.Property(t => t.Creator).HasColumnName("Creator");
            // 创建时间
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            // 修改者
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            // 最近修改时间
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");
        }

    }

}
