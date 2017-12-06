
using System.Data.Entity.ModelConfiguration;
using CZManageSystem.Data.Domain.ITSupport;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class ConsumableMap : EntityTypeConfiguration<Consumable>
	{
		public ConsumableMap()
		{
            // Primary Key
            this.HasKey(t=>t.ID);
            this.Property(t => t.ID);
		/// <summary>
		/// 耗材类别
		/// <summary>
			  this.Property(t => t.Type)
			.HasMaxLength(100);
		/// <summary>
		/// 耗材型号
		/// <summary>
			  this.Property(t => t.Model)
			.HasMaxLength(100);
		/// <summary>
		/// 耗材名称
		/// <summary>
			  this.Property(t => t.Name)
			.HasMaxLength(100);
		/// <summary>
		/// 适用设备
		/// <summary>
			  this.Property(t => t.Equipment)
			.HasMaxLength(100);
		/// <summary>
		/// 单位
		/// <summary>
			  this.Property(t => t.Unit)
			.HasMaxLength(100);
		/// <summary>
		/// 耗材品牌
		/// <summary>
			  this.Property(t => t.Trademark)
			.HasMaxLength(100);
            /// <summary>
            /// 价值类型，0低价值，1非低价值
            /// <summary>
            this.Property(t => t.IsValue).HasMaxLength(100);

            //备注
            this.Property(t => t.Remark).HasMaxLength(100);
            //是否删除  0代表否  1代表是
            this.Property(t => t.IsDelete).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Consumable"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 耗材类别
			this.Property(t => t.Type).HasColumnName("Type"); 
			// 耗材型号
			this.Property(t => t.Model).HasColumnName("Model"); 
			// 耗材名称
			this.Property(t => t.Name).HasColumnName("Name"); 
			// 适用设备
			this.Property(t => t.Equipment).HasColumnName("Equipment"); 
			// 单位
			this.Property(t => t.Unit).HasColumnName("Unit"); 
			// 耗材品牌
			this.Property(t => t.Trademark).HasColumnName("Trademark"); 
			// 是否低值
			this.Property(t => t.IsValue).HasColumnName("IsValue");
            // 耗材当前拥有量
            this.Property(t => t.Amount).HasColumnName("Amount");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            //是否删除  0代表否  1代表是
            this.Property(t => t.IsDelete).HasColumnName("IsDelete");
        }
	 }
}
