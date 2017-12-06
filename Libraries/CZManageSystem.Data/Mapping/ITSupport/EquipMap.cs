using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class EquipMap : EntityTypeConfiguration<Equip>
	{
		public EquipMap()
		{
            // Primary Key
            this.Property(t => t.Id);

		   this.Property(t => t.Edittime)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("Equip"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			this.Property(t => t.EquipClass).HasColumnName("EquipClass"); 
			this.Property(t => t.Editor).HasColumnName("Editor"); 
			this.Property(t => t.Edittime).HasColumnName("Edittime"); 
		 }
	 }
}
