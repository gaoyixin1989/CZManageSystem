using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_MenuPackageCommandMap : EntityTypeConfiguration<OrderMeal_MenuPackageCommand>
    {
        public OrderMeal_MenuPackageCommandMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.PackageName)
;
            this.Property(t => t.Command)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_MenuPackageCommand");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.PackageId).HasColumnName("PackageId");
            this.Property(t => t.PackageName).HasColumnName("PackageName");
            this.Property(t => t.CommandId).HasColumnName("CommandId");
            this.Property(t => t.Command).HasColumnName("Command");
            this.Property(t => t.State).HasColumnName("State");


        }

    }
}
