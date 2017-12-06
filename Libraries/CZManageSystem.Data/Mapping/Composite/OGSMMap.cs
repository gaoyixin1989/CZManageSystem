using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class OGSMMap : EntityTypeConfiguration<OGSM>
    {
        public OGSMMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Group_Name)
                .HasMaxLength(50);

            this.Property(t => t.Town)
                .HasMaxLength(200);

            this.Property(t => t.USR_NBR)
                .HasMaxLength(50);

            this.Property(t => t.PowerStation)
                .HasMaxLength(50);

            this.Property(t => t.BaseStation)
                .HasMaxLength(150);

            this.Property(t => t.PowerType)
                .HasMaxLength(50);

            this.Property(t => t.PropertyRight)
                .HasMaxLength(50);

            this.Property(t => t.IsRemove)
                .HasMaxLength(50);

            this.Property(t => t.IsShare)
                .HasMaxLength(50);

            this.Property(t => t.Price)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.Property)
                .HasMaxLength(50);

            this.Property(t => t.LinkMan)
                .HasMaxLength(50);

            this.Property(t => t.Mobile)
                .HasMaxLength(50);

            this.Property(t => t.IsWarn)
                .HasMaxLength(10);

            this.Property(t => t.Remark)
                .HasMaxLength(200);

            this.Property(t => t.IsNew)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.AttachmentId);

            // Table & Column Mappings
            this.ToTable("OGSM");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Group_Name).HasColumnName("Group_Name");
            this.Property(t => t.Town).HasColumnName("Town");
            this.Property(t => t.USR_NBR).HasColumnName("USR_NBR");
            this.Property(t => t.PowerStation).HasColumnName("PowerStation");
            this.Property(t => t.BaseStation).HasColumnName("BaseStation");
            this.Property(t => t.PowerType).HasColumnName("PowerType");
            this.Property(t => t.PropertyRight).HasColumnName("PropertyRight");
            this.Property(t => t.IsRemove).HasColumnName("IsRemove");
            this.Property(t => t.RemoveTime).HasColumnName("RemoveTime");
            this.Property(t => t.IsShare).HasColumnName("IsShare");
            this.Property(t => t.ContractStartTime).HasColumnName("ContractStartTime");
            this.Property(t => t.ContractEndTime).HasColumnName("ContractEndTime");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.PAY_CYC).HasColumnName("PAY_CYC");
            this.Property(t => t.Property).HasColumnName("Property");
            this.Property(t => t.LinkMan).HasColumnName("LinkMan");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.IsWarn).HasColumnName("IsWarn");
            this.Property(t => t.WarnCount).HasColumnName("WarnCount");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.IsNew).HasColumnName("IsNew");
            this.Property(t => t.AttachmentId).HasColumnName("AttachmentId");
        }
    }
}
