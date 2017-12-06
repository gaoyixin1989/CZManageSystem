using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Stock
	{
		public int Id { get; set;}
		public Nullable<int> EquipNum { get; set;}
		public string ProjSn { get; set;}
		public DateTime? StockTime { get; set;}
		public DateTime? EditTime { get; set;}
		public string EquipInfo { get; set;}
		public string EquipClass { get; set;}
		public string LableNo { get; set;}
		public string Content { get; set;}
		public Nullable<int> StockType { get; set;}
       // public Nullable<int> Totalnum { get; set; }
        
    }
    public class StockQueryBuilder
    {
        public string LableNo { get; set; }
        public string EquipClass { get; set; }
        public Nullable<int> StockType { get; set; }
        public string ProjSn { get; set; }
        public DateTime? Createdtime_Start { get; set; }
        public DateTime? Createdtime_End { get; set; }
    }
   
}
