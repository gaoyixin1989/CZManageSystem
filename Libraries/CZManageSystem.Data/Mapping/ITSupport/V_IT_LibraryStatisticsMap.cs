using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class V_IT_LibraryStatisticsMap : EntityTypeConfiguration<V_IT_LibraryStatistics>
	{
		public V_IT_LibraryStatisticsMap()
		{
			// Primary Key
			  this.Property(t => t.Title)

			.HasMaxLength(200);
			  this.Property(t => t.Code)

			.HasMaxLength(50);
			  this.Property(t => t.Remark)

			.HasMaxLength(200);
			  this.Property(t => t.Type)

			.HasMaxLength(100);
			  this.Property(t => t.Model)

			.HasMaxLength(100);
			  this.Property(t => t.Name)

			.HasMaxLength(100);
			  this.Property(t => t.Trademark)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("V_IT_LibraryStatistics"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.Title).HasColumnName("Title"); 
			this.Property(t => t.Code).HasColumnName("Code"); 
			this.Property(t => t.CreateTime).HasColumnName("CreateTime"); 
			this.Property(t => t.InputTime).HasColumnName("InputTime"); 
			this.Property(t => t.Operator).HasColumnName("Operator"); 
			this.Property(t => t.Remark).HasColumnName("Remark"); 
			this.Property(t => t.State).HasColumnName("State"); 
			this.Property(t => t.SumbitUser).HasColumnName("SumbitUser"); 
			this.Property(t => t.Amount).HasColumnName("Amount"); 
			this.Property(t => t.Type).HasColumnName("Type"); 
			this.Property(t => t.Model).HasColumnName("Model"); 
			this.Property(t => t.Name).HasColumnName("Name"); 
			this.Property(t => t.Trademark).HasColumnName("Trademark"); 
		 }
	 }
}
