using CZManageSystem.Data.Domain.HumanResources.Knowledge;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.Knowledge
{
	public class SysKnowledgeMap : EntityTypeConfiguration<SysKnowledge>
	{
		public SysKnowledgeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// 标题
		/// <summary>
			  this.Property(t => t.Title)
			 .IsRequired()
			.HasMaxLength(50);
		/// <summary>
		/// 内容
		/// <summary>
			  this.Property(t => t.Content)
			 .IsRequired();
			// Table & Column Mappings
 			 this.ToTable("SysKnowledge"); 
			// id
			this.Property(t => t.ID).HasColumnName("ID"); 
			// 标题
			this.Property(t => t.Title).HasColumnName("Title"); 
			// 内容
			this.Property(t => t.Content).HasColumnName("Content"); 
			this.Property(t => t.OrderNo).HasColumnName("OrderNo"); 
			this.Property(t => t.Createdtime).HasColumnName("Createdtime"); 
			this.Property(t => t.CreatorID).HasColumnName("CreatorID");

            //外键
            this.HasOptional(t => t.CreatorObj).WithMany().HasForeignKey(d => d.CreatorID);
            //this.HasMany(t => t.Attachments).WithOptional().HasForeignKey(d => d.Upguid);
        }
	 }
}
