
using CZManageSystem.Data.Domain.ITSupport;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class Consumable_InputListMap : EntityTypeConfiguration<Consumable_InputList>
	{
		public Consumable_InputListMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 标题
		/// <summary>
			  this.Property(t => t.Title)
			.HasMaxLength(200);
		/// <summary>
		/// 入库单号
		/// <summary>
			  this.Property(t => t.Code)
			.HasMaxLength(50);
		/// <summary>
		/// 备注
		/// <summary>
			  this.Property(t => t.Remark)
			.HasMaxLength(200);
			// Table & Column Mappings
 			 this.ToTable("Consumable_InputList"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 标题
			this.Property(t => t.Title).HasColumnName("Title"); 
			// 入库单号
			this.Property(t => t.Code).HasColumnName("Code"); 
			// 创建时间
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			// 入库时间
			this.Property(t => t.InputTime).HasColumnName("InputTime"); 
			// 操作人
			this.Property(t => t.Operator).HasColumnName("Operator");
            // 操作人
            this.Property(t => t.SumbitUser).HasColumnName("SumbitUser");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            // 入库单状态：0-保存，1-提交入库
            this.Property(t => t.State).HasColumnName("State");
        }
	 }
}
