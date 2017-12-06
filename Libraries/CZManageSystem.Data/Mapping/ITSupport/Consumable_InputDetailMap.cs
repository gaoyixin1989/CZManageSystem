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
			// ����б�ID
			this.Property(t => t.InputListID).HasColumnName("InputListID"); 
			// �Ĳ�ID
			this.Property(t => t.ConsumableID).HasColumnName("ConsumableID"); 
			// �������
			this.Property(t => t.Amount).HasColumnName("Amount"); 
		 }
	 }
}
