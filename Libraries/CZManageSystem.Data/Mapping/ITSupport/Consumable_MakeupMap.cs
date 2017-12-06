using CZManageSystem.Data.Domain.ITSupport;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.ITSupport
{
    public class Consumable_MakeupMap : EntityTypeConfiguration<Consumable_Makeup>
    {
        public Consumable_MakeupMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);
            /// <summary>
            /// 流程实例ID
            /// <summary>
            this.Property(t => t.WorkflowInstanceId);
            /// <summary>
            /// 流程单号
            /// <summary>
            this.Property(t => t.Series)

          .HasMaxLength(200);
            /// <summary>
            /// 申请部门id
            /// <summary>
            this.Property(t => t.AppDept)

          .HasMaxLength(200);
            /// <summary>
            /// 申请人手机
            /// <summary>
            this.Property(t => t.Mobile)

          .HasMaxLength(200);
            /// <summary>
            /// 申请标题
            /// <summary>
            this.Property(t => t.Title)

          .HasMaxLength(200);
            /// <summary>
            /// 退库原因
            /// <summary>
            this.Property(t => t.Content)

          .HasMaxLength(2147483647);
            // Table & Column Mappings
            this.ToTable("Consumable_Makeup");
            // 编号
            this.Property(t => t.ID).HasColumnName("ID");
            // 申请提交时间
            this.Property(t => t.AppTime).HasColumnName("AppTime");
            // 申请部门id
            this.Property(t => t.AppDept).HasColumnName("AppDept");
            // 申请人
            this.Property(t => t.AppPerson).HasColumnName("AppPerson");
            // 申请人手机
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            // 申请标题
            this.Property(t => t.Title).HasColumnName("Title");
            // 退库原因
            this.Property(t => t.Content).HasColumnName("Content");
            // 状态：0保存、1提交
            this.Property(t => t.State).HasColumnName("State");
            // 使用人ID
            this.Property(t => t.UsePerson).HasColumnName("UsePerson");

            // Relationships
            this.HasOptional(t => t.UsersForApp).WithMany()
                .HasForeignKey(d => d.AppPerson);
            this.HasOptional(t => t.UsersForUse).WithMany()
                .HasForeignKey(d => d.UsePerson);
            this.HasOptional(t => t.DeptsForApp).WithMany()
                .HasForeignKey(d => d.AppDept);
            this.HasOptional(t => t.Tracking_Workflow).WithMany()
                .HasForeignKey(d => d.WorkflowInstanceId);

        }
    }
}
