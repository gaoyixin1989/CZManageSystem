using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
	public class GdPayId
	{
		public int payid { get; set;}
		public string payname { get; set;}
		public int pid { get; set;}
		public string bz { get; set;}
		public int sort { get; set;}
		/// <summary>
		/// 是否独占一行
		/// <summary>
		public bool RowExclusive { get; set;}
		/// <summary>
		/// 是否沿用上一个周期的数据
		/// <summary>
		public bool Inherit { get; set;}
		public string DataType { get; set;} 
        
    }
}
