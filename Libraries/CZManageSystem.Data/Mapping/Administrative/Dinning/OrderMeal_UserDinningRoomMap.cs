using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_UserDinningRoomMap : EntityTypeConfiguration<OrderMeal_UserDinningRoom>
    {
        public OrderMeal_UserDinningRoomMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            // Table & Column Mappings
            this.ToTable("OrderMeal_UserDinningRoom");
            this.Property(t => t.Id).HasColumnName("Id");
            // 所属食堂
            this.Property(t => t.DinningRoomID).HasColumnName("DinningRoomID");
            // 食堂用户ID (OrderMeal_UserBaseinfo中的ID)
            this.Property(t => t.UserBaseinfoID).HasColumnName("UserBaseinfoID");
            // 是否接受所属食堂的菜谱短信通知
            this.Property(t => t.GetSms).HasColumnName("GetSms");
        }
    }
}
