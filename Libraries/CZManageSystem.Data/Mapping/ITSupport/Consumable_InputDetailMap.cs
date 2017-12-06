using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class Consumable_InputDetailMap : EntityTypeConfiguration<Consumable_InputDetail>
	{
		public Consumable_InputDetailMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			// Table & Column Mappings
 			 this.ToTable("Consumable_InputDetail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 入库列表单ID
			this.Property(t => t.InputListID).HasColumnName("InputListID"); 
			// 耗材ID
			this.Property(t => t.ConsumableID).HasColumnName("ConsumableID"); 
			// 入库数量
			this.Property(t => t.Amount).HasColumnName("Amount"); 
		 }
	 }
}
