using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class ProjMap : EntityTypeConfiguration<Proj>
	{
		public ProjMap()
		{
			// Primary Key
			 this.HasKey(t => t.Id);
			  this.Property(t => t.ProjSn)

			.HasMaxLength(100);
			  this.Property(t => t.ProjName)

			.HasMaxLength(200);
            this.Property(t => t.EditTime);
			// Table & Column Mappings
 			 this.ToTable("Proj"); 
			this.Property(t => t.Id).HasColumnName("id"); 
			this.Property(t => t.ProjSn).HasColumnName("ProjSn"); 
			this.Property(t => t.ProjName).HasColumnName("ProjName");
            this.Property(t => t.Editor).HasColumnName("Editor");
            this.Property(t => t.EditTime).HasColumnName("EditTime"); 
		 }
	 }
}
