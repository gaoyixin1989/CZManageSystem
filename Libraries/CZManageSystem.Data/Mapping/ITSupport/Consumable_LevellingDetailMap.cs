using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class Consumable_LevellingDetailMap : EntityTypeConfiguration<Consumable_LevellingDetail>
	{
		public Consumable_LevellingDetailMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			// Table & Column Mappings
 			 this.ToTable("Consumable_LevellingDetail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ���뵥ID
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// �Ĳ�ID
			this.Property(t => t.ConsumableID).HasColumnName("ConsumableID"); 
			// ����
			this.Property(t => t.Amount).HasColumnName("Amount"); 
		 }
	 }
}
