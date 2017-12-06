using CZManageSystem.Data.Domain.HumanResources.ShiftManages;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.HumanResources.ShiftManages
{
    /// <summary>
    /// 班次值班安排
    /// </summary>
	public class ShiftRichMap : EntityTypeConfiguration<ShiftRich>
    {
        public ShiftRichMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.Day01);
            this.Property(t => t.Day02);
            this.Property(t => t.Day03);
            this.Property(t => t.Day04);
            this.Property(t => t.Day05);
            this.Property(t => t.Day06);
            this.Property(t => t.Day07);
            this.Property(t => t.Day08);
            this.Property(t => t.Day09);
            this.Property(t => t.Day10);
            this.Property(t => t.Day11);
            this.Property(t => t.Day12);
            this.Property(t => t.Day13);
            this.Property(t => t.Day14);
            this.Property(t => t.Day15);
            this.Property(t => t.Day16);
            this.Property(t => t.Day17);
            this.Property(t => t.Day18);
            this.Property(t => t.Day19);
            this.Property(t => t.Day20);
            this.Property(t => t.Day21);
            this.Property(t => t.Day22);
            this.Property(t => t.Day23);
            this.Property(t => t.Day24);
            this.Property(t => t.Day25);
            this.Property(t => t.Day26);
            this.Property(t => t.Day27);
            this.Property(t => t.Day28);
            this.Property(t => t.Day29);
            this.Property(t => t.Day30);
            this.Property(t => t.Day31);
            /// <summary>
            /// 备注
            /// <summary>
            this.Property(t => t.Remark)

          .HasMaxLength(200);
            // Table & Column Mappings
            this.ToTable("ShiftRich");
            // 编号
            this.Property(t => t.Id).HasColumnName("Id");
            // 编辑人
            this.Property(t => t.Editor).HasColumnName("Editor");
            // 编辑时间
            this.Property(t => t.EditTime).HasColumnName("EditTime");
            // 班次id
            this.Property(t => t.BanciId).HasColumnName("BanciId");
            this.Property(t => t.Day01).HasColumnName("Day01");
            this.Property(t => t.Day02).HasColumnName("Day02");
            this.Property(t => t.Day03).HasColumnName("Day03");
            this.Property(t => t.Day04).HasColumnName("Day04");
            this.Property(t => t.Day05).HasColumnName("Day05");
            this.Property(t => t.Day06).HasColumnName("Day06");
            this.Property(t => t.Day07).HasColumnName("Day07");
            this.Property(t => t.Day08).HasColumnName("Day08");
            this.Property(t => t.Day09).HasColumnName("Day09");
            this.Property(t => t.Day10).HasColumnName("Day10");
            this.Property(t => t.Day11).HasColumnName("Day11");
            this.Property(t => t.Day12).HasColumnName("Day12");
            this.Property(t => t.Day13).HasColumnName("Day13");
            this.Property(t => t.Day14).HasColumnName("Day14");
            this.Property(t => t.Day15).HasColumnName("Day15");
            this.Property(t => t.Day16).HasColumnName("Day16");
            this.Property(t => t.Day17).HasColumnName("Day17");
            this.Property(t => t.Day18).HasColumnName("Day18");
            this.Property(t => t.Day19).HasColumnName("Day19");
            this.Property(t => t.Day20).HasColumnName("Day20");
            this.Property(t => t.Day21).HasColumnName("Day21");
            this.Property(t => t.Day22).HasColumnName("Day22");
            this.Property(t => t.Day23).HasColumnName("Day23");
            this.Property(t => t.Day24).HasColumnName("Day24");
            this.Property(t => t.Day25).HasColumnName("Day25");
            this.Property(t => t.Day26).HasColumnName("Day26");
            this.Property(t => t.Day27).HasColumnName("Day27");
            this.Property(t => t.Day28).HasColumnName("Day28");
            this.Property(t => t.Day29).HasColumnName("Day29");
            this.Property(t => t.Day30).HasColumnName("Day30");
            this.Property(t => t.Day31).HasColumnName("Day31");
            // 排序
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            // 备注
            this.Property(t => t.Remark).HasColumnName("Remark");
            


        }
    }
}
