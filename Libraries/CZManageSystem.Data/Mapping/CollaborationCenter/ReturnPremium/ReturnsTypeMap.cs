using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ReturnsTypeMap : EntityTypeConfiguration<ReturnsType>
	{
		public ReturnsTypeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 退费方式
		/// <summary>
			  this.Property(t => t.Type)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("ReturnsType"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 退费方式
			this.Property(t => t.Type).HasColumnName("Type"); 
			// 排序
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
