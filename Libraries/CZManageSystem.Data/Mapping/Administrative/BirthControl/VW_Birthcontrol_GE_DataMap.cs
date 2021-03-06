﻿using CZManageSystem.Data.Domain.Administrative.BirthControl;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Mapping.Administrative.BirthControl
{
    public class VW_Birthcontrol_GE_DataMap : EntityTypeConfiguration<VW_Birthcontrol_GE_Data>
    {
        public VW_Birthcontrol_GE_DataMap()
        {
            // Primary Key
            this.Property(t => t.Id)

          .HasMaxLength(30);
            this.Property(t => t.EmployeeId)

          .HasMaxLength(50);
            this.Property(t => t.UserName)

          .HasMaxLength(50);
            this.Property(t => t.RealName)

          .HasMaxLength(50);
            this.Property(t => t.DpName)

          .HasMaxLength(50);
            this.Property(t => t.SpouseName)

          .HasMaxLength(50);
            this.Property(t => t.ifGE)
           .IsRequired()
          .HasMaxLength(2);
            // Table & Column Mappings
            this.ToTable("vw_Birthcontrol_GE_Data");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.RealName).HasColumnName("RealName");
            this.Property(t => t.DpName).HasColumnName("DpName");
            this.Property(t => t.SpouseName).HasColumnName("SpouseName");
            this.Property(t => t.ifGE).HasColumnName("ifGE");

        }

    }
}
