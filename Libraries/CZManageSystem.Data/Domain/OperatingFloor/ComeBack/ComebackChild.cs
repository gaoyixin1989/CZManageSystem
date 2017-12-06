using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ComebackChild
	{
		public Guid ID { get; set;}
        /// <summary>
        /// typeid
        /// </summary>
		public Nullable<Guid> PID { get; set;}
		public Nullable<int> Year { get; set;}
		public string Name { get; set;}
		public Nullable<decimal> Amount { get; set;}
		public string Remark { get; set;}
        public virtual ComebackType ComebackType { get; set; }

    }
    public class ComebackChildQueryBuilder
    {
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public string BudgetDept { get; set; }
        /// <summary>
        /// 归口小项
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 归口项目
        /// </summary>
        public string ProName { get; set; }

    }
}
