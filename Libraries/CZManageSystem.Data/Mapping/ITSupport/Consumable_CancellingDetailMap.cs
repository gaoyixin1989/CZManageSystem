using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class Consumable_CancellingDetailMap : EntityTypeConfiguration<Consumable_CancellingDetail>
	{
		public Consumable_CancellingDetailMap()
		{
            // Primary Key
            this.HasKey(t=>t.ID);
			// Table & Column Mappings
 			 this.ToTable("Consumable_CancellingDetail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ÍË¿âÉêÇëµ¥ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ºÄ²ÄID
			this.Property(t => t.ConsumableID).HasColumnName("ConsumableID"); 
			// ÍË¿âÊýÁ¿
			this.Property(t => t.CancelNumber).HasColumnName("CancelNumber"); 
		 }
	 }
}
