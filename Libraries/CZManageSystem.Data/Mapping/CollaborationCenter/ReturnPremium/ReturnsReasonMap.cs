using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ReturnsReasonMap : EntityTypeConfiguration<ReturnsReason>
	{
		public ReturnsReasonMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �˷�ԭ��
		/// <summary>
			  this.Property(t => t.Reason)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("ReturnsReason"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �˷�ԭ��
			this.Property(t => t.Reason).HasColumnName("Reason"); 
			// ���
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
