using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ReturnsTypeMap : EntityTypeConfiguration<ReturnsType>
	{
		public ReturnsTypeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �˷ѷ�ʽ
		/// <summary>
			  this.Property(t => t.Type)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("ReturnsType"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �˷ѷ�ʽ
			this.Property(t => t.Type).HasColumnName("Type"); 
			// ����
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
