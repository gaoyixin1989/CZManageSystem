using CZManageSystem.Data.Domain.Administrative.Dinning;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_UserBaseinfoMap : EntityTypeConfiguration<OrderMeal_UserBaseinfo>
    {
        public OrderMeal_UserBaseinfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            /// <summary>
            /// 用户名
            /// <summary>
            this.Property(t => t.RealName)

          .HasMaxLength(50);
            /// <summary>
            /// 用户登录名
            /// <summary>
            this.Property(t => t.LoginName)

          .HasMaxLength(50);
            /// <summary>
            /// 员工编号
            /// <summary>
            this.Property(t => t.EmployId)

          .HasMaxLength(20);
            this.Property(t => t.MealCardID)

          .HasMaxLength(50);
            /// <summary>
            /// 手机号码
            /// <summary>
            this.Property(t => t.Telephone)

          .HasMaxLength(15);
            /// <summary>
            /// 部门
            /// <summary>
            this.Property(t => t.DeptId)

          .HasMaxLength(255);
            // Table & Column Mappings
            this.ToTable("OrderMeal_UserBaseinfo");
            this.Property(t => t.Id).HasColumnName("Id");
            // 用户名
            this.Property(t => t.RealName).HasColumnName("RealName");
            // 用户登录名
            this.Property(t => t.LoginName).HasColumnName("LoginName");
            // 员工编号
            this.Property(t => t.EmployId).HasColumnName("EmployId");
            this.Property(t => t.MealCardID).HasColumnName("MealCardID");
            // 手机号码
            this.Property(t => t.Telephone).HasColumnName("Telephone");
            // 部门
            this.Property(t => t.DeptId).HasColumnName("DeptId");
            // 状态
            this.Property(t => t.State).HasColumnName("State");
            // 余额
            this.Property(t => t.Balance).HasColumnName("Balance");

        }
    }
}
