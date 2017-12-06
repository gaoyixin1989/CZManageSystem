using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class Consumable_MakeupDetailMap : EntityTypeConfiguration<Consumable_MakeupDetail>
	{
		public Consumable_MakeupDetailMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			// Table & Column Mappings
 			 this.ToTable("Consumable_MakeupDetail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ²¹Â¼¹éµµID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ºÄ²ÄID
			this.Property(t => t.ConsumableID).HasColumnName("ConsumableID"); 
			// ÊýÁ¿
			this.Property(t => t.Amount).HasColumnName("Amount"); 
		 }
	 }
}
