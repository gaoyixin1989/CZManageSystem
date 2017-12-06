using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Models.Mapping
{
	public class ReturnsChannelMap : EntityTypeConfiguration<ReturnsChannel>
	{
		public ReturnsChannelMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// �Ǽ�����
		/// <summary>
			  this.Property(t => t.Channel)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("ReturnsChannel"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �Ǽ�����
			this.Property(t => t.Channel).HasColumnName("Channel"); 
			// ����
			this.Property(t => t.Order).HasColumnName("Order"); 
		 }
	 }
}
