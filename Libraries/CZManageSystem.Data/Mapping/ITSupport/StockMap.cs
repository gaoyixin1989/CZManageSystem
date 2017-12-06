using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class StockMap : EntityTypeConfiguration<Stock>
	{
		public StockMap()
		{
            // Primary Key
            this.Property(t => t.Id);

			  this.Property(t => t.ProjSn)

			.HasMaxLength(100);
            this.Property(t => t.StockTime)
            ;
            this.Property(t => t.EditTime);

			  this.Property(t => t.EquipInfo)

			.HasMaxLength(200);
			  this.Property(t => t.EquipClass)

			.HasMaxLength(200);
			  this.Property(t => t.LableNo)

			.HasMaxLength(100);
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
			// Table & Column Mappings
 			 this.ToTable("Stock"); 
			this.Property(t => t.Id).HasColumnName("Id"); 
			this.Property(t => t.EquipNum).HasColumnName("EquipNum"); 
			this.Property(t => t.ProjSn).HasColumnName("ProjSn"); 
			this.Property(t => t.StockTime).HasColumnName("StockTime"); 
			this.Property(t => t.EditTime).HasColumnName("EditTime"); 
			this.Property(t => t.EquipInfo).HasColumnName("EquipInfo"); 
			this.Property(t => t.EquipClass).HasColumnName("EquipClass"); 
			this.Property(t => t.LableNo).HasColumnName("LableNo"); 
			this.Property(t => t.Content).HasColumnName("Content"); 
			this.Property(t => t.StockType).HasColumnName("StockType");
           // this.Property(t => t.Totalnum).HasColumnName("Totalnum");
        }
	 }
}
