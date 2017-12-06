using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMsg_LogMap : EntityTypeConfiguration<OrderMsg_Log>
    {
        public OrderMsg_LogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.Number)

          .HasMaxLength(50);
            this.Property(t => t.MealTimeType)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMsg_Log");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RoomId).HasColumnName("RoomId");
            // 子端口号,早餐1午餐2晚餐3其他4
            this.Property(t => t.Num).HasColumnName("Num");
            // 0已发送,1已回复,2已过期
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.WorkingDate).HasColumnName("WorkingDate");
            this.Property(t => t.InsertDate).HasColumnName("InsertDate");
            this.Property(t => t.Number).HasColumnName("Number");
            this.Property(t => t.MealTimeType).HasColumnName("MealTimeType");
        }
    }
}
