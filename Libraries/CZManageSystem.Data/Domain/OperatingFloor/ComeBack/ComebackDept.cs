using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ComebackDept
	{
		public Guid ID { get; set;}
		public Nullable<int> Year { get; set;}
		public string BudgetDept { get; set;}
		public Nullable<decimal> Amount { get; set;}
		public string Remark { get; set;}

	}
    public class ComebackQueryBuilder
    {
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string Name { get; set; }

        public string BudgetDept { get; set; }

    }
}
