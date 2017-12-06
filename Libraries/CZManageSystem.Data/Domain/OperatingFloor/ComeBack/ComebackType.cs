using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ComebackType
	{
        public ComebackType()
        {
            this.ComebackChilds = new List<ComebackChild>();
        }
        public Guid ID { get; set;}
        /// <summary>
        /// sourceid×ÊÔ´id
        /// </summary>
		public Nullable<Guid> PID { get; set;}
		public string BudgetDept { get; set;}
		public Nullable<decimal> Amount { get; set;}
		public string Remark { get; set;}
        public virtual ComebackSource ComebackSource { get; set; }
        public virtual ICollection<ComebackChild> ComebackChilds { get; set; }

    }
    public class ComebackTypeQueryBuilder
    {
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string Name { get; set; }

        public string BudgetDept { get; set; }

    }
    public class ComebackReporteQueryBuilder
    {
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string BudgetDept { get; set; }

    }
}
