using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Consumable_ScrapDetailMap : EntityTypeConfiguration<Consumable_ScrapDetail>
	{
		public Consumable_ScrapDetailMap()
		{
			// Primary Key
			  this.Property(t => t.ApplyID)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("Consumable_ScrapDetail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			this.Property(t => t.ConsumableID).HasColumnName("ConsumableID"); 
			this.Property(t => t.ScrapNumber).HasColumnName("ScrapNumber"); 
		 }
	 }
}
