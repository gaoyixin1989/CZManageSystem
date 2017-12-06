using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentHallMap : EntityTypeConfiguration<PaymentHall>
	{
		public PaymentHallMap()
		{
			// Primary Key
			 this.HasKey(t => t.HallID);
		/// <summary>
		/// 服营厅名称
		/// <summary>
			  this.Property(t => t.HallName)

			.HasMaxLength(50);
		/// <summary>
		/// 关联服营厅Code
		/// <summary>
			  this.Property(t => t.DpId)

			.HasMaxLength(250);
			// Table & Column Mappings
 			 this.ToTable("PaymentHall"); 
			// 服营厅ID
			this.Property(t => t.HallID).HasColumnName("HallID"); 
			// 服营厅名称
			this.Property(t => t.HallName).HasColumnName("HallName"); 
			// 关联服营厅Code
			this.Property(t => t.DpId).HasColumnName("DpId");
            this.HasOptional(t => t.Depts).WithMany().HasForeignKey(t=>t.DpId);
            this.HasMany(t => t.PaymentPayees ).WithOptional ().HasForeignKey (t=>t.HallID );
        }
	 }
}
