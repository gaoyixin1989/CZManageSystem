using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ReturnsRangeMap : EntityTypeConfiguration<ReturnsRange>
	{
		public ReturnsRangeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 退费区间
		/// <summary>
			  this.Property(t => t.Range)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("ReturnsRange"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 区间最低值
			this.Property(t => t.MiniValue).HasColumnName("MiniValue"); 
			// 区间最大值
			this.Property(t => t.MaxValue).HasColumnName("MaxValue"); 
			// 退费区间
			this.Property(t => t.Range).HasColumnName("Range"); 
			// 排序
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
