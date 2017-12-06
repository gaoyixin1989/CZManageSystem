using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ReturnsRangeMap : EntityTypeConfiguration<ReturnsRange>
	{
		public ReturnsRangeMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �˷�����
		/// <summary>
			  this.Property(t => t.Range)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("ReturnsRange"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �������ֵ
			this.Property(t => t.MiniValue).HasColumnName("MiniValue"); 
			// �������ֵ
			this.Property(t => t.MaxValue).HasColumnName("MaxValue"); 
			// �˷�����
			this.Property(t => t.Range).HasColumnName("Range"); 
			// ����
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
