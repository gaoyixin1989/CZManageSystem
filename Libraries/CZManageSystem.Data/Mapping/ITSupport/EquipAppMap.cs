using CZManageSystem.Data.Domain.ITSupport;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.ITSupport
{
    public class EquipAppMap : EntityTypeConfiguration<EquipApp>
    {
        public EquipAppMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.ApplyId)

          .HasMaxLength(100);
            this.Property(t => t.ApplyName)

          .HasMaxLength(50);
            this.Property(t => t.Deptname)

          .HasMaxLength(200);
            this.Property(t => t.Job)

          .HasMaxLength(100);
            this.Property(t => t.ApplyTitle)

          .HasMaxLength(200);
            this.Property(t => t.ApplySn)

          .HasMaxLength(200);
            this.Property(t => t.Nature)

          .HasMaxLength(100);
            this.Property(t => t.Tel)

          .HasMaxLength(100);
            this.Property(t => t.EquipClass)

          .HasMaxLength(100);
            this.Property(t => t.ApplyReason)

          .HasMaxLength(200);
            this.Property(t => t.Chief)

          .HasMaxLength(100);
            this.Property(t => t.EquipInfo)

          .HasMaxLength(200);
            this.Property(t => t.ProjSn)

          .HasMaxLength(200);
            this.Property(t => t.AssetSn)

          .HasMaxLength(200);
            this.Property(t => t.OutFlag)

          .HasMaxLength(50);
            this.Property(t => t.BUsername)

          .HasMaxLength(100);
            this.Property(t => t.Remark)

          .HasMaxLength(250);
            this.Property(t => t.Editor)

          .HasMaxLength(100);
            this.Property(t => t.CancleReason)

          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("EquipApp");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ApplyId).HasColumnName("ApplyId");
            this.Property(t => t.ApplyName).HasColumnName("ApplyName");
            this.Property(t => t.Deptname).HasColumnName("Deptname");
            this.Property(t => t.Job).HasColumnName("Job");
            this.Property(t => t.ApplyTime).HasColumnName("ApplyTime");
            this.Property(t => t.ApplyTitle).HasColumnName("ApplyTitle");
            this.Property(t => t.ApplySn).HasColumnName("ApplySn");
            this.Property(t => t.Nature).HasColumnName("Nature");
            this.Property(t => t.Tel).HasColumnName("Tel");
            this.Property(t => t.EquipClass).HasColumnName("EquipClass");
            this.Property(t => t.ApplyReason).HasColumnName("ApplyReason");
            this.Property(t => t.Chief).HasColumnName("Chief");
            this.Property(t => t.AppNum).HasColumnName("AppNum");
            this.Property(t => t.EquipInfo).HasColumnName("EquipInfo");
            this.Property(t => t.ProjSn).HasColumnName("ProjSn");
            this.Property(t => t.AssetSn).HasColumnName("AssetSn");
            this.Property(t => t.OutFlag).HasColumnName("OutFlag");
            this.Property(t => t.StockType).HasColumnName("StockType");
            this.Property(t => t.BUsername).HasColumnName("BUsername");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Editor).HasColumnName("Editor");
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            this.Property(t => t.CancleReason).HasColumnName("CancleReason");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.WorkflowInstanceId).HasColumnName("WorkflowInstanceId");
        }
    }
}
