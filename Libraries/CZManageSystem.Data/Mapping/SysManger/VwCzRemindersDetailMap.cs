using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class VwCzRemindersDetailMap : EntityTypeConfiguration<VwCzRemindersDetail>
	{
		public VwCzRemindersDetailMap()
		{
			// Primary Key
			  this.Property(t => t.SystemID)

			.HasMaxLength(128);
			  this.Property(t => t.MsgRefID)

			.HasMaxLength(32);
			  this.Property(t => t.Sender)

			.HasMaxLength(64);
			  this.Property(t => t.Receiver)

			.HasMaxLength(64);
			  this.Property(t => t.Subject)

			.HasMaxLength(256);
			  this.Property(t => t.Content)

			.HasMaxLength(1024);
			  this.Property(t => t.EntityType)

			.HasMaxLength(32);
			  this.Property(t => t.EntityId)

			.HasMaxLength(50);
			  this.Property(t => t.SenderEmployeeId)

			.HasMaxLength(50);
			  this.Property(t => t.ReceiverEmployeeId)

			.HasMaxLength(50);
			  this.Property(t => t.ReceiverEmail)

			.HasMaxLength(512);
			  this.Property(t => t.ReceiverMobile)

			.HasMaxLength(32);
			// Table & Column Mappings
 			 this.ToTable("vw_cz_Reminders_Detail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.SystemID).HasColumnName("SystemID"); 
			this.Property(t => t.MsgRefID).HasColumnName("MsgRefID"); 
			this.Property(t => t.MsgType).HasColumnName("MsgType"); 
			this.Property(t => t.Sender).HasColumnName("Sender"); 
			this.Property(t => t.Receiver).HasColumnName("Receiver"); 
			this.Property(t => t.Subject).HasColumnName("Subject"); 
			this.Property(t => t.Content).HasColumnName("Content"); 
			this.Property(t => t.State).HasColumnName("State"); 
			this.Property(t => t.CreatedTime).HasColumnName("CreatedTime"); 
			this.Property(t => t.ProcessedTime).HasColumnName("ProcessedTime"); 
			this.Property(t => t.RetriedTimes).HasColumnName("RetriedTimes"); 
			this.Property(t => t.EntityType).HasColumnName("EntityType"); 
			this.Property(t => t.EntityId).HasColumnName("EntityId"); 
			this.Property(t => t.SenderEmployeeId).HasColumnName("SenderEmployeeId"); 
			this.Property(t => t.ReceiverEmployeeId).HasColumnName("ReceiverEmployeeId"); 
			this.Property(t => t.ReceiverEmail).HasColumnName("ReceiverEmail"); 
			this.Property(t => t.ReceiverMobile).HasColumnName("ReceiverMobile"); 
		 }
	 }
}
