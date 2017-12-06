using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class Consumable_SporadicDetailMap : EntityTypeConfiguration<Consumable_SporadicDetail>
	{
		public Consumable_SporadicDetailMap()
		{
			// Primary Key
			  this.Property(t => t.Relation)

			.HasMaxLength(1000);
			// Table & Column Mappings
 			 this.ToTable("Consumable_SporadicDetail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			this.Property(t => t.Relation).HasColumnName("Relation"); 
			this.Property(t => t.ApplyCount).HasColumnName("ApplyCount"); 
			this.Property(t => t.Amount).HasColumnName("Amount"); 
		 }
	 }
}
