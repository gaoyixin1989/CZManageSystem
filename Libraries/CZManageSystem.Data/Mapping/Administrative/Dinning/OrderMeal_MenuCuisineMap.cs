using CZManageSystem.Data.Domain.Administrative.Dinning;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.Dinning
{
    public class OrderMeal_MenuCuisineMap : EntityTypeConfiguration<OrderMeal_MenuCuisine>
    {
        public OrderMeal_MenuCuisineMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.CuisineName)

          .HasMaxLength(50);
            // Table & Column Mappings
            this.ToTable("OrderMeal_MenuCuisine");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.CuisineId).HasColumnName("CuisineId");
            this.Property(t => t.CuisineName).HasColumnName("CuisineName");
            this.Property(t => t.State).HasColumnName("State");

        }

    }
}
