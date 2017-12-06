using CZManageSystem.Data.Domain.ITSupport;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.ITSupport
{
	public class V_Consumable_CancellingDetailMap : EntityTypeConfiguration<V_Consumable_CancellingDetail>
	{
		public V_Consumable_CancellingDetailMap()
		{
			// Primary Key
			  this.Property(t => t.Series)

			.HasMaxLength(200);
			  this.Property(t => t.ApplyDpFullName)

			.HasMaxLength(256);
			  this.Property(t => t.AppDept)

			.HasMaxLength(640);
			  this.Property(t => t.ApplicantName)

			.HasMaxLength(50);
			  this.Property(t => t.AppPerson)

			.HasMaxLength(200);
			  this.Property(t => t.Mobile)

			.HasMaxLength(200);
			  this.Property(t => t.Title)

			.HasMaxLength(200);
			  this.Property(t => t.Content)

			.HasMaxLength(2147483647);
			  this.Property(t => t.Type)

			.HasMaxLength(100);
			  this.Property(t => t.Name)

			.HasMaxLength(100);
			  this.Property(t => t.Model)

			.HasMaxLength(100);
			// Table & Column Mappings
 			 this.ToTable("V_Consumable_CancellingDetail"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			this.Property(t => t.CancelNumber).HasColumnName("CancelNumber"); 
			this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId"); 
			this.Property(t => t.Series).HasColumnName("Series"); 
			this.Property(t => t.ApplyTime).HasColumnName("ApplyTime"); 
			this.Property(t => t.ApplyDpFullName).HasColumnName("ApplyDpFullName"); 
			this.Property(t => t.AppDept).HasColumnName("AppDept"); 
			this.Property(t => t.ApplicantName).HasColumnName("ApplicantName"); 
			this.Property(t => t.AppPerson).HasColumnName("AppPerson"); 
			this.Property(t => t.Mobile).HasColumnName("Mobile"); 
			this.Property(t => t.Title).HasColumnName("Title"); 
			this.Property(t => t.Content).HasColumnName("Content"); 
			this.Property(t => t.Type).HasColumnName("Type"); 
			this.Property(t => t.Name).HasColumnName("Name"); 
			this.Property(t => t.Model).HasColumnName("Model"); 
			this.Property(t => t.State).HasColumnName("State"); 
		 }
	 }
}
