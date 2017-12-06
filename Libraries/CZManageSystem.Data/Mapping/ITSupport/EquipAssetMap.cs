
using System.Data.Entity.ModelConfiguration;
using CZManageSystem.Data.Domain.ITSupport;

namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class EquipAssetMap : EntityTypeConfiguration<EquipAsset>
	{
		public EquipAssetMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
			  this.Property(t => t.AssetSn)

			.HasMaxLength(50);
			  this.Property(t => t.BUsername)

			.HasMaxLength(200);
			// Table & Column Mappings
 			 this.ToTable("EquipAsset"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			this.Property(t => t.AssetSn).HasColumnName("AssetSn"); 
			this.Property(t => t.ApplyId).HasColumnName("ApplyId"); 
			this.Property(t => t.BUsername).HasColumnName("BUsername"); 
		 }
	 }
}
