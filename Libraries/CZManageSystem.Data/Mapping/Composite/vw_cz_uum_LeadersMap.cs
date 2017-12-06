using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class vw_cz_uum_LeadersMap : EntityTypeConfiguration<vw_cz_uum_Leaders>
	{
		public vw_cz_uum_LeadersMap()
		{
			// Primary Key
			  this.Property(t => t.UserName)
			 .IsRequired()
			.HasMaxLength(50);
			  this.Property(t => t.OUID)

			.HasMaxLength(20);
			  this.Property(t => t.EmployeeClass)

			.HasMaxLength(10);
			  this.Property(t => t.EmployeeLevel)

			.HasMaxLength(10);
			  this.Property(t => t.DpId)

			.HasMaxLength(255);
			// Table & Column Mappings
 			 this.ToTable("vw_cz_uum_Leaders"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.UserName).HasColumnName("UserName"); 
			this.Property(t => t.OUID).HasColumnName("OUID"); 
			this.Property(t => t.EmployeeClass).HasColumnName("EmployeeClass"); 
			this.Property(t => t.EmployeeLevel).HasColumnName("EmployeeLevel"); 
			this.Property(t => t.SortOrder).HasColumnName("SortOrder"); 
			this.Property(t => t.DpId).HasColumnName("DpId"); 
		 }
	 }
}
