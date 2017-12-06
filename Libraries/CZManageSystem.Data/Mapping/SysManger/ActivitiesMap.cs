using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.SysManger
{
	public class ActivitiesMap : EntityTypeConfiguration<Activities>
	{
		public ActivitiesMap()
		{
			// Primary Key
			 this.HasKey(t => t.ActivityId);

			  this.Property(t => t.ActivityName)
			 .IsRequired()
			.HasMaxLength(50);

			  this.Property(t => t.JoinCondition)
			.HasMaxLength(255);

            this.Property(t => t.SplitCondition)
			.HasMaxLength(255);

			  this.Property(t => t.CommandRules)
			.HasMaxLength(1073741823);

			  this.Property(t => t.ExecutionHandler)
			.HasMaxLength(255);

			  this.Property(t => t.PostHandler)
			.HasMaxLength(255);

			  this.Property(t => t.AllocatorResource)
			.HasMaxLength(50);

			  this.Property(t => t.AllocatorUsers)
			.HasMaxLength(255);

			  this.Property(t => t.ExtendAllocators)
			.HasMaxLength(255);

			  this.Property(t => t.ExtendAllocatorArgs)
			.HasMaxLength(255);

			  this.Property(t => t.DefaultAllocator)
			.HasMaxLength(50);

			  this.Property(t => t.DecisionType)
			.HasMaxLength(10);

			  this.Property(t => t.DecisionParser)
			.HasMaxLength(255);

			  this.Property(t => t.CountersignedCondition)
			.HasMaxLength(255);

			  this.Property(t => t.RejectOption)
			.HasMaxLength(50);

			// Table & Column Mappings
 			 this.ToTable("bwwf_Activities"); 
			this.Property(t => t.WorkflowId).HasColumnName("WorkflowId"); 
			this.Property(t => t.ActivityId).HasColumnName("ActivityId"); 
			this.Property(t => t.ActivityName).HasColumnName("ActivityName"); 
			this.Property(t => t.State).HasColumnName("State"); 
			this.Property(t => t.SortOrder).HasColumnName("SortOrder"); 
			this.Property(t => t.PrevActivitySetId).HasColumnName("PrevActivitySetId"); 
			this.Property(t => t.NextActivitySetId).HasColumnName("NextActivitySetId"); 
			this.Property(t => t.JoinCondition).HasColumnName("JoinCondition"); 
			this.Property(t => t.SplitCondition).HasColumnName("SplitCondition"); 
			this.Property(t => t.CommandRules).HasColumnName("CommandRules"); 
			this.Property(t => t.ExecutionHandler).HasColumnName("ExecutionHandler"); 
			this.Property(t => t.PostHandler).HasColumnName("PostHandler"); 
			this.Property(t => t.AllocatorResource).HasColumnName("AllocatorResource"); 
			this.Property(t => t.AllocatorUsers).HasColumnName("AllocatorUsers"); 
			this.Property(t => t.ExtendAllocators).HasColumnName("ExtendAllocators"); 
			this.Property(t => t.ExtendAllocatorArgs).HasColumnName("ExtendAllocatorArgs"); 
			this.Property(t => t.DefaultAllocator).HasColumnName("DefaultAllocator"); 
			this.Property(t => t.DecisionType).HasColumnName("DecisionType"); 
			this.Property(t => t.DecisionParser).HasColumnName("DecisionParser"); 
			this.Property(t => t.CountersignedCondition).HasColumnName("CountersignedCondition"); 
			this.Property(t => t.ParallelActivitySetId).HasColumnName("ParallelActivitySetId"); 
			this.Property(t => t.RejectOption).HasColumnName("RejectOption"); 
			this.Property(t => t.CanPrint).HasColumnName("CanPrint"); 
			this.Property(t => t.PrintAmount).HasColumnName("PrintAmount"); 
			this.Property(t => t.CanEdit).HasColumnName("CanEdit"); 
			this.Property(t => t.ReturnToPrev).HasColumnName("ReturnToPrev"); 
			this.Property(t => t.IsMobile).HasColumnName("IsMobile"); 
			this.Property(t => t.IsTimeOutContinue).HasColumnName("IsTimeOutContinue");

            this.HasMany(t => t.ActivitySets).WithOptional()
               .HasForeignKey(d => d.ActivityId);
        }
	 }
}
