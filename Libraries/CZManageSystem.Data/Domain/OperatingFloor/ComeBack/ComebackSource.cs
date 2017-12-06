using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ComebackSource
	{
        public ComebackSource()
        {
            this.ComebackTypes = new List<ComebackType>();
        }
        public Guid ID { get; set;}
		public Nullable<int> Year { get; set;}
		public string Name { get; set;}
		public string BudgetDept { get; set;}
		public Nullable<decimal> Amount { get; set;}
		public string Remark { get; set;}
        public virtual ICollection<ComebackType> ComebackTypes { get; set; }

    }
}
