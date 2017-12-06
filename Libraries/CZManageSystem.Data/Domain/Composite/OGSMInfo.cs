using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class OGSMInfo
    {
        public int Id { get; set; }
        public string USR_NBR { get; set; }
        public Nullable<int> PAY_MON { get; set; }
        
        public decimal PreKwh { get; set; }
        public decimal NowKwh { get; set; }
        public int MF { get; set; }
        public decimal CHG { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Adjustment { get; set; }
        public Nullable<decimal> Money { get; set; }
        public string CHG_COMPARE { get; set; }
        public string Money_COMPARE { get; set; }
        public int New_Meter { get; set; }
        public Nullable<System.DateTime> RTime { get; set; }
        public string Payee { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string BankAcount { get; set; }
        public string Bank { get; set; }
        public string Address { get; set; }
        public Nullable<int> PSubPayMonth { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<System.DateTime> LastModTime { get; set; }

        //public virtual OGSM OGSM { get; set; }

    }
    public class OGSMInfoQueryBuilder
    {
        public string USR_NBR { get; set; }
        public string Group_Name { get; set; }
        public string BaseStation { get; set; }
        public string PowerType { get; set; }
        public string CHG_COMPARE { get; set; }
        public string Money_COMPARE { get; set; }
        public string IsRemove { get; set; }
        public string PAY_MON_Start { get; set; }
        public string PAY_MON_End { get; set; }
    }
    public partial class OGSM_Info
    {
        public int Id { get; set; }
        public string USR_NBR { get; set; }
        public Nullable<int> PAY_MON { get; set; }
        public decimal PreKwh { get; set; }
        public decimal NowKwh { get; set; }
        public int MF { get; set; }
        public decimal CHG { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Adjustment { get; set; }
        public Nullable<decimal> Money { get; set; }
        public string CHG_COMPARE { get; set; }
        public string Money_COMPARE { get; set; }
        public int New_Meter { get; set; }
        public Nullable<System.DateTime> RTime { get; set; }
        public string Payee { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string BankAcount { get; set; }
        public string Bank { get; set; }
        public string Address { get; set; }
        public Nullable<int> PSubPayMonth { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<System.DateTime> LastModTime { get; set; }

        public string Group_Name { get; set; }

        public string BaseStation { get; set; }
        public string PowerType { get; set; }

        public string IsRemove { get; set; }
        //public virtual OGSM OGSM { get; set; }

    }


}
