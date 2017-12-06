using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.Composite
{
    public class InnerVoicesMap : EntityTypeConfiguration<InnerVoices>
    {
        public InnerVoicesMap()
        {
            // Primary Key
            this.Property(t => t.Applytitle)

          .HasMaxLength(200);
            this.Property(t => t.Applysn)


          .HasMaxLength(50);
            this.Property(t => t.Creator)

          .HasMaxLength(50);
            this.Property(t => t.Themetype)

          .HasMaxLength(100);
            this.Property(t => t.Content)
;
            this.Property(t => t.IsNiming)

          .HasMaxLength(50);
            this.Property(t => t.Attids)

          .HasMaxLength(150);
            this.Property(t => t.Remark)

          .HasMaxLength(2147483647);
            this.Property(t => t.IsInfo)

          .HasMaxLength(50);
            this.Property(t => t.Username)

          .HasMaxLength(50);
            this.Property(t => t.DeptName)

          .HasMaxLength(100);
            this.Property(t => t.Phone)

          .HasMaxLength(50);
            this.Property(t => t.IsManager)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("InnerVoices");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
            this.Property(t => t.Applytitle).HasColumnName("Applytitle");
            this.Property(t => t.Applysn).HasColumnName("Applysn");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.Creatorid).HasColumnName("Creatorid");
            this.Property(t => t.Themetype).HasColumnName("Themetype");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.IsNiming).HasColumnName("IsNiming");
            this.Property(t => t.Attids).HasColumnName("Attids");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.IsInfo).HasColumnName("IsInfo");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.DeptName).HasColumnName("DeptName");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.IsManager).HasColumnName("IsManager");
            // Relationships
            this.HasOptional(t => t.TrackingWorkflow)
                .WithMany()
                .HasForeignKey(d => d.WorkflowInstanceId);
        }   

    }
}
