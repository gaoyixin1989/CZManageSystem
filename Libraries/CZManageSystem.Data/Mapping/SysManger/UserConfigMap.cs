using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class UserConfigMap : EntityTypeConfiguration<UserConfig>
	{
		public UserConfigMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 配置标识
		/// <summary>
			  this.Property(t => t.ConfigName)

			.HasMaxLength(500);
		/// <summary>
		/// 配置值
		/// <summary>
			  this.Property(t => t.ConfigValue)
;
			// Table & Column Mappings
 			 this.ToTable("UserConfig"); 
			// 编号
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 用户id
			this.Property(t => t.UserID).HasColumnName("UserID"); 
			// 配置标识
			this.Property(t => t.ConfigName).HasColumnName("ConfigName"); 
			// 配置值
			this.Property(t => t.ConfigValue).HasColumnName("ConfigValue");

            //外键
           this.HasOptional(t => t.UserObj).WithMany().HasForeignKey(d => d.UserID);
        }
	 }
}
