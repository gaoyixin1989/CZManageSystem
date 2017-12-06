using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class StockAssetMap : EntityTypeConfiguration<StockAsset>
	{
		public StockAssetMap()
		{
            // Primary Key
            this.Property(t => t.Id);

			// Table & Column Mappings
 			 this.ToTable("StockAsset"); 
			this.Property(t => t.AssetSn).HasColumnName("AssetSn"); 
			this.Property(t => t.StockId).HasColumnName("StockId"); 
			this.Property(t => t.State).HasColumnName("State"); 
		 }
	 }
}
