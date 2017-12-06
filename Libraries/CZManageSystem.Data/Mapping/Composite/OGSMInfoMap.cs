using CZManageSystem.Data.Domain.Composite;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CZManageSystem.Data.Mapping.Composite
{
    public class OGSMInfoMap : EntityTypeConfiguration<OGSMInfo>
    {
        public OGSMInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.USR_NBR)
                .HasMaxLength(50);

            this.Property(t => t.Price)
               .HasPrecision(18, 3);

            this.Property(t => t.CHG_COMPARE)
                .HasMaxLength(50);

            this.Property(t => t.Money_COMPARE)
                .HasMaxLength(50);

            this.Property(t => t.Payee)
                .HasMaxLength(50);

            this.Property(t => t.Mobile1)
                .HasMaxLength(50);

            this.Property(t => t.Mobile2)
                .HasMaxLength(50);

            this.Property(t => t.BankAcount)
                .HasMaxLength(50);

            this.Property(t => t.Bank)
                .HasMaxLength(50);

            this.Property(t => t.Address)
                .HasMaxLength(200);

            this.Property(t => t.Remark)
                .HasMaxLength(200);

            this.Property(t => t.Creator)
                .HasMaxLength(50);

            this.Property(t => t.LastModifier)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OGSMInfo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.USR_NBR).HasColumnName("USR_NBR");
            this.Property(t => t.PAY_MON).HasColumnName("PAY_MON");
            this.Property(t => t.PreKwh).HasColumnName("PreKwh");
            this.Property(t => t.NowKwh).HasColumnName("NowKwh");
            this.Property(t => t.MF).HasColumnName("MF");
            this.Property(t => t.CHG).HasColumnName("CHG");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Adjustment).HasColumnName("Adjustment");
            this.Property(t => t.Money).HasColumnName("Money");
            this.Property(t => t.CHG_COMPARE).HasColumnName("CHG_COMPARE");
            this.Property(t => t.Money_COMPARE).HasColumnName("Money_COMPARE");
            this.Property(t => t.New_Meter).HasColumnName("New_Meter");
            this.Property(t => t.RTime).HasColumnName("RTime");
            this.Property(t => t.Payee).HasColumnName("Payee");
            this.Property(t => t.Mobile1).HasColumnName("Mobile1");
            this.Property(t => t.Mobile2).HasColumnName("Mobile2");
            this.Property(t => t.BankAcount).HasColumnName("BankAcount");
            this.Property(t => t.Bank).HasColumnName("Bank");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.PSubPayMonth).HasColumnName("PSubPayMonth");
            this.Property(t => t.Remark).HasColumnName("Remark");
            this.Property(t => t.Creator).HasColumnName("Creator");
            this.Property(t => t.CreatedTime).HasColumnName("CreatedTime");
            this.Property(t => t.LastModifier).HasColumnName("LastModifier");
            this.Property(t => t.LastModTime).HasColumnName("LastModTime");



        }
    }
}
