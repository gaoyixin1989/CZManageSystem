using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class CzUumPostsMap : EntityTypeConfiguration<CzUumPosts>
	{
		public CzUumPostsMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
			  this.Property(t => t.UserName)
			 .IsRequired()
			.HasMaxLength(50);
			  this.Property(t => t.OUID)

			.HasMaxLength(50);
			  this.Property(t => t.EmployeeClass)

			.HasMaxLength(10);
			  this.Property(t => t.EmployeeLevel)

			.HasMaxLength(10);
			// Table & Column Mappings
 			 this.ToTable("cz_uum_Posts"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.OUID).HasColumnName("OUID"); 
			this.Property(t => t.EmployeeClass).HasColumnName("EmployeeClass"); 
			this.Property(t => t.EmployeeLevel).HasColumnName("EmployeeLevel"); 
			this.Property(t => t.IsSync).HasColumnName("IsSync"); 
		 }
	 }
}
