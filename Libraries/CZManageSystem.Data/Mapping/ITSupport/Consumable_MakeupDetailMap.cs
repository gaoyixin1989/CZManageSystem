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
			// ��¼�鵵ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// �Ĳ�ID
			this.Property(t => t.ConsumableID).HasColumnName("ConsumableID"); 
			// ����
			this.Property(t => t.Amount).HasColumnName("Amount"); 
		 }
	 }
}
